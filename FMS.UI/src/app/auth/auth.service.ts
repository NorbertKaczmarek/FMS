import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, catchError, tap, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from './user.model';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private url = 'auth';
  token = new BehaviorSubject<string>(null);
  private tokenExpirationTimer: any;

  constructor(private http: HttpClient, private router: Router) {}

  signup(
    email: string,
    fullName: string,
    password: string,
    confirmPassword: string
  ) {
    return this.http
      .post<any>(`${environment.apiUrl}/${this.url}/signup`, {
        email: email,
        fullName: fullName,
        password: password,
        confirmPassword: confirmPassword,
      })
      .pipe(catchError(this.handleError));
  }

  login(email: string, password: string) {
    return this.http
      .post(
        `${environment.apiUrl}/${this.url}/login`,
        {
          email: email,
          password: password,
        },
        {
          responseType: 'text',
        }
      )
      .pipe(
        catchError(this.handleError),
        tap((authToken) => {
          localStorage.setItem('authToken', authToken);
          localStorage.setItem('email', email);
          this.token.next(authToken);
        })
      );
  }

  autoLogin() {
    const authToken: string = localStorage.getItem('authToken');

    if (!authToken) {
      return;
    }

    if (authToken) {
      this.token.next(authToken);
      const decoded: any = jwtDecode(authToken);

      let expDate = new Date(0);
      expDate.setUTCSeconds(decoded.exp);

      const expirationDuration =
        new Date(expDate).getTime() - new Date().getTime();
      this.autoLogout(expirationDuration);
    }
  }

  logout() {
    this.token.next(null);
    this.router.navigate(['/login']);
    localStorage.removeItem('authToken');
    localStorage.removeItem('email');
    if (this.tokenExpirationTimer) {
      clearTimeout(this.tokenExpirationTimer);
    }
  }

  autoLogout(expirationDuration: number) {
    this.tokenExpirationTimer = setTimeout(() => {
      this.logout();
    }, expirationDuration);
  }

  getAccountInfo(): Observable<User> {
    return this.http.get<User>(`${environment.apiUrl}/${this.url}/account`);
  }

  private handleError(errorRes: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred!';
    if (!errorRes.error) {
      return throwError(errorMessage);
    }
    return throwError(errorRes.error);
  }
}
