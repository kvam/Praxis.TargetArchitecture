namespace Praxis.TargetArchitecture.Data.Seed.Examples;

public static class HookExamples
{
    public const string OneHookPerEndpoint = """
            The endpoint and the hook are one to one:

                // RIGHT
                export const useArchitecturePrinciples = () =>
                    useQuery({
                        queryKey: queryKeys.architecturePrinciples,
                        queryFn: () => getArchitecturePrinciples(),
                    })

            Components call the hook and never touch the client, the query key, or the
            cache directly. When the endpoint changes, exactly one file changes with it.

            A single `useApi()` that takes a URL saves a few files and gives up the thing
            that mattered: a named, typed, findable place per endpoint. Search for the
            hook and you have found every screen that depends on that call.
            """;

    public const string AFailedMutationIsReadNotCaught = """
            Catching by hand duplicates state the mutation already keeps:

                // WRONG
                const [formError, setFormError] = useState<string>()

                try {
                    await createArchitecturePrinciple(form)
                    setForm(initialForm)
                } catch (submitError) {
                    setFormError(submitError instanceof Error ? submitError.message : "...")
                }

            Now there are two sources of truth for "did this fail", and the local one has to
            be cleared by hand on the next submit or it lingers.

            The hook owns it, so the component hands over the success path and reads the
            failure:

                // RIGHT
                const { createArchitecturePrinciple, isCreating, createError } =
                    useCreateArchitecturePrincipleMutation()

                const submit = (event: React.FormEvent<HTMLFormElement>) => {
                    event.preventDefault()

                    createArchitecturePrinciple(form, { onSuccess: () => setForm(initialForm) })
                }

                {createError && <Alert severity="error">{createError}</Alert>}

            No try, no catch, no error state to reset: the next submit replaces it.
            """;

    public const string ErrorMessagesComeFromTheBackend = """
            Invented on the frontend, and wrong the moment a rule changes:

                // WRONG
                if (form.tags.length < 2) {
                    setError("You need at least two tags")   // is it two? still?
                }

            Now the rule lives in two places, in two languages, and only one of them is
            enforced.

            The backend already answers the question, and the mutation carries the answer:

                // RIGHT
                const { createArchitecturePrinciple, createError } = useCreateArchitecturePrincipleMutation()

                {createError && <Alert severity="error">{createError}</Alert>}

            The client reads the message out of the problem response, so createError is
            literally "Between two and three tags are required". The validator owns the
            wording, the frontend owns the presentation. Users see one consistent message
            no matter which client they came from.
            """;

    public const string DomainRulesAreNotReimplementedInTheUI = """
            A rule quietly forked into the client:

                // WRONG
                const canPublish = principle.tags.length >= 2
                    && principle.title.length <= 200
                    && principle.layer !== ArchitectureLayer.Contract

            This will drift. The backend will gain a fourth condition, the button will
            keep enabling, and the user will get a 400 they were told could not happen.

            Let the backend decide and send the answer:

                // RIGHT
                {principle.canPublish && <PublishButton />}

            The UI may disable a button to be helpful, but it must never be the place the
            rule is defined. Two copies of a rule is one rule and one bug in waiting.
            """;

    public const string DoNotImportAToolbeltForOneFunction = """
            A dependency, its transitive tree, and a supply-chain surface, for this:

                // WRONG
                import groupBy from "lodash/groupBy"

                const byLayer = groupBy(principles, (principle) => principle.layer)

            The platform has shipped it for years:

                // RIGHT
                const byLayer = Object.groupBy(principles, (principle) => principle.layer)

            Same for `isEmpty`, `flatten`, `uniq`, and most of the rest. Reach for the
            package when it solves something genuinely hard — dates, virtualisation,
            charts — not when it saves you a line you could read.
            """;
}
