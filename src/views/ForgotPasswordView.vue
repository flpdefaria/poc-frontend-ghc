<script setup lang="ts">
import { ref } from 'vue'
import AuthLayout from '../layouts/AuthLayout.vue'
import ForgotPasswordCard from '../components/forgot-password/ForgotPasswordCard.vue'
import ForgotPasswordHero from '../components/forgot-password/ForgotPasswordHero.vue'
import type { ForgotPasswordPayload } from '../types/auth.types'
import { AuthService } from '../services/auth.service'
import { ApiError } from '../services/api'

const emit = defineEmits<{
  goToLogin: []
}>()

const loading = ref(false)
const errorMessage = ref('')
const success = ref(false)

const onForgotPassword = async (payload: ForgotPasswordPayload) => {
  loading.value = true
  errorMessage.value = ''
  try {
    await AuthService.forgotPassword(payload)
    success.value = true
  } catch (error) {
    errorMessage.value = error instanceof ApiError ? error.message : 'Something went wrong. Please try again.'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <AuthLayout>
    <ForgotPasswordHero />
    <ForgotPasswordCard
      :loading="loading"
      :error-message="errorMessage"
      :success="success"
      @submit="onForgotPassword"
      @go-to-login="emit('goToLogin')"
    />
  </AuthLayout>
</template>
