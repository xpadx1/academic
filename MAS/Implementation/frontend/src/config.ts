/**
 * Runtime configuration points. Kept in one place — components must never
 * hard-code URLs or customer ids.
 */

const customerIdRaw = import.meta.env.VITE_CUSTOMER_ID as string | undefined
const parsedCustomerId = customerIdRaw
  ? Number.parseInt(customerIdRaw, 10)
  : Number.NaN

export const config = {
  customerId:
    Number.isInteger(parsedCustomerId) && parsedCustomerId > 0
      ? parsedCustomerId
      : 1,
} as const