import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
  // Load env file based on `mode` in the current working directory.
  // Set the third parameter to '' to load all env regardless of the `VITE_` prefix.
  const env = loadEnv(mode, process.cwd(), '');

  return {
    plugins: [react()],
    resolve: {
      alias: {
        '@': path.resolve(__dirname, './src'),
      },
    },
    server: {
      port: 3000,
      proxy: {
        // Proxy API requests to the backend API Gateway during development
        // to avoid CORS issues.
        // The target URL should be defined in the .env.development file
        // e.g., VITE_API_PROXY_TARGET=http://localhost:5000
        '/api': {
          target: env.VITE_API_PROXY_TARGET || 'http://localhost:5000',
          changeOrigin: true,
          secure: false,
          rewrite: (path) => path.replace(/^\/api/, '/api'),
        },
        // Proxy SignalR requests for real-time communication
        '/hubs': {
          target: env.VITE_API_PROXY_TARGET || 'http://localhost:5000',
          ws: true, // Enable WebSocket proxying
          changeOrigin: true,
          secure: false,
        }
      },
    },
    build: {
      outDir: 'build',
      sourcemap: true,
    },
    test: {
      globals: true,
      environment: 'jsdom',
      setupFiles: './src/setupTests.ts',
      css: true,
    },
  };
});