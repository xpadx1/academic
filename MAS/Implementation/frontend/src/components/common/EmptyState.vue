<script setup lang="ts">
import Icon from './Icon.vue'
import { icons } from './icons'
import type { IconName } from './icons'

defineProps<{
  icon?: IconName
  title: string
  message?: string
}>()
</script>

<template>
  <div class="empty-state">
    <Icon :path="icons[icon ?? 'cartEmpty']" :size="48" class="empty-state__icon" />
    <p class="empty-state__title">{{ title }}</p>
    <p v-if="message" class="empty-state__message">{{ message }}</p>
  </div>
</template>

<style scoped>
.empty-state__message {
  font-size: var(--font-size-sm);
  max-width: 280px;
}
</style>