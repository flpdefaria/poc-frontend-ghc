<script setup lang="ts">
import { ref } from 'vue'
import type { AuthUser, AuthView } from './types/auth.types'
import LoginView from './views/LoginView.vue'
import RegisterView from './views/RegisterView.vue'
import ForgotPasswordView from './views/ForgotPasswordView.vue'
import WelcomeView from './views/WelcomeView.vue'

const view = ref<AuthView>('login')
const loggedInUser = ref<AuthUser | null>(null)

const onLoginSuccess = (user: AuthUser) => {
  loggedInUser.value = user
  view.value = 'welcome'
}

const onLogout = () => {
  loggedInUser.value = null
  view.value = 'login'
}
</script>

<template>
  <LoginView
    v-if="view === 'login'"
    @go-to-register="view = 'register'"
    @go-to-forgot-password="view = 'forgot-password'"
    @login-success="onLoginSuccess"
  />
  <RegisterView
    v-else-if="view === 'register'"
    @go-to-login="view = 'login'"
  />
  <ForgotPasswordView
    v-else-if="view === 'forgot-password'"
    @go-to-login="view = 'login'"
  />
  <WelcomeView
    v-else-if="view === 'welcome' && loggedInUser"
    :user="loggedInUser"
    @logout="onLogout"
  />
</template>
