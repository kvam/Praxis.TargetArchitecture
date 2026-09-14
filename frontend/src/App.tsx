import { useState } from "react"
import { Alert, Box, CircularProgress, Container, Paper, Stack, Typography } from "@mui/material"
import styled, { keyframes } from "styled-components"

import { useGetArchitecturePrinciples } from "@/api/queries/architecture/useGetArchitecturePrinciples"
import {
    type HighlightFilter,
    type LayerFilter,
    type TagFilter,
    allLayersValue,
    allTagsValue,
} from "@/features/architecturePrinciples/architectureLabels"
import { type ArchitecturePrinciple, PrincipleCard } from "@/features/architecturePrinciples/PrincipleCard"
import { layerLabels, tagLabels } from "@/models/generated/constants"
import { PrincipleExampleDialog } from "@/features/architecturePrinciples/PrincipleExampleDialog"
import { PrincipleFilters } from "@/features/architecturePrinciples/PrincipleFilters"

const markRise = keyframes`
    from {
        opacity: 0;
        transform: translateY(18px);
    }

    to {
        opacity: 1;
        transform: translateY(0);
    }
`

const AppShell = styled.div`
    min-height: 100vh;
    background: rgba(238, 241, 238, 0.86);
`

const HeroBand = styled.header`
    position: relative;
    isolation: isolate;
    overflow: hidden;
    padding: 58px 0 68px;
    border-bottom: 1px solid #2d5269;
    background: #102a43;
    color: #f7f4ee;

    &::before {
        position: absolute;
        z-index: -1;
        inset: 0;
        background: linear-gradient(115deg, rgba(16, 42, 67, 0) 35%, rgba(76, 141, 190, 0.16) 100%);
        content: "";
        pointer-events: none;
    }

    @media (max-width: 760px) {
        padding: 48px 0 58px;
    }
`

const HeroRow = styled(Stack)`
    justify-content: space-between;
    align-items: flex-start;
    gap: 24px;
`

const Eyebrow = styled(Typography)`
    font-family: "DM Mono", monospace !important;
    font-size: 0.72rem !important;
    font-weight: 500 !important;
    letter-spacing: 0.16em !important;
`

const HeroTitle = styled(Typography)`
    max-width: 700px;
    margin: 16px 0 18px !important;
    font-size: clamp(2.6rem, 5vw, 4.8rem) !important;
    font-weight: 800 !important;
    letter-spacing: -0.05em !important;
    line-height: 0.94 !important;
`

const HeroCopy = styled(Typography)`
    max-width: 590px;
    margin-bottom: 16px !important;
    color: #c8d6df;
    font-size: 1.08rem !important;
    line-height: 1.65 !important;
`

const HeroContext = styled(Typography)`
    max-width: 620px;
    margin-bottom: 10px !important;
    color: #9ec2d6;
    font-size: 0.98rem !important;
    line-height: 1.6 !important;
`

const ExpandToggle = styled.button`
    margin: 12px 0 0 0;
    padding: 0;
    border: none;
    background: none;
    color: #9ec2d6;
    font-size: 0.98rem;
    text-decoration: underline;
    cursor: pointer;
    transition: color 160ms ease;

    &:hover {
        color: #c8d6df;
    }
`

const HeroMark = styled.div`
    display: grid;
    grid-template-columns: repeat(3, 38px);
    gap: 8px;
    padding-top: 14px;

    > div {
        height: 112px;
        border: 1px solid #a9d6c7;
        animation: ${markRise} 700ms cubic-bezier(0.2, 0.8, 0.2, 1) both;
    }

    > div:nth-child(2) {
        margin-top: 28px;
        border-color: #ed6a5a;
        background: #ed6a5a;
        animation-delay: 100ms;
    }

    > div:nth-child(3) {
        margin-top: 56px;
        animation-delay: 200ms;
    }

    @media (max-width: 760px) {
        display: none;
    }
`

const ContentWrap = styled(Container)`
    padding-top: 48px;
    padding-bottom: 100px;
`

const SectionHeading = styled.section`
    position: relative;
    display: flex;
    align-items: end;
    justify-content: space-between;
    gap: 30px;
    margin-bottom: 22px;
    padding-bottom: 22px;
    border-bottom: 1px solid #cbd8d4;

    &::after {
        position: absolute;
        bottom: -2px;
        left: 0;
        width: 72px;
        height: 3px;
        background: #ed6a5a;
        content: "";
    }

    @media (max-width: 760px) {
        flex-direction: column;
        align-items: flex-start;
    }
`

const SectionEyebrow = styled(Eyebrow)`
    color: #d35445;
`

const SectionTitle = styled(Typography)`
    margin: 8px 0 0 !important;
    font-size: clamp(1.8rem, 3vw, 2.8rem) !important;
    font-weight: 800 !important;
    letter-spacing: -0.04em !important;
`

const LoadingState = styled.div`
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 12px;
    min-height: 180px;
`

const EmptyState = styled(Paper)`
    padding: 44px !important;
    border: 1px dashed #aebfba;
    border-radius: 3px !important;
    background: rgba(247, 250, 249, 0.72) !important;
    text-align: center;
`

const PrinciplesGrid = styled.div`
    display: grid;
    grid-template-columns: repeat(5, minmax(280px, 1fr));
    gap: 18px;
`

const App = () => {
    const { data: principles, isLoading, isError, error } = useGetArchitecturePrinciples()
    const [search, setSearch] = useState("")
    const [layerFilter, setLayerFilter] = useState<LayerFilter>(allLayersValue)
    const [tagFilter, setTagFilter] = useState<TagFilter>(allTagsValue)
    const [highlightFilter, setHighlightFilter] = useState<HighlightFilter>(allLayersValue)
    const [examplePrinciple, setExamplePrinciple] = useState<ArchitecturePrinciple>()
    const [showAbout, setShowAbout] = useState(false)
    const hasFilter = search !== "" || layerFilter !== allLayersValue || tagFilter !== allTagsValue || highlightFilter !== allLayersValue
    const principlesWithHighlights = (principles ?? []) as ArchitecturePrinciple[]
    const filteredPrinciples = principlesWithHighlights.filter((principle) => {
        const tagText = principle.tags.map((tag) => tagLabels[tag]).join(" ")
        const searchableText = `${principle.title} ${principle.rationale} ${layerLabels[principle.layer]} ${tagText}`.toLowerCase()
        const matchesSearch = searchableText.includes(search.trim().toLowerCase())
        const matchesLayer = layerFilter === allLayersValue || principle.layer === layerFilter
        const matchesTag = tagFilter === allTagsValue || principle.tags.includes(tagFilter)
        const matchesHighlight = highlightFilter === allLayersValue || principle.isHighlighted
        return matchesSearch && matchesLayer && matchesTag && matchesHighlight
    })
    return (
        <AppShell>
            <HeroBand>
                <Container maxWidth="xl">
                    <HeroRow direction="row">
                        <Box>
                            <Eyebrow>ARCHITECTURE REGISTER</Eyebrow>
                            <HeroTitle variant="h1">Target architecture</HeroTitle>
                            <HeroContext>
                                Best suited for .NET backends with TypeScript + React frontends, especially REST API systems that are mostly CRUD.
                            </HeroContext>
                            <HeroCopy>
                                These are principles I have tested and found to be reasonable and durable over the last 12–14 years. They were refined across dozens of systems, from small internal tools to teams of fifteen people shipping every day.
                            </HeroCopy>
                            {showAbout && (
                                <>
                                    <HeroCopy>
                                        The goal is readability and simplicity: anyone with a year of experience should be able to work in this system without a tour. Every other principle here exists to serve that. We prefer boring, obvious code over clever patterns. Duplication is acceptable when it keeps code close to the work it does.
                                    </HeroCopy>
                                    <HeroCopy>
                                        This register documents the architecture, the technology choices, the git practices, and the ways teams stay small and move fast. It is the north star: when opinions diverge, we come back to these principles to find our way.
                                    </HeroCopy>
                                    <HeroCopy>
                                        That said, these are guidelines, not commandments. You are welcome to ignore them, criticise them, ridicule them, adapt them, or change them entirely. What has worked well in my journey may not be a good fit for yours. The best system is one you and your team have thought through together and chosen because it makes sense for your constraints and your people, not one you inherited and have to defend.
                                    </HeroCopy>
                                </>
                            )}
                            <ExpandToggle onClick={() => setShowAbout(!showAbout)}>
                                {showAbout ? "Hide details" : "Read more"}
                            </ExpandToggle>
                        </Box>
                        <HeroMark aria-hidden="true"><div /><div /><div /></HeroMark>
                    </HeroRow>
                </Container>
            </HeroBand>
            <ContentWrap maxWidth="xl">
                <SectionHeading>
                    <Box>
                        <SectionEyebrow>GUIDING PRINCIPLES</SectionEyebrow>
                        <SectionTitle variant="h2">The decisions behind the system</SectionTitle>
                    </Box>
                    <PrincipleFilters
                        search={search}
                        onSearchChange={setSearch}
                        layerFilter={layerFilter}
                        onLayerFilterChange={setLayerFilter}
                        tagFilter={tagFilter}
                        onTagFilterChange={setTagFilter}
                        highlightFilter={highlightFilter}
                        onHighlightFilterChange={setHighlightFilter}
                        shownCount={filteredPrinciples.length}
                    />
                </SectionHeading>
                {isLoading && (
                    <LoadingState>
                        <CircularProgress size={28} />
                        <Typography>Loading principles...</Typography>
                    </LoadingState>
                )}
                {isError && <Alert severity="error">Could not load principles: {error.message}</Alert>}
                {!isLoading && !isError && filteredPrinciples.length === 0 && (
                    <EmptyState>
                        <Typography variant="h4">
                            {hasFilter ? "No matching principles" : "No principles yet"}
                        </Typography>
                        <Typography>
                            {hasFilter
                                ? "Try a different search term, layer, or tag."
                                : "Use the form below to add the first decision to the register."}
                        </Typography>
                    </EmptyState>
                )}
                <PrinciplesGrid>
                    {filteredPrinciples.map((principle) => (
                        <PrincipleCard
                            key={principle.architecturePrincipleId}
                            principle={principle}
                            onShowExample={setExamplePrinciple}
                        />
                    ))}
                </PrinciplesGrid>
                {examplePrinciple && (
                    <PrincipleExampleDialog
                        principle={examplePrinciple}
                        onClose={() => setExamplePrinciple(undefined)}
                    />
                )}
            </ContentWrap>
        </AppShell>
    )
}
export default App
