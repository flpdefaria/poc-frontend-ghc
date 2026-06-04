<script setup lang="ts">
import LoginCard from './components/login/LoginCard.vue'
import LoginHero from './components/login/LoginHero.vue'

type LoginPayload = {
  email: string
  password: string
  remember: boolean
}

const onLogin = (payload: LoginPayload) => {
  console.log('Login requested', { email: payload.email, remember: payload.remember })
}
</script>

<template>
  <main class="login-page">
    <LoginHero />
    <LoginCard @submit="onLogin" />
  </main>
</template>

<style scoped>
.login-page {
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
  .login-page {
    grid-template-columns: 1fr;
    width: min(480px, 100%);
    gap: 2.5rem;
    padding: 2rem 1.25rem;
  }
}
</style>
