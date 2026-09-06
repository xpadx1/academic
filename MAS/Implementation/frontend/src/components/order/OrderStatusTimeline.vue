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
      <div
        class="timeline__progress"
        :style="{ width: `${(activeIndex / (steps.length - 1)) * 100}%` }"
      ></div>
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

<style scoped>
.timeline {
  display: flex;
  flex-direction: column;
  gap: var(--space-2);
}

.timeline__track {
  position: relative;
  height: 4px;
  background: var(--color-border);
  border-radius: var(--radius-pill);
  margin: 0 var(--space-2);
}

.timeline__progress {
  position: absolute;
  top: 0;
  left: 0;
  height: 100%;
  background: var(--color-success);
  border-radius: var(--radius-pill);
  transition: width 0.3s ease;
}

.timeline__steps {
  display: flex;
  justify-content: space-between;
  list-style: none;
  margin: 0;
  padding: 0;
}

.timeline__step {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: var(--space-1);
  flex: 1;
}

.timeline__dot {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 22px;
  height: 22px;
  border-radius: 50%;
  background: var(--color-surface);
  border: 2px solid var(--color-border-strong);
  color: var(--color-primary-contrast);
}

.timeline__step--done .timeline__dot {
  background: var(--color-success);
  border-color: var(--color-success);
}

.timeline__step--active .timeline__dot {
  background: var(--color-primary);
  border-color: var(--color-primary);
  box-shadow: 0 0 0 4px rgba(217, 84, 43, 0.18);
}

.timeline__label {
  font-size: var(--font-size-xs);
  color: var(--color-text-muted);
  text-align: center;
}

.timeline__step--active .timeline__label {
  color: var(--color-text);
  font-weight: 700;
}

.timeline__step--done .timeline__label {
  color: var(--color-success);
}
</style>