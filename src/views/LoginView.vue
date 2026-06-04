<script setup lang="ts">
import AuthLayout from '../layouts/AuthLayout.vue'
import LoginCard from '../components/login/LoginCard.vue'
import LoginHero from '../components/login/LoginHero.vue'
import type { LoginPayload } from '../types/auth.types'

const emit = defineEmits<{
  goToRegister: []
  goToForgotPassword: []
}>()

const onLogin = (payload: LoginPayload) => {
  console.log('Login requested', { email: payload.email, remember: payload.remember })
}
</script>

<template>
  <AuthLayout>
    <LoginHero />
    <LoginCard
      @submit="onLogin"
      @go-to-register="emit('goToRegister')"
      @go-to-forgot-password="emit('goToForgotPassword')"
    />
  </AuthLayout>
</template>
