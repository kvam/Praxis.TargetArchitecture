import { defineConfig } from "@hey-api/openapi-ts"

export default defineConfig({
    input: "http://localhost:5153/openapi/v1.json",
    output: {
        path: "src/models/generated",
        // The backend writes constants.ts into this folder too. Cleaning would delete it.
        clean: false,
    },
    plugins: [
        {
            name: "@hey-api/typescript",
            enums: {
                mode: "javascript",
                case: "preserve",
            },
        },
    ],
})
