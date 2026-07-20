<script setup lang="ts">
import { ref } from 'vue'
import AuthLayout from '../layouts/AuthLayout.vue'
import LoginCard from '../components/login/LoginCard.vue'
import LoginHero from '../components/login/LoginHero.vue'
import type { AuthUser, LoginPayload } from '../types/auth.types'
import { AuthService } from '../services/auth.service'
import { ApiError } from '../services/api'

const emit = defineEmits<{
  goToRegister: []
  goToForgotPassword: []
  loginSuccess: [user: AuthUser]
}>()

const loading = ref(false)
const errorMessage = ref('')
const successMessage = ref('')

const onLogin = async (payload: LoginPayload) => {
  loading.value = true
  errorMessage.value = ''
  successMessage.value = ''
  try {
    const user = await AuthService.login(payload)
    successMessage.value = `Welcome back, ${user.name}!`
    emit('loginSuccess', user)
  } catch (error) {
    errorMessage.value = error instanceof ApiError ? error.message : 'Something went wrong. Please try again.'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <AuthLayout>
    <LoginHero />
    <LoginCard
      :loading="loading"
      :error-message="errorMessage"
      :success-message="successMessage"
      @submit="onLogin"
      @go-to-register="emit('goToRegister')"
      @go-to-forgot-password="emit('goToForgotPassword')"
    />
  </AuthLayout>
</template>
