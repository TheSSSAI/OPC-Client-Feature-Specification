#!/bin/sh

# run-performance-tests.sh
#
# This script executes performance tests against a target URL using k6.
# It requires k6 to be installed on the runner.
#
# It is designed to be called from a GitHub Actions workflow and will exit
# with a non-zero status code if the performance thresholds are not met,
# causing the workflow step to fail.
#
# Environment Variables:
#   TARGET_URL: (Required) The base URL of the service to test.
#   VUS: (Optional) The number of virtual users to simulate. Default: 10.
#   DURATION: (Optional) The duration of the test. Default: '30s'.
#   P95_LATENCY_THRESHOLD_MS: (Optional) The P95 latency threshold in ms. Default: 500.
#   ERROR_RATE_THRESHOLD: (Optional) The maximum allowed error rate. Default: '0.01' (1%).

# --- Configuration ---
set -e # Exit immediately if a command exits with a non-zero status.

# Check for required dependencies
if ! command -v k6 >/dev/null 2>&1; then
    echo "Error: k6 is not installed. Please install k6 to run performance tests."
    exit 1
fi

# Check for required environment variables
if [ -z "$TARGET_URL" ]; then
    echo "Error: TARGET_URL environment variable is not set."
    exit 1
fi

# Set defaults for optional variables
VUS=${VUS:-10}
DURATION=${DURATION:-'30s'}
P95_LATENCY_THRESHOLD_MS=${P95_LATENCY_THRESHOLD_MS:-500}
ERROR_RATE_THRESHOLD=${ERROR_RATE_THRESHOLD:-'0.01'}

echo "--- Performance Test Configuration ---"
echo "Target URL: $TARGET_URL"
echo "Virtual Users: $VUS"
echo "Duration: $DURATION"
echo "P95 Latency Threshold: ${P95_LATENCY_THRESHOLD_MS}ms"
echo "Error Rate Threshold: $ERROR_RATE_THRESHOLD"
echo "------------------------------------"

# --- k6 Script Definition ---
# This script is passed to k6 via stdin.
# It defines a simple load test that hits the /healthz endpoint.
# In a real-world scenario, you would have more complex scenarios in separate JS files.
K6_SCRIPT=$(cat <<EOF
import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend, Rate } from 'k6/metrics';

// Define custom metrics
let p95Latency = new Trend('p95_latency');
let errorRate = new Rate('error_rate');

export let options = {
  vus: ${VUS},
  duration: '${DURATION}',
  thresholds: {
    'http_req_failed': ['rate<${ERROR_RATE_THRESHOLD}'], // http errors should be less than 1%
    'http_req_duration': ['p(95)<${P95_LATENCY_THRESHOLD_MS}'], // 95% of requests must complete below threshold
  },
};

export default function () {
  let res = http.get('${TARGET_URL}/healthz');

  // Check for successful response
  let success = check(res, {
    'status is 200': (r) => r.status === 200,
  });

  // Track error rate
  errorRate.add(!success);

  // Add latency to custom trend metric
  p95Latency.add(res.timings.duration);

  sleep(1);
}
EOF
)

# --- Execution ---
echo "Starting k6 performance test..."

# Run k6 with the script from stdin.
# The exit code of k6 will determine the success or failure of this script.
echo "$K6_SCRIPT" | k6 run -

echo "k6 performance test completed successfully."
exit 0