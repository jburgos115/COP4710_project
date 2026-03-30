#!/usr/bin/env bash
set -euxo pipefail
echo "Runner: ${RUNNER_NAME:-unknown} on ${RUNNER_OS:-?}/${RUNNER_ARCH:-?}"
echo "CPUs: $(nproc)"
free -h || true
if [ -f /sys/fs/cgroup/memory.max ]; then
  echo "cgroup v2 memory.max: $(cat /sys/fs/cgroup/memory.max)"
fi
if [ -f /sys/fs/cgroup/memory/memory.limit_in_bytes ]; then
  echo "cgroup v1 memory.limit_in_bytes: $(cat /sys/fs/cgroup/memory/memory.limit_in_bytes)"
fi
