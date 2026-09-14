export class ApiError extends Error {
    constructor(public readonly status: number, message: string) {
        super(message)
        this.name = "ApiError"
    }
}

/**
 * The backend owns the wording, so the body's message is the message. Checking the content
 * type keeps this a plain read instead of a try/catch around a parse that may fail.
 */
const readErrorMessage = async (response: Response) => {
    const isProblem = response.headers.get("content-type")?.includes("application/json") ?? false
    const problem = isProblem ? ((await response.json()) as { message?: string }) : undefined

    return problem?.message ?? `API request failed with status ${response.status}: ${response.statusText}`
}

export const usePraxisTargetArchitectureApiClient = () => {
    const handleResponse = async <T>(response: Response): Promise<T | undefined> => {
        if (!response.ok) {
            throw new ApiError(response.status, await readErrorMessage(response))
        }

        return response.json() as Promise<T>
    }

    const get = async <T>(url: string): Promise<T> => {
        const response = await fetch(url, { method: "GET" })
        const result = await handleResponse<T>(response)

        if (result === undefined) {
            throw new Error("GET request returned no content")
        }

        return result
    }

    const send = async <T>(method: "POST" | "PUT" | "PATCH" | "DELETE", url: string, body?: unknown): Promise<T | undefined> => {
        const response = await fetch(url, {
            method,
            headers: { "Content-Type": "application/json" },
            ...(body === undefined ? {} : { body: JSON.stringify(body) }),
        })

        return handleResponse<T>(response)
    }

    const post = async <T>(url: string, body?: unknown): Promise<T> => {
        const result = await send<T>("POST", url, body)

        if (result === undefined) {
            throw new Error("POST request returned no content")
        }

        return result
    }

    const put = async <T>(url: string, body?: unknown): Promise<T> => {
        const result = await send<T>("PUT", url, body)

        if (result === undefined) {
            throw new Error("PUT request returned no content")
        }

        return result
    }

    const remove = async (url: string): Promise<void> => {
        await send<void>("DELETE", url)
    }

    return {
        get,
        post,
        put,
        remove,
    }
}
