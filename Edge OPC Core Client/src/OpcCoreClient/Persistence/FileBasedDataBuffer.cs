using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Edge.OpcCoreClient.Configuration;
using System.Edge.OpcCoreClient.Models;
using System.Buffers.Binary;

namespace System.Edge.OpcCoreClient.Persistence
{
    /// <summary>
    /// Implements a persistent, on-disk, circular buffer for DataPoint objects.
    /// This is used to store data during network outages (REQ-1-079).
    /// The implementation is designed for high performance and durability.
    /// It is NOT thread-safe for concurrent writes or reads; concurrent access must be synchronized externally.
    /// </summary>
    public class FileBasedDataBuffer : IDataBuffer, IDisposable
    {
        private const int HeaderSize = 24; // Version(8) + ReadPos(8) + WritePos(8)
        private const ulong CurrentVersion = 1;

        private readonly ILogger<FileBasedDataBuffer> _logger;
        private readonly BufferOptions _options;
        private readonly string _bufferFilePath;
        private FileStream _fileStream;
        private long _readPosition;
        private long _writePosition;

        public FileBasedDataBuffer(IOptions<BufferOptions> options, ILogger<FileBasedDataBuffer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrWhiteSpace(_options.FilePath))
            {
                throw new ArgumentException("Buffer file path cannot be empty.", nameof(options));
            }

            _bufferFilePath = Path.GetFullPath(_options.FilePath);
            InitializeBuffer();
        }

        private void InitializeBuffer()
        {
            try
            {
                var directory = Path.GetDirectoryName(_bufferFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                _fileStream = new FileStream(_bufferFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

                if (_fileStream.Length < HeaderSize)
                {
                    // New or corrupt file, initialize it.
                    _logger.LogInformation("Initializing new or empty buffer file at {FilePath}.", _bufferFilePath);
                    _fileStream.SetLength(_options.MaxSizeInBytes + HeaderSize);
                    _readPosition = HeaderSize;
                    _writePosition = HeaderSize;
                    WriteHeader();
                }
                else
                {
                    ReadHeader();
                }

                _logger.LogInformation("File-based data buffer initialized. Read Position: {ReadPos}, Write Position: {WritePos}", _readPosition, _writePosition);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to initialize the file-based data buffer at {FilePath}. Store-and-forward will be disabled.", _bufferFilePath);
                _fileStream?.Dispose();
                _fileStream = null;
            }
        }

        public async Task EnqueueAsync(DataPoint dataPoint)
        {
            if (_fileStream == null)
            {
                _logger.LogWarning("Buffer is not initialized. Cannot enqueue data point for TagId {TagId}.", dataPoint.TagId);
                return;
            }

            try
            {
                var serialized = SerializeDataPoint(dataPoint);
                int recordLength = serialized.Length;

                if (_writePosition + sizeof(int) + recordLength > _options.MaxSizeInBytes + HeaderSize)
                {
                    // Wrap around
                    _writePosition = HeaderSize;
                    _logger.LogDebug("Write position wrapping around to the beginning of the buffer.");
                }

                // Check for lapping the read pointer
                if (_writePosition < _readPosition && _writePosition + sizeof(int) + recordLength >= _readPosition)
                {
                    _logger.LogWarning("Buffer is full, overwriting oldest data. Write position is lapping the read position.");
                    // In a real scenario, we might advance the read pointer here, but for simplicity
                    // we just log. The Dequeue will simply read the newly written data.
                    // A more robust implementation would use sequence numbers or a separate index file.
                    _readPosition = _writePosition + sizeof(int) + recordLength;
                }
                
                _fileStream.Position = _writePosition;
                await _fileStream.WriteAsync(BitConverter.GetBytes(recordLength));
                await _fileStream.WriteAsync(serialized);

                _writePosition += sizeof(int) + recordLength;

                // Persist header changes periodically or on dispose instead of every write for performance
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to enqueue data point for TagId {TagId}.", dataPoint.TagId);
            }
        }

        public async Task<IEnumerable<DataPoint>> DequeueBatchAsync(int batchSize)
        {
            if (_fileStream == null || _readPosition == _writePosition)
            {
                return Array.Empty<DataPoint>();
            }

            var batch = new List<DataPoint>(batchSize);
            try
            {
                for (int i = 0; i < batchSize; i++)
                {
                    if (_readPosition == _writePosition)
                    {
                        break; // Buffer is empty
                    }

                    if (_readPosition + sizeof(int) > _options.MaxSizeInBytes + HeaderSize)
                    {
                        // Wrapped around, start from the beginning
                        _readPosition = HeaderSize;
                        _logger.LogDebug("Read position wrapping around to the beginning of the buffer.");
                        if (_readPosition == _writePosition) break;
                    }

                    _fileStream.Position = _readPosition;
                    
                    byte[] lengthBytes = new byte[sizeof(int)];
                    await _fileStream.ReadExactlyAsync(lengthBytes);
                    int recordLength = BitConverter.ToInt32(lengthBytes, 0);

                    if (recordLength <= 0 || _readPosition + sizeof(int) + recordLength > _options.MaxSizeInBytes + HeaderSize)
                    {
                        _logger.LogWarning("Found invalid record length ({RecordLength}) at position {Position}. Resetting read pointer to write pointer.", recordLength, _readPosition);
                        _readPosition = _writePosition;
                        WriteHeader(); // Persist the reset
                        return batch;
                    }

                    byte[] dataBytes = new byte[recordLength];
                    await _fileStream.ReadExactlyAsync(dataBytes);
                    
                    var dataPoint = DeserializeDataPoint(dataBytes);
                    batch.Add(dataPoint);

                    _readPosition += sizeof(int) + recordLength;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to dequeue batch from buffer. Resetting read pointer to prevent further errors.");
                _readPosition = _writePosition;
            }
            finally
            {
                WriteHeader(); // Persist the new read position after a batch read
            }
            
            return batch;
        }

        private byte[] SerializeDataPoint(DataPoint dataPoint)
        {
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, false);

            writer.Write(dataPoint.Timestamp.ToUnixTimeMilliseconds());
            writer.Write(dataPoint.TagId);
            writer.Write(dataPoint.Quality);
            
            // Basic type handling for serialization
            var value = dataPoint.Value;
            if (value is null)
            {
                writer.Write((byte)0); // Type marker for null
            }
            else if (value is bool b)
            {
                writer.Write((byte)1); writer.Write(b);
            }
            else if (value is int i)
            {
                writer.Write((byte)2); writer.Write(i);
            }
            else if (value is double d)
            {
                writer.Write((byte)3); writer.Write(d);
            }
            else if (value is float f)
            {
                writer.Write((byte)4); writer.Write(f);
            }
            else
            {
                writer.Write((byte)255); writer.Write(value.ToString()); // Fallback to string
            }

            return ms.ToArray();
        }

        private DataPoint DeserializeDataPoint(byte[] data)
        {
            using var ms = new MemoryStream(data);
            using var reader = new BinaryReader(ms, Encoding.UTF8, false);

            var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(reader.ReadInt64());
            var tagId = reader.ReadString();
            var quality = reader.ReadUInt16();
            
            object value;
            var typeMarker = reader.ReadByte();
            value = typeMarker switch
            {
                0 => null,
                1 => reader.ReadBoolean(),
                2 => reader.ReadInt32(),
                3 => reader.ReadDouble(),
                4 => reader.ReadSingle(),
                255 => reader.ReadString(),
                _ => null // Unknown type
            };
            
            return new DataPoint(tagId, timestamp, value, quality);
        }

        private void ReadHeader()
        {
            _fileStream.Position = 0;
            var buffer = new byte[HeaderSize];
            _fileStream.Read(buffer, 0, HeaderSize);
            
            var version = BitConverter.ToUInt64(buffer, 0);
            if (version != CurrentVersion)
            {
                _logger.LogWarning("Buffer file version mismatch. Expected {ExpectedVersion}, found {FoundVersion}. Re-initializing buffer.", CurrentVersion, version);
                _fileStream.SetLength(_options.MaxSizeInBytes + HeaderSize);
                _readPosition = HeaderSize;
                _writePosition = HeaderSize;
                WriteHeader();
                return;
            }

            _readPosition = BitConverter.ToInt64(buffer, 8);
            _writePosition = BitConverter.ToInt64(buffer, 16);

            // Sanity checks
            if (_readPosition < HeaderSize || _readPosition > _options.MaxSizeInBytes + HeaderSize ||
                _writePosition < HeaderSize || _writePosition > _options.MaxSizeInBytes + HeaderSize)
            {
                _logger.LogWarning("Buffer header contains invalid positions. Read: {ReadPos}, Write: {WritePos}. Resetting buffer.", _readPosition, _writePosition);
                _readPosition = HeaderSize;
                _writePosition = HeaderSize;
                WriteHeader();
            }
        }

        private void WriteHeader()
        {
            if (_fileStream == null) return;
            
            var buffer = new byte[HeaderSize];
            BitConverter.GetBytes(CurrentVersion).CopyTo(buffer, 0);
            BitConverter.GetBytes(_readPosition).CopyTo(buffer, 8);
            BitConverter.GetBytes(_writePosition).CopyTo(buffer, 16);
            
            _fileStream.Position = 0;
            _fileStream.Write(buffer, 0, HeaderSize);
            _fileStream.Flush(true); // Ensure header is written to disk
        }

        public void Dispose()
        {
            if (_fileStream != null)
            {
                WriteHeader(); // Final flush of positions before closing
                _fileStream.Dispose();
                _fileStream = null;
            }
        }
    }
}