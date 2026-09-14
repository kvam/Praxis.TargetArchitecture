import { ArchitectureLayer, ArchitectureTag } from "@/models/generated"

export const layerAccentColors: Record<ArchitectureLayer, string> = {
    [ArchitectureLayer.Backend]: "#6aa892",
    [ArchitectureLayer.Frontend]: "#ed6a5a",
    [ArchitectureLayer.Contract]: "#c99a12",
    [ArchitectureLayer.Testing]: "#7b61a8",
    [ArchitectureLayer.Process]: "#4c8dbe",
    [ArchitectureLayer.Persistence]: "#c47a3c",
    [ArchitectureLayer.Collaboration]: "#9a6fb0",
}

export const allLayersValue = "all"

export const allTagsValue = "all"

export const highlightedOnlyValue = "highlighted"

export type LayerFilter = ArchitectureLayer | typeof allLayersValue

export type TagFilter = ArchitectureTag | typeof allTagsValue

export type HighlightFilter = typeof allLayersValue | typeof highlightedOnlyValue
