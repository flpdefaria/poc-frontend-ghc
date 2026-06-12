---
name: create-primevue-component
description: "Create a Vue 3 component using PrimeVue following this project's standards. Use when: creating a new component, adding a form card, building a hero panel, implementing form validation with PrimeVue inputs, adding Password/InputText/Checkbox/Button, scaffolding a new screen."
argument-hint: "Describe the component to create (e.g., 'ResetPasswordCard with email + password fields', 'ProfileHero left panel')"
---

# Create PrimeVue Component

Creates Vue 3 components using PrimeVue 4.x following this project's exact conventions. Every output must be production-ready and pass `npm run type-check`.

## When to Use

- Scaffolding a new `*Card.vue` or `*Hero.vue` component
- Adding a form with PrimeVue inputs (text, password, checkbox)
- Implementing the glassmorphism card pattern
- Wiring up validation with the `submitted` guard pattern
- Creating any `src/**/*.vue` file in this project

---

## Step 1 — Read Project Instructions

Before writing any code, always read:

```
.github/instructions/frontend-standards.instructions.md
```

It is the source of truth. Everything below is a condensed quick-reference. If there is a conflict, the instructions file wins.

---

## Step 2 — Determine Component Type

| Type | Location | Responsibility |
|---|---|---|
| `*Card.vue` | `src/components/<feature>/` | Form inputs, validation, emits upward |
| `*Hero.vue` | `src/components/<feature>/` | Left-column visual/branding panel, no data |
| `*View.vue` | `src/views/` | Wraps AuthLayout + Hero + Card; owns submit handler |
| `*Layout.vue` | `src/layouts/` | CSS grid/spacing only, no state, no imports beyond `<slot />` |

---

## Step 3 — File Template

### `<script setup>` Rules

- Always `<script setup lang="ts">`
- Import each PrimeVue component individually:

```ts
import Button from 'primevue/button'
import Card from 'primevue/card'
import Checkbox from 'primevue/checkbox'
import FloatLabel from 'primevue/floatlabel'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'
import Tag from 'primevue/tag'
```

- Never use `import { Button } from 'primevue'` (barrel import)
- Define emits with typed payload — never use `any`

---

## Step 4 — Patterns

### Text Input (FloatLabel + InputText)

```vue
<div class="field">
  <FloatLabel>
    <InputText
      id="email"
      v-model="email"
      type="email"
      autocomplete="email"
      fluid
      :invalid="submitted && !!errors.email"
    />
    <label for="email">Email</label>
  </FloatLabel>
  <small v-if="submitted && errors.email" class="field-error">
    <i class="pi pi-exclamation-circle" /> {{ errors.email }}
  </small>
</div>
```

### Password Input

```vue
<div class="field">
  <FloatLabel>
    <Password
      v-model="password"
      inputId="password"
      toggleMask
      :feedback="false"
      autocomplete="current-password"
      fluid
      :invalid="submitted && !!errors.password"
    />
    <label for="password">Password</label>
  </FloatLabel>
  <small v-if="submitted && errors.password" class="field-error">
    <i class="pi pi-exclamation-circle" /> {{ errors.password }}
  </small>
</div>
```

### Checkbox

```vue
<label class="remember">
  <Checkbox v-model="remember" :binary="true" inputId="remember" />
  <span>Remember me</span>
</label>
```

### OR Separator (never `<Divider>`)

```vue
<div class="or-separator" role="separator" aria-label="or">
  <span class="or-line" />
  <span class="or-label">or</span>
  <span class="or-line" />
</div>
```

### Primary Submit Button

```vue
<Button
  type="submit"
  label="Sign in"
  icon="pi pi-arrow-right"
  icon-pos="right"
  class="submit-button"
  fluid
/>
```

### Secondary / Outlined Button

```vue
<Button
  type="button"
  label="Create an account"
  icon="pi pi-user-plus"
  variant="outlined"
  severity="secondary"
  class="secondary-button"
  fluid
  @click="emit('goToRegister')"
/>
```

---

## Step 5 — Validation Pattern

Always use `computed` errors + a `submitted` guard. Never validate before the first submit.

```ts
const submitted = ref(false)

const errors = computed(() => ({
  email: !email.value.trim()
    ? 'Email is required.'
    : !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value)
      ? 'Enter a valid email address.'
      : '',
  password: !password.value ? 'Password is required.' : '',
}))

const hasErrors = computed(() => Object.values(errors.value).some(Boolean))

const onSubmit = () => {
  submitted.value = true
  if (hasErrors.value) return
  emit('submit', { email: email.value, password: password.value })
}
```

Rules:
- Error messages: sentence case, end with `.`
- Show errors only after `submitted === true`
- `hasErrors` drives the submit gate — never check individual fields manually

---

## Step 6 — `<style scoped>` Rules

### Glassmorphism Card (every `*Card.vue`)

```css
.your-card {
  border-radius: 22px;
  border: 1px solid color-mix(in srgb, var(--p-surface-0) 35%, transparent);
  background: color-mix(in srgb, var(--p-surface-0) 94%, transparent);
  backdrop-filter: blur(18px);
  box-shadow:
    0 1px 0 color-mix(in srgb, var(--p-surface-0) 60%, transparent) inset,
    0 24px 60px color-mix(in srgb, var(--p-primary-950) 28%, transparent);
  width: min(420px, 100%);
  justify-self: center;
}

.your-card :deep(.p-card-body) {
  padding: 2.25rem 2rem;
  gap: 1rem;
}
```

### Required `:deep()` for Form Components

```css
/* Password eye stays inside input */
.your-form :deep(.p-password) {
  width: 100%;
  position: relative;           /* ← ONLY here, never on .field */
}

/* Input styling */
.your-form :deep(.p-inputtext) {
  background: var(--p-surface-0);
  color: var(--p-surface-700);
  transition: border-color 0.2s ease, box-shadow 0.2s ease, background-color 0.2s ease;
}

/* Validation error */
.your-form :deep(.p-inputtext.p-invalid) {
  border-color: var(--p-red-400) !important;
}

/* FloatLabel — no background flash on focus */
.your-form :deep(.p-floatlabel label) {
  background: transparent !important;
}
```

> **Critical**: `.field` must NOT have `position: relative`. This breaks the Password eye icon.

### Button Hover Effects

```css
/* Primary submit — lift + glow */
.your-form :deep(.submit-button:not(:disabled):hover) {
  transform: translateY(-2px);
  box-shadow: 0 8px 24px color-mix(in srgb, var(--p-primary-500) 45%, transparent);
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

/* Secondary outlined */
.your-form :deep(.secondary-button:not(:disabled):hover) {
  transform: translateY(-2px);
  background: color-mix(in srgb, var(--p-primary-100) 35%, transparent) !important;
  border-color: color-mix(in srgb, var(--p-primary-400) 60%, var(--p-content-border-color)) !important;
  box-shadow: 0 6px 18px color-mix(in srgb, var(--p-primary-400) 20%, transparent);
}
```

### CSS Design Token Reference

| Token | Usage |
|---|---|
| `var(--p-primary-700)` | Card titles |
| `var(--p-primary-600)` | Hover / link color |
| `var(--p-primary-500)` | Primary brand blue |
| `var(--p-surface-0)` | White / card background |
| `var(--p-surface-700)` | Input text color |
| `var(--p-text-muted-color)` | Secondary / muted text |
| `var(--p-content-border-color)` | Borders, dividers |
| `var(--p-red-400)` | Validation error border |
| `var(--p-red-500)` | Validation error text |

**Never hardcode hex or rgb values.** Always use `var(--p-*)` or `color-mix(in srgb, var(--p-*) X%, transparent)`.

---

## Step 7 — New Screen Checklist

When the component is part of a new screen (e.g., `reset-password`):

- [ ] Add screen name to `AuthView` union in `src/types/auth.types.ts`
- [ ] Add payload type to `src/types/auth.types.ts`
- [ ] Create `src/components/<feature>/<Feature>Hero.vue`
- [ ] Create `src/components/<feature>/<Feature>Card.vue`
- [ ] Create `src/views/<Feature>View.vue` (wraps `AuthLayout` + Hero + Card)
- [ ] Add `v-else-if` case in `App.vue` template
- [ ] Run `npm run type-check` — must exit 0

---

## Step 8 — Quality Check

Before delivering:

- [ ] `<script setup lang="ts">` on every component
- [ ] All PrimeVue imports are individual (no barrel imports)
- [ ] `.field` does NOT have `position: relative`
- [ ] No hardcoded `#hex`, `rgb()`, or pixel values for colors
- [ ] All colors use `var(--p-*)` or `color-mix()`
- [ ] `computed` errors + `submitted` guard in every form
- [ ] `npm run type-check` exits 0
