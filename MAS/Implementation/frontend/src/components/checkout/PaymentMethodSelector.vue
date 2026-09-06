<script setup lang="ts">
import type { PaymentMethod } from '@/types/api'
import Icon from '@/components/common/Icon.vue'
import { icons } from '@/components/common/icons'

defineProps<{ modelValue: PaymentMethod | null }>()
const emit = defineEmits<{ 'update:modelValue': [value: PaymentMethod] }>()

const methods: { value: PaymentMethod; label: string; icon: string; desc: string }[] = [
  { value: 'Card', label: 'Card', icon: icons.card, desc: 'Pay with debit or credit card' },
  { value: 'Cash', label: 'Cash', icon: icons.cash, desc: 'Pay when your order is ready' },
]
</script>

<template>
  <div class="payment">
    <span class="payment__label">Payment method</span>
    <div class="payment__options">
      <button
        v-for="method in methods"
        :key="method.value"
        type="button"
        :class="['payment__option', modelValue === method.value ? 'payment__option--selected' : '']"
        :aria-pressed="modelValue === method.value"
        @click="emit('update:modelValue', method.value)"
      >
        <span class="payment__option-icon">
          <Icon :path="method.icon" :size="22" />
        </span>
        <span class="payment__option-text">
          <span class="payment__option-label">{{ method.label }}</span>
          <span class="payment__option-desc">{{ method.desc }}</span>
        </span>
        <span :class="['payment__radio', modelValue === method.value ? 'payment__radio--on' : '']">
          <span v-if="modelValue === method.value" class="payment__radio-dot"></span>
        </span>
      </button>
    </div>
  </div>
</template>

<style scoped></style>
