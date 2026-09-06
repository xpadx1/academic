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
    <p v-if="ingredients.length === 0" class="ingredients__empty">No ingredient information available.</p>
  </div>
</template>

<style scoped></style>
