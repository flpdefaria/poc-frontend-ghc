<script setup lang="ts">
import Tag from 'primevue/tag'

const features = [
  { icon: 'pi pi-user', text: 'Free account, no credit card' },
  { icon: 'pi pi-lock', text: 'Your data stays private & secure' },
  { icon: 'pi pi-star', text: 'Full access from day one' }
]
</script>

<template>
  <section class="hero-panel">
    <Tag value="Get started" rounded class="eyebrow" />

    <h1 class="hero-title">
      Create your account<br />
      <span class="hero-title-accent">in seconds.</span>
    </h1>

    <p class="hero-copy">
      Join thousands of users already building beautiful apps — it only takes a moment.
    </p>

    <ul class="feature-list">
      <li v-for="feature in features" :key="feature.text" class="feature">
        <span class="feature-icon">
          <i :class="feature.icon" />
        </span>
        <span>{{ feature.text }}</span>
      </li>
    </ul>
  </section>
</template>

<style scoped>
.hero-panel {
  color: color-mix(in srgb, var(--p-surface-0) 92%, var(--p-primary-50));
  display: grid;
  gap: 1.5rem;
  max-width: 30rem;
}

.eyebrow {
  justify-self: start;
  background: color-mix(in srgb, var(--p-primary-400) 22%, transparent) !important;
  color: color-mix(in srgb, var(--p-surface-0) 92%, var(--p-primary-100)) !important;
  border: 1px solid color-mix(in srgb, var(--p-primary-300) 30%, transparent);
  backdrop-filter: blur(6px);
}

.hero-title {
  font-size: clamp(2.4rem, 5.4vw, 3.6rem);
  line-height: 1.08;
  font-weight: 700;
  letter-spacing: -0.02em;
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
  font-size: 1.05rem;
  line-height: 1.6;
  color: color-mix(in srgb, var(--p-surface-0) 75%, var(--p-primary-200));
}

.feature-list {
  list-style: none;
  padding: 0;
  margin: 0.5rem 0 0;
  display: grid;
  gap: 0.85rem;
}

.feature {
  display: flex;
  align-items: center;
  gap: 0.85rem;
  color: color-mix(in srgb, var(--p-surface-0) 88%, var(--p-primary-100));
  font-size: 0.95rem;
}

.feature-icon {
  width: 1.85rem;
  height: 1.85rem;
  flex-shrink: 0;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  border-radius: 999px;
  background: color-mix(in srgb, var(--p-primary-400) 22%, transparent);
  border: 1px solid color-mix(in srgb, var(--p-primary-300) 28%, transparent);
  color: var(--p-primary-100);
  font-size: 0.85rem;
}

@media (max-width: 900px) {
  .hero-panel {
    text-align: center;
    justify-items: center;
    margin: 0 auto;
  }

  .eyebrow {
    justify-self: center;
  }

  .feature-list {
    justify-items: start;
  }
}
</style>
