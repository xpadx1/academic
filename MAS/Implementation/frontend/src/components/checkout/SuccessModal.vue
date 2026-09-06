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
          <dd>
            <span class="badge badge--veg">{{ order.status }}</span>
          </dd>
        </div>
        <div class="success__row">
          <dt>Payment method</dt>
          <dd>Card</dd>
        </div>
        <div class="success__row">
          <dt>Total paid</dt>
          <dd class="success__total">{{ formatPrice(order.finalPayableAmount) }}</dd>
        </div>
      </dl>

      <div class="success__actions">
        <button type="button" class="btn btn--primary btn--lg" @click="emit('new-order')">Start a new order</button>
      </div>
    </div>
  </BaseModal>
</template>

<style scoped></style>
