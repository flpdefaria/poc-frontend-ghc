import './assets/main.css'
import 'primeicons/primeicons.css'

import { createApp } from 'vue'
import PrimeVue from 'primevue/config'
import Aura from '@primeuix/themes/aura'
import { definePreset } from '@primeuix/themes'
import App from './App.vue'

const AppPreset = definePreset(Aura, {
	semantic: {
		primary: {
			50: '{blue.50}',
			100: '{blue.100}',
			200: '{blue.200}',
			300: '{blue.300}',
			400: '{blue.400}',
			500: '{blue.500}',
			600: '{blue.600}',
			700: '{blue.700}',
			800: '{blue.800}',
			900: '{blue.900}',
			950: '{blue.950}'
		},
		formField: {
			paddingX: '0.9rem',
			paddingY: '0.95rem',
			borderRadius: '12px'
		},
		focusRing: {
			width: '0',
			style: 'none',
			color: 'transparent',
			offset: '0',
			shadow: '0 0 0 0.22rem color-mix(in srgb, {primary.300} 28%, transparent)'
		}
	}
})

const app = createApp(App)

app.use(PrimeVue, {
	theme: {
		preset: AppPreset
	}
})

app.mount('#app')
