import type {
  AddOrderItemRequest,
  CheckoutRequest,
  Order,
  OrderStatusInfo,
  PaymentMethod,
  PaymentRequest,
  PaymentResult,
  UpdateOrderItemQuantityRequest,
} from '@/types/api'
import { httpClient } from './http'

/**
 * Order/cart endpoints. All money values (subtotal, final payable amount)
 * are returned by the backend and displayed as-is.
 * The backend creates/accepts the order only after payment.
 */
export const orderService = {
  /** Creates an empty order in status Created (the "cart"). */
  createOrder(customerId: number): Promise<Order> {
    return httpClient.post<Order>(`/api/customers/${customerId}/orders`)
  },

  getOrder(customerId: number, orderId: number): Promise<Order> {
    return httpClient.get<Order>(
      `/api/customers/${customerId}/orders/${orderId}`,
    )
  },

  getCustomerOrders(customerId: number): Promise<Order[]> {
    return httpClient.get<Order[]>(`/api/customers/${customerId}/orders`)
  },

  addItem(
    customerId: number,
    orderId: number,
    request: AddOrderItemRequest,
  ): Promise<Order> {
    return httpClient.post<Order>(
      `/api/customers/${customerId}/orders/${orderId}/items`,
      request,
    )
  },

  updateItemQuantity(
    customerId: number,
    orderId: number,
    orderItemId: number,
    request: UpdateOrderItemQuantityRequest,
  ): Promise<Order> {
    return httpClient.patch<Order>(
      `/api/customers/${customerId}/orders/${orderId}/items/${orderItemId}`,
      request,
    )
  },

  removeItem(
    customerId: number,
    orderId: number,
    orderItemId: number,
  ): Promise<Order> {
    return httpClient.delete<Order>(
      `/api/customers/${customerId}/orders/${orderId}/items/${orderItemId}`,
    )
  },

/**
   * Validates the order (items available, quantities valid) before payment.
   * Returns the refreshed order.
   */
  checkout(
    customerId: number,
    orderId: number,
    request: CheckoutRequest,
  ): Promise<Order> {
    return httpClient.post<Order>(
      `/api/customers/${customerId}/orders/${orderId}/checkout`,
      request,
    )
  },

  /**
   * Processes payment. On success the backend records the payment, moves the
   * order to Accepted and returns the payment result. On failure the API
   * responds 402 with an error detail.
   */
  processPayment(
    customerId: number,
    orderId: number,
    request: PaymentRequest,
  ): Promise<PaymentResult> {
    return httpClient.post<PaymentResult>(
      `/api/customers/${customerId}/orders/${orderId}/payment`,
      request satisfies PaymentRequest,
    )
  },

  getOrderStatus(
    customerId: number,
    orderId: number,
  ): Promise<OrderStatusInfo> {
    return httpClient.get<OrderStatusInfo>(
      `/api/customers/${customerId}/orders/${orderId}/status`,
    )
  },

  getOrderPaymentMethodLabel(method: PaymentMethod | null): string {
    return method ?? '—'
  },
}