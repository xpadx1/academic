<script setup lang="ts">
import type { MenuItem } from '@/types/api'
import { formatPrice } from '@/utils/format'

defineProps<{ item: MenuItem }>()
const emit = defineEmits<{ details: [id: number] }>()
</script>

<template>
  <article :class="['menu-card', !item.isAvailable ? 'menu-card--unavailable' : '']">
    <div class="menu-card__badges">
      <span v-if="item.itemType === 'Seasonal'" class="badge badge--seasonal">Seasonal</span>
    </div>
    <h3 class="menu-card__name">{{ item.name }}</h3>
    <p class="menu-card__desc">{{ item.description }}</p>
    <div class="menu-card__meta">
      <span class="menu-card__calories">{{ item.calories }} kcal</span>
    </div>
    <div class="menu-card__footer">
      <span class="menu-card__price">{{ formatPrice(item.currentPrice) }}</span>
      <button
        type="button"
        class="btn btn--secondary btn--sm"
        @click="emit('details', item.id)"
      >
        Details
      </button>
    </div>
    <p v-if="!item.isAvailable" class="menu-card__unavailable-note">Currently unavailable</p>
  </article>
</template>

<style scoped>
.menu-card {
  display: flex;
  flex-direction: column;
  gap: var(--space-2);
  padding: var(--space-4);
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-sm);
  transition: box-shadow 0.12s ease, border-color 0.12s ease;
}

.menu-card:hover {
  box-shadow: var(--shadow-md);
  border-color: var(--color-border-strong);
}

.menu-card--unavailable {
  opacity: 0.6;
}

.menu-card__badges {
  display: flex;
  gap: var(--space-1);
  min-height: 20px;
}

.menu-card__name {
  font-size: var(--font-size-lg);
  font-weight: 700;
}

.menu-card__desc {
  font-size: var(--font-size-sm);
  color: var(--color-text-muted);
  line-height: var(--line-normal);
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.menu-card__meta {
  font-size: var(--font-size-xs);
  color: var(--color-text-muted);
}

.menu-card__footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-top: auto;
  padding-top: var(--space-2);
}

.menu-card__price {
  font-size: var(--font-size-lg);
  font-weight: 700;
  color: var(--color-text);
}

.menu-card__unavailable-note {
  font-size: var(--font-size-xs);
  font-weight: 600;
  color: var(--color-danger);
  margin-top: var(--space-1);
}
</style>