import type { UseQueryResult } from "@tanstack/react-query"
import { useQuery } from "@tanstack/react-query"

import { useArchitectureApi } from "@/api/clients/architectureApiClient"
import type { ArchitecturePrincipleDto } from "@/models/generated"

import { QueryKeys, defaultQueryConfig } from "../queryConfig"

export const useGetArchitecturePrinciples = (): UseQueryResult<ArchitecturePrincipleDto[], Error> => {
    const client = useArchitectureApi()

    return useQuery({
        queryKey: [QueryKeys.ArchitecturePrinciples],
        queryFn: () => client.getArchitecturePrinciples(),
        ...defaultQueryConfig<ArchitecturePrincipleDto[]>(),
    })
}
