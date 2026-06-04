<script setup lang="ts">
import AuthLayout from '../layouts/AuthLayout.vue'
import ForgotPasswordCard from '../components/forgot-password/ForgotPasswordCard.vue'
import ForgotPasswordHero from '../components/forgot-password/ForgotPasswordHero.vue'
import type { ForgotPasswordPayload } from '../types/auth.types'

const emit = defineEmits<{
  goToLogin: []
}>()

const onForgotPassword = (payload: ForgotPasswordPayload) => {
  console.log('Reset requested', { email: payload.email })
}
</script>

<template>
  <AuthLayout>
    <ForgotPasswordHero />
    <ForgotPasswordCard
      @submit="onForgotPassword"
      @go-to-login="emit('goToLogin')"
    />
  </AuthLayout>
</template>
