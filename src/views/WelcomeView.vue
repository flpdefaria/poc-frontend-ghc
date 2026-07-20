<script setup lang="ts">
import Button from 'primevue/button'
import Card from 'primevue/card'
import type { AuthUser } from '../types/auth.types'

defineProps<{
  user: AuthUser
}>()

const emit = defineEmits<{
  logout: []
}>()
</script>

<template>
  <div class="min-h-screen flex items-center justify-center px-5 py-8">
    <Card class="w-full max-w-[440px] mx-auto welcome-card">
      <template #content>
        <div class="flex flex-col items-center text-center gap-5 py-2">
          <span class="w-16 h-16 rounded-full flex items-center justify-center welcome-icon">
            <i class="pi pi-check text-3xl" />
          </span>

          <div class="flex flex-col gap-1.5">
            <h1 class="text-2xl font-bold tracking-tight text-primary-700 m-0">
              Welcome, <span class="font-bold">{{ user.name }}</span>!
            </h1>
            <p class="text-sm text-surface-500 m-0">You're signed in as {{ user.email }}.</p>
          </div>

          <Button
            label="Sign out"
            icon="pi pi-sign-out"
            variant="outlined"
            severity="secondary"
            class="w-full font-semibold mt-2"
            fluid
            @click="emit('logout')"
          />
        </div>
      </template>
    </Card>
  </div>
</template>

<style scoped>
.welcome-card {
  border-radius: 22px;
  border: 1px solid color-mix(in srgb, var(--p-surface-0) 35%, transparent);
  background: color-mix(in srgb, var(--p-surface-0) 94%, transparent);
  backdrop-filter: blur(18px);
  box-shadow:
    0 1px 0 color-mix(in srgb, var(--p-surface-0) 60%, transparent) inset,
    0 24px 60px color-mix(in srgb, var(--p-primary-950) 28%, transparent);
}

.welcome-card :deep(.p-card-body) {
  padding: 2.25rem 2rem;
}

.welcome-icon {
  background: color-mix(in srgb, var(--p-primary-100) 40%, var(--p-surface-0));
  border: 1px solid color-mix(in srgb, var(--p-primary-300) 40%, transparent);
  color: var(--p-primary-600);
}

.welcome-card :deep(.p-button-outlined:not(:disabled):hover) {
  transform: translateY(-2px);
  background: color-mix(in srgb, var(--p-primary-100) 35%, transparent) !important;
  border-color: color-mix(in srgb, var(--p-primary-400) 60%, var(--p-content-border-color)) !important;
  box-shadow: 0 6px 18px color-mix(in srgb, var(--p-primary-400) 20%, transparent);
  transition:
    transform 0.15s ease,
    background 0.15s ease,
    border-color 0.15s ease,
    box-shadow 0.15s ease;
}

.welcome-card :deep(.p-button-outlined:not(:disabled):active) {
  transform: translateY(0);
  box-shadow: none;
}
</style>
