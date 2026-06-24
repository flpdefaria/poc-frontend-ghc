<script setup lang="ts">
import Tag from 'primevue/tag'

const features = [
  { icon: 'pi pi-bolt', text: 'Lightning-fast workflows' },
  { icon: 'pi pi-shield', text: 'Enterprise-grade security' },
  { icon: 'pi pi-sparkles', text: 'Beautifully crafted experience' }
]
</script>

<template>
  <section class="flex flex-col gap-6 max-w-[30rem] lg:text-left text-center items-center lg:items-start mx-auto lg:mx-0">
    <Tag value="Welcome back" rounded class="lg:self-start self-center eyebrow" />

    <h1 class="text-4xl lg:text-5xl font-bold tracking-tight leading-[1.08] hero-title">
      Sign in to continue<br />
      <span class="font-normal hero-title-accent">your journey.</span>
    </h1>

    <p class="text-[1.05rem] leading-relaxed hero-copy">
      Access your workspace and pick up exactly where you left off — with a polished
      experience powered by our platform.
    </p>

    <ul class="flex flex-col gap-3.5 w-full items-start lg:items-start justify-center lg:justify-start">
      <li v-for="feature in features" :key="feature.text" class="flex items-center gap-3.5 text-sm feature-text">
        <span class="w-[1.85rem] h-[1.85rem] flex items-center justify-center rounded-full feature-icon">
          <i :class="feature.icon" />
        </span>
        <span>{{ feature.text }}</span>
      </li>
    </ul>
  </section>
</template>

<style scoped>
.eyebrow {
  background: color-mix(in srgb, var(--p-primary-400) 22%, transparent) !important;
  color: color-mix(in srgb, var(--p-surface-0) 92%, var(--p-primary-100)) !important;
  border: 1px solid color-mix(in srgb, var(--p-primary-300) 30%, transparent);
  backdrop-filter: blur(6px);
}

.hero-title {
  color: color-mix(in srgb, var(--p-surface-0) 98%, var(--p-primary-50));
}

.hero-title-accent {
  background: linear-gradient(
    120deg,
    color-mix(in srgb, var(--p-primary-200) 90%, white),
    color-mix(in srgb, var(--p-primary-400) 80%, white)
  );
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
}

.hero-copy {
  color: color-mix(in srgb, var(--p-surface-0) 75%, var(--p-primary-200));
}

.feature-text {
  color: color-mix(in srgb, var(--p-surface-0) 88%, var(--p-primary-100));
}

.feature-icon {
  background: color-mix(in srgb, var(--p-primary-400) 22%, transparent);
  border: 1px solid color-mix(in srgb, var(--p-primary-300) 28%, transparent);
  color: var(--p-primary-100);
  font-size: 0.85rem;
}
</style>
