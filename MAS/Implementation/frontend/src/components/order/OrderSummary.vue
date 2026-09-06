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

<style scoped>
.order-summary {
  display: flex;
  flex-direction: column;
  gap: var(--space-3);
  padding: var(--space-4);
  background: var(--color-surface-pale);
  border-radius: var(--radius-md);
}

.order-summary__title {
  font-size: var(--font-size-md);
  font-weight: 700;
}

.order-summary__list {
  list-style: none;
  margin: 0;
  padding: 0;
  display: flex;
  flex-direction: column;
  gap: var(--space-2);
}

.order-summary__row {
  display: flex;
  justify-content: space-between;
  gap: var(--space-3);
  font-size: var(--font-size-sm);
}

.order-summary__name {
  display: flex;
  gap: var(--space-2);
}

.order-summary__qty {
  font-weight: 700;
  color: var(--color-primary);
  min-width: 22px;
}

.order-summary__price {
  font-weight: 600;
  white-space: nowrap;
}

.order-summary__totals {
  display: flex;
  flex-direction: column;
  gap: var(--space-1);
  padding-top: var(--space-3);
  border-top: 1px solid var(--color-border);
}

.order-summary__total-row {
  display: flex;
  justify-content: space-between;
  font-size: var(--font-size-sm);
  color: var(--color-text-muted);
}

.order-summary__total-row--discount {
  color: var(--color-success);
}

.order-summary__total-row--final {
  font-size: var(--font-size-md);
  font-weight: 700;
  color: var(--color-text);
  padding-top: var(--space-1);
}
</style>