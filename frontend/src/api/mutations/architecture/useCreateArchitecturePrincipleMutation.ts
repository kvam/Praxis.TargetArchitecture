import { useMutation, useQueryClient } from "@tanstack/react-query"

import { useArchitectureApi } from "@/api/clients/architectureApiClient"
import { QueryKeys } from "@/api/queries/queryConfig"
import type { CreateArchitecturePrincipleDto } from "@/models/generated"

export const useCreateArchitecturePrincipleMutation = () => {
    const client = useArchitectureApi()
    const queryClient = useQueryClient()

    const { mutate, isPending, error } = useMutation({
        mutationFn: (dto: CreateArchitecturePrincipleDto) => client.createArchitecturePrinciple(dto),
        onSuccess: () => {
            void queryClient.invalidateQueries({ queryKey: [QueryKeys.ArchitecturePrinciples] })
        },
    })

    return {
        createArchitecturePrinciple: mutate,
        isCreating: isPending,
        createError: error?.message,
    }
}
