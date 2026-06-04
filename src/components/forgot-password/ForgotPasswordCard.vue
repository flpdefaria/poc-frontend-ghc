<script setup lang="ts">
import { ref, computed } from 'vue'
import Button from 'primevue/button'
import Card from 'primevue/card'
import FloatLabel from 'primevue/floatlabel'
import InputText from 'primevue/inputtext'

const emit = defineEmits<{
  submit: [payload: { email: string }]
  goToLogin: []
}>()

const email = ref('')
const submitted = ref(false)
const success = ref(false)

const errors = computed(() => ({
  email: !email.value.trim()
    ? 'Email is required.'
    : !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value)
      ? 'Enter a valid email address.'
      : ''
}))

const hasErrors = computed(() => Object.values(errors.value).some(Boolean))

const onSubmit = () => {
  submitted.value = true
  if (hasErrors.value) return
  success.value = true
  emit('submit', { email: email.value })
}
</script>

<template>
  <Card class="forgot-card">
    <template #title>
      <div class="header">
        <h2 class="title">Reset password</h2>
        <p class="description">
          Enter your email and we'll send you a link to reset your password.
        </p>
      </div>
    </template>

    <template #content>
      <!-- Success state -->
      <div v-if="success" class="success-state">
        <span class="success-icon">
          <i class="pi pi-check" />
        </span>
        <h3 class="success-title">Check your inbox</h3>
        <p class="success-copy">
          We sent a password reset link to <strong>{{ email }}</strong>. It may take a few minutes to arrive.
        </p>
        <Button
          label="Back to Sign in"
          icon="pi pi-arrow-left"
          variant="outlined"
          severity="secondary"
          class="back-button"
          fluid
          @click="emit('goToLogin')"
        />
      </div>

      <!-- Form state -->
      <form v-else class="forgot-form" @submit.prevent="onSubmit">
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

        <Button
          type="submit"
          label="Send reset link"
          icon="pi pi-send"
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
          label="Back to Sign in"
          icon="pi pi-arrow-left"
          variant="outlined"
          severity="secondary"
          class="back-button"
          fluid
          @click="emit('goToLogin')"
        />
      </form>
    </template>
  </Card>
</template>

<style scoped>
.forgot-card {
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

.forgot-card :deep(.p-card-body) {
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

/* Form */
.forgot-form {
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
  color: var(--p-red-500);
  font-size: 0.8rem;
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

.submit-button {
  font-weight: 600;
  letter-spacing: 0.01em;
}

.back-button {
  font-weight: 600;
}

/* Success state */
.success-state {
  margin-top: 1.25rem;
  display: grid;
  gap: 1.25rem;
  justify-items: center;
  text-align: center;
}

.success-icon {
  width: 3.5rem;
  height: 3.5rem;
  border-radius: 999px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  background: color-mix(in srgb, var(--p-primary-100) 40%, var(--p-surface-0));
  border: 1px solid color-mix(in srgb, var(--p-primary-300) 40%, transparent);
  color: var(--p-primary-600);
  font-size: 1.4rem;
}

.success-title {
  font-size: 1.1rem;
  font-weight: 700;
  color: var(--p-primary-700);
  margin: 0;
}

.success-copy {
  color: var(--p-text-muted-color);
  font-size: 0.92rem;
  line-height: 1.55;
  margin: 0;
}

.success-copy strong {
  color: var(--p-text-color);
  font-weight: 600;
}

/* Inputs */
.forgot-form :deep(.p-inputtext) {
  background: color-mix(in srgb, var(--p-primary-50) 28%, var(--p-surface-0));
  border-color: color-mix(in srgb, var(--p-primary-200) 45%, var(--p-content-border-color));
  border-radius: 12px;
  padding: 0.95rem 0.9rem;
  color: var(--p-surface-700);
  transition:
    border-color 0.2s ease,
    box-shadow 0.2s ease,
    background-color 0.2s ease;
}

.forgot-form :deep(.p-inputtext:hover) {
  border-color: color-mix(in srgb, var(--p-primary-300) 60%, var(--p-content-border-color));
  background: color-mix(in srgb, var(--p-primary-50) 40%, var(--p-surface-0));
}

.forgot-form :deep(.p-inputtext:focus) {
  border-color: var(--p-primary-400);
  background: var(--p-surface-0);
  box-shadow: 0 0 0 0.22rem color-mix(in srgb, var(--p-primary-300) 28%, transparent);
}

.forgot-form :deep(.p-floatlabel label) {
  color: var(--p-surface-400);
  background: transparent !important;
  padding: 0;
}

.forgot-form :deep(.p-floatlabel input:focus ~ label),
.forgot-form :deep(.p-floatlabel input.p-filled ~ label) {
  background: transparent !important;
  color: var(--p-surface-500) !important;
}

.forgot-form :deep(.p-inputtext::placeholder) {
  color: var(--p-surface-400);
}

.forgot-form :deep(.p-inputtext.p-invalid) {
  border-color: var(--p-red-400) !important;
}

.forgot-form :deep(.p-inputtext.p-invalid:focus) {
  border-color: var(--p-red-400) !important;
  box-shadow: 0 0 0 0.22rem color-mix(in srgb, var(--p-red-300) 30%, transparent);
}

/* Buttons */
.forgot-form :deep(.p-button),
.success-state :deep(.p-button) {
  border-radius: 12px;
  padding: 0.85rem 1rem;
  transition:
    background 0.2s ease,
    border-color 0.2s ease,
    box-shadow 0.2s ease,
    transform 0.15s ease;
}

.forgot-form :deep(.submit-button:not(:disabled):hover) {
  transform: translateY(-2px);
  box-shadow: 0 8px 24px color-mix(in srgb, var(--p-primary-500) 45%, transparent);
}

.forgot-form :deep(.submit-button:not(:disabled):active) {
  transform: translateY(0);
  box-shadow: 0 2px 8px color-mix(in srgb, var(--p-primary-500) 30%, transparent);
}

.forgot-form :deep(.back-button:not(:disabled):hover),
.success-state :deep(.back-button:not(:disabled):hover) {
  transform: translateY(-2px);
  background: color-mix(in srgb, var(--p-primary-100) 35%, transparent) !important;
  border-color: color-mix(in srgb, var(--p-primary-400) 60%, var(--p-content-border-color)) !important;
  box-shadow: 0 6px 18px color-mix(in srgb, var(--p-primary-400) 20%, transparent);
}

.forgot-form :deep(.back-button:not(:disabled):active),
.success-state :deep(.back-button:not(:disabled):active) {
  transform: translateY(0);
  box-shadow: none;
}
</style>
