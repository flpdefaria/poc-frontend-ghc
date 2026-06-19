# MIGRANDO VUE + PRIMEVUE PARA TAILWIND CSS V4 (HYBRID GOVERNANCE)

Este guia de migração (Skill) orienta o dev/agente na refatoração de layouts e cartões de formulários para usem Tailwind CSS v4 mantendo a governança visual híbrida.

---

## Estrutura Regente (Hybrid Governance Split)

1. **Tailwind CSS**: Controla o layout (flex, grid), espaçamentos (padding, margin, gap), tipografia secundária, separadores e alinhamentos inline.
2. **CSS Scoped + Design Tokens**: Mantém controle sobre cores semânticas complexas (ex: `var(--p-surface-0)`), efeitos de Glassmorphism (blur, border semi-transparente) e sobressalência de componentes PrimeVue (`:deep()`).

---

## Blueprints Lado a Lado (Old CSS vs Tailwind Hybrid)

### 1. Auth Layout (Main Centering Shell)

#### Antes (CSS Customizado)
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

#### Depois (Tailwind Híbrido)
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

### 2. Form Fields (Inputs, FloatLabel e Password Wrapper)

Sempre preserve `position: relative` e a largura no input wrapper do PrimeVue, enquanto o layout externo usa Tailwind.

#### Antes (CSS Customizado)
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

#### Depois (Tailwind Híbrido)
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

### 3. "OR" Separador Visual

Use as utilidades de borda e flexibilidade do Tailwind CSS em vez de pseudos-elementos manuais no CSS scoped.

#### Antes (CSS Customizado)
```vue
<template>
  <div class="divider">ou</div>
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

#### Depois (Tailwind Híbrido)
```vue
<template>
  <div class="flex items-center my-5 text-sm text-surface-500">
    <div class="flex-1 h-px bg-surface-200" />
    <span class="px-3">ou</span>
    <div class="flex-1 h-px bg-surface-200" />
  </div>
</template>
```

---

### 4. Card Form + Scoped Overrides

O cartão mantém a classe Glassmorphism (`glass-card`) via scoped CSS, mas todo o espaçamento interno, largura e filhos são controlados de forma fluida pelo Tailwind.

#### Antes (CSS Customizado)
```vue
<template>
  <div class="glass-card">
    <h2 class="title">Acessar Conta</h2>
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

#### Depois (Tailwind Híbrido)
```vue
<template>
  <div class="w-full max-w-[440px] p-8 glass-card">
    <h2 class="text-2xl font-bold text-center mb-6 text-surface-900">Acessar Conta</h2>
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

## Checklist de Qualidade de Migração (Defesa de Governança)

- [ ] Nenhum padding (`p-`), margin (`m-`), gap (`gap-`) ou flex/grid permaneceu no `<style scoped>`.
- [ ] Cores de bordas semânticas ou de fundos especiais usam scoped CSS com variables `var(--p-surface-*)` ou `color-mix()`.
- [ ] Font-sizes, weights e cores de texto secundárias neutras migraram para Tailwind (ex: `text-surface-500`, `font-bold`).
- [ ] `:deep(.p-password)` continua com `position: relative` e largura garantidas no CSS scoped para evitar sobreposição do ícone de olho.
- [ ] Compilação com `npm run type-check` não gera bugs de tipagem ou exportação.
