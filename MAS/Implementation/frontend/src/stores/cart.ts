import { computed, ref } from "vue";
import { defineStore } from "pinia";

import { config } from "@/config";
import { orderService } from "@/services/orderService";
import type { Order } from "@/types/api";

export const useCartStore = defineStore("cart", () => {
  const order = ref<Order | null>(null);

  const orderId = computed<number | null>(() => order.value?.id ?? null);
  const items = computed(() => order.value?.items ?? []);
  const itemCount = computed(() => order.value?.items.reduce((sum, item) => sum + item.quantity, 0) ?? 0);
  const subtotal = computed(() => order.value?.subtotal ?? 0);
  const total = computed(() => order.value?.finalPayableAmount ?? 0);
  const isEmpty = computed(() => items.value.length === 0);

  async function createOrder(): Promise<void> {
    order.value = await orderService.createOrder(config.customerId);
  }

  async function ensureOrder(): Promise<void> {
    if (order.value !== null) {
      return;
    }
    await createOrder();
  }

  async function refreshOrder(): Promise<void> {
    if (order.value === null) {
      return;
    }
    order.value = await orderService.getOrder(config.customerId, order.value.id);
  }

  async function addItem(menuItemId: number, quantity: number): Promise<void> {
    if (order.value === null) {
      await createOrder();
    }
    order.value = await orderService.addItem(config.customerId, order.value!.id, {
      menuItemId,
      quantity,
    });
  }

  async function updateQuantity(orderItemId: number, quantity: number): Promise<void> {
    if (order.value === null) {
      return;
    }
    order.value = await orderService.updateItemQuantity(config.customerId, order.value.id, orderItemId, { quantity });
  }

  async function removeItem(orderItemId: number): Promise<void> {
    if (order.value === null) {
      return;
    }
    order.value = await orderService.removeItem(config.customerId, order.value.id, orderItemId);
  }

  function resetCart(): void {
    order.value = null;
  }

  return {
    order,
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
  };
});