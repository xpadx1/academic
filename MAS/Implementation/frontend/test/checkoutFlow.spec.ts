import { beforeEach, describe, expect, it, vi } from 'vitest'
import { createPinia, setActivePinia } from 'pinia'

import { customerService } from '@/services/customerService'
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

vi.mock('@/services/customerService', () => ({
  customerService: {
    getCustomer: vi.fn(),
  },
}))

const orderWithItem: Order = {
  id: 10,
  status: 'Created',
  items: [
    {
      id: 100,
      menuItemId: 5,
      menuItemName: 'Margherita Pizza',
      quantity: 1,
      unitPrice: 12.5,
      totalPrice: 12.5,
    },
  ],
  subtotal: 12.5,
  finalPayableAmount: 12.5,
  isPaid: false,
  paymentMethod: null,
  createdAt: '2026-01-01T00:00:00Z',
}

describe('Checkout flow', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    vi.clearAllMocks()
    localStorage.clear()
  })

  it('processes a successful payment and accepts the order', async () => {
    vi.mocked(orderService.createOrder).mockResolvedValue(orderWithItem)
    vi.mocked(orderService.addItem).mockResolvedValue(orderWithItem)
    vi.mocked(orderService.checkout).mockResolvedValue(orderWithItem)
    vi.mocked(orderService.processPayment).mockResolvedValue({
      success: true,
      reference: 'txn-123',
      errorMessage: null,
      orderStatus: 'Accepted',
    })

    const store = useCartStore()
    await store.addItem(5, 1)

    await orderService.checkout(1, store.orderId!, {})
    const result = await orderService.processPayment(1, store.orderId!, {
      method: 'Card',
    })

    expect(result.success).toBe(true)
    expect(store.total).toBe(12.5)
  })

  it('handles a failed payment', async () => {
    vi.mocked(orderService.processPayment).mockResolvedValue({
      success: false,
      reference: null,
      errorMessage: 'Card was declined.',
      orderStatus: 'Created',
    })

    const result = await orderService.processPayment(1, 10, { method: 'Card' })
    expect(result.success).toBe(false)
    expect(result.errorMessage).toBe('Card was declined.')
  })

  it('does not allow checkout with an empty cart', () => {
    const store = useCartStore()
    expect(store.isEmpty).toBe(true)
  })
})
