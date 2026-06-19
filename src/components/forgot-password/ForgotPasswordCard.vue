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
  <Card class="w-full max-w-[420px] mx-auto forgot-card">
    <template #title>
      <div class="flex flex-col gap-1.5">
        <h2 class="text-2xl font-bold tracking-tight text-surface-900 m-0">Reset password</h2>
        <p class="text-sm text-surface-500 m-0">
          Enter your email and we'll send you a link to reset your password.
        </p>
      </div>
    </template>

    <template #content>
      <!-- Success state -->
      <div v-if="success" class="mt-5 flex flex-col gap-5 items-center text-center">
        <span class="w-14 h-14 rounded-full flex items-center justify-center success-icon">
          <i class="pi pi-check text-2xl" />
        </span>
        <h3 class="text-lg font-bold text-surface-900 m-0">Check your inbox</h3>
        <p class="text-sm text-surface-500 max-w-sm m-0 leading-relaxed">
          We sent a password reset link to <strong>{{ email }}</strong>. It may take a few minutes to arrive.
        </p>
        <Button
          label="Back to Sign in"
          icon="pi pi-arrow-left"
          variant="outlined"
          severity="secondary"
          class="w-full font-semibold"
          fluid
          @click="emit('goToLogin')"
        />
      </div>

      <!-- Form state -->
      <form v-else class="mt-5 flex flex-col gap-7" @submit.prevent="onSubmit">
        <div class="flex flex-col gap-1.5">
          <FloatLabel class="w-full">
            <InputText
              id="email"
              v-model="email"
              type="email"
              autocomplete="email"
              fluid
              :invalid="submitted && !!errors.email"
              class="w-full"
            />
            <label for="email">Email</label>
          </FloatLabel>
          <small v-if="submitted && errors.email" class="flex items-center gap-1.5 text-xs text-red-500">
            <i class="pi pi-exclamation-circle" /> {{ errors.email }}
          </small>
        </div>

        <Button
          type="submit"
          label="Send reset link"
          icon="pi pi-send"
          icon-pos="right"
          class="mt-1 font-semibold tracking-wide"
          fluid
        />

        <div class="flex items-center gap-3.5 my-1" role="separator" aria-label="or">
          <span class="flex-1 h-px bg-surface-200" />
          <span class="text-xs uppercase text-surface-400 tracking-[0.18em]">or</span>
          <span class="flex-1 h-px bg-surface-200" />
        </div>

        <Button
          type="button"
          label="Back to Sign in"
          icon="pi pi-arrow-left"
          variant="outlined"
          severity="secondary"
          class="font-semibold uppercase tracking-[0.18em] text-xs"
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
}

.forgot-card :deep(.p-card-body) {
  padding: 2.25rem 2rem;
  gap: 1rem;
}

.success-icon {
  background: color-mix(in srgb, var(--p-primary-100) 40%, var(--p-surface-0));
  border: 1px solid color-mix(in srgb, var(--p-primary-300) 40%, transparent);
  color: var(--p-primary-600);
}

/* Inputs */
.forgot-card :deep(.p-inputtext) {
  background: var(--p-surface-0);
  border-color: color-mix(in srgb, var(--p-primary-200) 45%, var(--p-content-border-color));
  color: var(--p-surface-700);
  transition:
    border-color 0.2s ease,
    box-shadow 0.2s ease,
    background-color 0.2s ease;
}

.forgot-card :deep(.p-inputtext:hover) {
  border-color: color-mix(in srgb, var(--p-primary-300) 60%, var(--p-content-border-color));
  background: var(--p-surface-0);
}

.forgot-card :deep(.p-inputtext:focus) {
  border-color: var(--p-primary-400);
  background: var(--p-surface-0);
}

.forgot-card :deep(.p-floatlabel label) {
  color: var(--p-surface-400);
  background: transparent !important;
  padding: 0;
}

.forgot-card :deep(.p-floatlabel input:focus ~ label),
.forgot-card :deep(.p-floatlabel input.p-filled ~ label) {
  background: transparent !important;
  color: var(--p-surface-500) !important;
}

.forgot-card :deep(.p-inputtext.p-invalid) {
  border-color: var(--p-red-400) !important;
}

/* Buttons */
.forgot-card :deep(.p-button) {
  padding: 0.85rem 1rem;
  transition:
    background 0.2s ease,
    border-color 0.2s ease,
    box-shadow 0.2s ease,
    transform 0.15s ease;
}

.forgot-card :deep(.p-button:not(.p-button-link):not(.p-button-outlined):not(:disabled):hover) {
  transform: translateY(-2px);
  box-shadow: 0 8px 24px color-mix(in srgb, var(--p-primary-500) 45%, transparent);
}

.forgot-card :deep(.p-button:not(.p-button-link):not(.p-button-outlined):not(:disabled):active) {
  transform: translateY(0);
  box-shadow: 0 2px 8px color-mix(in srgb, var(--p-primary-500) 30%, transparent);
}

.forgot-card :deep(.p-button-outlined:not(:disabled):hover) {
  transform: translateY(-2px);
  background: color-mix(in srgb, var(--p-primary-100) 35%, transparent) !important;
  border-color: color-mix(in srgb, var(--p-primary-400) 60%, var(--p-content-border-color)) !important;
  box-shadow: 0 6px 18px color-mix(in srgb, var(--p-primary-400) 20%, transparent);
}

.forgot-card :deep(.p-button-outlined:not(:disabled):active) {
  transform: translateY(0);
  box-shadow: none;
}
</style>
