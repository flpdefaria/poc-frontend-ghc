<script setup lang="ts">
import { ref } from 'vue'
import LoginCard from './components/login/LoginCard.vue'
import LoginHero from './components/login/LoginHero.vue'
import RegisterCard from './components/register/RegisterCard.vue'
import RegisterHero from './components/register/RegisterHero.vue'
import ForgotPasswordCard from './components/forgot-password/ForgotPasswordCard.vue'
import ForgotPasswordHero from './components/forgot-password/ForgotPasswordHero.vue'

type View = 'login' | 'register' | 'forgot-password'
const view = ref<View>('login')

type LoginPayload = {
  email: string
  password: string
  remember: boolean
}

type RegisterPayload = {
  name: string
  email: string
  password: string
  terms: boolean
}

const onLogin = (payload: LoginPayload) => {
  console.log('Login requested', { email: payload.email, remember: payload.remember })
}

const onRegister = (payload: RegisterPayload) => {
  console.log('Register requested', { name: payload.name, email: payload.email })
}

const onForgotPassword = (payload: { email: string }) => {
  console.log('Reset requested', { email: payload.email })
}
</script>

<template>
  <main class="login-page">
    <template v-if="view === 'login'">
      <LoginHero />
      <LoginCard
        @submit="onLogin"
        @go-to-register="view = 'register'"
        @go-to-forgot-password="view = 'forgot-password'"
      />
    </template>
    <template v-else-if="view === 'register'">
      <RegisterHero />
      <RegisterCard @submit="onRegister" @go-to-login="view = 'login'" />
    </template>
    <template v-else>
      <ForgotPasswordHero />
      <ForgotPasswordCard @submit="onForgotPassword" @go-to-login="view = 'login'" />
    </template>
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
