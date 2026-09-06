import type { Customer } from '@/types/api'
import { httpClient } from './http'

/**
 * Customer endpoints. The backend has no authentication — the customer is
 * configured via VITE_CUSTOMER_ID (single clear configuration point).
 */
export const customerService = {
  getCustomer(customerId: number): Promise<Customer> {
    return httpClient.get<Customer>(`/api/customers/${customerId}`)
  },
}
