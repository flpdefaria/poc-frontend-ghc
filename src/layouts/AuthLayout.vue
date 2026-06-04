<template>
  <main class="auth-layout">
    <slot />
  </main>
</template>

<style scoped>
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

@media (max-width: 900px) {
  .auth-layout {
    grid-template-columns: 1fr;
    width: min(480px, 100%);
    gap: 2.5rem;
    padding: 2rem 1.25rem;
  }
}
</style>
