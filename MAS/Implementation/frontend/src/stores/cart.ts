import { computed, ref } from 'vue'
import { defineStore } from 'pinia'

import { config } from '@/config'
import { orderService } from '@/services/orderService'
import type { Order } from '@/types/api'

const ORDER_ID_KEY = 'restaurant.currentOrderId'

/**
 * The customer's current order ("cart"). The orderId is persisted in
 * localStorage so a refresh keeps the same cart. All money values are
 * read from the backend response — never calculated here.
 */
export const useCartStore = defineStore('cart', () => {
  const order = ref<Order | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  const orderId = computed<number | null>(() => order.value?.id ?? null)
  const items = computed(() => order.value?.items ?? [])
  const itemCount = computed(() =>
    order.value?.items.reduce((sum, item) => sum + item.quantity, 0) ?? 0,
  )
const subtotal = computed(() => order.value?.subtotal ?? 0)
const total = computed(() => order.value?.finalPayableAmount ?? 0)
  const isEmpty = computed(() => items.value.length === 0)

  function loadPersistedOrderId(): number | null {
    const raw = localStorage.getItem(ORDER_ID_KEY)
    if (!raw) {
      return null
    }
    const id = Number.parseInt(raw, 10)
    return Number.isInteger(id) && id > 0 ? id : null
  }

  function persistOrderId(id: number | null): void {
    if (id === null) {
      localStorage.removeItem(ORDER_ID_KEY)
    } else {
      localStorage.setItem(ORDER_ID_KEY, String(id))
    }
  }

  /** Creates a fresh empty order (status Created). */
  async function createOrder(): Promise<void> {
    loading.value = true
    error.value = null
    try {
      order.value = await orderService.createOrder(config.customerId)
      persistOrderId(order.value.id)
    } catch (err) {
      error.value = (err as Error).message
    } finally {
      loading.value = false
    }
  }

  /** Restores the persisted order from the backend, or creates a new one. */
  async function ensureOrder(): Promise<void> {
    const persistedId = loadPersistedOrderId()
    if (persistedId !== null) {
      try {
        order.value = await orderService.getOrder(config.customerId, persistedId)
        // A paid/completed persisted order is no longer editable — start fresh.
        if (order.value.status !== 'Created') {
          await createOrder()
        }
        return
      } catch {
        // Persisted order no longer valid — fall through to a new one.
        persistOrderId(null)
      }
    }
    await createOrder()
  }

  async function refreshOrder(): Promise<void> {
    if (order.value === null) {
      return
    }
    order.value = await orderService.getOrder(config.customerId, order.value.id)
  }

  async function addItem(menuItemId: number, quantity: number): Promise<void> {
    loading.value = true
    error.value = null
    try {
      if (order.value === null) {
        await createOrder()
      }
      order.value = await orderService.addItem(config.customerId, order.value!.id, {
        menuItemId,
        quantity,
      })
      persistOrderId(order.value.id)
    } catch (err) {
      error.value = (err as Error).message
      throw err
    } finally {
      loading.value = false
    }
  }

  async function updateQuantity(orderItemId: number, quantity: number): Promise<void> {
    if (order.value === null) {
      return
    }
    loading.value = true
    error.value = null
    try {
      order.value = await orderService.updateItemQuantity(
        config.customerId,
        order.value.id,
        orderItemId,
        { quantity },
      )
    } catch (err) {
      error.value = (err as Error).message
      throw err
    } finally {
      loading.value = false
    }
  }

  async function removeItem(orderItemId: number): Promise<void> {
    if (order.value === null) {
      return
    }
    loading.value = true
    error.value = null
    try {
      order.value = await orderService.removeItem(
        config.customerId,
        order.value.id,
        orderItemId,
      )
    } catch (err) {
      error.value = (err as Error).message
      throw err
    } finally {
      loading.value = false
    }
  }

  /** Clears the cart after a successful order (start a new order). */
  function resetCart(): void {
    order.value = null
    persistOrderId(null)
    error.value = null
  }

  return {
    order,
    loading,
    error,
    orderId,
items,
itemCount,
subtotal,
total,
    isEmpty,
    ensureOrder,
    refreshOrder,
    addItem,
    updateQuantity,
    removeItem,
    resetCart,
  }
})