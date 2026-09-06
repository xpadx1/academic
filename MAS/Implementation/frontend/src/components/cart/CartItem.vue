<script setup lang="ts">
import type { OrderItem } from '@/types/api'
import { formatPrice } from '@/utils/format'
import QuantitySelector from '@/components/common/QuantitySelector.vue'

const props = defineProps<{ item: OrderItem }>()
const emit = defineEmits<{
  updateQuantity: [orderItemId: number, quantity: number]
  remove: [orderItemId: number]
}>()
</script>

<template>
  <div class="cart-item">
    <div class="cart-item__main">
      <span class="cart-item__name">{{ item.menuItemName }}</span>
      <span class="cart-item__unit">{{ formatPrice(item.unitPrice) }} each</span>
    </div>
    <div class="cart-item__controls">
      <QuantitySelector
        :model-value="item.quantity"
        :sm="true"
        @update:model-value="emit('updateQuantity', item.id, $event)"
      />
      <button
        type="button"
        class="btn btn--ghost btn--sm cart-item__remove"
        aria-label="Remove item"
        @click="emit('remove', item.id)"
      >
        ✕
      </button>
    </div>
    <span class="cart-item__total">{{ formatPrice(item.totalPrice) }}</span>
  </div>
</template>

<style scoped>
.cart-item {
  display: grid;
  grid-template-columns: 1fr auto;
  grid-template-areas:
    'main controls'
    'total total';
  gap: var(--space-2);
  padding: var(--space-3) 0;
  border-bottom: 1px solid var(--color-border);
}

.cart-item:last-child {
  border-bottom: none;
}

.cart-item__main {
  grid-area: main;
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.cart-item__name {
  font-size: var(--font-size-md);
  font-weight: 600;
}

.cart-item__unit {
  font-size: var(--font-size-xs);
  color: var(--color-text-muted);
}

.cart-item__controls {
  grid-area: controls;
  display: flex;
  align-items: center;
  gap: var(--space-2);
}

.cart-item__remove {
  color: var(--color-text-muted);
  font-size: var(--font-size-md);
}

.cart-item__remove:hover {
  color: var(--color-danger);
}

.cart-item__total {
  grid-area: total;
  text-align: right;
  font-size: var(--font-size-md);
  font-weight: 700;
}
</style>