---
name: copilot-inline-suggestions
description: 'Guidance on using GitHub Copilot inline (editor) suggestions in VS Code: ghost text completions, accepting/cycling suggestions with Tab/Ctrl+Enter, using comments as context/directives/questions, the sparkle-icon context menu, Copilot code review (default and custom instructions), and IDE completion settings. Use when: explaining how Copilot inline completions work, writing comments to steer Copilot, requesting alternative suggestions, reviewing code with Copilot, configuring completion settings, or teaching Copilot inline workflow (as opposed to Chat/Agent mode).'
---

# GitHub Copilot — Inline Suggestions (Editor Mode)

## When to Use
- Explaining how Copilot's inline/ghost-text completions work in the editor
- Helping someone accept, reject, or cycle between multiple completions
- Writing comments that provide context, act as directives, or ask questions to steer Copilot
- Using the sparkle-icon context menu or Command Palette Copilot actions
- Running Copilot code review (default or with custom instructions)
- Configuring Copilot completion behavior in VS Code settings

> Scope: this covers *inline* editor interactions only. Chat/Agent Mode is a separate interaction model (see Chapters 3–4 style topics) and is out of scope here.

## Core Workflow (Inline Completions)
1. Accept or dismiss a Copilot suggestion (ghost text).
2. Type additional code manually if needed.
3. Pause briefly to let Copilot generate options.
4. Copilot presents suggestion(s) — shown as italic "ghost text".
5. Repeat 1–4 until the task is complete.

Key shortcuts:
- **Tab** — accept the full suggestion
- Word-by-word accept — shortcut shown in the suggestion toolbar
- **< 1/2 >** style indicator — cycle between multiple generated alternatives
- **Ctrl+Enter** — open the Completions Panel with a longer list of alternative suggestions (slower, but more options; works best *before* accepting an inline suggestion, and select code first when using it on existing code for better context)

## Using Comments to Steer Copilot
Comments influence suggestions in three ways:

1. **Passive context** — an explanatory comment (e.g. `# a function to determine whether a number is prime`) gives Copilot context to complete the next line/function.
2. **Directives** — phrase the comment as an instruction (e.g. `# create a function to determine if a number is prime`) to get Copilot to generate an implementation. Directives that don't request code (e.g. `# explain the code above`, `# explain the code above line by line`) make Copilot respond with additional comments instead of code.
3. **Questions** — ask Copilot a question directly in a comment (e.g. `# what does the code above do?`). Optionally prefix with `q:` to get an explicit `a:` answer. This predates Copilot Chat and is mainly useful for quick, inline answers.

## Context Menu & Command Palette
- The **sparkle icon** (✨✨) next to incomplete/erroring code opens a popup menu with actions like "Fix using Copilot" (can double as a way to nudge code completion, though it won't produce a full implementation).
- Right-click → **Copilot** submenu exposes context-appropriate actions (e.g. in the terminal, "Explain" refers to terminal output, not editor code).
- Command Palette (**Cmd/Ctrl+Shift+P** or **F1**) → type "Copilot" to list all available Copilot commands.

## Code Review with Copilot
- Select code → right-click → **Copilot > Review and Comment** to get inline review feedback (diff-style: `-` removed, `+` suggested) with apply/dismiss controls per comment.
- By default, Copilot only reports significant issues.
- To get stricter/custom feedback, add custom review instructions via:
  - `settings.json`, using `github.copilot.chat.reviewSelection.instructions`:
    ```json
    "github.copilot.chat.reviewSelection.instructions": [
      { "text": "Ensure all functions have proper docstrings" }
    ]
    ```
  - Or reference an external Markdown file:
    ```json
    "github.copilot.chat.reviewSelection.instructions": [
      { "file": "./docs/review-guide.md" }
    ]
    ```
  - Or drop up to 5 rules in `.github/copilot-review-guidelines.md`, which Copilot picks up automatically.

## Commit Messages
In the Source Control panel, click the sparkle icon at the end of the commit message box to have Copilot generate a message from the staged diff. Custom instructions for commit message generation can also be configured (see VS Code docs).

## Configuring Completions in VS Code
- **Status bar Copilot icon** — quickly disable completions globally or per active language.
- **Title bar Copilot icon** → **Configure Code Completions**:
  - *Open Completions Panel* — same as `Ctrl+Enter`
  - *Change Completions Model* — pick a different model for completions only (not chat)
  - *Disable Completions* — toggle suggestions on/off
- **Edit Settings** → opens Settings filtered to "GitHub Copilot"; supports both User and Workspace scopes, including Preview/Experimental features.

## Key Takeaways
- Suggestion quality depends on available context: filename, existing code, comments, other open files, and the local index.
- Generative AI is non-deterministic — the same context can yield different suggestions across attempts, and sometimes multiple valid alternatives with only subtle differences.
- Comments are a lightweight but powerful way to add context, direct behavior, or ask questions without leaving the editor.
