<script setup lang="ts">
import { computed, ref } from 'vue'

import { useCartStore } from '@/stores/cart'
import { orderService } from '@/services/orderService'
import type { PaymentMethod } from '@/types/api'
import { formatPrice } from '@/utils/format'
import BaseModal from '@/components/common/BaseModal.vue'
import OrderSummary from '@/components/order/OrderSummary.vue'
import PaymentMethodSelector from './PaymentMethodSelector.vue'

const props = defineProps<{ customerId: number; open: boolean }>()
const emit = defineEmits<{ close: []; success: [] }>()

const cart = useCartStore()
const paymentMethod = ref<PaymentMethod | null>(null)
const loyaltyCardNumber = ref('')
const placing = ref(false)

const canPlace = computed(
  () => !cart.isEmpty && paymentMethod.value !== null && !placing.value,
)

async function placeOrder(): Promise<void> {
  if (!canPlace.value || cart.orderId === null) {
    return
  }

  placing.value = true

  await orderService.checkout(props.customerId, cart.orderId, {})

  const result = await orderService.processPayment(props.customerId, cart.orderId, {
    method: paymentMethod.value!,
  })

  placing.value = false

  if (result.success) {
    emit('success')
  }
}
</script>

<template>
  <BaseModal :open="open" title="Checkout" :wide="true" @close="emit('close')">
    <div v-if="cart.isEmpty" class="checkout">
      <p class="checkout__empty">Your cart is empty. Add items before checking out.</p>
    </div>

    <div v-else class="checkout">
      <p class="checkout__hint">Review your order and choose how you'd like to pay.</p>

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

      <div class="checkout__actions">
        <button type="button" class="btn btn--secondary" :disabled="placing" @click="emit('close')">
          Back to menu
        </button>
        <button type="button" class="btn btn--primary btn--lg" :disabled="!canPlace" @click="placeOrder">
          <span>{{ placing ? 'Processing…' : 'Place order' }}</span>
        </button>
      </div>
    </div>
  </BaseModal>
</template>

<style scoped></style>