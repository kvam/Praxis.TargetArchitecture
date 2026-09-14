import react from "@vitejs/plugin-react"
import { defineConfig } from "vite"
import { fileURLToPath, URL } from "node:url"
import checker from "vite-plugin-checker"

export default defineConfig({
    plugins: [
        react(),
        checker({
            typescript: { tsconfigPath: "./tsconfig.json" },
            eslint: { lintCommand: 'eslint "src/**/*.{ts,tsx}"' },
        }),
    ],
    resolve: {
        alias: {
            "@": fileURLToPath(new URL("./src", import.meta.url)),
        },
    },
    server: {
        port: 5173,
        proxy: {
            "/api": {
                target: "http://localhost:5153",
                changeOrigin: true,
            },
        },
    },
})
