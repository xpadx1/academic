<script setup lang="ts">
import { computed } from 'vue'

import Icon from './Icon.vue'
import { icons } from './icons'

const props = withDefaults(
  defineProps<{
    modelValue: number
    min?: number
    max?: number
    disabled?: boolean
    sm?: boolean
  }>(),
  { min: 1, max: 99, disabled: false, sm: false },
)

const emit = defineEmits<{ 'update:modelValue': [value: number] }>()

const canDecrease = computed(() => props.modelValue > props.min)
const canIncrease = computed(() => props.modelValue < props.max)

function decrease(): void {
  if (canDecrease.value && !props.disabled) {
    emit('update:modelValue', props.modelValue - 1)
  }
}

function increase(): void {
  if (canIncrease.value && !props.disabled) {
    emit('update:modelValue', props.modelValue + 1)
  }
}
</script>

<template>
  <div :class="['qty', sm ? 'qty--sm' : '']">
    <button
      type="button"
      class="qty__btn"
      :disabled="!canDecrease || disabled"
      aria-label="Decrease quantity"
      @click="decrease"
    >
      <Icon :path="icons.minus" :size="sm ? 14 : 16" />
    </button>
    <span class="qty__value" aria-live="polite">{{ modelValue }}</span>
    <button
      type="button"
      class="qty__btn"
      :disabled="!canIncrease || disabled"
      aria-label="Increase quantity"
      @click="increase"
    >
      <Icon :path="icons.plus" :size="sm ? 14 : 16" />
    </button>
  </div>
</template>

<style scoped>
.qty {
  display: inline-flex;
  align-items: center;
  border: 1px solid var(--color-border-strong);
  border-radius: var(--radius-md);
  background: var(--color-surface);
  overflow: hidden;
}

.qty--sm {
  border-radius: var(--radius-sm);
}

.qty__btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 32px;
  height: 32px;
  border: none;
  background: var(--color-surface);
  color: var(--color-text);
  transition: background-color 0.12s ease;
}

.qty--sm .qty__btn {
  width: 26px;
  height: 26px;
}

.qty__btn:hover:not(:disabled) {
  background: var(--color-surface-pale);
}

.qty__btn:disabled {
  color: var(--color-border-strong);
  cursor: not-allowed;
}

.qty__value {
  min-width: 28px;
  text-align: center;
  font-size: var(--font-size-md);
  font-weight: 600;
  border-left: 1px solid var(--color-border);
  border-right: 1px solid var(--color-border);
  padding: 0 var(--space-1);
}

.qty--sm .qty__value {
  min-width: 22px;
  font-size: var(--font-size-sm);
}
</style>