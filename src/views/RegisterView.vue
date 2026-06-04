<script setup lang="ts">
import AuthLayout from '../layouts/AuthLayout.vue'
import RegisterCard from '../components/register/RegisterCard.vue'
import RegisterHero from '../components/register/RegisterHero.vue'
import type { RegisterPayload } from '../types/auth.types'

const emit = defineEmits<{
  goToLogin: []
}>()

const onRegister = (payload: RegisterPayload) => {
  console.log('Register requested', { name: payload.name, email: payload.email })
}
</script>

<template>
  <AuthLayout>
    <RegisterHero />
    <RegisterCard
      @submit="onRegister"
      @go-to-login="emit('goToLogin')"
    />
  </AuthLayout>
</template>
