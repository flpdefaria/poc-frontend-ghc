<script setup lang="ts">
import { ref } from 'vue'
import type { AuthView } from './types/auth.types'
import LoginView from './views/LoginView.vue'
import RegisterView from './views/RegisterView.vue'
import ForgotPasswordView from './views/ForgotPasswordView.vue'

const view = ref<AuthView>('login')
</script>

<template>
  <LoginView
    v-if="view === 'login'"
    @go-to-register="view = 'register'"
    @go-to-forgot-password="view = 'forgot-password'"
  />
  <RegisterView
    v-else-if="view === 'register'"
    @go-to-login="view = 'login'"
  />
  <ForgotPasswordView
    v-else
    @go-to-login="view = 'login'"
  />
</template>
