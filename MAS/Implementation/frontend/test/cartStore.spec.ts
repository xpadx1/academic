import { beforeEach, describe, expect, it, vi } from 'vitest'
import { createPinia, setActivePinia } from 'pinia'

import { orderService } from '@/services/orderService'
import { useCartStore } from '@/stores/cart'
import type { Order } from '@/types/api'

vi.mock('@/services/orderService', () => ({
  orderService: {
    createOrder: vi.fn(),
    getOrder: vi.fn(),
    addItem: vi.fn(),
    updateItemQuantity: vi.fn(),
    removeItem: vi.fn(),
    checkout: vi.fn(),
    processPayment: vi.fn(),
  },
}))

const emptyOrder: Order = {
  id: 10,
  status: 'Created',
  items: [],
  subtotal: 0,
  finalPayableAmount: 0,
  isPaid: false,
  paymentMethod: null,
  createdAt: '2026-01-01T00:00:00Z',
}

const orderWithItem: Order = {
  ...emptyOrder,
  items: [
    {
      id: 100,
      menuItemId: 5,
      menuItemName: 'Margherita Pizza',
      quantity: 2,
      unitPrice: 12.5,
      totalPrice: 25,
    },
  ],
  subtotal: 25,
  finalPayableAmount: 25,
}

describe('useCartStore', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    vi.clearAllMocks()
    localStorage.clear()
  })

  it('starts empty', () => {
    const store = useCartStore()
    expect(store.isEmpty).toBe(true)
    expect(store.itemCount).toBe(0)
  })

  it('creates an order when adding the first item', async () => {
    vi.mocked(orderService.createOrder).mockResolvedValue(emptyOrder)
    vi.mocked(orderService.addItem).mockResolvedValue(orderWithItem)

    const store = useCartStore()
    await store.addItem(5, 2)

    expect(orderService.createOrder).toHaveBeenCalledOnce()
    expect(orderService.addItem).toHaveBeenCalledOnce()
    expect(store.itemCount).toBe(2)
    expect(store.subtotal).toBe(25)
  })

  it('updates item quantity via the backend', async () => {
    vi.mocked(orderService.createOrder).mockResolvedValue(emptyOrder)
    vi.mocked(orderService.addItem).mockResolvedValue(orderWithItem)
    vi.mocked(orderService.updateItemQuantity).mockResolvedValue({
      ...orderWithItem,
      items: [{ ...orderWithItem.items![0], quantity: 3, totalPrice: 37.5 }],
      subtotal: 37.5,
      finalPayableAmount: 37.5,
    })

    const store = useCartStore()
    await store.addItem(5, 2)
    await store.updateQuantity(100, 3)

    expect(store.itemCount).toBe(3)
    expect(store.total).toBe(37.5)
  })

  it('removes an item from the cart', async () => {
    vi.mocked(orderService.createOrder).mockResolvedValue(emptyOrder)
    vi.mocked(orderService.addItem).mockResolvedValue(orderWithItem)
    vi.mocked(orderService.removeItem).mockResolvedValue(emptyOrder)

    const store = useCartStore()
    await store.addItem(5, 2)
    await store.removeItem(100)

    expect(store.isEmpty).toBe(true)
  })

  it('persists the order id in localStorage', async () => {
    vi.mocked(orderService.createOrder).mockResolvedValue(emptyOrder)
    const store = useCartStore()
    await store.ensureOrder()
    expect(localStorage.getItem('restaurant.currentOrderId')).toBe('10')
  })

  it('resets the cart after a completed order', async () => {
    vi.mocked(orderService.createOrder).mockResolvedValue(emptyOrder)
    const store = useCartStore()
    await store.ensureOrder()
    store.resetCart()
    expect(store.orderId).toBeNull()
    expect(localStorage.getItem('restaurant.currentOrderId')).toBeNull()
  })
})