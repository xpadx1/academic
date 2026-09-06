<script setup lang="ts">
import { computed } from 'vue'

import type { OrderStatus } from '@/types/api'
import Icon from '@/components/common/Icon.vue'
import { icons } from '@/components/common/icons'

const props = defineProps<{ status: OrderStatus }>()

const steps: { key: OrderStatus; label: string }[] = [
  { key: 'Created', label: 'Created' },
  { key: 'Accepted', label: 'Accepted' },
  { key: 'Preparing', label: 'Preparing' },
  { key: 'Ready', label: 'Ready' },
  { key: 'Completed', label: 'Completed' },
]

const activeIndex = computed(() => steps.findIndex((s) => s.key === props.status))
</script>

<template>
  <div class="timeline" aria-label="Order status progress">
    <div class="timeline__track">
      <div class="timeline__progress" :style="{ width: `${(activeIndex / (steps.length - 1)) * 100}%` }"></div>
    </div>
    <ol class="timeline__steps">
      <li
        v-for="(step, index) in steps"
        :key="step.key"
        :class="[
          'timeline__step',
          index < activeIndex ? 'timeline__step--done' : '',
          index === activeIndex ? 'timeline__step--active' : '',
        ]"
      >
        <span class="timeline__dot">
          <Icon v-if="index < activeIndex" :path="icons.check" :size="12" />
        </span>
        <span class="timeline__label">{{ step.label }}</span>
      </li>
    </ol>
  </div>
</template>

<style scoped></style>
