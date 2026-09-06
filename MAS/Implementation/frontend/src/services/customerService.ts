import type { Customer } from "@/types/api";
import { httpClient } from "./http";

export const customerService = {
  getCustomer(customerId: number): Promise<Customer> {
    return httpClient.get<Customer>(`/api/customers/${customerId}`);
  },
};
