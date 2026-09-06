<script setup lang="ts">
import { useCartStore } from '@/stores/cart'
import { formatPrice } from '@/utils/format'
import CartItem from './CartItem.vue'
import EmptyState from '@/components/common/EmptyState.vue'

const emit = defineEmits<{ checkout: [] }>()
const cart = useCartStore()
</script>

<template>
  <aside class="order-panel">
    <h2 class="order-panel__title">Your Order</h2>

    <EmptyState
      v-if="cart.isEmpty"
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
        @click="emit('checkout')"
      >
        Proceed to checkout
      </button>
    </div>
  </aside>
</template>

<style scoped></style>
