import type {
  AddOrderItemRequest,
  CheckoutRequest,
  Order,
  OrderStatusInfo,
  PaymentMethod,
  PaymentRequest,
  PaymentResult,
  UpdateOrderItemQuantityRequest,
} from "@/types/api";
import { httpClient } from "./http";

export const orderService = {
  createOrder(customerId: number): Promise<Order> {
    return httpClient.post<Order>(`/api/customers/${customerId}/orders`);
  },

  getOrder(customerId: number, orderId: number): Promise<Order> {
    return httpClient.get<Order>(`/api/customers/${customerId}/orders/${orderId}`);
  },

  getCustomerOrders(customerId: number): Promise<Order[]> {
    return httpClient.get<Order[]>(`/api/customers/${customerId}/orders`);
  },

  addItem(customerId: number, orderId: number, request: AddOrderItemRequest): Promise<Order> {
    return httpClient.post<Order>(`/api/customers/${customerId}/orders/${orderId}/items`, request);
  },

  updateItemQuantity(
    customerId: number,
    orderId: number,
    orderItemId: number,
    request: UpdateOrderItemQuantityRequest,
  ): Promise<Order> {
    return httpClient.patch<Order>(`/api/customers/${customerId}/orders/${orderId}/items/${orderItemId}`, request);
  },

  removeItem(customerId: number, orderId: number, orderItemId: number): Promise<Order> {
    return httpClient.delete<Order>(`/api/customers/${customerId}/orders/${orderId}/items/${orderItemId}`);
  },

  checkout(customerId: number, orderId: number, request: CheckoutRequest): Promise<Order> {
    return httpClient.post<Order>(`/api/customers/${customerId}/orders/${orderId}/checkout`, request);
  },

  processPayment(customerId: number, orderId: number, request: PaymentRequest): Promise<PaymentResult> {
    return httpClient.post<PaymentResult>(
      `/api/customers/${customerId}/orders/${orderId}/payment`,
      request satisfies PaymentRequest,
    );
  },

  getOrderStatus(customerId: number, orderId: number): Promise<OrderStatusInfo> {
    return httpClient.get<OrderStatusInfo>(`/api/customers/${customerId}/orders/${orderId}/status`);
  },

  getOrderPaymentMethodLabel(method: PaymentMethod | null): string {
    return method ?? "—";
  },
};
