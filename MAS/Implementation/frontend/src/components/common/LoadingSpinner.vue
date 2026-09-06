<script setup lang="ts">
defineProps<{ size?: 'sm' | 'md'; label?: string }>()
</script>

<template>
  <div class="loading-spinner" role="status" :aria-label="label ?? 'Loading'">
    <div :class="['spinner', size === 'sm' ? 'spinner--sm' : '']"></div>
    <span v-if="label" class="loading-spinner__label">{{ label }}</span>
  </div>
</template>

<style scoped>
.loading-spinner {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: var(--space-3);
  padding: var(--space-8);
  color: var(--color-text-muted);
  font-size: var(--font-size-sm);
}
</style>