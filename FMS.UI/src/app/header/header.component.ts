import { Component } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { Subscription } from 'rxjs';
import { User } from '../auth/user.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  isAuthenticated = true; // false
  email: string = null;

  private userSub: Subscription;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.userSub = this.authService.token.subscribe((token) => {
      this.isAuthenticated = !!token; // !token ? false : true;
      if (this.isAuthenticated){
        this.email = localStorage.getItem('email')
      }
    });
  }

  ngOnDestroy(): void {
    this.userSub.unsubscribe();
  }

  onLogout() {
    this.authService.logout();
  }

  onDemoLogin() {
    const email = 'demo1@demo.com';
    const fullName = 'demo1';
    const password = 'demo1@demo.com';
    const confirmPassword = 'demo1@demo.com';

    this.authService
      .signup(email, fullName, password, confirmPassword)
      .subscribe(
        () => {
          this.authService.login(email, password).subscribe(
            (token: string) => {
              localStorage.setItem('authToken', token);
              this.router.navigate(['flight']);
            },
            (errorMessage) => {}
          );
        },
        (errorMessage) => {
          this.authService.login(email, password).subscribe(
            (token: string) => {
              localStorage.setItem('authToken', token);
              this.router.navigate(['flight']);
            },
            (errorMessage) => {}
          );
        }
      );
  }
}
