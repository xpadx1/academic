<script setup lang="ts">
defineProps<{
  /** SVG path data (24x24 viewBox). */
  path: string
  /** Pixel size; defaults to 24. */
  size?: number
}>()
</script>

<template>
  <svg
    :width="size ?? 24"
    :height="size ?? 24"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    stroke-width="1.8"
    stroke-linecap="round"
    stroke-linejoin="round"
    aria-hidden="true"
    focusable="false"
  >
    <path :d="path" />
  </svg>
</template>