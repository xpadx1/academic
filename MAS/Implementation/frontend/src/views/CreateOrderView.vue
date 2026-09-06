<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'

import { useCartStore } from '@/stores/cart'
import { useCustomerStore } from '@/stores/customer'
import { useMenuStore } from '@/stores/menu'
import { config } from '@/config'
import { menuService } from '@/services/menuService'
import type { MenuItem, MenuItemDetails } from '@/types/api'
import AppHeader from '@/components/common/AppHeader.vue'
import LoadingSpinner from '@/components/common/LoadingSpinner.vue'
import ErrorMessage from '@/components/common/ErrorMessage.vue'
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
const addError = ref<string | null>(null)

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
  addError.value = null
  try {
    // Fetch details to get the current price (backend authority) and availability.
    const details = await menuService.getMenuItemDetails(selectedMenuItemId.value)
    if (!details.isAvailable) {
      addError.value = 'This item is currently unavailable.'
      return
    }
    await cartStore.addItem(details.id, quantity)
    selectedMenuItemId.value = null
  } catch (err) {
    addError.value = (err as Error).message
  } finally {
    addingItemId.value = null
  }
}

function onCheckout(): void {
  checkoutOpen.value = true
}

async function onOrderSuccess(): Promise<void> {
  checkoutOpen.value = false
  // Refresh the order to show the accepted state in the success modal.
  await cartStore.refreshOrder()
  lastCompletedOrder.value = cartStore.order as unknown as MenuItemDetails
  successOpen.value = true
  // Reset the cart immediately so the new order flow works regardless of
  // how the success modal is closed (button click or click-outside).
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
            <p class="create-order__subtitle">
              Browse our selection and add items to your order.
            </p>
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

        <LoadingSpinner v-if="menuStore.loading" label="Loading menus" />
        <ErrorMessage v-else-if="menuStore.error" :message="menuStore.error" />

        <template v-else>
          <EmptyState
            v-if="!hasItems"
            icon="search"
            title="No menu items match your filter"
            message="Try a different filter."
          />
          <div v-else class="create-order__sections">
            <div
              v-for="menu in menuStore.filteredMenus"
              :key="menu.id"
              class="create-order__section"
            >
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
              <p v-else class="create-order__section-empty">
                No items in this category match your filter.
              </p>
            </div>
          </div>
        </template>
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

<style scoped>
.create-order {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

.create-order__main {
  flex: 1;
  display: grid;
  grid-template-columns: 1fr var(--order-panel-width);
  gap: var(--space-6);
  max-width: var(--content-max-width);
  width: 100%;
  margin: 0 auto;
  padding: var(--space-6);
  align-items: start;
}

.create-order__menu {
  display: flex;
  flex-direction: column;
  gap: var(--space-4);
  min-width: 0;
}

.create-order__title {
  font-size: var(--font-size-2xl);
  font-weight: 700;
}

.create-order__subtitle {
  font-size: var(--font-size-md);
  color: var(--color-text-muted);
  margin-top: var(--space-1);
}

.create-order__controls {
  display: flex;
  flex-direction: column;
  gap: var(--space-3);
}

.create-order__filters {
  display: flex;
  gap: var(--space-2);
  flex-wrap: wrap;
}

.chip {
  padding: 6px 14px;
  border-radius: var(--radius-pill);
  border: 1px solid var(--color-border-strong);
  background: var(--color-surface);
  font-size: var(--font-size-sm);
  font-weight: 500;
  color: var(--color-text-muted);
  transition: all 0.12s ease;
}

.chip:hover {
  border-color: var(--color-primary);
  color: var(--color-primary);
}

.chip--active {
  background: var(--color-primary);
  border-color: var(--color-primary);
  color: var(--color-primary-contrast);
}

.create-order__sections {
  display: flex;
  flex-direction: column;
  gap: var(--space-6);
}

.create-order__section {
  display: flex;
  flex-direction: column;
  gap: var(--space-3);
}

.create-order__section-title {
  font-size: var(--font-size-xl);
  font-weight: 700;
  padding-bottom: var(--space-2);
  border-bottom: 2px solid var(--color-border);
}

.create-order__section-desc {
  font-size: var(--font-size-sm);
  color: var(--color-text-muted);
  margin-top: calc(var(--space-1) * -1);
}

.create-order__section-empty {
  font-size: var(--font-size-sm);
  color: var(--color-text-muted);
  font-style: italic;
}

.create-order__grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
  gap: var(--space-4);
}

@media (max-width: 860px) {
  .create-order__main {
    grid-template-columns: 1fr;
  }
}
</style>