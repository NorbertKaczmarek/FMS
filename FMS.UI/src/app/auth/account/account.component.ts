import { Component, OnInit } from '@angular/core';
import { User } from '../user.model';
import { AuthService } from '../auth.service';
import { Title } from '@angular/platform-browser';
import jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrl: './account.component.css',
})
export class AccountComponent implements OnInit {
  user?: User;
  id: number;

  constructor(private authService: AuthService, private titleService: Title) {}

  ngOnInit(): void {
    this.authService.getAccountInfo().subscribe((user: User) => {
      this.user = user;
      this.titleService.setTitle(`User: ${this.user.fullName}`);
    });
    // this.authService.token.subscribe((token) => {
    //   this.id =
    //     +this.decodeToken(token)[
    //       'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
    //     ];

    //   this.authService.getAccountInfo().subscribe((user: User) => {
    //     this.user = user;
    //     this.titleService.setTitle(`User: ${this.user.fullName}`);
    //   });
    // });
  }

  public decodeToken(token: string) {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join('')
    );

    return JSON.parse(jsonPayload);
  }

  // ngOnInit2(): void {
  //   this.route.paramMap
  //     .pipe(
  //       switchMap((params) => {
  //         this.id = Number(params.get('id'));
  //         return this.flightService.getFlightById(this.id);
  //       })
  //     )
  //     .subscribe((flight: Flight) => {
  //       this.flight = flight;
  //       this.titleService.setTitle(
  //         `${this.flight.numerLotu}: ${this.flight.miejsceWylotu} -> ${this.flight.miejscePrzylotu}`
  //       );
  //     });
  // }
}
