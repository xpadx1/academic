/**
 * Centralized HTTP client for the Restaurant API.
 *
 * - single source of the backend base URL (VITE_API_BASE_URL)
 * - JSON handling
 * - error normalization into a typed ApiError with user-friendly messages
 */

const BASE_URL = import.meta.env.VITE_API_BASE_URL as string | undefined

if (!BASE_URL) {
  throw new Error(
    'VITE_API_BASE_URL is not configured. Copy .env.example to .env and set the backend URL.',
  )
}

export const API_BASE_URL = BASE_URL.replace(/\/+$/, '')

export class ApiError extends Error {
  readonly status: number
  /** Machine-readable error category, useful for tests/branching. */
  readonly kind:
    | 'not-found'
    | 'conflict'
    | 'business-rule'
    | 'payment-failed'
    | 'validation'
    | 'server'
    | 'network'

  constructor(message: string, status: number, kind: ApiError['kind']) {
    super(message)
    this.name = 'ApiError'
    this.status = status
    this.kind = kind
  }
}

/** Friendly fallbacks — never expose raw stack traces. */
const NETWORK_ERROR_MESSAGE =
  'Unable to connect to the restaurant service. Please try again.'

function friendlyMessage(status: number, detail: string | undefined): string {
  if (!detail) {
    return friendlyFallback(status)
  }

  // The backend already returns customer-appropriate messages (e.g.
  // "Menu item 'X' is currently unavailable."). Use them directly.
  return detail
}

function friendlyFallback(status: number): string {
  switch (status) {
    case 400:
      return 'The request was invalid. Please review your input and try again.'
    case 404:
      return 'The requested item could not be found. It may have been removed.'
    case 409:
      return 'This action conflicts with the current state. Please refresh and try again.'
    case 422:
      return 'This action is not allowed right now.'
    case 402:
      return 'The payment could not be completed. Please try another payment method.'
    case 500:
      return 'Something went wrong on our side. Please try again.'
    default:
      return NETWORK_ERROR_MESSAGE
  }
}

interface ErrorBody {
  status?: number
  detail?: string
}

async function parseErrorBody(response: Response): Promise<ErrorBody> {
  try {
    const text = await response.text()
    if (!text) {
      return {}
    }
    return JSON.parse(text) as ErrorBody
  } catch {
    return {}
  }
}

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  let response: Response

  try {
    response = await fetch(`${API_BASE_URL}${path}`, {
      headers: {
        Accept: 'application/json',
        ...(init?.body ? { 'Content-Type': 'application/json' } : {}),
        ...init?.headers,
      },
      ...init,
    })
  } catch {
    // fetch only rejects on network-level failures
    throw new ApiError(NETWORK_ERROR_MESSAGE, 0, 'network')
  }

  if (!response.ok) {
    const body = await parseErrorBody(response)
    const message = friendlyMessage(response.status, body.detail)

    const kind: ApiError['kind'] =
      response.status === 404
        ? 'not-found'
        : response.status === 409
          ? 'conflict'
          : response.status === 422
            ? 'business-rule'
            : response.status === 402
              ? 'payment-failed'
              : response.status >= 500
                ? 'server'
                : 'validation'

    throw new ApiError(message, response.status, kind)
  }

  if (response.status === 204) {
    return undefined as T
  }

  return (await response.json()) as T
}

export const httpClient = {
  get<T>(path: string): Promise<T> {
    return request<T>(path, { method: 'GET' })
  },

  post<T>(path: string, body?: unknown): Promise<T> {
    return request<T>(path, {
      method: 'POST',
      body: body === undefined ? undefined : JSON.stringify(body),
    })
  },

  patch<T>(path: string, body: unknown): Promise<T> {
    return request<T>(path, { method: 'PATCH', body: JSON.stringify(body) })
  },

  delete<T>(path: string): Promise<T> {
    return request<T>(path, { method: 'DELETE' })
  },
}