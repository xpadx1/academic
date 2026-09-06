<script setup lang="ts">
import { useCartStore } from '@/stores/cart'
import Icon from './Icon.vue'
import { icons } from './icons'

const cartStore = useCartStore()
</script>

<template>
  <header class="app-header">
    <div class="app-header__left">
      <div class="app-header__brand">
        <span class="app-header__title">Restaurant panel</span>
        <span class="app-header__subtitle">Create order</span>
      </div>
    </div>
  </header>
</template>

<style scoped></style>
