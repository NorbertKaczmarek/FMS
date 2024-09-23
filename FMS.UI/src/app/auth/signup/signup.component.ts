import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css',
})
export class SignupComponent {
  error: string = null;

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(form: NgForm) {
    if (!form.valid) return;
    const email = form.value.email;
    const fullName = form.value.fullName;
    const password = form.value.password;
    const confirmPassword = form.value.confirmPassword;

    if (password == confirmPassword) {
      this.authService
        .signup(email, fullName, password, confirmPassword)
        .subscribe(
          () => {
            this.router.navigate(['login']);
            form.reset();
          },
          (errorMessage) => {
            this.error = errorMessage;
          }
        );
    } else {
      this.error = 'Passwords are different.';
    }
  }
}
