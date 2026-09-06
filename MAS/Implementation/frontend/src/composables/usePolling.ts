import { onBeforeUnmount, ref } from 'vue'

/**
 * Simple polling helper. Calls `callback` every `intervalMs`, stopping when
 * the component unmounts or `shouldStop()` returns true. Errors are surfaced
 * via the `error` ref but do not stop polling (the caller decides).
 */
export function usePolling(
  callback: () => Promise<void>,
  intervalMs: number,
  shouldStop: () => boolean,
) {
  const error = ref<string | null>(null)
  const polling = ref(false)
  let timer: ReturnType<typeof setInterval> | null = null

  async function tick(): Promise<void> {
    if (shouldStop()) {
      stop()
      return
    }
    try {
      await callback()
      error.value = null
    } catch (err) {
      error.value = (err as Error).message
    }
  }

  function start(): void {
    if (timer !== null) {
      return
    }
    polling.value = true
    timer = setInterval(tick, intervalMs)
  }

  function stop(): void {
    if (timer !== null) {
      clearInterval(timer)
      timer = null
    }
    polling.value = false
  }

  onBeforeUnmount(stop)

  return { error, polling, start, stop, tick }
}