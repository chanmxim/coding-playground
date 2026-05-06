import { inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private router = inject(Router)

  session = signal<any>(JSON.parse(localStorage.getItem('session') || 'null'));

  setSession(payload: any) {
    localStorage.setItem('session', JSON.stringify(payload));
    this.session.set(payload);
  }

  logout() {
    localStorage.removeItem('session');
    this.session.set(null);
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    return !!this.session();
  }

  getToken(): string | null {
    return this.session()?.token || null;
  }
}
