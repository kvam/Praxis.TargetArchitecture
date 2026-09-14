export type ExampleTone = "wrong" | "right"

export type ExamplePart =
    | { type: "text"; value: string }
    | { type: "code"; value: string; language?: string; tone?: ExampleTone }

const dedent = (code: string) => {
    const lines = code.split("\n")
    const indents = lines
        .filter((line) => line.trim() !== "")
        .map((line) => line.match(/^ */)?.[0].length ?? 0)

    const common = indents.length > 0 ? Math.min(...indents) : 0

    return lines.map((line) => line.slice(common)).join("\n").trim()
}

const isIndented = (line: string) => /^ {4}/.test(line)

const tonePattern = /^\s*\/\/ (WRONG|RIGHT)\s*\n/

/** The seed marks a sample as the no-go or the good path; the marker itself is not code. */
const readTone = (code: string): { value: string; tone?: ExampleTone } => {
    const match = code.match(tonePattern)

    if (!match) return { value: code }

    return {
        value: code.replace(tonePattern, ""),
        tone: match[1] === "WRONG" ? "wrong" : "right",
    }
}

/**
 * A blank line inside one sample must not break it into two boxes. Only the first chunk
 * carries the marker, so an unmarked chunk continues whatever came before it. Chunks are
 * still raw here: dedenting each one separately would flatten the indentation of a chunk
 * that sits entirely inside a method body.
 */
const mergeAdjacentCode = (parts: ExamplePart[]): ExamplePart[] =>
    parts.reduce<ExamplePart[]>((merged, part) => {
        const previous = merged.at(-1)

        if (
            part.type === "code" &&
            previous?.type === "code" &&
            (part.tone === undefined || part.tone === previous.tone)
        ) {
            previous.value = `${previous.value}\n\n${part.value}`
            return merged
        }

        return [...merged, part]
    }, [])

/**
 * Seeded examples put prose at the margin and indent code by four spaces. Splitting on that
 * is exact, where guessing from punctuation is not: a sentence ending in a semicolon is
 * still a sentence.
 */
const splitByIndentation = (example: string): ExamplePart[] =>
    example
        .split(/\n[ \t]*\n/)
        .filter((block) => block.trim() !== "")
        .map((block) => {
            const lines = block.split("\n").filter((line) => line.trim() !== "")

            if (!lines.every(isIndented)) {
                return {
                    type: "text" as const,
                    value: lines.map((line) => line.trim()).join(" "),
                }
            }

            const { value, tone } = readTone(block)

            return tone
                ? { type: "code" as const, value, tone }
                : { type: "code" as const, value }
        })

/**
 * Fenced blocks win where an example uses them; everything else falls back to indentation.
 */
export const splitExample = (example: string): ExamplePart[] => {
    const parts: ExamplePart[] = []
    const fencePattern = /```([\w-]*)\r?\n([\s\S]*?)```/g
    let lastIndex = 0

    for (const match of example.matchAll(fencePattern)) {
        const matchIndex = match.index ?? 0

        if (matchIndex > lastIndex) {
            parts.push(...splitByIndentation(example.slice(lastIndex, matchIndex)))
        }

        const language = match[1]
        parts.push(
            language
                ? { type: "code", value: dedent(match[2] ?? ""), language }
                : { type: "code", value: dedent(match[2] ?? "") },
        )

        lastIndex = matchIndex + match[0].length
    }

    if (lastIndex < example.length) {
        parts.push(...splitByIndentation(example.slice(lastIndex)))
    }

    return mergeAdjacentCode(parts).map((part) =>
        part.type === "code" ? { ...part, value: dedent(part.value) } : part,
    )
}
