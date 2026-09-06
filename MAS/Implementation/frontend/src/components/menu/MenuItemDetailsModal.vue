<script setup lang="ts">
import { computed, ref, watch } from 'vue'

import { menuService } from '@/services/menuService'
import type { MenuItemDetails } from '@/types/api'
import { formatPrice } from '@/utils/format'
import BaseModal from '@/components/common/BaseModal.vue'
import IngredientList from './IngredientList.vue'
import QuantitySelector from '@/components/common/QuantitySelector.vue'

const props = defineProps<{ menuItemId: number | null }>()
const emit = defineEmits<{ close: []; added: [quantity: number] }>()

const item = ref<MenuItemDetails | null>(null)
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
  item.value = null
  quantity.value = 1
  item.value = await menuService.getMenuItemDetails(props.menuItemId)
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
  <BaseModal :open="menuItemId !== null" :title="item?.name ?? 'Menu item'" :wide="true" @close="emit('close')">
    <div v-if="item" class="details">
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
        <button type="button" class="btn btn--primary btn--lg details__add-btn" :disabled="adding" @click="onAdd">
          {{ addLabel }}
        </button>
      </div>
      <p v-else class="details__unavailable">This item is currently unavailable and cannot be added to your order.</p>
    </div>
  </BaseModal>
</template>

<style scoped></style>