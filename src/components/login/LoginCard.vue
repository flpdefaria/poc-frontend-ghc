<script setup lang="ts">
import { ref } from 'vue'
import Button from 'primevue/button'
import Card from 'primevue/card'
import Checkbox from 'primevue/checkbox'
import FloatLabel from 'primevue/floatlabel'
import InputText from 'primevue/inputtext'

const emit = defineEmits<{
  submit: [payload: { email: string; password: string; remember: boolean }]
}>()

const email = ref('')
const password = ref('')
const remember = ref(true)

const onSubmit = () => {
  emit('submit', {
    email: email.value,
    password: password.value,
    remember: remember.value
  })
}
</script>

<template>
  <Card class="login-card">
    <template #title>
      <div class="header">
        <h2 class="title">Sign in</h2>
        <p class="description">Enter your credentials to access your account.</p>
      </div>
    </template>

    <template #content>
      <form class="login-form" @submit.prevent="onSubmit">
        <FloatLabel variant="on">
          <InputText id="email" v-model="email" type="email" autocomplete="email" fluid />
          <label for="email">Email</label>
        </FloatLabel>

        <FloatLabel variant="on">
          <InputText
            id="password"
            v-model="password"
            type="password"
            autocomplete="current-password"
            fluid
          />
          <label for="password">Password</label>
        </FloatLabel>

        <div class="form-row">
          <label class="remember">
            <Checkbox v-model="remember" :binary="true" inputId="remember" />
            <span>Remember me</span>
          </label>

          <Button
            label="Forgot password?"
            variant="link"
            size="small"
            class="forgot"
            severity="secondary"
          />
        </div>

        <Button
          type="submit"
          label="Sign in"
          icon="pi pi-arrow-right"
          icon-pos="right"
          class="login-button"
          fluid
        />

        <div class="or-separator" role="separator" aria-label="or">
          <span class="or-line" />
          <span class="or-label">or</span>
          <span class="or-line" />
        </div>

        <Button
          type="button"
          label="Create an account"
          icon="pi pi-user-plus"
          variant="outlined"
          severity="secondary"
          class="create-account"
          fluid
        />
      </form>
    </template>
  </Card>
</template>

<style scoped>
.login-card {
  border-radius: 22px;
  border: 1px solid color-mix(in srgb, var(--p-surface-0) 35%, transparent);
  background: color-mix(in srgb, var(--p-surface-0) 94%, transparent);
  backdrop-filter: blur(18px);
  box-shadow:
    0 1px 0 color-mix(in srgb, var(--p-surface-0) 60%, transparent) inset,
    0 24px 60px color-mix(in srgb, var(--p-primary-950) 28%, transparent);
  width: min(420px, 100%);
  justify-self: center;
}

.login-card :deep(.p-card-body) {
  padding: 2.25rem 2rem;
  gap: 1rem;
}

.header {
  display: grid;
  gap: 0.4rem;
}

.title {
  font-size: 1.65rem;
  font-weight: 700;
  letter-spacing: -0.01em;
  color: var(--p-primary-700);
  margin: 0;
}

.description {
  color: var(--p-text-muted-color);
  font-size: 0.95rem;
  margin: 0;
}

.login-form {
  margin-top: 0.75rem;
  display: grid;
  gap: 1.15rem;
}

.form-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 1rem;
  margin-top: -0.25rem;
}

.remember {
  display: inline-flex;
  align-items: center;
  gap: 0.55rem;
  font-size: 0.9rem;
  color: var(--p-text-muted-color);
  cursor: pointer;
  user-select: none;
}

.forgot {
  padding: 0 !important;
  color: var(--p-primary-600) !important;
}

.login-button {
  margin-top: 0.25rem;
  font-weight: 600;
  letter-spacing: 0.01em;
}

.divider {
  margin: 0.1rem 0;
}

.or-separator {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  margin: 0.2rem 0;
}

.or-line {
  flex: 1;
  height: 1px;
  background: color-mix(in srgb, var(--p-content-border-color) 80%, transparent);
}

.or-label {
  color: var(--p-text-muted-color);
  font-size: 0.8rem;
  text-transform: uppercase;
  letter-spacing: 0.18em;
}

.divider-label {
  color: var(--p-text-muted-color);
  font-size: 0.8rem;
  text-transform: uppercase;
  letter-spacing: 0.18em;
}

.create-account {
  font-weight: 600;
}

.login-form :deep(.p-inputtext) {
  background: color-mix(in srgb, var(--p-primary-50) 28%, var(--p-surface-0));
  border-color: color-mix(in srgb, var(--p-primary-200) 45%, var(--p-content-border-color));
  border-radius: 12px;
  padding: 0.95rem 0.9rem;
  transition:
    border-color 0.2s ease,
    box-shadow 0.2s ease,
    background-color 0.2s ease;
}

.login-form :deep(.p-inputtext:hover) {
  border-color: color-mix(in srgb, var(--p-primary-300) 60%, var(--p-content-border-color));
  background: color-mix(in srgb, var(--p-primary-50) 40%, var(--p-surface-0));
}

.login-form :deep(.p-inputtext:focus) {
  border-color: var(--p-primary-400);
  background: var(--p-surface-0);
  box-shadow: 0 0 0 0.22rem color-mix(in srgb, var(--p-primary-300) 28%, transparent);
}

.login-form :deep(.p-floatlabel label) {
  color: color-mix(in srgb, var(--p-text-muted-color) 75%, var(--p-primary-500));
  background: transparent !important;
  padding: 0 0.35rem;
}

.login-form :deep(.p-floatlabel-on label),
.login-form :deep(.p-floatlabel input:focus ~ label),
.login-form :deep(.p-floatlabel input.p-filled ~ label),
.login-form :deep(.p-floatlabel input:not(:placeholder-shown) ~ label) {
  background: var(--p-surface-0) !important;
  color: var(--p-primary-600) !important;
}

.login-form :deep(.p-button) {
  border-radius: 12px;
  padding: 0.85rem 1rem;
}

.login-form :deep(.p-checkbox .p-checkbox-box) {
  background: color-mix(in srgb, var(--p-primary-50) 28%, var(--p-surface-0));
  border-color: color-mix(in srgb, var(--p-primary-200) 60%, var(--p-content-border-color));
  border-radius: 6px;
}

.login-form :deep(.p-checkbox:not(.p-disabled):hover .p-checkbox-box) {
  border-color: var(--p-primary-400);
}

.login-form :deep(.p-checkbox.p-checkbox-checked .p-checkbox-box) {
  background: var(--p-primary-500);
  border-color: var(--p-primary-500);
}
</style>
