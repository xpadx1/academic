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
        <p class="not-found__message">
          The page you're looking for doesn't exist or has been moved.
        </p>
        <button type="button" class="btn btn--primary btn--lg" @click="router.push({ name: 'home' })">
          Back to Create order
        </button>
      </div>
    </main>
  </div>
</template>

<style scoped>
.not-found {
  min-height: 100vh;
}

.not-found__main {
  max-width: 480px;
  margin: 0 auto;
  padding: var(--space-12) var(--space-6);
}

.not-found__card {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: var(--space-3);
  padding: var(--space-10) var(--space-6);
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-sm);
  text-align: center;
}

.not-found__code {
  font-size: 64px;
  font-weight: 800;
  color: var(--color-primary);
  line-height: 1;
}

.not-found__title {
  font-size: var(--font-size-xl);
  font-weight: 700;
}

.not-found__message {
  font-size: var(--font-size-md);
  color: var(--color-text-muted);
  margin-bottom: var(--space-2);
}
</style>