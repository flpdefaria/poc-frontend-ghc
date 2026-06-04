<script setup lang="ts">
import { ref } from 'vue'
import Button from 'primevue/button'
import Card from 'primevue/card'
import FloatLabel from 'primevue/floatlabel'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'

const emit = defineEmits<{
  submit: [payload: { email: string; password: string }]
}>()

const email = ref('')
const password = ref('')

const onSubmit = () => {
  emit('submit', {
    email: email.value,
    password: password.value
  })
}
</script>

<template>
  <Card class="login-card">
    <template #title>
      <h2 class="title">Login</h2>
    </template>
    <template #subtitle>
      <p class="description">Access your workspace and continue your journey.</p>
    </template>
    <template #content>
      <form class="login-form" @submit.prevent="onSubmit">
        <FloatLabel variant="on">
          <InputText id="email" v-model="email" type="email" fluid />
          <label for="email">Email</label>
        </FloatLabel>

        <FloatLabel variant="on">
          <Password id="password" v-model="password" :feedback="false" toggle-mask fluid />
          <label for="password">Password</label>
        </FloatLabel>

        <Button type="submit" label="Login" icon="pi pi-sign-in" class="login-button" fluid />

        <a href="#" class="create-account">For create account</a>
      </form>
    </template>
  </Card>
</template>

<style scoped>
.login-card {
  border-radius: 24px;
  border: 1px solid var(--p-content-border-color);
  backdrop-filter: blur(10px);
  background: color-mix(in srgb, var(--p-surface-0) 86%, transparent);
  box-shadow: 0 24px 50px color-mix(in srgb, var(--p-primary-950) 30%, transparent);
}

.title {
  font-size: 1.6rem;
  font-weight: 700;
  color: var(--p-text-color);
}

.description {
  color: var(--p-text-muted-color);
}

.login-form {
  margin-top: 0.5rem;
  display: grid;
  gap: 1.2rem;
}

.login-button {
  margin-top: 0.1rem;
}

.create-account {
  justify-self: center;
  color: var(--p-primary-color);
  text-decoration: none;
  font-weight: 600;
}

.create-account:hover {
  color: var(--p-primary-700);
  text-decoration: underline;
}
</style>
