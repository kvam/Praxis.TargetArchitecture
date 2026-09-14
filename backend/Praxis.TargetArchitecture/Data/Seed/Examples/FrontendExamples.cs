namespace Praxis.TargetArchitecture.Data.Seed.Examples;

public static class FrontendExamples
{
    public const string HooksReturnNamedValues = """
            The hook translates plumbing into intent:

                // RIGHT
                export const useCreateArchitecturePrincipleMutation = () => {
                    const { mutateAsync, isPending } = useMutation({ ... })

                    return {
                        createArchitecturePrinciple: mutateAsync,
                        isCreating: isPending,
                    }
                }

            So the component reads as what it does:

                // RIGHT
                const { createArchitecturePrinciple, isCreating } = useCreateArchitecturePrincipleMutation()

            Returning mutateAsync and isPending directly makes every call site say "pending"
            without saying what is pending — and breaks the moment a component needs two.
            """;

    public const string MutationsInvalidateWhatTheyChanged = """
            The write hook invalidates the keys it affected:

                // RIGHT
                useMutation({
                    mutationFn: createArchitecturePrinciple,
                    onSuccess: () => queryClient.invalidateQueries({
                        queryKey: [QueryKeys.ArchitecturePrinciples],
                    }),
                })

            The list then refetches itself. The alternative is every component that creates
            a principle remembering to refetch — and one of them eventually forgetting,
            producing a screen that is quietly a version behind.
            """;

    public const string ServerStateLivesInTheQueryCache = """
            Read it from the cache:

                // RIGHT
                const { data: principles, isLoading } = useGetArchitecturePrinciples()

            Do not copy it into component state:

                // WRONG
                const [principles, setPrinciples] = useState<ArchitecturePrincipleDto[]>([])
                useEffect(() => { getPrinciples().then(setPrinciples) }, [])

            The copy goes stale the moment anything else writes, and nothing tells you.
            Component state is for what the user is doing right now — the open dialog, the
            text in the search box — not for what the backend owns.
            """;

    public const string TheBrowserIsTheStandardLibrary = """
            Already installed, already patched, already familiar:

                // RIGHT
                fetch(...)                   instead of axios
                crypto.randomUUID()          instead of uuid
                new Intl.DateTimeFormat()    instead of moment or date-fns
                new URLSearchParams(...)     instead of query-string
                `card ${isActive && "on"}`   instead of classnames

            Each of those packages is small, popular, and well written. That is not the
            point: every one is a version to upgrade, a supply-chain link to trust, and one
            more thing a newcomer has to look up before they can read the line.
            """;
}
