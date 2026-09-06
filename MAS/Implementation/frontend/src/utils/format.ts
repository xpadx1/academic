/**
 * Formatting helpers for prices, dates, and pluralisation.
 */

/**
 * Formats a number as a price with two decimal places and a dollar sign.
 * Uses the backend value directly — no calculation happens here.
 */
export function formatPrice(value: number): string {
  if (!Number.isFinite(value)) {
    return '$0.00'
  }
  return `$${value.toFixed(2)}`
}

/**
 * Formats an ISO date/time string into a readable local format.
 */
export function formatDateTime(iso: string): string {
  const date = new Date(iso)
  if (Number.isNaN(date.getTime())) {
    return iso
  }
  return date.toLocaleString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

/**
 * Returns the singular or plural form based on a count.
 */
export function pluralize(count: number, singular: string, plural?: string): string {
  return count === 1 ? singular : plural ?? `${singular}s`
}