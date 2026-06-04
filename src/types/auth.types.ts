export type AuthView = 'login' | 'register' | 'forgot-password'

export type LoginPayload = {
  email: string
  password: string
  remember: boolean
}

export type RegisterPayload = {
  name: string
  email: string
  password: string
  terms: boolean
}

export type ForgotPasswordPayload = {
  email: string
}
