<script setup lang="ts">
import Button from '@/components/ui/button/Button.vue';
</script>

<template>
  <div class="container mx-auto px-4">
    <section class="py-20 text-center">
      <h1 class="mb-6 text-5xl font-bold leading-tight">404</h1>
      <p class="mx-auto mb-8 max-w-2xl text-balance text-xl text-muted-foreground">Not Found</p>
      <RouterLink to="/">
        <Button size="lg">Go to Dashboard</Button>
      </RouterLink>
    </section>
  </div>
</template>

<style scoped></style>
