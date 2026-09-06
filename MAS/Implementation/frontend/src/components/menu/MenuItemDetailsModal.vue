<script setup lang="ts">
import { computed, ref, watch } from 'vue'

import { menuService } from '@/services/menuService'
import type { MenuItemDetails } from '@/types/api'
import { formatPrice } from '@/utils/format'
import BaseModal from '@/components/common/BaseModal.vue'
import IngredientList from './IngredientList.vue'
import QuantitySelector from '@/components/common/QuantitySelector.vue'
import LoadingSpinner from '@/components/common/LoadingSpinner.vue'
import ErrorMessage from '@/components/common/ErrorMessage.vue'

const props = defineProps<{ menuItemId: number | null }>()
const emit = defineEmits<{ close: []; added: [quantity: number] }>()

const item = ref<MenuItemDetails | null>(null)
const loading = ref(false)
const error = ref<string | null>(null)
const quantity = ref(1)
const adding = ref(false)

const totalPrice = computed(() =>
  item.value ? item.value.currentPrice * quantity.value : 0,
)

const addLabel = computed(() => {
  const price = formatPrice(totalPrice.value)
  return quantity.value === 1 ? `Add · ${price}` : `Add ${quantity.value} · ${price}`
})

async function loadItem(): Promise<void> {
  if (props.menuItemId === null) {
    return
  }
  loading.value = true
  error.value = null
  item.value = null
  quantity.value = 1
  try {
    item.value = await menuService.getMenuItemDetails(props.menuItemId)
  } catch (err) {
    error.value = (err as Error).message
  } finally {
    loading.value = false
  }
}

watch(() => props.menuItemId, loadItem, { immediate: true })

function onAdd(): void {
  if (!item.value) {
    return
  }
  emit('added', quantity.value)
}
</script>

<template>
  <BaseModal
    :open="menuItemId !== null"
    :title="item?.name ?? 'Menu item'"
    :wide="true"
    @close="emit('close')"
  >
    <LoadingSpinner v-if="loading" label="Loading item details" />

    <ErrorMessage v-else-if="error" :message="error" />

    <div v-else-if="item" class="details">
      <div class="details__badges">
        <span v-if="item.itemType === 'Seasonal'" class="badge badge--seasonal">Seasonal</span>
        <span v-if="!item.isAvailable" class="badge badge--neutral">Unavailable</span>
      </div>

      <p class="details__price">{{ formatPrice(item.currentPrice) }}</p>
      <p class="details__desc">{{ item.description }}</p>

      <div class="details__meta">
        <span>{{ item.calories }} kcal</span>
        <span>·</span>
        <span>{{ item.itemType }} item</span>
      </div>

      <IngredientList :ingredients="item.ingredients" />

      <div v-if="item.isAvailable" class="details__add">
        <QuantitySelector v-model="quantity" />
        <button
          type="button"
          class="btn btn--primary btn--lg details__add-btn"
          :disabled="adding"
          @click="onAdd"
        >
          {{ addLabel }}
        </button>
      </div>
      <p v-else class="details__unavailable">
        This item is currently unavailable and cannot be added to your order.
      </p>
    </div>
  </BaseModal>
</template>

<style scoped>
.details {
  display: flex;
  flex-direction: column;
  gap: var(--space-4);
}

.details__badges {
  display: flex;
  gap: var(--space-1);
}

.details__price {
  font-size: var(--font-size-2xl);
  font-weight: 700;
  color: var(--color-primary);
}

.details__desc {
  font-size: var(--font-size-md);
  color: var(--color-text-muted);
  line-height: var(--line-normal);
}

.details__meta {
  display: flex;
  gap: var(--space-2);
  font-size: var(--font-size-sm);
  color: var(--color-text-muted);
}

.details__add {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--space-4);
  padding-top: var(--space-3);
  border-top: 1px solid var(--color-border);
}

.details__add-btn {
  flex: 1;
}

.details__unavailable {
  padding: var(--space-3) var(--space-4);
  background: var(--color-danger-bg);
  border-radius: var(--radius-md);
  color: var(--color-danger);
  font-size: var(--font-size-sm);
  font-weight: 500;
}
</style>