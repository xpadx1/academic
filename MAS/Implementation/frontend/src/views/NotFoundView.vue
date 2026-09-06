<script setup lang="ts">
import { useRouter } from 'vue-router'

const router = useRouter()
</script>

<template>
  <div class="not-found">
    <main class="not-found__main">
      <div class="not-found__card">
        <h1 class="not-found__code">404</h1>
        <h2 class="not-found__title">Page not found</h2>
        <p class="not-found__message">The page you're looking for doesn't exist or has been moved.</p>
        <button type="button" class="btn btn--primary btn--lg" @click="router.push({ name: 'home' })">
          Back to Create order
        </button>
      </div>
    </main>
  </div>
</template>

<style scoped></style>
