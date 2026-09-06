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

<style scoped></style>
