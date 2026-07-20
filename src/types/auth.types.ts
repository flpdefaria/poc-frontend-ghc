export type AuthView = 'login' | 'register' | 'forgot-password' | 'welcome'

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

export type AuthUser = {
  id: string
  name: string
  email: string
}

export type ApiMessageResponse = {
  message: string
}
