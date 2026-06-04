---
description: "Use when creating new screens, views, components, layouts, or any Vue file in this project. Enforces folder structure, PrimeVue patterns, and visual identity."
applyTo: "src/**/*.vue"
---

# Frontend Standards — poc-frontend-ghc

## Stack

- **Vue 3.5+**: Composition API, `<script setup lang="ts">` on every component
- **PrimeVue 4.x** with `@primeuix/themes` Aura preset (blue primary override — see `main.ts`)
- **TypeScript**: strict mode; `vue-tsc --build` must exit 0 before committing
- **primeicons**: always import via `import 'primeicons/primeicons.css'` in `main.ts`
- **Vite 8.x**: build tooling
- **Navigation**: `ref<AuthView>` in `App.vue` — Vue Router not installed yet

---

## Folder Structure

```
src/
  App.vue              ← only switches between views via ref<AuthView>; zero business logic
  main.ts              ← PrimeVue + blue Aura preset config; no other setup here
  types/
    *.types.ts         ← all shared TypeScript types (payloads, union view names)
  layouts/
    *Layout.vue        ← structural grid/spacing shells; no data, no emits
  views/
    *View.vue          ← one per screen; wraps layout + hero + card; owns submit handlers
  components/
    <feature>/
      *Card.vue        ← form card: inputs, validation, emits upward
      *Hero.vue        ← left-column branding/visual panel
  assets/
    main.css           ← global styles using only PrimeVue CSS variables; no hardcoded colors
    base.css
```

### Rules

1. Every **new screen** gets its own subfolder under `components/` (e.g., `components/reset-password/`)
2. Every **view** (`views/*.vue`) wraps `AuthLayout` + one `*Hero` + one `*Card`
3. **Payload types** and view union types always go in `types/*.types.ts` — never inline in `App.vue`
4. **Layouts** contain only CSS (grid, spacing, padding, responsive breakpoints) — no state, no imports beyond `<slot />`
5. After adding a new screen, always update the `AuthView` union type in `src/types/auth.types.ts`

---

## New Screen Checklist

When adding a new screen (e.g., `reset-password`):

1. Add its name to `AuthView` union in `src/types/auth.types.ts`
2. Add its payload type to `src/types/auth.types.ts`
3. Create `src/components/reset-password/ResetPasswordHero.vue` (left panel)
4. Create `src/components/reset-password/ResetPasswordCard.vue` (form + validation)
5. Create `src/views/ResetPasswordView.vue` (wraps `AuthLayout` + Hero + Card; owns `onSubmit`)
6. Add the new `v-else-if` case to `App.vue` template
7. Run `npm run type-check` — must exit 0

---

## PrimeVue Component Usage

### Component Imports

Always import components individually from their own package path:

```ts
import Button from 'primevue/button'
import Card from 'primevue/card'
import Checkbox from 'primevue/checkbox'
import FloatLabel from 'primevue/floatlabel'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'
import Tag from 'primevue/tag'
```

### Text Input Pattern (FloatLabel + InputText)

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

### Password Input Pattern

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

### OR Separator (never use PrimeVue `<Divider>` for this)

```vue
<div class="or-separator" role="separator" aria-label="or">
  <span class="or-line" />
  <span class="or-label">or</span>
  <span class="or-line" />
</div>
```

```css
.or-separator { display: flex; align-items: center; gap: 0.75rem; }
.or-line { flex: 1; height: 1px; background: color-mix(in srgb, var(--p-content-border-color) 80%, transparent); }
.or-label { color: var(--p-text-muted-color); font-size: 0.8rem; text-transform: uppercase; letter-spacing: 0.18em; }
```

### Required `:deep()` CSS for Every Form Component

Add these to the `<style scoped>` of every component that has form inputs:

```css
/* Password eye icon stays inside input */
.your-form :deep(.p-password) {
  width: 100%;
  position: relative;
}

/* Input text color and shape */
.your-form :deep(.p-inputtext) {
  color: var(--p-surface-700);
  border-radius: 12px;
  padding: 0.95rem 0.9rem;
}

/* Validation border */
.your-form :deep(.p-inputtext.p-invalid) {
  border-color: var(--p-red-400) !important;
}

/* FloatLabel background on focus */
.your-form :deep(.p-floatlabel label) {
  background: transparent !important;
}
```

> **Critical bug to avoid**: `.field` must NOT have `position: relative`. This breaks the Password eye icon placement. Only `.p-password` gets `position: relative`.

---

## Validation Pattern

Use `computed` errors + a `submitted` ref guard. Never validate eagerly before first submit attempt.

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

Error messages use sentence case and end with a period. Show errors only after `submitted` is `true`.

---

## Emits Convention

```ts
// In a view (navigates between screens)
const emit = defineEmits<{
  goToLogin: []
  goToRegister: []
}>()

// In a card/form component (actions + navigation)
const emit = defineEmits<{
  submit: [payload: LoginPayload]
  goToRegister: []
  goToForgotPassword: []
}>()
```

Types for payloads always come from `src/types/*.types.ts` — never defined inline.

---

## Visual Identity

### Theme — Blue Aura Preset (`main.ts`)

The primary palette is always **blue**. Never change this:

```ts
const AppPreset = definePreset(Aura, {
  semantic: {
    primary: {
      50: '{blue.50}', 100: '{blue.100}', 200: '{blue.200}', 300: '{blue.300}',
      400: '{blue.400}', 500: '{blue.500}', 600: '{blue.600}', 700: '{blue.700}',
      800: '{blue.800}', 900: '{blue.900}', 950: '{blue.950}'
    }
  }
})
```

### CSS Design Tokens — Use Exclusively

Never hardcode hex/rgb values. Always use PrimeVue CSS variables:

| Token | Usage |
|---|---|
| `var(--p-primary-500)` | Primary brand blue |
| `var(--p-primary-600)` | Hover state |
| `var(--p-primary-700)` | Card titles |
| `var(--p-surface-0)` | White / card background |
| `var(--p-surface-700)` | Input text color |
| `var(--p-text-color)` | Body text |
| `var(--p-text-muted-color)` | Secondary / muted text |
| `var(--p-content-border-color)` | Borders, dividers |
| `var(--p-red-400)` | Validation error border |
| `var(--p-red-500)` | Validation error text |

For blended/transparent values, always use `color-mix`:

```css
color-mix(in srgb, var(--p-primary-500) 45%, transparent)
```

### Card Style

All form cards use the glassmorphism pattern:

```css
.feature-card {
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

.feature-card :deep(.p-card-body) {
  padding: 2.25rem 2rem;
  gap: 1rem;
}
```

### Button Hover Effects

Every primary submit button must have a lift + glow on hover:

```css
.submit-button:not(:disabled):hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 24px color-mix(in srgb, var(--p-primary-500) 45%, transparent);
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}
```

Secondary / outlined buttons:

```css
.secondary-button:not(:disabled):hover {
  transform: translateY(-1px);
  background: color-mix(in srgb, var(--p-primary-500) 8%, transparent);
  transition: transform 0.15s ease, background 0.15s ease;
}
```

### Hero Panel Style

Left-column hero panels use:

```css
.hero-panel {
  color: color-mix(in srgb, var(--p-surface-0) 92%, var(--p-primary-50));
  display: grid;
  gap: 1.5rem;
  max-width: 30rem;
}
```

Gradient accent on hero titles:

```css
.hero-title-accent {
  background: linear-gradient(135deg, var(--p-primary-400), var(--p-primary-600));
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}
```

### Typography

- Font stack: `'Inter', 'SF Pro Display', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif`
- Defined globally in `assets/main.css` — do not redefine in components
- Card titles: `font-size: 1.65rem; font-weight: 700; letter-spacing: -0.01em; color: var(--p-primary-700)`
- Body/description: `font-size: 0.95rem; color: var(--p-text-muted-color)`

### Border Radius

- Inputs: `border-radius: 12px`
- Cards: `border-radius: 22px`
- Buttons: use PrimeVue defaults

### Page Background

The atmospheric blue radial gradient background is defined in `assets/main.css` and applied to `body`. Do not add `background` to views, layouts, or cards — it bleeds through the glassmorphism transparency.

### Layout Grid

Always use `AuthLayout.vue` as the wrapper for all screens. Do not re-implement the grid inside views:

```vue
<template>
  <AuthLayout>
    <FeatureHero />
    <FeatureCard @submit="onSubmit" @go-to-login="emit('goToLogin')" />
  </AuthLayout>
</template>
```

The layout is two columns on desktop (≥ 900px) and single column on mobile.
