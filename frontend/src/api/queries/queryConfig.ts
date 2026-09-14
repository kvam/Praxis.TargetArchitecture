import type { UseQueryOptions } from "@tanstack/react-query"

export const defaultQueryConfig = <T>(): Partial<UseQueryOptions<T>> => ({
    staleTime: 3 * 60 * 1000,
    refetchOnWindowFocus: true,
    refetchOnMount: true,
    refetchOnReconnect: true,
})

export enum QueryKeys {
    ArchitectureOverview = "architectureOverview",
    ArchitecturePrinciples = "architecturePrinciples",
}
