<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'

import { useCartStore } from '@/stores/cart'
import { useCustomerStore } from '@/stores/customer'
import { useMenuStore } from '@/stores/menu'
import { config } from '@/config'
import { menuService } from '@/services/menuService'
import type { MenuItem, MenuItemDetails } from '@/types/api'
import AppHeader from '@/components/common/AppHeader.vue'
import EmptyState from '@/components/common/EmptyState.vue'
import MenuItemCard from '@/components/menu/MenuItemCard.vue'
import MenuItemDetailsModal from '@/components/menu/MenuItemDetailsModal.vue'
import OrderPanel from '@/components/cart/OrderPanel.vue'
import CheckoutModal from '@/components/checkout/CheckoutModal.vue'
import SuccessModal from '@/components/checkout/SuccessModal.vue'

const customerStore = useCustomerStore()
const menuStore = useMenuStore()
const cartStore = useCartStore()

const selectedMenuItemId = ref<number | null>(null)
const checkoutOpen = ref(false)
const successOpen = ref(false)
const lastCompletedOrder = ref<MenuItemDetails | null>(null)
const addingItemId = ref<number | null>(null)

const selectedItem = computed(() =>
  selectedMenuItemId.value
    ? menuStore.filteredItems.find((i) => i.id === selectedMenuItemId.value) ?? null
    : null,
)

const hasItems = computed(() => menuStore.filteredItems.length > 0)

function itemsForMenu(menuId: number): MenuItem[] {
  const items = menuStore.itemsByMenu[menuId] ?? []
  if (menuStore.typeFilter === 'all') {
    return items
  }
  return items.filter((i) => i.itemType === menuStore.typeFilter)
}

onMounted(async () => {
  await Promise.all([customerStore.loadCustomer(), menuStore.loadMenus()])
  const menus = menuStore.menus
  if (menus.length > 0) {
    await Promise.all(menus.map((m) => menuStore.loadMenuItems(m.id)))
  }
  await cartStore.ensureOrder()
})

async function openDetails(itemId: number): Promise<void> {
  selectedMenuItemId.value = itemId
}

async function addSelectedToCart(quantity: number): Promise<void> {
  if (selectedMenuItemId.value === null) {
    return
  }
  addingItemId.value = selectedMenuItemId.value
  const details = await menuService.getMenuItemDetails(selectedMenuItemId.value)
  await cartStore.addItem(details.id, quantity)
  selectedMenuItemId.value = null
  addingItemId.value = null
}

function onCheckout(): void {
  checkoutOpen.value = true
}

async function onOrderSuccess(): Promise<void> {
  checkoutOpen.value = false
  await cartStore.refreshOrder()
  lastCompletedOrder.value = cartStore.order as unknown as MenuItemDetails
  successOpen.value = true
  cartStore.resetCart()
}

function onNewOrder(): void {
  successOpen.value = false
  lastCompletedOrder.value = null
}
</script>

<template>
  <div class="create-order">
    <AppHeader />

    <main class="create-order__main">
      <section class="create-order__menu">
        <div class="create-order__menu-header">
          <div>
            <h1 class="create-order__title">Lunch & Dinner Menu</h1>
            <p class="create-order__subtitle">Browse our selection and add items to your order.</p>
          </div>
        </div>

        <div class="create-order__controls">
          <div class="create-order__filters">
            <button
              type="button"
              :class="['chip', menuStore.menuFilter === 'all' ? 'chip--active' : '']"
              @click="menuStore.menuFilter = 'all'"
            >
              All Menus
            </button>
            <button
              v-for="menu in menuStore.menus"
              :key="menu.id"
              type="button"
              :class="['chip', menuStore.menuFilter === menu.id ? 'chip--active' : '']"
              @click="menuStore.menuFilter = menu.id"
            >
              {{ menu.name }}
            </button>
          </div>
        </div>

        <EmptyState
          v-if="!hasItems"
          icon="search"
          title="No menu items match your filter"
          message="Try a different filter."
        />
        <div v-else class="create-order__sections">
          <div v-for="menu in menuStore.filteredMenus" :key="menu.id" class="create-order__section">
            <h2 class="create-order__section-title">{{ menu.name }}</h2>
            <p v-if="menu.description" class="create-order__section-desc">
              {{ menu.description }}
            </p>
            <div v-if="itemsForMenu(menu.id).length > 0" class="create-order__grid">
              <MenuItemCard
                v-for="item in itemsForMenu(menu.id)"
                :key="item.id"
                :item="item"
                @details="openDetails"
              />
            </div>
            <p v-else class="create-order__section-empty">No items in this category match your filter.</p>
          </div>
        </div>
      </section>

      <OrderPanel @checkout="onCheckout" />
    </main>

    <MenuItemDetailsModal
      :menu-item-id="selectedMenuItemId"
      @close="selectedMenuItemId = null"
      @added="addSelectedToCart"
    />

    <CheckoutModal
      :open="checkoutOpen"
      :customer-id="config.customerId"
      @close="checkoutOpen = false"
      @success="onOrderSuccess"
    />

    <SuccessModal
      :open="successOpen"
      :order="(lastCompletedOrder as any)"
      @close="successOpen = false"
      @new-order="onNewOrder"
    />
  </div>
</template>

<style scoped></style>