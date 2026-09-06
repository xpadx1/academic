<script setup lang="ts">
import { onBeforeUnmount, onMounted, watch } from 'vue'

import Icon from './Icon.vue'
import { icons } from './icons'

const props = withDefaults(
  defineProps<{
    open: boolean
    title: string
    wide?: boolean
  }>(),
  { wide: false },
)

const emit = defineEmits<{ close: [] }>()

function onKeydown(event: KeyboardEvent): void {
  if (event.key === 'Escape') {
    emit('close')
  }
}

watch(
  () => props.open,
  (isOpen) => {
    document.body.style.overflow = isOpen ? 'hidden' : ''
  },
  { immediate: true },
)

onMounted(() => {
  document.addEventListener('keydown', onKeydown)
})

onBeforeUnmount(() => {
  document.removeEventListener('keydown', onKeydown)
  document.body.style.overflow = ''
})
</script>

<template>
  <Teleport to="body">
    <div
      v-if="open"
      class="modal-overlay"
      role="dialog"
      aria-modal="true"
      :aria-label="title"
      @click.self="emit('close')"
    >
      <div :class="['modal', wide ? 'modal--wide' : '']">
        <div class="modal__header">
          <h2 class="modal__title">{{ title }}</h2>
          <button
            type="button"
            class="modal__close"
            aria-label="Close"
            @click="emit('close')"
          >
            <Icon :path="icons.close" :size="20" />
          </button>
        </div>
        <div class="modal__body">
          <slot />
        </div>
      </div>
    </div>
  </Teleport>
</template>