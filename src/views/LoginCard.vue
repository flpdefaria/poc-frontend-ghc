<script setup lang="ts">
import { ref, computed } from 'vue'
import Button from 'primevue/button'
import Card from 'primevue/card'
import Checkbox from 'primevue/checkbox'
import FloatLabel from 'primevue/floatlabel'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'

const emit = defineEmits<{
  submit: [payload: { email: string; password: string; remember: boolean }]
  goToRegister: []
  goToForgotPassword: []
}>()

const email = ref('')
const password = ref('')
const remember = ref(true)
const submitted = ref(false)

const errors = computed(() => ({
  email: !email.value.trim()
    ? 'Email is required.'
    : !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value)
      ? 'Enter a valid email address.'
      : '',
  password: !password.value ? 'Password is required.' : ''
}))

const hasErrors = computed(() => Object.values(errors.value).some(Boolean))

const onSubmit = () => {
  submitted.value = true
  if (hasErrors.value) return
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
        <div class="field">
          <FloatLabel>
            <InputText
              id="email"
              v-model="email"
              type="email"
              autocomplete="email"
              fluid
              :invalid="submitted && !!errors.email"
            />
            <label for="email">Email</label>
          </FloatLabel>
          <small v-if="submitted && errors.email" class="field-error">
            <i class="pi pi-exclamation-circle" /> {{ errors.email }}
          </small>
        </div>

        <div class="field">
          <FloatLabel>
            <Password
              v-model="password"
              inputId="password"
              toggleMask
              :feedback="false"
              autocomplete="current-password"
              fluid
              :invalid="submitted && !!errors.password"
            />
            <label for="password">Password</label>
          </FloatLabel>
          <small v-if="submitted && errors.password" class="field-error">
            <i class="pi pi-exclamation-circle" /> {{ errors.password }}
          </small>
        </div>

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
            @click="emit('goToForgotPassword')"
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
          @click="emit('goToRegister')"
        />
      </form>
    </template>
  </Card>
</template>

<style scoped>
.login-card {
  border-radius: 22px;
  border: 1px solid rgba(255, 255, 255, 0.35);
  background: rgba(255, 255, 255, 0.94);
  backdrop-filter: blur(18px);
  box-shadow:
    0 1px 0 rgba(255, 255, 255, 0.6) inset,
    0 24px 60px rgba(23, 37, 84, 0.28);
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
  color: #1d4ed8;
  margin: 0;
}

.description {
  color: #6b7280;
  font-size: 0.95rem;
  margin: 0;
}

.login-form {
  margin-top: 1.25rem;
  display: grid;
  gap: 1.75rem;
}

.field {
  display: grid;
  gap: 0.4rem;
}

.field-error {
  display: flex;
  align-items: center;
  gap: 0.35rem;
  color: #ef4444;
  font-size: 0.8rem;
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
  color: #6b7280;
  cursor: pointer;
  user-select: none;
}

.forgot {
  padding: 0 !important;
  color: #2563eb !important;
}

.login-button {
  margin-top: 0.25rem;
  font-weight: 600;
  letter-spacing: 0.01em;
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
  background: rgba(209, 213, 219, 0.8);
}

.or-label {
  color: #6b7280;
  font-size: 0.8rem;
  text-transform: uppercase;
  letter-spacing: 0.18em;
}

.create-account {
  font-weight: 600;
}

.login-form :deep(.p-password) {
  width: 100%;
  position: relative;
}

.login-form :deep(.p-password-toggle-mask-icon) {
  color: #9ca3af;
  transition: color 0.15s ease;
}

.login-form :deep(.p-password-toggle-mask-icon:hover) {
  color: #2563eb;
}

.login-form :deep(.p-inputtext) {
  background: #ffffff;
  border-color: #93c5fd;
  color: #374151;
  padding: 0.95rem 0.9rem;
  border-radius: 12px;
  transition:
    border-color 0.2s ease,
    box-shadow 0.2s ease,
    background-color 0.2s ease;
}

.login-form :deep(.p-inputtext:hover) {
  border-color: #60a5fa;
  background: #ffffff;
}

.login-form :deep(.p-inputtext:focus) {
  border-color: #60a5fa;
  background: #ffffff;
  box-shadow: 0 0 0 0.22rem rgba(147, 197, 253, 0.28);
}

.login-form :deep(.p-floatlabel label) {
  color: #9ca3af;
  background: transparent !important;
  padding: 0;
}

.login-form :deep(.p-floatlabel input:focus ~ label),
.login-form :deep(.p-floatlabel input.p-filled ~ label) {
  background: transparent !important;
  color: #6b7280 !important;
}

.login-form :deep(.p-inputtext::placeholder) {
  color: #9ca3af;
}

.login-form :deep(.p-button) {
  padding: 0.85rem 1rem;
  transition:
    background 0.2s ease,
    border-color 0.2s ease,
    box-shadow 0.2s ease,
    transform 0.15s ease;
}

.login-form :deep(.login-button:not(:disabled):hover) {
  transform: translateY(-2px);
  box-shadow: 0 8px 24px rgba(59, 130, 246, 0.45);
}

.login-form :deep(.login-button:not(:disabled):active) {
  transform: translateY(0);
  box-shadow: 0 2px 8px rgba(59, 130, 246, 0.30);
}

.login-form :deep(.create-account:not(:disabled):hover) {
  transform: translateY(-2px);
  background: rgba(219, 234, 254, 0.35) !important;
  border-color: rgba(96, 165, 250, 0.6) !important;
  box-shadow: 0 6px 18px rgba(96, 165, 250, 0.20);
}

.login-form :deep(.create-account:not(:disabled):active) {
  transform: translateY(0);
  box-shadow: none;
}

.login-form :deep(.p-inputtext.p-invalid) {
  border-color: #f87171 !important;
}

.login-form :deep(.p-checkbox .p-checkbox-box) {
  background: #ffffff;
  border: 1px solid #93c5fd;
  border-radius: 6px;
  transition:
    border-color 0.15s ease,
    background-color 0.15s ease;
}

.login-form :deep(.p-checkbox:not(.p-disabled):hover .p-checkbox-box) {
  background: #ffffff;
  border-color: #3b82f6;
}

.login-form :deep(.p-checkbox.p-checkbox-checked .p-checkbox-box),
.login-form :deep(.p-checkbox.p-checkbox-checked:hover .p-checkbox-box),
.login-form :deep(.p-checkbox.p-checkbox-checked.p-focus .p-checkbox-box) {
  background: #ffffff !important;
  border-color: #3b82f6 !important;
}

.login-form :deep(.p-checkbox.p-checkbox-checked .p-checkbox-icon),
.login-form :deep(.p-checkbox.p-checkbox-checked:hover .p-checkbox-icon),
.login-form :deep(.p-checkbox.p-checkbox-checked.p-focus .p-checkbox-icon) {
  color: #3b82f6 !important;
  fill: #3b82f6 !important;
}
</style>
