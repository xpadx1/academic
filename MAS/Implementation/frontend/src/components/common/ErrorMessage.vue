<script setup lang="ts">
import Icon from './Icon.vue'
import { icons } from './icons'

defineProps<{ message: string }>()
</script>

<template>
  <div class="error-message" role="alert">
    <Icon :path="icons.alert" :size="18" />
    <span>{{ message }}</span>
  </div>
</template>