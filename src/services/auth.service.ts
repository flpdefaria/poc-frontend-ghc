import { postJson } from './api'
import type {
  AuthUser,
  ApiMessageResponse,
  LoginPayload,
  RegisterPayload,
  ForgotPasswordPayload
} from '../types/auth.types'

export const AuthService = {
  login(payload: LoginPayload): Promise<AuthUser> {
    return postJson<AuthUser>('/login', payload)
  },

  register(payload: RegisterPayload): Promise<AuthUser> {
    return postJson<AuthUser>('/register', payload)
  },

  forgotPassword(payload: ForgotPasswordPayload): Promise<ApiMessageResponse> {
    return postJson<ApiMessageResponse>('/forgot-password', payload)
  }
}
