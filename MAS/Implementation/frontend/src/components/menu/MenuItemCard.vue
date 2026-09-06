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
      <button type="button" class="btn btn--secondary btn--sm" @click="emit('details', item.id)">Details</button>
    </div>
    <p v-if="!item.isAvailable" class="menu-card__unavailable-note">Currently unavailable</p>
  </article>
</template>

<style scoped></style>
