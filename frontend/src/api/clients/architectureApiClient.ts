import type { ArchitectureOverviewDto, ArchitecturePrincipleDto, CreateArchitecturePrincipleDto } from "@/models/generated"

import { usePraxisTargetArchitectureApiClient } from "./praxisTargetArchitectureApiClient"

export const useArchitectureApi = () => {
    const client = usePraxisTargetArchitectureApiClient()

    return {
        getArchitectureOverview: (): Promise<ArchitectureOverviewDto> =>
            client.get<ArchitectureOverviewDto>("api/architecture/overview"),

        getArchitecturePrinciples: (): Promise<ArchitecturePrincipleDto[]> =>
            client.get<ArchitecturePrincipleDto[]>("api/architecture-principles"),

        createArchitecturePrinciple: (dto: CreateArchitecturePrincipleDto): Promise<ArchitecturePrincipleDto> =>
            client.post<ArchitecturePrincipleDto>("api/architecture-principles", dto),
    }
}
