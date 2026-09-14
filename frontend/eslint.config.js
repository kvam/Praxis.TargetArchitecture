export default [
    {
        files: ["src/**/*.{ts,tsx}"],
        languageOptions: {
            parser: (await import("@typescript-eslint/parser")).default,
        },
        rules: {
            "sort-imports": ["error", { ignoreDeclarationSort: true }],
            "no-multiple-empty-lines": ["error", { max: 1, maxEOF: 1, maxBOF: 0 }],
        },
    },
]
