<script setup lang="ts">
import LoginCard from './components/login/LoginCard.vue'
import LoginHero from './components/login/LoginHero.vue'

type LoginPayload = {
  email: string
  password: string
}

const onLogin = (payload: LoginPayload) => {
  console.log('Login requested', { email: payload.email })
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
  width: min(1080px, 100%);
  margin: 0 auto;
  display: grid;
  grid-template-columns: 1.1fr 1fr;
  gap: 2rem;
  align-items: center;
  padding: 2.5rem 1rem;
}

@media (max-width: 900px) {
  .login-page {
    grid-template-columns: 1fr;
    width: min(560px, 100%);
    padding-top: 1.5rem;
  }
}
</style>
