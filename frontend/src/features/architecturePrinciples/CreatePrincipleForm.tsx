import { useState } from "react"
import {
    Alert,
    Button,
    FormControl,
    InputLabel,
    MenuItem,
    Paper,
    Select,
    TextField,
    Typography,
} from "@mui/material"
import styled from "styled-components"

import { useCreateArchitecturePrincipleMutation } from "@/api/mutations/architecture/useCreateArchitecturePrincipleMutation"
import { ArchitectureLayer, type ArchitectureTag, type CreateArchitecturePrincipleDto } from "@/models/generated"

import { layerLabels, tagLabels } from "@/models/generated/constants"

const initialForm: CreateArchitecturePrincipleDto = {
    title: "",
    rationale: "",
    layer: ArchitectureLayer.Contract,
    tags: [],
    isHighlighted: false,
    example: null,
}

const FormPanel = styled(Paper)`
    position: relative;
    display: grid;
    grid-template-columns: minmax(220px, 0.85fr) 1.15fr;
    gap: 54px;
    margin-top: 72px;
    padding: 42px;
    overflow: hidden;
    border: 1px solid #c3d4ce;
    border-radius: 3px !important;
    background: #dce9e4 !important;
    box-shadow: 0 16px 36px rgba(16, 42, 67, 0.08) !important;

    @media (max-width: 760px) {
        grid-template-columns: 1fr;
        gap: 30px;
        padding: 26px;
    }
`

const FormIntro = styled.div`
    position: relative;
    z-index: 1;
    align-self: start;
    padding-right: 16px;

    > p:last-child {
        margin-top: 18px;
        color: #526574;
        line-height: 1.6;
    }
`

const FormEyebrow = styled(Typography)`
    color: #d35445;
    font-family: "DM Mono", monospace !important;
    font-size: 0.72rem !important;
    font-weight: 500 !important;
    letter-spacing: 0.16em !important;
`

const FormTitle = styled(Typography)`
    margin: 8px 0 0 !important;
    font-size: clamp(1.8rem, 3vw, 2.8rem) !important;
    font-weight: 800 !important;
    letter-spacing: -0.04em !important;
`

const PrincipleForm = styled.form`
    position: relative;
    z-index: 1;
    display: grid;
    gap: 16px;
`

const SubmitButton = styled(Button)`
    justify-self: start;
    padding: 12px 24px !important;
    border-radius: 2px !important;
    background: #c84f41 !important;
    box-shadow: none !important;

    &:hover {
        background: #a94136 !important;
    }
`

export const CreatePrincipleForm = () => {
    const { createArchitecturePrinciple, isCreating, createError } = useCreateArchitecturePrincipleMutation()
    const [form, setForm] = useState(initialForm)

    const updateForm = <Key extends keyof CreateArchitecturePrincipleDto>(key: Key, value: CreateArchitecturePrincipleDto[Key]) => {
        setForm((current) => ({ ...current, [key]: value }))
    }

    const submit = (event: React.SyntheticEvent<HTMLFormElement>) => {
        event.preventDefault()

        createArchitecturePrinciple(form, { onSuccess: () => setForm(initialForm) })
    }

    return (
        <FormPanel as="section">
            <FormIntro>
                <FormEyebrow>ADD TO THE REGISTER</FormEyebrow>
                <FormTitle variant="h2">Capture a new principle</FormTitle>
                <Typography>Make an architectural decision visible, explain why it matters, and place it in the right layer.</Typography>
            </FormIntro>
            <PrincipleForm onSubmit={submit}>
                <TextField
                    label="Title"
                    value={form.title}
                    onChange={(event) => updateForm("title", event.target.value)}
                    required
                    fullWidth
                />
                <TextField
                    label="Rationale"
                    value={form.rationale}
                    onChange={(event) => updateForm("rationale", event.target.value)}
                    required
                    fullWidth
                    multiline
                    minRows={3}
                />
                <TextField
                    label="Example (optional)"
                    value={form.example ?? ""}
                    onChange={(event) => updateForm("example", event.target.value || null)}
                    fullWidth
                    multiline
                    minRows={3}
                />
                <FormControl fullWidth>
                    <InputLabel id="layer-label">Layer</InputLabel>
                    <Select
                        labelId="layer-label"
                        label="Layer"
                        value={form.layer}
                        onChange={(event) => updateForm("layer", Number(event.target.value) as ArchitectureLayer)}
                    >
                        {Object.entries(layerLabels).map(([value, label]) => (
                            <MenuItem key={value} value={value}>{label}</MenuItem>
                        ))}
                    </Select>
                </FormControl>
                <FormControl fullWidth>
                    <InputLabel id="tags-label">Tags (two or three)</InputLabel>
                    <Select
                        labelId="tags-label"
                        label="Tags (two or three)"
                        multiple
                        value={form.tags}
                        onChange={(event) => {
                            const value = event.target.value as ArchitectureTag[] | string
                            updateForm("tags", typeof value === "string" ? [] : value)
                        }}
                        renderValue={(tags) => tags.map((tag) => tagLabels[tag]).join(", ")}
                    >
                        {Object.entries(tagLabels).map(([value, label]) => (
                            <MenuItem key={value} value={Number(value)}>{label}</MenuItem>
                        ))}
                    </Select>
                </FormControl>
                {createError && <Alert severity="error">{createError}</Alert>}
                <SubmitButton type="submit" variant="contained" disabled={isCreating}>
                    {isCreating ? "Adding..." : "Add principle"}
                </SubmitButton>
            </PrincipleForm>
        </FormPanel>
    )
}
