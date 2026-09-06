<script setup lang="ts">
import { computed, ref } from 'vue'

import { useCartStore } from '@/stores/cart'
import { orderService } from '@/services/orderService'
import type { PaymentMethod } from '@/types/api'
import { formatPrice } from '@/utils/format'
import BaseModal from '@/components/common/BaseModal.vue'
import OrderSummary from '@/components/order/OrderSummary.vue'
import PaymentMethodSelector from './PaymentMethodSelector.vue'
import ErrorMessage from '@/components/common/ErrorMessage.vue'
import LoadingSpinner from '@/components/common/LoadingSpinner.vue'

const props = defineProps<{ customerId: number; open: boolean }>()
const emit = defineEmits<{ close: []; success: [] }>()

const cart = useCartStore()
const paymentMethod = ref<PaymentMethod | null>(null)
const loyaltyCardNumber = ref('')
const placing = ref(false)
const error = ref<string | null>(null)

const canPlace = computed(
  () => !cart.isEmpty && paymentMethod.value !== null && !placing.value,
)

async function placeOrder(): Promise<void> {
  if (!canPlace.value || cart.orderId === null) {
    return
  }

  placing.value = true
  error.value = null

  try {
    // Step 1: validate the order (items, availability).
    await orderService.checkout(props.customerId, cart.orderId, {})

    // Step 2: process payment — backend records payment and moves to Accepted.
    const result = await orderService.processPayment(props.customerId, cart.orderId, {
      method: paymentMethod.value!,
    })

    if (!result.success) {
      error.value = result.errorMessage ?? 'Payment could not be completed.'
      return
    }

    emit('success')
  } catch (err) {
    error.value = (err as Error).message
  } finally {
    placing.value = false
  }
}
</script>

<template>
  <BaseModal :open="open" title="Checkout" :wide="true" @close="emit('close')">
    <div v-if="cart.isEmpty" class="checkout">
      <p class="checkout__empty">Your cart is empty. Add items before checking out.</p>
    </div>

    <div v-else class="checkout">
      <p class="checkout__hint">
        Review your order and choose how you'd like to pay.
      </p>

      <OrderSummary :order="cart.order!" title="Order summary" />

      <div class="checkout__loyalty">
        <label class="checkout__loyalty-label" for="checkout-loyalty-card">Loyalty card number (optional)</label>
        <input
          id="checkout-loyalty-card"
          class="input"
          type="text"
          placeholder="e.g. LOYAL-1001"
          v-model="loyaltyCardNumber"
          autocomplete="off"
        />
      </div>

      <PaymentMethodSelector v-model="paymentMethod" />

      <div class="checkout__total">
        <span>Total to pay</span>
        <span>{{ formatPrice(cart.total) }}</span>
      </div>

      <ErrorMessage v-if="error" :message="error" />

      <div class="checkout__actions">
        <button
          type="button"
          class="btn btn--secondary"
          :disabled="placing"
          @click="emit('close')"
        >
          Back to menu
        </button>
        <button
          type="button"
          class="btn btn--primary btn--lg"
          :disabled="!canPlace"
          @click="placeOrder"
        >
          <LoadingSpinner v-if="placing" size="sm" />
          <span>{{ placing ? 'Processing…' : 'Place order' }}</span>
        </button>
      </div>
    </div>
  </BaseModal>
</template>

<style scoped>
.checkout {
  display: flex;
  flex-direction: column;
  gap: var(--space-5);
}

.checkout__hint {
  font-size: var(--font-size-sm);
  color: var(--color-text-muted);
}

.checkout__empty {
  font-size: var(--font-size-md);
  color: var(--color-text-muted);
  padding: var(--space-4) 0;
}

.checkout__total {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: var(--space-3) var(--space-4);
  background: var(--color-surface-pale);
  border-radius: var(--radius-md);
  font-size: var(--font-size-lg);
  font-weight: 700;
}

.checkout__loyalty {
  display: flex;
  flex-direction: column;
  gap: var(--space-2);
}

.checkout__loyalty-label {
  font-size: var(--font-size-sm);
  font-weight: 500;
  color: var(--color-text-muted);
}

.checkout__actions {
  display: flex;
  gap: var(--space-3);
  justify-content: flex-end;
  padding-top: var(--space-2);
}
</style>