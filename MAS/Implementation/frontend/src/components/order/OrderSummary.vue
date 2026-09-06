<script setup lang="ts">
import type { Order } from '@/types/api'
import { formatPrice } from '@/utils/format'

defineProps<{ order: Order; title?: string }>()
</script>

<template>
  <div class="order-summary">
    <span v-if="title" class="order-summary__title">{{ title }}</span>
    <ul class="order-summary__list">
      <li v-for="item in order.items" :key="item.id" class="order-summary__row">
        <span class="order-summary__name">
          <span class="order-summary__qty">{{ item.quantity }}×</span>
          {{ item.menuItemName }}
        </span>
        <span class="order-summary__price">{{ formatPrice(item.totalPrice) }}</span>
      </li>
    </ul>
  </div>
</template>

<style scoped></style>
