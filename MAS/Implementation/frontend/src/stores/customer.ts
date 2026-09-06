import { defineStore } from "pinia";
import { ref } from "vue";

import { config } from "@/config";
import { customerService } from "@/services/customerService";
import type { Customer } from "@/types/api";

export const useCustomerStore = defineStore("customer", () => {
  const customer = ref<Customer | null>(null);

  async function loadCustomer(): Promise<void> {
    customer.value = await customerService.getCustomer(config.customerId);
  }

  function reset(): void {
    customer.value = null;
  }

  return { customer, loadCustomer, reset };
});