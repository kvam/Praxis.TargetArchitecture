import { Link, Tooltip, Typography } from "@mui/material"
import styled from "styled-components"

type Props = {
    text: string
}

const inlineCodePattern = /`([^`]+)`/g

type GlossaryEntry = {
    label: string
    href: string
    description: string
}

const glossaryEntries: GlossaryEntry[] = [
    {
        label: "vertical slice",
        href: "https://learn.microsoft.com/azure/architecture/patterns/vertical-slice-architecture",
        description: "A feature-oriented structure where one slice owns its endpoint, handler logic, and DTOs instead of splitting code by technical layer.",
    },
    {
        label: "vertical slices",
        href: "https://learn.microsoft.com/azure/architecture/patterns/vertical-slice-architecture",
        description: "A feature-oriented structure where each slice owns its own endpoint, logic, and contract types.",
    },
    {
        label: "dto",
        href: "https://en.wikipedia.org/wiki/Data_transfer_object",
        description: "A data transfer object: a type used to carry data across a boundary such as an API response or request.",
    },
    {
        label: "openapi",
        href: "https://www.openapis.org/what-is-openapi",
        description: "A standard format for describing HTTP APIs so tools can generate clients, documentation, and validation from one contract.",
    },
    {
        label: "inbox and outbox patterns",
        href: "https://microservices.io/patterns/data/transactional-outbox.html",
        description: "Patterns for reliably storing outgoing and incoming integration messages so delivery can be retried separately from the main application flow.",
    },
    {
        label: "outbox patterns",
        href: "https://microservices.io/patterns/data/transactional-outbox.html",
        description: "A pattern where outgoing messages are stored first, then published reliably outside the main request.",
    },
    {
        label: "inbox/outbox patterns",
        href: "https://microservices.io/patterns/data/transactional-outbox.html",
        description: "Patterns for handling asynchronous integration messages through persisted inbox and outbox records.",
    },
    {
        label: "lazy loading",
        href: "https://learn.microsoft.com/ef/core/querying/related-data/lazy",
        description: "A data-access feature that loads related data when a property is accessed, which can hide extra database queries.",
    },
    {
        label: "n+1",
        href: "https://securelist.com/n1-query-problem/",
        description: "A query pattern where one initial query triggers many more queries inside a loop, often by accident.",
    },
    {
        label: "openapi schema transformer",
        href: "https://learn.microsoft.com/aspnet/core/fundamentals/openapi/customize-openapi",
        description: "Customization that changes the generated OpenAPI document so downstream tools get better contract metadata.",
    },
    {
        label: "idempotent",
        href: "https://en.wikipedia.org/wiki/Idempotence",
        description: "An operation that can run more than once with the same input without changing the end result after the first successful run.",
    },
    {
        label: "projection",
        href: "https://learn.microsoft.com/dotnet/csharp/linq/standard-query-operators/projection-operations",
        description: "A query step that selects only the fields needed and shapes them into a new result type.",
    },
    {
        label: "asnotracking",
        href: "https://learn.microsoft.com/ef/core/querying/tracking",
        description: "An Entity Framework query mode that skips change tracking for read-only queries.",
    },
    {
        label: "unit of work",
        href: "https://martinfowler.com/eaaCatalog/unitOfWork.html",
        description: "A pattern where related data changes are tracked and committed together as one logical operation.",
    },
    {
        label: "repository",
        href: "https://martinfowler.com/eaaCatalog/repository.html",
        description: "A pattern that presents data access like a collection, often used to hide storage details behind query methods.",
    },
    {
        label: "middleware",
        href: "https://learn.microsoft.com/aspnet/core/fundamentals/middleware/",
        description: "ASP.NET request pipeline components that run around a request and response to apply cross-cutting behavior.",
    },
    {
        label: "contract",
        href: "https://martinfowler.com/bliki/PublishedInterface.html",
        description: "The published shape and behavior another system depends on, such as an API route, DTO, or enum value.",
    },
    {
        label: "polling",
        href: "https://en.wikipedia.org/wiki/Polling_(computer_science)",
        description: "Repeatedly checking whether another system has new work or a new state instead of waiting for it to push an event.",
    },
    {
        label: "retry",
        href: "https://learn.microsoft.com/azure/architecture/patterns/retry",
        description: "Repeating a failed operation after a delay when the failure might be temporary.",
    },
    {
        label: "timeouts",
        href: "https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken",
        description: "Limits on how long an operation is allowed to run before it is cancelled or treated as failed.",
    },
    {
        label: "change tracker",
        href: "https://learn.microsoft.com/ef/core/change-tracking/",
        description: "Entity Framework’s in-memory record of loaded entities and the changes that should be saved.",
    },
    {
        label: "transaction",
        href: "https://learn.microsoft.com/ef/core/saving/transactions",
        description: "A database boundary that applies a group of changes together or rolls all of them back.",
    },
    {
        label: "migration",
        href: "https://learn.microsoft.com/ef/core/managing-schemas/migrations/",
        description: "A versioned database schema change captured in code and applied in sequence.",
    },
    {
        label: "query cache",
        href: "https://tanstack.com/query/latest/docs/framework/react/overview",
        description: "The client-side store where React Query keeps fetched server data and its freshness state.",
    },
    {
        label: "mutation",
        href: "https://tanstack.com/query/latest/docs/framework/react/guides/mutations",
        description: "A client-side operation that changes server state, such as creating, updating, or deleting data.",
    },
]

const Text = styled(Typography)`
    margin: 16px 0 !important;
    color: #46596a !important;
    line-height: 1.7 !important;
`

const InlineCode = styled.code`
    padding: 1px 5px;
    border: 1px solid rgba(203, 216, 212, 0.9);
    border-radius: 4px;
    background: #f4f7f6;
    color: #2f4858;
    font-family: "JetBrains Mono", ui-monospace, monospace;
    font-size: 0.84em;
`

const GlossaryLink = styled(Link)`
    color: #215d83 !important;
    text-decoration-color: rgba(33, 93, 131, 0.35) !important;
    text-underline-offset: 2px;
`

const splitGlossaryTerms = (text: string) => {
    const escapedLabels = glossaryEntries.map((entry) => entry.label.replace(/[.*+?^${}()|[\]\\]/g, "\\$&"))
    const glossaryPattern = new RegExp(`(${escapedLabels.join("|")})`, "gi")

    return text.split(glossaryPattern).filter((segment) => segment !== "")
}

const findGlossaryEntry = (segment: string) =>
    glossaryEntries.find((entry) => entry.label.toLowerCase() === segment.toLowerCase())

export const ExampleProse = ({ text }: Props) => {
    const segments = text.split(inlineCodePattern)

    return (
        <Text as="div">
            {segments.map((segment, index) =>
                index % 2 === 1 ? (
                    <InlineCode key={index}>
                        {segment}
                    </InlineCode>
                ) : (
                    splitGlossaryTerms(segment).map((part, partIndex) => {
                        const glossaryEntry = findGlossaryEntry(part)

                        if (!glossaryEntry) {
                            return <span key={`${index}-${partIndex}`}>{part}</span>
                        }

                        return (
                            <Tooltip key={`${index}-${partIndex}`} title={glossaryEntry.description} arrow>
                                <GlossaryLink
                                    href={glossaryEntry.href}
                                    target="_blank"
                                    rel="noreferrer"
                                >
                                    {part}
                                </GlossaryLink>
                            </Tooltip>
                        )
                    })
                ),
            )}
        </Text>
    )
}
