<template>
  <div class="hero-panel">
    <p class="eyebrow">PrimeVue App</p>
    <h1 class="hero-title">Welcome Back</h1>
    <p class="hero-copy">Access your workspace and continue your journey.</p>
  </div>
</template>

<style scoped>
.hero-panel {
  color: #f8fafc;
}

.eyebrow {
  font-size: 0.85rem;
  letter-spacing: 0.2em;
  text-transform: uppercase;
  opacity: 0.85;
  margin-bottom: 1rem;
}

.hero-title {
  font-size: clamp(2.4rem, 6vw, 4rem);
  line-height: 1.04;
  margin-bottom: 0.9rem;
  font-weight: 700;
}

.hero-copy {
  font-size: 1.05rem;
  max-width: 36ch;
  opacity: 0.92;
}

@media (max-width: 900px) {
  .hero-panel {
    text-align: center;
  }

  .hero-copy {
    margin: 0 auto;
  }
}
</style>
