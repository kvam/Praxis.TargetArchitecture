import { Button, Chip, Paper, Stack, Typography } from "@mui/material"
import styled from "styled-components"

import type { ArchitecturePrincipleDto } from "@/models/generated"

import { layerLabels, tagLabels } from "@/models/generated/constants"
import { layerAccentColors } from "./architectureLabels"

type Props = {
    principle: ArchitecturePrinciple
    onShowExample: (principle: ArchitecturePrinciple) => void
}

export type ArchitecturePrinciple = ArchitecturePrincipleDto & {
    isHighlighted: boolean
}

const Card = styled(Paper) <{ $accentColor: string }>`
    position: relative;
    display: flex;
    flex-direction: column;
    min-height: 286px;
    padding: 24px;
    border: 1px solid #d1ddd8;
    border-top: 5px solid ${({ $accentColor }) => $accentColor};
    border-radius: 3px !important;
    background: rgba(247, 250, 249, 0.94) !important;
    box-shadow: 0 10px 24px rgba(16, 42, 67, 0.06) !important;
    transition: box-shadow 180ms ease, border-color 180ms ease, background 180ms ease;

    &:hover {
        border-right-color: #8fb3aa;
        border-bottom-color: #8fb3aa;
        border-left-color: #8fb3aa;
        background: #fff !important;
        box-shadow: 0 20px 40px rgba(16, 42, 67, 0.2) !important;
    }

    @media (prefers-reduced-motion: reduce) {
        transition: none;
    }
`

const Number = styled(Typography)`
    color: #2f4a5c !important;
    font-family: "DM Mono", monospace !important;
    font-size: 1.1rem !important;
    font-weight: 700 !important;
    letter-spacing: 0.04em;
`

const LayerLabel = styled(Typography) <{ $accentColor: string }>`
    display: inline-flex;
    align-items: center;
    gap: 7px;
    color: #33505f !important;
    font-size: 0.68rem !important;
    font-weight: 800 !important;
    letter-spacing: 0.12em;
    text-transform: uppercase;

    &::before {
        width: 9px;
        height: 9px;
        border-radius: 50%;
        background: ${({ $accentColor }) => $accentColor};
        content: "";
    }
`

const KeyStar = styled.span`
    color: #ed6a5a;
    font-size: 1.3rem;
    line-height: 1;
    flex-shrink: 0;
`

const CardTitle = styled(Typography)`
    margin: 42px 0 14px !important;
    font-size: 1.18rem !important;
    font-weight: 800 !important;
    letter-spacing: -0.02em !important;
`

const Rationale = styled(Typography)`
    margin-bottom: auto !important;
    color: #526574;
    line-height: 1.65 !important;
`

const CardFooter = styled(Stack)`
    align-items: center;
    justify-content: space-between;
    gap: 10px;
    margin-top: 18px;
    padding-top: 12px;
    border-top: 1px solid #e4ebe9;
`

const TagRow = styled(Stack)`
    flex-wrap: wrap;
    gap: 2px 6px;
    min-width: 0;
`

const TagChip = styled(Chip)`
    height: 19px !important;
    padding: 0 !important;
    border-radius: 2px !important;
    background: transparent !important;
    color: #6b7f8c !important;
    cursor: pointer;
    font-size: 0.69rem !important;
    letter-spacing: 0.01em;
    text-transform: lowercase;

    &::before {
        margin-right: 1px;
        color: #b3c4c3;
        content: "#";
    }

    .MuiChip-label {
        padding-right: 0 !important;
        padding-left: 0 !important;
    }

    &:hover {
        background: transparent !important;
        color: #1f6f5c !important;
    }

    &:hover::before {
        color: #1f6f5c;
    }
`

const ExampleButton = styled(Button)`
    flex: 0 0 auto;
    min-width: 0 !important;
    padding: 0 !important;
    border-bottom: 1px solid #9ec6b9 !important;
    border-radius: 0 !important;
    background: transparent !important;
    color: #1f6f5c !important;
    font-size: 0.76rem !important;
    font-weight: 600 !important;
    letter-spacing: 0 !important;
    text-transform: none !important;
    white-space: nowrap;
    transition: color 160ms ease, border-color 160ms ease;

    &:hover {
        border-bottom-color: #17503f !important;
        background: transparent !important;
        color: #17503f !important;
    }

    @media (prefers-reduced-motion: reduce) {
        transition: none;
    }
`

export const PrincipleCard = ({ principle, onShowExample }: Props) => (
    <Card $accentColor={layerAccentColors[principle.layer]}>
        <Stack direction="row" sx={{ justifyContent: "space-between", gap: 2 }}>
            <Number>
                {String(principle.number).padStart(3, "0")}
            </Number>
            <Stack direction="row" spacing={1} sx={{ alignItems: "center", justifyContent: "flex-end", flexWrap: "wrap" }}>
                {principle.isHighlighted && <KeyStar title="An essential principle - read these first">★</KeyStar>}
                <LayerLabel $accentColor={layerAccentColors[principle.layer]}>{layerLabels[principle.layer]}</LayerLabel>
            </Stack>
        </Stack>
        <CardTitle variant="h3">{principle.title}</CardTitle>
        <Rationale>{principle.rationale}</Rationale>
        <CardFooter direction="row">
            <TagRow direction="row">
                {principle.tags.map((tag) => (
                    <TagChip
                        key={tag}
                        label={tagLabels[tag]}
                        size="small"
                    />
                ))}
            </TagRow>
            {principle.example && (
                <ExampleButton
                    size="small"
                    onClick={() => onShowExample(principle)}
                >
                    Example
                </ExampleButton>
            )}
        </CardFooter>
    </Card>
)
