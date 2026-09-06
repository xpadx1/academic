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

<style scoped>
.app-header {
  position: sticky;
  top: 0;
  z-index: 50;
  display: flex;
  align-items: center;
  justify-content: space-between;
  height: var(--header-height);
  padding: 0 var(--space-6);
  background: var(--color-surface);
  border-bottom: 1px solid var(--color-border);
}

.app-header__left {
  display: flex;
  align-items: center;
  gap: var(--space-3);
}

.app-header__logo {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 36px;
  height: 36px;
  border-radius: var(--radius-md);
  background: var(--color-surface-pale);
  color: var(--color-primary);
}

.app-header__brand {
  display: flex;
  flex-direction: column;
  line-height: var(--line-tight);
}

.app-header__title {
  font-size: var(--font-size-md);
  font-weight: 700;
}

.app-header__subtitle {
  font-size: var(--font-size-xs);
  color: var(--color-text-muted);
}

.app-header__right {
  display: flex;
  align-items: center;
  gap: var(--space-4);
}

.app-header__cart {
  position: relative;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 36px;
  height: 36px;
  border-radius: var(--radius-md);
  color: var(--color-text);
}

.app-header__cart-count {
  position: absolute;
  top: -4px;
  right: -4px;
  min-width: 18px;
  height: 18px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 0 4px;
  border-radius: var(--radius-pill);
  background: var(--color-primary);
  color: var(--color-primary-contrast);
  font-size: var(--font-size-xs);
  font-weight: 700;
}

</style>