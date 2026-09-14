import { createGlobalStyle } from "styled-components"

export const GlobalStyle = createGlobalStyle`
    @import url("https://fonts.googleapis.com/css2?family=DM+Mono:wght@400;500&family=Manrope:wght@400;500;600;700;800&display=swap");

    :root {
        color: #102a43;
        background: #eef1ee;
        font-family: "Manrope", sans-serif;
        font-synthesis: none;
        text-rendering: optimizeLegibility;
    }

    * { box-sizing: border-box; }

    body {
        min-width: 320px;
        margin: 0;
        background-color: #eef1ee;
        background-image: linear-gradient(#dce5e1 1px, transparent 1px), linear-gradient(90deg, #dce5e1 1px, transparent 1px);
        background-size: 44px 44px;
    }
`