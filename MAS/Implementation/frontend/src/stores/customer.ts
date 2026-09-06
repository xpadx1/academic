import { defineStore } from 'pinia'
import { ref } from 'vue'

import { config } from '@/config'
import { customerService } from '@/services/customerService'
import type { Customer } from '@/types/api'

/**
 * Current customer (demo configuration via VITE_CUSTOMER_ID).
 * The backend has no authentication — this is a single clear config point.
 */
export const useCustomerStore = defineStore('customer', () => {
  const customer = ref<Customer | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function loadCustomer(): Promise<void> {
    loading.value = true
    error.value = null
    try {
      customer.value = await customerService.getCustomer(config.customerId)
    } catch (err) {
      error.value = (err as Error).message
      customer.value = null
    } finally {
      loading.value = false
    }
  }

  function reset(): void {
    customer.value = null
    loading.value = false
    error.value = null
  }

  return { customer, loading, error, loadCustomer, reset }
})