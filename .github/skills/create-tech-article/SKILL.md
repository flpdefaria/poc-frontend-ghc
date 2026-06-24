---
name: create-tech-article
description: 'Write a structured technical article from source code and reference material. Use when: writing a tech article, blog post, or tutorial; authoring developer documentation; explaining code with step-by-step narrative; creating content from a codebase or code snippets with supporting images and official docs.'
argument-hint: 'Provide: codebase path or snippets | image list (e.g. <image1: description>) | official docs URLs'
---

# Create Tech Article

## When to Use
- Writing a technical article, blog post, or developer tutorial
- Explaining a feature, integration, or architecture using real code
- Producing content that references screenshots, diagrams, or setup images
- Authoring documentation backed by official sources

## Required Inputs

Collect the following before generating the article:

| Input | Format | Example |
|-------|--------|---------|
| **Codebase / Snippets** | Absolute path OR inline code blocks | `/projects/my-api/src/` or pasted code |
| **Images** | Tagged list with short descriptions | `<image1: Creating Azure subscription>` |
| **Official Docs** | URLs or named doc pages | `https://learn.microsoft.com/...` |

If any input is missing, ask the user before proceeding.

## Procedure

### Step 1 — Understand the Subject
1. If a **codebase path** is provided, explore the directory: list key files, read entry points, identify the main technologies and patterns used.
2. If **code snippets** are provided, analyze them inline: identify language, purpose, and notable implementation decisions.
3. Summarize internally: _"This article is about X, using Y technology, demonstrating Z."_

### Step 2 — Catalog the Images
Parse the image list. For each entry:
- Extract the tag (e.g., `imagem1`) and the description.
- Map where the image logically fits in the article narrative (setup, configuration, result, etc.).
- Reserve a placeholder in the outline: `<imagem1: description>`.

### Step 3 — Review Official Docs
For each provided URL or doc reference:
- Fetch or summarize the relevant content.
- Note key terms, API names, version requirements, and best-practice guidance.
- Use these as the source of truth for technical accuracy; link them inline throughout the text.

### Step 4 — Build the Article Outline
Draft a **loose narrative outline** — not a rigid document structure. The article reads like a blog post, not formal documentation. A typical flow:

```
1. Opening paragraph — what the technology is, why it matters, what the reader will build
2. Concept context — brief theory, architecture notes, tiers/auth/config as needed (woven into prose)
3. Setup / scaffolding — commands and initial project structure
4. Implementation walk-through — classes and files introduced one at a time, each followed by an explanation
5. Running and validating — how to execute, expected output, portal screenshots
6. Closing paragraph — recap and tease next parts
```

There are **no section headers** (`#`, `##`, `###`) anywhere in the article body. Use prose transitions between topics for all context shifts, including major ones.

### Step 5 — Write the Article

#### Language and Tone
- **Narrative language**: Brazilian Portuguese (pt-BR).
- **Technical terms**: Always in en-US — never translate Azure product names, open-source project names, or established technical terms. Examples: Full-Text Search, RAG, Azure App Service, API Keys, Role-Based Access Control, Managed Identity, Blob Storage, Azure Functions, GitHub Actions, Docker, Kubernetes, gRPC, REST, WebSockets, Deployment Slots, etc.
- **Tone**: Conversational but precise. Write as if explaining to a developer colleague. Avoid corporate stiffness and generic filler.

#### Structure and Formatting Rules (follow the examples exactly)
- **Title**: One clear, action-oriented line. Not wrapped in a header tag in the body — treat it as the article title above the text.
- **Intro paragraph**: Opens directly with what the technology is, hyperlinking the product name to its official docs on first mention. No "In this article we will..." preamble.
- **No section headers**: Do not use any Markdown headers (`#`, `##`, `###`) anywhere in the article body. This includes `## Prerequisites`, `## Implementation`, `## Conclusion`, `## Criando o projeto`, `## A classe de serviço`, and any other heading. Transition between all topics, including major context shifts, using bridging sentences in prose.
- **Code blocks**: Fenced with the correct language tag (`bash`, `csharp`, `json`, etc.). Pull real snippets from the codebase. Do not add comments to lines that do not need them.
- **Code explanations**: Immediately after each code block, write a short explanation. If the block contains multiple methods or classes, explain each one using a bullet list in this form:
  ```
  MethodName()
  - One-sentence description of what it does.
  ```
- **Image placeholders**: Insert at the exact moment the narrative references them, using this format:
  ```
  <imagemN: short description of what the screenshot shows>
  ```
  Use Portuguese descriptions. Number sequentially (`imagem1`, `imagem2`, …).
- **Official doc links**: Inline-link key terms on first mention. Do not collect them in a separate References section — weave them naturally into the prose.
- **Commands**: Use `bash` code blocks for terminal commands. Add a brief sentence before each block explaining what the command does.
- **Short paragraphs**: Keep paragraphs to 3–5 sentences. Break up dense content.

### Step 6 — Quality Check
Before delivering, verify:
- [ ] Every image tag from the input appears in the article exactly once, using `<imagemN: ...>` format
- [ ] All code snippets are real and compile/run correctly (no pseudocode unless labeled)
- [ ] No section is placeholder-only (no "TODO" left in the output)
- [ ] No Markdown headers (`#`, `##`, `###`) anywhere in the article body — all transitions must be prose
- [ ] All official doc links are inline; there is no trailing References section
- [ ] Article length is appropriate — aim for completeness, not padding

### Step 7 — Deliver
Output the complete Markdown article. Then briefly note:
- Any images whose placement felt ambiguous
- Any official doc that could not be fetched (summarized from URL instead)
- Suggested follow-up articles or related topics

## Output Format

The article must read like the examples in [examples.md](./references/examples.md):
- Flowing prose blog post in pt-BR
- Technical terms in en-US
- Inline links, no trailing References section
- `<imagemN: description>` placeholders
- Code blocks followed by bullet-point method/class explanations
- Closing line as specified below

## Examples

See [examples](./references/examples.md) for real article samples and reference implementations.

## Link Convention

All external links **must** include the query string `?wt.mc_id=MVP_407589` (or `&wt.mc_id=MVP_407589` if the URL already contains a query string).

Links **must not** use Markdown hyperlink syntax `[text](url)`. Instead, write the link text followed by the URL in parentheses:

```
Azure Speech Service (https://learn.microsoft.com/en-us/azure/ai-services/speech-service/overview?wt.mc_id=MVP_407589)
```

Never write:
```
[Azure Speech Service](https://learn.microsoft.com/en-us/azure/ai-services/speech-service/overview?wt.mc_id=MVP_407589)
```

This applies to every link in the article — inline text links, product name mentions, and any URL reference.

## Closing Line

The very last lines of every article **must** be exactly:

```txt
Você já pode baixar o projeto por esse link, e não esquece de me seguir no LinkedIn!
Até a próxima, abraços!
````

Do not paraphrase, translate, or omit this closing. Place it as the final paragraph of the article, after all content. There is **no References section after it**.

## Anti-Patterns to Avoid
- Do not invent API signatures or configuration options — use only what the codebase and docs confirm.
- Do not skip images — every tagged image must appear in the article.
- Do not add a trailing `## References` section — all links belong inline.
- Do not use any Markdown headers (`#`, `##`, `###`) anywhere in the article body — no exceptions. All topic transitions, including major context shifts, must be handled with prose.
- Do not use generic filler text ("Lorem ipsum", "Here we can see that...", "In this article we will...").
- Do not emit any external link without the `wt.mc_id=MVP_407589` query parameter.
- Do not write the article in English — the narrative must be in pt-BR.
- Do not use `-` dash bullet lists anywhere in the article body. Convert lists to flowing prose or use the method-explanation pattern (`MethodName()` followed by a plain sentence).
- Do not use em dashes (`—`) anywhere in the article body. Replace with a comma, period, or rewrite the sentence.
- Do not use jokes, playful analogies, or humorous comparisons — the tone is conversational and precise, not entertaining. Write as a developer explaining to another developer, not as a comedian.
