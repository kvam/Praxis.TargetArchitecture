import {
    Button,
    Chip,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Paper,
    Stack,
    Typography,
} from "@mui/material"
import hljs from "highlight.js/lib/common"
import styled from "styled-components"

import type { ArchitecturePrincipleDto } from "@/models/generated"

import { layerLabels, tagLabels } from "@/models/generated/constants"
import { layerAccentColors } from "./architectureLabels"
import { ExampleProse } from "./ExampleProse"
import { splitExample } from "./exampleParts"

type Props = {
    principle: ArchitecturePrincipleDto
    onClose: () => void
}

const highlightCode = (code: string, language?: string) => {
    if (language && hljs.getLanguage(language)) {
        return hljs.highlight(code, { language }).value
    }
    return hljs.highlightAuto(code).value
}

const ExampleDialog = styled(Dialog)`
    .MuiDialog-paper {
        max-height: min(88vh, 960px);
        overflow: hidden;
        border: 1px solid rgba(203, 216, 212, 0.8);
        border-radius: 8px;
        box-shadow: 0 24px 70px rgba(16, 42, 67, 0.2);
    }

    .MuiBackdrop-root {
        background: rgba(16, 42, 67, 0.42);
        backdrop-filter: blur(3px);
    }
`

const Title = styled(DialogTitle)`
    padding: 28px 32px 22px !important;
    border-bottom: 1px solid #dbe4e2;
    background: #fbfcfb;
`

const Meta = styled(Stack)`
    align-items: center;
    flex-wrap: wrap;
    gap: 8px 10px;
`

const Number = styled(Typography)`
    color: #2f4a5c !important;
    font-family: "DM Mono", monospace !important;
    font-size: 1.05rem !important;
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

const TitleText = styled(Typography)`
    margin: 18px 0 0 !important;
    color: #102a43;
    font-size: 1.65rem !important;
    line-height: 1.15 !important;
    letter-spacing: -0.03em !important;
`

const Content = styled(DialogContent)`
    padding: 24px 32px 8px !important;
    overflow-y: auto;
    overscroll-behavior: contain;
`

const Rationale = styled(Typography)`
    color: #526574;
    line-height: 1.65 !important;
`

const Code = styled(Paper) <{ $tone?: "wrong" | "right" }>`
    position: relative;
    margin: 14px 0;
    padding: ${({ $tone }) => $tone ? "30px 22px 22px" : "22px"};
    overflow-x: auto;
    border: 1px solid #dbe4e2;
    border-left: ${({ $tone }) => $tone === "wrong" ? "3px solid #c8553d" : $tone === "right" ? "3px solid #2f7d5f" : undefined};
    border-radius: 2px !important;
    background: ${({ $tone }) => $tone === "wrong" ? "#fdf5f3" : $tone === "right" ? "#f3f9f5" : "#f7faf9"} !important;
    box-shadow: none !important;
    font-family: "DM Mono", ui-monospace, monospace;
    font-size: 0.82rem;
    line-height: 1.75;
    white-space: pre-wrap;
    transition: border-color 180ms ease, box-shadow 180ms ease;

    &:hover { border-color: #b7cbc5; box-shadow: 0 8px 20px rgba(16, 42, 67, 0.05) !important; }
    &::before { display: ${({ $tone }) => $tone ? "block" : "none"}; position: absolute; top: 8px; left: 22px; color: ${({ $tone }) => $tone === "wrong" ? "#c8553d" : "#2f7d5f"}; font-family: "Manrope", sans-serif; font-size: 0.64rem; font-weight: 800; letter-spacing: 0.14em; content: "${({ $tone }) => $tone === "wrong" ? "AVOID" : "DO THIS"}"; }
    .hljs-keyword, .hljs-selector-tag, .hljs-built_in { color: #9b4dca; }
    .hljs-string, .hljs-attr, .hljs-template-variable { color: #1f7a68; }
    .hljs-title, .hljs-title.class_, .hljs-title.function_ { color: #b65345; }
    .hljs-number, .hljs-literal { color: #bd7d19; }
    .hljs-comment { color: #71858d; font-style: italic; }
`

const Actions = styled(DialogActions)`
    align-items: center;
    justify-content: space-between;
    gap: 16px;
    padding: 8px 24px 20px !important;
`

const TagRow = styled(Stack)`
    flex: 1 1 auto;
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
    font-size: 0.69rem !important;
    letter-spacing: 0.01em;
    text-transform: lowercase;
    &::before { margin-right: 1px; color: #b3c4c3; content: "#"; }
    .MuiChip-label { padding: 0 !important; }
`

const CloseButton = styled(Button)`
    padding: 8px 18px !important;
    border: 1px solid #c7d3d0 !important;
    border-radius: 2px !important;
    background: transparent !important;
    color: #526574 !important;
    font-size: 0.74rem !important;
    font-weight: 700 !important;
    letter-spacing: 0.08em;
    text-transform: uppercase;
`

export const PrincipleExampleDialog = ({ principle, onClose }: Props) => {
    const exampleParts = splitExample(principle.example ?? "")

    return (
        <ExampleDialog open onClose={onClose} maxWidth="md" fullWidth>
            <Title>
                <Meta direction="row">
                    <Number>
                        {String(principle.number).padStart(3, "0")}
                    </Number>
                    <LayerLabel $accentColor={layerAccentColors[principle.layer]}>{layerLabels[principle.layer]}</LayerLabel>
                </Meta>
                <TitleText variant="h4" as="p">{principle.title}</TitleText>
            </Title>
            <Content>
                <Rationale>{principle.rationale}</Rationale>
                {exampleParts.map((part, index) =>
                    part.type === "code" ? (
                        <Code
                            as="pre"
                            {...(part.tone ? { $tone: part.tone } : {})}
                            key={`${part.type}-${index}`}
                            dangerouslySetInnerHTML={{ __html: highlightCode(part.value, part.language) }}
                        />
                    ) : (
                        <ExampleProse key={`${part.type}-${index}`} text={part.value} />
                    ),
                )}
            </Content>
            <Actions>
                <TagRow direction="row">
                    {principle.tags.map((tag) => (
                        <TagChip key={tag} label={tagLabels[tag]} size="small" />
                    ))}
                </TagRow>
                <CloseButton onClick={onClose}>Close</CloseButton>
            </Actions>
        </ExampleDialog>
    )
}
