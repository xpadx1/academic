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

<style scoped></style>
