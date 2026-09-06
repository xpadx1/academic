<script setup lang="ts">
import type { Order } from '@/types/api'
import { formatDateTime, formatPrice } from '@/utils/format'
import BaseModal from '@/components/common/BaseModal.vue'
import Icon from '@/components/common/Icon.vue'
import { icons } from '@/components/common/icons'

defineProps<{ open: boolean; order: Order | null }>()
const emit = defineEmits<{ close: []; 'new-order': [] }>()
</script>

<template>
  <BaseModal :open="open" title="" @close="emit('close')">
    <div v-if="order" class="success">
      <div class="success__icon">
        <Icon :path="icons.check" :size="32" />
      </div>
      <h2 class="success__title">Order placed successfully</h2>
      <p class="success__message">
        Your order <strong>#{{ order.id }}</strong> has been accepted and is being prepared.
      </p>

      <dl class="success__details">
        <div class="success__row">
          <dt>Status</dt>
          <dd><span class="badge badge--veg">{{ order.status }}</span></dd>
        </div>
        <div class="success__row">
          <dt>Payment method</dt>
          <dd>{{ order.paymentMethod ?? '—' }}</dd>
        </div>
        <div class="success__row">
          <dt>Total paid</dt>
          <dd class="success__total">{{ formatPrice(order.finalPayableAmount) }}</dd>
        </div>
      </dl>

      <div class="success__actions">
        <button
          type="button"
          class="btn btn--primary btn--lg"
          @click="emit('new-order')"
        >
          Start a new order
        </button>
      </div>
    </div>
  </BaseModal>
</template>

<style scoped>
.success {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: var(--space-3);
  text-align: center;
  padding-bottom: var(--space-2);
}

.success__icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 64px;
  height: 64px;
  border-radius: 50%;
  background: var(--color-success-bg);
  color: var(--color-success);
}

.success__title {
  font-size: var(--font-size-xl);
  font-weight: 700;
}

.success__message {
  font-size: var(--font-size-md);
  color: var(--color-text-muted);
  max-width: 360px;
}

.success__details {
  width: 100%;
  margin: var(--space-2) 0;
  padding: var(--space-4);
  background: var(--color-surface-pale);
  border-radius: var(--radius-md);
  display: flex;
  flex-direction: column;
  gap: var(--space-2);
}

.success__row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: var(--font-size-sm);
}

.success__row dt {
  color: var(--color-text-muted);
}

.success__row dd {
  margin: 0;
  font-weight: 600;
}

.success__total {
  color: var(--color-primary);
  font-size: var(--font-size-md);
}

.success__actions {
  display: flex;
  gap: var(--space-3);
  width: 100%;
  justify-content: center;
  margin-top: var(--space-2);
}
</style>