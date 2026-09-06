<script setup lang="ts">
import type { Ingredient } from '@/types/api'

defineProps<{ ingredients: Ingredient[] }>()
</script>

<template>
  <div class="ingredients">
    <h4 class="ingredients__title">Ingredients</h4>
    <ul class="ingredients__list">
      <li v-for="ingredient in ingredients" :key="ingredient.id" class="ingredients__row">
        <span class="ingredients__name">{{ ingredient.name }}</span>
        <span class="ingredients__badges">
          <span v-if="ingredient.isAllergen" class="badge badge--allergen">Allergen</span>
          <span v-if="ingredient.isVegetarian" class="badge badge--veg">Veg</span>
        </span>
      </li>
    </ul>
    <p v-if="ingredients.length === 0" class="ingredients__empty">
      No ingredient information available.
    </p>
  </div>
</template>

<style scoped>
.ingredients {
  display: flex;
  flex-direction: column;
  gap: var(--space-2);
}

.ingredients__title {
  font-size: var(--font-size-md);
  font-weight: 700;
}

.ingredients__list {
  list-style: none;
  margin: 0;
  padding: 0;
  display: flex;
  flex-direction: column;
  border: 1px solid var(--color-border);
  border-radius: var(--radius-md);
  overflow: hidden;
}

.ingredients__row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--space-3);
  padding: var(--space-2) var(--space-3);
  border-bottom: 1px solid var(--color-border);
}

.ingredients__row:last-child {
  border-bottom: none;
}

.ingredients__name {
  font-size: var(--font-size-sm);
}

.ingredients__badges {
  display: flex;
  gap: var(--space-1);
  flex-shrink: 0;
}

.ingredients__empty {
  font-size: var(--font-size-sm);
  color: var(--color-text-muted);
}
</style>