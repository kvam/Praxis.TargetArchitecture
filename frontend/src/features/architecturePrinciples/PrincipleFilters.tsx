import { Chip, Collapse, Divider, Stack, TextField, Typography } from "@mui/material"
import { useState } from "react"
import styled from "styled-components"
import { ArchitectureLayer, ArchitectureTag } from "@/models/generated"
import { layerLabels, tagLabels } from "@/models/generated/constants"

import {
    type HighlightFilter,
    type LayerFilter,
    type TagFilter,
    allLayersValue,
    allTagsValue,
    highlightedOnlyValue,
} from "./architectureLabels"

type Props = {
    search: string
    onSearchChange: (search: string) => void
    layerFilter: LayerFilter
    onLayerFilterChange: (layerFilter: LayerFilter) => void
    tagFilter: TagFilter
    onTagFilterChange: (tagFilter: TagFilter) => void
    highlightFilter: HighlightFilter
    onHighlightFilterChange: (highlightFilter: HighlightFilter) => void
    shownCount: number
}

const Filters = styled(Stack)`
    width: min(100%, 765px);
    padding: 14px;
    border: 1px solid #cbd8d4;
    background: rgba(247, 250, 249, 0.72);
    box-shadow: 0 8px 20px rgba(16, 42, 67, 0.05);

    .MuiOutlinedInput-root {
        background: #f7faf9;
    }
`

const Toolbar = styled(Stack)`
    align-items: center;
    justify-content: space-between;
    gap: 10px;

    @media (max-width: 760px) {
        align-items: stretch;
    }
`

const SearchField = styled(TextField)`
    min-width: 0;
    flex: 0 1 340px;

    .MuiInputBase-root {
        height: 40px;
    }
`

const ToolbarActions = styled(Stack)`
    align-items: center;
    justify-content: flex-end;
    flex-wrap: wrap;
    gap: 8px;

    @media (max-width: 760px) {
        justify-content: flex-start;
    }
`

const CountChip = styled(Chip)`
    background: #dce8e4 !important;
    color: #285b51 !important;
    font-family: "DM Mono", monospace !important;
`

// Weight stays constant so a pill is the same width selected or not; selection reads from
// the border, background, and text colour instead.
const FilterChip = styled(Chip) <{ $selected?: boolean }>`
    border: 1px solid ${({ $selected }) => ($selected ? "#2f6f60" : "#c7d6d1")} !important;
    border-radius: 999px !important;
    background: ${({ $selected }) => ($selected ? "#dce8e4" : "#f7faf9")} !important;
    color: ${({ $selected }) => ($selected ? "#1f5a4e" : "#526574")} !important;
    font-weight: 600 !important;
`

const Groups = styled.div`
    display: grid;
    gap: 12px;
    padding-top: 4px;
`

const Group = styled.div`
    display: grid;
    gap: 8px;
`

const GroupLabel = styled(Typography)`
    color: #526574;
    font-size: 0.72rem !important;
    font-weight: 700 !important;
    letter-spacing: 0.08em !important;
    text-transform: uppercase;
`

const ChipRow = styled(Stack)`
    align-items: center;
    flex-wrap: wrap;
    gap: 8px;
`

const activeFilterCount = (
    search: string,
    layerFilter: LayerFilter,
    tagFilter: TagFilter,
    highlightFilter: HighlightFilter,
) => {
    var count = 0

    if (search.trim() !== "")
    {
        count += 1
    }

    if (layerFilter !== allLayersValue)
    {
        count += 1
    }

    if (tagFilter !== allTagsValue)
    {
        count += 1
    }

    if (highlightFilter !== allLayersValue)
    {
        count += 1
    }

    return count
}

export const PrincipleFilters = ({
    search,
    onSearchChange,
    layerFilter,
    onLayerFilterChange,
    tagFilter,
    onTagFilterChange,
    highlightFilter,
    onHighlightFilterChange,
    shownCount,
}: Props) => {
    const [showFilters, setShowFilters] = useState(false)
    const selectedFilterCount = activeFilterCount(search, layerFilter, tagFilter, highlightFilter)
    const hasActiveFilters = selectedFilterCount > 0

    return (
        <Filters>
            <Toolbar direction="row">
                <SearchField
                    label="Search principles"
                    value={search}
                    onChange={(event) => onSearchChange(event.target.value)}
                    size="small"
                />
                <ToolbarActions direction="row">
                    <FilterChip
                        label={showFilters ? "Hide filters" : hasActiveFilters ? `Filters (${selectedFilterCount})` : "Show filters"}
                        clickable
                        $selected={showFilters || hasActiveFilters}
                        onClick={() => setShowFilters((current) => !current)}
                    />
                    {hasActiveFilters && (
                        <FilterChip
                            label="Clear"
                            clickable
                            onClick={() => {
                                onSearchChange("")
                                onLayerFilterChange(allLayersValue)
                                onTagFilterChange(allTagsValue)
                                onHighlightFilterChange(allLayersValue)
                            }}
                        />
                    )}
                    <CountChip label={`${shownCount} shown`} />
                </ToolbarActions>
            </Toolbar>
            {showFilters && <Divider sx={{ margin: "8px 0" }} />}
            <Collapse in={showFilters}>
                <Groups>
                    <Group>
                        <GroupLabel>Where to start</GroupLabel>
                        <ChipRow direction="row">
                            <FilterChip
                                label="All principles"
                                clickable
                                $selected={highlightFilter === allLayersValue}
                                onClick={() => onHighlightFilterChange(allLayersValue)}
                            />
                            <FilterChip
                                label="★ Essentials only"
                                clickable
                                title="The starred principles that introduce the standard - read these first"
                                $selected={highlightFilter === highlightedOnlyValue}
                                onClick={() => onHighlightFilterChange(highlightedOnlyValue)}
                            />
                        </ChipRow>
                    </Group>
                    <Group>
                        <GroupLabel>Layer</GroupLabel>
                        <ChipRow direction="row">
                            <FilterChip
                                label="All"
                                clickable
                                $selected={layerFilter === allLayersValue}
                                onClick={() => onLayerFilterChange(allLayersValue)}
                            />
                            {Object.entries(layerLabels).map(([value, label]) => (
                                <FilterChip
                                    key={value}
                                    label={label}
                                    clickable
                                    $selected={layerFilter === Number(value)}
                                    onClick={() => onLayerFilterChange(
                                        layerFilter === Number(value)
                                            ? allLayersValue
                                            : Number(value) as ArchitectureLayer,
                                    )}
                                />
                            ))}
                        </ChipRow>
                    </Group>
                    <Group>
                        <GroupLabel>Tag</GroupLabel>
                        <ChipRow direction="row">
                            <FilterChip
                                label="All"
                                clickable
                                $selected={tagFilter === allTagsValue}
                                onClick={() => onTagFilterChange(allTagsValue)}
                            />
                            {Object.entries(tagLabels).map(([value, label]) => (
                                <FilterChip
                                    key={value}
                                    label={`#${label}`}
                                    clickable
                                    $selected={tagFilter === Number(value)}
                                    onClick={() => onTagFilterChange(
                                        tagFilter === Number(value)
                                            ? allTagsValue
                                            : Number(value) as ArchitectureTag,
                                    )}
                                />
                            ))}
                        </ChipRow>
                    </Group>
                </Groups>
            </Collapse>
        </Filters>
    )
}
