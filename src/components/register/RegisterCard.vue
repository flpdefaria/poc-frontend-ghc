<script setup lang="ts">
import { ref, computed } from 'vue'
import Button from 'primevue/button'
import Card from 'primevue/card'
import Checkbox from 'primevue/checkbox'
import FloatLabel from 'primevue/floatlabel'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'

const emit = defineEmits<{
  submit: [payload: { name: string; email: string; password: string; terms: boolean }]
  goToLogin: []
}>()

const name = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const terms = ref(false)
const submitted = ref(false)

const errors = computed(() => ({
  name: !name.value.trim() ? 'Full name is required.' : '',
  email: !email.value.trim()
    ? 'Email is required.'
    : !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value)
      ? 'Enter a valid email address.'
      : '',
  password: !password.value
    ? 'Password is required.'
    : password.value.length < 8
      ? 'Password must be at least 8 characters.'
      : '',
  confirmPassword: !confirmPassword.value
    ? 'Please confirm your password.'
    : confirmPassword.value !== password.value
      ? 'Passwords do not match.'
      : '',
  terms: !terms.value ? 'You must accept the terms to continue.' : ''
}))

const hasErrors = computed(() => Object.values(errors.value).some(Boolean))

const onSubmit = () => {
  submitted.value = true
  if (hasErrors.value) return
  emit('submit', {
    name: name.value,
    email: email.value,
    password: password.value,
    terms: terms.value
  })
}
</script>

<template>
  <Card class="register-card">
    <template #title>
      <div class="header">
        <h2 class="title">Create account</h2>
        <p class="description">Fill in the details below to get started.</p>
      </div>
    </template>

    <template #content>
      <form class="register-form" @submit.prevent="onSubmit">
        <div class="field">
          <FloatLabel>
            <InputText
              id="name"
              v-model="name"
              type="text"
              autocomplete="name"
              fluid
              :invalid="submitted && !!errors.name"
            />
            <label for="name">Full name</label>
          </FloatLabel>
          <small v-if="submitted && errors.name" class="field-error">
            <i class="pi pi-exclamation-circle" /> {{ errors.name }}
          </small>
        </div>

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
              autocomplete="new-password"
              fluid
              :invalid="submitted && !!errors.password"
            />
            <label for="password">Password</label>
          </FloatLabel>
          <small v-if="submitted && errors.password" class="field-error">
            <i class="pi pi-exclamation-circle" /> {{ errors.password }}
          </small>
        </div>

        <div class="field">
          <FloatLabel>
            <Password
              v-model="confirmPassword"
              inputId="confirm-password"
              toggleMask
              :feedback="false"
              autocomplete="new-password"
              fluid
              :invalid="submitted && !!errors.confirmPassword"
            />
            <label for="confirm-password">Confirm password</label>
          </FloatLabel>
          <small v-if="submitted && errors.confirmPassword" class="field-error">
            <i class="pi pi-exclamation-circle" /> {{ errors.confirmPassword }}
          </small>
        </div>

        <div class="terms-field">
          <label class="terms">
            <Checkbox
              v-model="terms"
              :binary="true"
              inputId="terms"
              :invalid="submitted && !!errors.terms"
            />
            <span>
              I agree to the
              <a href="#" class="terms-link">Terms of Service</a>
              and
              <a href="#" class="terms-link">Privacy Policy</a>
            </span>
          </label>
          <small v-if="submitted && errors.terms" class="field-error">
            <i class="pi pi-exclamation-circle" /> {{ errors.terms }}
          </small>
        </div>

        <Button
          type="submit"
          label="Create account"
          icon="pi pi-arrow-right"
          icon-pos="right"
          class="submit-button"
          fluid
        />

        <div class="or-separator" role="separator" aria-label="or">
          <span class="or-line" />
          <span class="or-label">or</span>
          <span class="or-line" />
        </div>

        <Button
          type="button"
          label="Already have an account? Sign in"
          icon="pi pi-sign-in"
          variant="outlined"
          severity="secondary"
          class="sign-in-button"
          fluid
          @click="emit('goToLogin')"
        />
      </form>
    </template>
  </Card>
</template>

<style scoped>
.register-card {
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

.register-card :deep(.p-card-body) {
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

.register-form {
  margin-top: 1.25rem;
  display: grid;
  gap: 1.75rem;
}

.field {
  display: grid;
  gap: 0.4rem;
}

.terms-field {
  display: grid;
  gap: 0.5rem;
  margin-top: -0.5rem;
}

.terms {
  display: inline-flex;
  align-items: flex-start;
  gap: 0.6rem;
  font-size: 0.9rem;
  color: var(--p-text-muted-color);
  cursor: pointer;
  user-select: none;
  line-height: 1.5;
}

.field-error {
  display: flex;
  align-items: center;
  gap: 0.35rem;
  color: var(--p-red-500);
  font-size: 0.8rem;
}

.terms-link {
  color: var(--p-primary-600);
  text-decoration: none;
  font-weight: 500;
}

.terms-link:hover {
  text-decoration: underline;
}

.submit-button {
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
  background: color-mix(in srgb, var(--p-content-border-color) 80%, transparent);
}

.or-label {
  color: var(--p-text-muted-color);
  font-size: 0.8rem;
  text-transform: uppercase;
  letter-spacing: 0.18em;
}

.sign-in-button {
  font-weight: 600;
}

.register-form :deep(.p-password) {
  width: 100%;
  position: relative;
}

.register-form :deep(.p-password-toggle-mask-icon) {
  color: var(--p-surface-400);
  transition: color 0.15s ease;
}

.register-form :deep(.p-password-toggle-mask-icon:hover) {
  color: var(--p-primary-600);
}

.register-form :deep(.p-inputtext) {
  background: color-mix(in srgb, var(--p-primary-50) 28%, var(--p-surface-0));
  border-color: color-mix(in srgb, var(--p-primary-200) 45%, var(--p-content-border-color));
  color: var(--p-surface-700);
  transition:
    border-color 0.2s ease,
    box-shadow 0.2s ease,
    background-color 0.2s ease;
}

.register-form :deep(.p-inputtext:hover) {
  border-color: color-mix(in srgb, var(--p-primary-300) 60%, var(--p-content-border-color));
  background: color-mix(in srgb, var(--p-primary-50) 40%, var(--p-surface-0));
}

.register-form :deep(.p-inputtext:focus) {
  border-color: var(--p-primary-400);
  background: var(--p-surface-0);
}

.register-form :deep(.p-floatlabel label) {
  color: var(--p-surface-400);
  background: transparent !important;
  padding: 0;
}

.register-form :deep(.p-floatlabel input:focus ~ label),
.register-form :deep(.p-floatlabel input.p-filled ~ label) {
  background: transparent !important;
  color: var(--p-surface-500) !important;
}

.register-form :deep(.p-inputtext::placeholder) {
  color: var(--p-surface-400);
}

.register-form :deep(.p-button) {
  padding: 0.85rem 1rem;
  transition:
    background 0.2s ease,
    border-color 0.2s ease,
    box-shadow 0.2s ease,
    transform 0.15s ease;
}

.register-form :deep(.submit-button:not(:disabled):hover) {
  transform: translateY(-2px);
  box-shadow: 0 8px 24px color-mix(in srgb, var(--p-primary-500) 45%, transparent);
}

.register-form :deep(.submit-button:not(:disabled):active) {
  transform: translateY(0);
  box-shadow: 0 2px 8px color-mix(in srgb, var(--p-primary-500) 30%, transparent);
}

.register-form :deep(.sign-in-button:not(:disabled):hover) {
  transform: translateY(-2px);
  background: color-mix(in srgb, var(--p-primary-100) 35%, transparent) !important;
  border-color: color-mix(in srgb, var(--p-primary-400) 60%, var(--p-content-border-color)) !important;
  box-shadow: 0 6px 18px color-mix(in srgb, var(--p-primary-400) 20%, transparent);
}

.register-form :deep(.sign-in-button:not(:disabled):active) {
  transform: translateY(0);
  box-shadow: none;
}

.register-form :deep(.p-inputtext.p-invalid) {
  border-color: var(--p-red-400) !important;
}

.register-form :deep(.p-inputtext.p-invalid:focus) {
  border-color: var(--p-red-400) !important;
}

.register-form :deep(.p-checkbox .p-checkbox-box) {
  background: var(--p-surface-0);
  border: 1px solid color-mix(in srgb, var(--p-primary-200) 60%, var(--p-content-border-color));
  border-radius: 6px;
  transition:
    border-color 0.15s ease,
    background-color 0.15s ease;
}

.register-form :deep(.p-checkbox:not(.p-disabled):hover .p-checkbox-box) {
  background: var(--p-surface-0);
  border-color: var(--p-primary-500);
}

.register-form :deep(.p-checkbox.p-checkbox-checked .p-checkbox-box),
.register-form :deep(.p-checkbox.p-checkbox-checked:hover .p-checkbox-box),
.register-form :deep(.p-checkbox.p-checkbox-checked.p-focus .p-checkbox-box) {
  background: var(--p-surface-0) !important;
  border-color: var(--p-primary-500) !important;
}

.register-form :deep(.p-checkbox.p-checkbox-checked .p-checkbox-icon),
.register-form :deep(.p-checkbox.p-checkbox-checked:hover .p-checkbox-icon),
.register-form :deep(.p-checkbox.p-checkbox-checked.p-focus .p-checkbox-icon) {
  color: var(--p-primary-500) !important;
  fill: var(--p-primary-500) !important;
}
</style>
