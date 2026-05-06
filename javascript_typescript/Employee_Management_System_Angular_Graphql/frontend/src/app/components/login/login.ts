import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Apollo } from 'apollo-angular';
import { Auth } from '../../services/auth/auth';
import { Router, RouterLink } from '@angular/router';
import { LOGIN_USER } from '../../graphql/operations';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html'
})
export class Login {
  private fb = inject(FormBuilder);
  private apollo = inject(Apollo);
  private auth = inject(Auth);
  private router = inject(Router);

  error = signal<string | null>(null);
  isLoading = signal<boolean>(false);
  
  loginForm = this.fb.group({
    usernameOrEmail: ['', [Validators.required]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  onSubmit(){
    if (this.loginForm.valid){
      this.error.set(null);
      this.isLoading.set(true);

      const {usernameOrEmail, password} = this.loginForm.value;

      this.apollo.query<any>({
        query: LOGIN_USER,
        variables: { 
          usernameOrEmail: usernameOrEmail ?? '', 
          password: password ?? '' 
        },
        fetchPolicy: 'no-cache'
      }).subscribe({
        next: (result) => {
          this.isLoading.set(false);
          const payload = result.data?.login;
          
          if (payload && payload.token) {
            this.auth.setSession(payload);
            this.router.navigate(['/employees']);
          }
        },
        error: (err) => {
          this.isLoading.set(false);
          const errMsg = err.graphQLErrors?.[0]?.message || "Invalid Credentials. Try again.";
          this.error.set(errMsg);
          console.error("Login Error:", err);
        }
      })
    }
  }
}
