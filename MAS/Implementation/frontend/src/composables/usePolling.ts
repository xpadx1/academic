import { onBeforeUnmount, ref } from "vue";

export function usePolling(callback: () => Promise<void>, intervalMs: number, shouldStop: () => boolean) {
  const polling = ref(false);
  let timer: ReturnType<typeof setInterval> | null = null;

  async function tick(): Promise<void> {
    if (shouldStop()) {
      stop();
      return;
    }
    await callback();
  }

  function start(): void {
    if (timer !== null) {
      return;
    }
    polling.value = true;
    timer = setInterval(tick, intervalMs);
  }

  function stop(): void {
    if (timer !== null) {
      clearInterval(timer);
      timer = null;
    }
    polling.value = false;
  }

  onBeforeUnmount(stop);

  return { polling, start, stop, tick };
}