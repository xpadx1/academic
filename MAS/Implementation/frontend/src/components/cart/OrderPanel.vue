<script setup lang="ts">
import { useCartStore } from '@/stores/cart'
import { formatPrice } from '@/utils/format'
import CartItem from './CartItem.vue'
import EmptyState from '@/components/common/EmptyState.vue'
import LoadingSpinner from '@/components/common/LoadingSpinner.vue'

const emit = defineEmits<{ checkout: [] }>()
const cart = useCartStore()
</script>

<template>
  <aside class="order-panel">
    <h2 class="order-panel__title">Your Order</h2>

    <LoadingSpinner v-if="cart.loading && cart.isEmpty" size="sm" label="Preparing your order" />

    <EmptyState
      v-else-if="cart.isEmpty"
      icon="cartEmpty"
      title="Your cart is empty"
      message="Browse the menu and add items to get started."
    />

<div v-else class="order-panel__content">
      <div class="order-panel__items">
        <CartItem
          v-for="item in cart.items"
          :key="item.id"
          :item="item"
          @update-quantity="(id, qty) => cart.updateQuantity(id, qty)"
          @remove="(id) => cart.removeItem(id)"
        />
      </div>

      <div class="order-panel__summary">
        <div class="order-panel__row">
          <span>Subtotal</span>
          <span>{{ formatPrice(cart.subtotal) }}</span>
        </div>
        <div class="order-panel__row order-panel__row--total">
          <span>Total</span>
          <span>{{ formatPrice(cart.total) }}</span>
        </div>
      </div>

      <button
        type="button"
        class="btn btn--primary btn--lg order-panel__checkout"
        :disabled="cart.loading"
        @click="emit('checkout')"
      >
        Proceed to checkout
      </button>
    </div>
  </aside>
</template>

<style scoped>
.order-panel {
  display: flex;
  flex-direction: column;
  gap: var(--space-4);
  padding: var(--space-5);
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-sm);
  height: fit-content;
  position: sticky;
  top: calc(var(--header-height) + var(--space-4));
}

.order-panel__title {
  font-size: var(--font-size-xl);
  font-weight: 700;
}

.order-panel__content {
  display: flex;
  flex-direction: column;
  gap: var(--space-4);
}

.order-panel__items {
  display: flex;
  flex-direction: column;
}

.order-panel__summary {
  display: flex;
  flex-direction: column;
  gap: var(--space-2);
  padding: var(--space-3) 0;
  border-top: 1px solid var(--color-border);
}

.order-panel__row {
  display: flex;
  justify-content: space-between;
  font-size: var(--font-size-md);
  color: var(--color-text-muted);
}

.order-panel__row--discount {
  color: var(--color-success);
}

.order-panel__row--total {
  font-size: var(--font-size-lg);
  font-weight: 700;
  color: var(--color-text);
  padding-top: var(--space-2);
  border-top: 1px dashed var(--color-border);
  margin-top: var(--space-1);
}

.order-panel__checkout {
  width: 100%;
}
</style>