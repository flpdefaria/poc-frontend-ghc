# Artigo 2: Arquitetura de Estilo Híbrida com PrimeVue 4 e Tailwind CSS v4 sob Governança de IA

## Introdução

No desenvolvimento moderno de front-end com Vue 3, nos deparamos frequentemente com um dilema doloroso:
1. **Ir com tudo no Tailwind CSS:** O que gera marcas de utilidades poluídas, repetições exaustivas de classes complexas de glassmorphism em telas de autenticação e perda de produtividade ao tentar customizar profundamente componentes complexos.
2. **Utilizar somente CSS Customizado:** O que gera arquivos gigantescos, repetições obsessivas de definições de layout (como grids flexíveis com paddings e margins de cartões) e regras `@media` verbosas para design responsivo.

Neste artigo prático, demonstraremos como desenhamos e implementamos um **Modelo de Governança de Estilo Híbrido** de alta fidelidade técnica integrando **Vue 3**, **PrimeVue 4** e o novíssimo **Tailwind CSS v4** (impulsionado pelo `@tailwindcss/vite` de alto desempenho). Mais do que isso: mostramos como automatizamos essa arquitetura através de **Agentes de IA e Skills de Governança corporativas no GitHub Copilot** para garantir consistência perfeita em projetos distribuídos.

---

## 1. O Conceito: Governança de Estilo Híbrida (Hybrid Governance Rule)

A essência do modelo é uma **separação pragmática de responsabilidades estéticas**:

* **Tailwind CSS v4** assume 100% da responsabilidade por layout estrutural, espaçamento fluido (paddings, margins, flexbox, grids, gaps), tipografia padrão de conteúdo e micro-alinhamentos responsivos.
* **Component-Scoped CSS (combinado com Design Tokens do PrimeVue)** assume o gerenciamento de estilizações de marca complexas — como o efeito de vidro (*glassmorphism*), sombras brilhantes (*glow projections*), filtros dinâmicos de backdrop — além de sobrescrever estados e elementos internos profundos do PrimeVue (usando `:deep()`).

Esta tabela de responsabilidades foi definida de forma estrita no corpo das nossas diretrizes globais:

| Responsabilidade | Tecnologia Indicada | Exemplo Prático |
|---|---|---|
| Layout, Spacing & Alinhamentos | **Tailwind CSS** | `flex`, `grid`, `gap-4`, `p-6`, `mx-auto` |
| Font-sizes & Pesos Secundários | **Tailwind CSS** | `text-sm`, `font-bold`, `tracking-tight` |
| Cores e Fundos de Cards/Telas | **CSS Scoped** via tokens | `var(--p-surface-0)` com `color-mix()` |
| Efeitos Visuais (Glassmorphism) | **CSS Scoped** | `backdrop-filter: blur(18px)` |
| Overrides de Inputs PrimeVue | **CSS Scoped** | `:deep(.p-password)` |
| Hexadecimal Strings Cruas | **Estritamente Proibido** | Usar tokens semânticos e `color-mix` |

---

## 2. A Fundação Técnica: Instalando Tailwind CSS v4 no Vite

O Tailwind v4 traz uma arquitetura reconstruída de raiz mais rápida, integrando-se nativamente como um plugin do Vite sem necessidade de um arquivo `tailwind.config.js` externo separado.

### Passo 1: Instalação das Dependências

Instalamos o Tailwind CSS v4, seu plugin para o Vite, e a integração PrimeVue:

```bash
npm install tailwindcss @tailwindcss/vite tailwindcss-primeui --save-dev
```

### Passo 2: Configuração no Vite (`vite.config.ts`)

O plugin `@tailwindcss/vite` é registrado diretamente no fluxo de pipelines:

```ts
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import tailwindcss from '@tailwindcss/vite'

export default defineConfig({
  plugins: [
    vue(),
    tailwindcss(),
  ],
})
```

### Passo 3: Injeção de Diretivas de Importação CSS (`src/assets/main.css`)

O CSS global importa de forma limpa o Tailwind e a integração oficial para sincronizar variáveis de core semântico:

```css
@import "tailwindcss";
@import "tailwindcss-primeui";

@import './base.css';
```

---

## 3. A Governança Codificada: Criando a Skill de IA

Para evitar que o desenvolvedor ou o próprio Agente Autónomo de IA violem essa separação, nós criamos uma **Skill de IA Especializada** sob [.github/skills/create-tailwind-migration/SKILL.md](.github/skills/create-tailwind-migration/SKILL.md). Ela atua como um blueprint de tradução de design.

Olhe o nível de detalhe de um blueprint lado a lado ensinado para a IA:

```markdown
### Exemplo de Refatoração de Layout (Ex: AuthLayout.vue)

#### Antes (CSS Customizado Verboso):
.auth-layout {
  min-height: 100vh;
  width: min(1140px, 100%);
  margin: 0 auto;
  display: grid;
  grid-template-columns: 1.05fr 1fr;
  gap: 4rem;
  align-items: center;
  padding: 3rem 1.5rem;
}

#### Depois (Classes Híbridas Limpas):
<template>
  <main class="grid grid-cols-1 lg:grid-cols-[1.05fr_1fr] gap-10 lg:gap-16 items-center w-full max-w-[480px] lg:max-w-[1140px] mx-auto min-h-screen px-5 py-8 lg:px-6 lg:py-12">
    <slot />
  </main>
</template>
```

---

## 4. O Resultado da Refatoração Real

### 4.1 Layout de Autenticação Centralizado
Nosso [AuthLayout.vue](src/layouts/AuthLayout.vue) foi limpo de 100% dos seus seletores customizados e `@media` queries manuais. Toda a estrutura tornou-se um único elemento governado por classes responsivas da nova engine do Tailwind v4:

```vue
<template>
  <main class="grid grid-cols-1 lg:grid-cols-[1.05fr_1fr] gap-10 lg:gap-16 items-center w-full max-w-[480px] lg:max-w-[1140px] mx-auto min-h-screen px-5 py-8 lg:px-6 lg:py-12">
    <slot />
  </main>
</template>
```

### 4.2 O Hero da Tela de Login (`LoginHero.vue`)
Eliminamos mais de 80 linhas de CSS scoped repetitivo usando classes flexíveis de grid e espaçamento:

```vue
<template>
  <section class="flex flex-col gap-6 max-w-[30rem] lg:text-left text-center items-center lg:items-start mx-auto lg:mx-0">
    <Tag value="Welcome back" rounded class="lg:self-start self-center eyebrow" />
    <h1 class="text-4xl lg:text-5xl font-bold tracking-tight leading-[1.08] hero-title">
      Sign in to continue<br />
      <span class="hero-title-accent">your journey.</span>
    </h1>
    ...
  </section>
</template>
```

### 4.3 O Cartão de Formulário Glassmorphism (`LoginCard.vue`)
Aqui reside o brilho do modelo híbrido. O formulário interno usa as classes fluidas `flex flex-col gap-7` e margins utilitárias para organizar perfeitamente os inputs. A estética de Glassmorphism elegante, por outro lado, permanece garantida pelo CSS Scoped, protegida contra poluição visual de classes gigantescas e complexas de gradiente em lote:

```vue
<template>
  <Card class="w-full max-w-[420px] mx-auto login-card">
    <template #title>
      <div class="flex flex-col gap-1.5">
        <h2 class="text-2xl font-bold tracking-tight text-surface-900 m-0">Sign in</h2>
        <p class="text-sm text-surface-500 m-0">Enter your credentials to access your account.</p>
      </div>
    </template>

    <template #content>
      <form class="mt-5 flex flex-col gap-7" @submit.prevent="onSubmit">
        <div class="flex flex-col gap-1.5">
          <FloatLabel class="w-full">
            <InputText id="email" v-model="email" type="email" fluid class="w-full" />
            <label for="email">Email</label>
          </FloatLabel>
        </div>
        ...
      </form>
    </template>
  </Card>
</template>

<style scoped>
/* Glassmorphism mantido em scoped para consistência de alto nível */
.login-card {
  border-radius: 22px;
  border: 1px solid color-mix(in srgb, var(--p-surface-0) 35%, transparent);
  background: color-mix(in srgb, var(--p-surface-0) 94%, transparent);
  backdrop-filter: blur(18px);
  box-shadow:
    0 1px 0 color-mix(in srgb, var(--p-surface-0) 60%, transparent) inset,
    0 24px 60px color-mix(in srgb, var(--p-primary-950) 28%, transparent);
}
/* Alinhamento de senha absoluto mantido no scoped */
.login-card :deep(.p-password) {
  width: 100%;
  position: relative;
}
</style>
```

---

## 5. Validação de Build e Compilação

Para confirmar que a migração não introduziu nenhum vazamento de renderização ou conflitos de tipagem estrita com as novas bibliotecas, executamos o comando de verificação rigorosa:

```bash
npm run type-check
```

**Resultado:** `vue-tsc --build` finalizado com código de saída `0` de forma impecável, provando que o TypeScript stricto do Vue 3.5 funciona perfeitamente com os estilos compostos do PrimeVue + Tailwind v4.

---

## Conclusão

Ao adotarmos uma postura de **Governança Híbrida**, resolvemos um dos maiores gargalos de manutenabilidade de interfaces modernas de front-end. O Tailwind CSS v4 nos garante o poder de orquestrar layouts rápidos, responsivos e fáceis de editar, enquanto o Scoped CSS garante que a alma visual de marca — as cores perfeitas, as texturas glassmorphism e as nuances dos interágicos do PrimeVue — permaneça blindada contra desvios estéticos.

Com as regras automatizadas sob a pasta `.github/` do projeto, garantimos que qualquer inteligência artificial agindo no código se comporte exatamente de acordo com essas premissas, salvaguardando a excelência do código a cada nova feature.
