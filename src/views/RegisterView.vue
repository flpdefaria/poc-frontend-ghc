<script setup lang="ts">
import { ref } from 'vue'
import AuthLayout from '../layouts/AuthLayout.vue'
import RegisterCard from '../components/register/RegisterCard.vue'
import RegisterHero from '../components/register/RegisterHero.vue'
import type { RegisterPayload } from '../types/auth.types'
import { AuthService } from '../services/auth.service'
import { ApiError } from '../services/api'

const emit = defineEmits<{
  goToLogin: []
}>()

const loading = ref(false)
const errorMessage = ref('')
const successMessage = ref('')

const onRegister = async (payload: RegisterPayload) => {
  loading.value = true
  errorMessage.value = ''
  successMessage.value = ''
  try {
    const user = await AuthService.register(payload)
    successMessage.value = `Account created for ${user.email}. You can now sign in.`
  } catch (error) {
    errorMessage.value = error instanceof ApiError ? error.message : 'Something went wrong. Please try again.'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <AuthLayout>
    <RegisterHero />
    <RegisterCard
      :loading="loading"
      :error-message="errorMessage"
      :success-message="successMessage"
      @submit="onRegister"
      @go-to-login="emit('goToLogin')"
    />
  </AuthLayout>
</template>
