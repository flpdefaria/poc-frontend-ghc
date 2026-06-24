# MIGRATING VUE + PRIMEVUE TO TAILWIND CSS V4 (HYBRID GOVERNANCE)

This migration skill guides the developer/agent when refactoring layouts and form cards to use Tailwind CSS v4 while maintaining the hybrid visual governance.

---

## Governance Split

1. **Tailwind CSS**: Controls layout (flex, grid), spacing (padding, margin, gap), secondary typography, separators, and inline alignment.
2. **Scoped CSS + Design Tokens**: Controls complex semantic colors (e.g., `var(--p-surface-0)`), Glassmorphism effects (blur, semi-transparent borders), and PrimeVue component deep overrides (`:deep()`).

---

## Side-by-Side Blueprints (Old CSS vs Tailwind Hybrid)

### 1. Auth Layout (Main Centering Shell)

#### Before (Custom CSS)
```vue
<template>
  <div class="auth-layout">
    <slot />
  </div>
</template>

<style scoped>
.auth-layout {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  width: 100%;
  padding: 24px;
  background: linear-gradient(135deg, var(--p-primary-950) 0%, var(--p-surface-950) 100%);
}
</style>
```

#### After (Tailwind Hybrid)
```vue
<template>
  <div class="flex items-center justify-center min-height-screen w-full p-6 auth-bg">
    <slot />
  </div>
</template>

<style scoped>
.auth-bg {
  min-height: 100vh;
  background: linear-gradient(135deg, var(--p-primary-950) 0%, var(--p-surface-950) 100%);
}
</style>
```

---

### 2. Form Fields (Inputs, FloatLabel and Password Wrapper)

Always preserve `position: relative` and width on the PrimeVue input wrapper, while the outer layout uses Tailwind.

#### Before (Custom CSS)
```vue
<template>
  <div class="form-field">
    <FloatLabel>
      <InputText id="email" v-model="email" />
      <label for="email">E-mail</label>
    </FloatLabel>
  </div>
</template>

<style scoped>
.form-field {
  margin-bottom: 20px;
}
.form-field :deep(.p-inputtext) {
  width: 100%;
}
</style>
```

#### After (Tailwind Hybrid)
```vue
<template>
  <div class="mb-5">
    <FloatLabel class="w-full">
      <InputText id="email" v-model="email" class="w-full" />
      <label for="email">E-mail</label>
    </FloatLabel>
  </div>
</template>
```

---

### 3. "OR" Visual Separator

Use Tailwind border and flex utilities instead of manual pseudo-elements in scoped CSS.

#### Before (Custom CSS)
```vue
<template>
  <div class="divider">or</div>
</template>

<style scoped>
.divider {
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 20px 0;
  color: var(--p-surface-500);
}
.divider::before, .divider::after {
  content: "";
  flex: 1;
  height: 1px;
  background: var(--p-surface-200);
  margin: 0 10px;
}
</style>
```

#### After (Tailwind Hybrid)
```vue
<template>
  <div class="flex items-center my-5 text-sm text-surface-500">
    <div class="flex-1 h-px bg-surface-200" />
    <span class="px-3">or</span>
    <div class="flex-1 h-px bg-surface-200" />
  </div>
</template>
```

---

### 4. Form Card + Scoped Overrides

The card keeps its Glassmorphism class (`glass-card`) via scoped CSS, but all internal spacing, width, and children are controlled by Tailwind utility classes.

#### Before (Custom CSS)
```vue
<template>
  <div class="glass-card">
    <h2 class="title">Sign in</h2>
    <form class="glass-form">
      ...
    </form>
  </div>
</template>

<style scoped>
.glass-card {
  width: 100%;
  max-width: 440px;
  padding: 40px;
  border-radius: 24px;
  background: color-mix(in srgb, var(--p-surface-0) 90%, transparent);
  border: 1px solid color-mix(in srgb, var(--p-surface-0) 30%, transparent);
  backdrop-filter: blur(12px);
}
.title {
  font-size: 24px;
  font-weight: 700;
  text-align: center;
  margin-bottom: 30px;
}
</style>
```

#### After (Tailwind Hybrid)
```vue
<template>
  <div class="w-full max-w-[440px] p-8 glass-card">
    <h2 class="text-2xl font-bold text-center mb-6 text-surface-900">Sign in</h2>
    <form class="flex flex-col gap-4">
      ...
    </form>
  </div>
</template>

<style scoped>
.glass-card {
  border-radius: 24px;
  background: color-mix(in srgb, var(--p-surface-0) 90%, transparent);
  border: 1px solid color-mix(in srgb, var(--p-surface-0) 30%, transparent);
  backdrop-filter: blur(12px);
}
</style>
```

---

## Migration Quality Checklist

- [ ] No padding (`p-`), margin (`m-`), gap (`gap-`), flex, or grid rules remain in `<style scoped>`.
- [ ] Semantic border colors and special backgrounds use scoped CSS with `var(--p-surface-*)` or `color-mix()`.
- [ ] Font sizes, weights, and neutral secondary text colors have moved to Tailwind (e.g., `text-surface-500`, `font-bold`).
- [ ] `:deep(.p-password)` still has `position: relative` and full width in scoped CSS to prevent the eye icon from escaping the input bounds.
- [ ] `npm run type-check` exits 0 with no TypeScript errors.

---

## Common Post-Migration Pitfalls

### Pitfall 1 — `font-weight: normal` on the `*` selector in `base.css`

Vite's default `base.css` includes `font-weight: normal` on the universal selector. Because Tailwind v4 generates utilities inside `@layer utilities`, CSS **outside any layer** has higher cascade priority and silently overrides `font-bold`, `font-semibold`, etc. on **every element**.

**Mandatory action before any migration:** check for and remove that line from `base.css`:

```css
/* ❌ REMOVE */
*, *::before, *::after {
  font-weight: normal;
}
```

### Pitfall 2 — `<span>` elements inside headings need an explicit `font-weight`

`<span>` inherits `font-weight` from its parent, but Tailwind does not automatically inject that weight into children. Whenever an accent `<span>` inside an `<h1 class="font-bold">` should have a different weight, declare it explicitly:

```vue
<!-- Accent span with normal weight inside a bold heading -->
<h1 class="text-5xl font-bold">
  Forgot your<br />
  <span class="font-normal hero-title-accent">password?</span>
</h1>
```

### Pitfall 3 — Layout does not vertically center without a flex wrapper

Applying `min-h-screen` directly to `<main class="grid ...">` **does not center** content in the viewport. Always use the correct structure:

```vue
<!-- ❌ Does NOT vertically center -->
<main class="grid grid-cols-[1.05fr_1fr] min-h-screen ...">
  <slot />
</main>

<!-- ✅ Correctly centered -->
<div class="min-h-screen flex items-center justify-center px-5 py-8 lg:px-6 lg:py-12">
  <main class="grid grid-cols-1 lg:grid-cols-[1.05fr_1fr] gap-10 lg:gap-16 items-center w-full max-w-[480px] lg:max-w-[1140px]">
    <slot />
  </main>
</div>
```
