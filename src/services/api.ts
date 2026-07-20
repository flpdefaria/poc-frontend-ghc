/**
 * Minimal fetch wrapper for the mocked backend under `/api` (proxied by Vite to the
 * local Azure Functions host in dev — see vite.config.ts).
 */
export class ApiError extends Error {
  constructor(
    message: string,
    public readonly status: number
  ) {
    super(message)
    this.name = 'ApiError'
  }
}

const DEFAULT_ERROR_MESSAGE = 'Something went wrong. Please try again.'

export async function postJson<TResponse>(path: string, body: unknown): Promise<TResponse> {
  let response: Response
  try {
    response = await fetch(`/api${path}`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(body)
    })
  } catch {
    throw new ApiError('Unable to reach the server. Check your connection and try again.', 0)
  }

  let data: unknown = null
  try {
    data = await response.json()
  } catch {
    // Empty or non-JSON body — fall through to status-based handling below.
  }

  if (!response.ok) {
    const message =
      data && typeof data === 'object' && 'message' in data && typeof data.message === 'string'
        ? data.message
        : DEFAULT_ERROR_MESSAGE
    throw new ApiError(message, response.status)
  }

  return data as TResponse
}
