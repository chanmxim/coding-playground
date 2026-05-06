import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Apollo } from 'apollo-angular';
import { Auth } from '../../services/auth/auth';
import { Router, RouterLink } from '@angular/router';
import { SIGNUP_USER } from '../../graphql/operations';

@Component({
  selector: 'app-signup',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './signup.html',
})
export class Signup {
  private fb = inject(FormBuilder);
  private apollo = inject(Apollo);
  private auth = inject(Auth);
  private router = inject(Router);

  error = signal<string | null>(null);
  isLoading = signal<boolean>(false);
  
  signupForm = this.fb.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  onSubmit(){
    if (this.signupForm.valid){
      this.error.set(null);
      this.isLoading.set(true);

      const {username, email, password} = this.signupForm.value;

      this.apollo.mutate<any>({
        mutation: SIGNUP_USER,
        variables: { 
          username: username ?? '', 
          email: email ?? '', 
          password: password ?? '' 
        },
      }).subscribe({
        next: (result) => {
          this.isLoading.set(false);

          if (result.data?.signup) {
            this.auth.setSession(result.data.signup);
            this.router.navigate(['/employees']);
          }
        },
        error: (err) => {
          this.isLoading.set(false);
          const errMsg = err.graphQLErrors?.[0]?.message || "Signup failed. Try again.";
          this.error.set(errMsg);
          console.error("Login Error:", err);
        }
      })
    }
  }
}
