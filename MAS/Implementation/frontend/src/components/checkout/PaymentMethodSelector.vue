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

<style scoped>
.payment {
  display: flex;
  flex-direction: column;
  gap: var(--space-2);
}

.payment__label {
  font-size: var(--font-size-md);
  font-weight: 700;
}

.payment__options {
  display: flex;
  flex-direction: column;
  gap: var(--space-2);
}

.payment__option {
  display: flex;
  align-items: center;
  gap: var(--space-3);
  padding: var(--space-3) var(--space-4);
  background: var(--color-surface);
  border: 1px solid var(--color-border-strong);
  border-radius: var(--radius-md);
  text-align: left;
  transition: border-color 0.12s ease, box-shadow 0.12s ease;
}

.payment__option:hover {
  border-color: var(--color-primary);
}

.payment__option--selected {
  border-color: var(--color-primary);
  box-shadow: 0 0 0 3px rgba(217, 84, 43, 0.12);
}

.payment__option-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 38px;
  height: 38px;
  border-radius: var(--radius-sm);
  background: var(--color-surface-pale);
  color: var(--color-primary);
  flex-shrink: 0;
}

.payment__option-text {
  display: flex;
  flex-direction: column;
  gap: 2px;
  flex: 1;
}

.payment__option-label {
  font-size: var(--font-size-md);
  font-weight: 600;
}

.payment__option-desc {
  font-size: var(--font-size-xs);
  color: var(--color-text-muted);
}

.payment__radio {
  width: 18px;
  height: 18px;
  border-radius: 50%;
  border: 2px solid var(--color-border-strong);
  display: inline-flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.payment__radio--on {
  border-color: var(--color-primary);
}

.payment__radio-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: var(--color-primary);
}
</style>