import { Component, OnInit } from '@angular/core';
import { FlightService } from '../flight.service';
import { Flight } from '../flight.model';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-flight-detail',
  templateUrl: './flight-detail.component.html',
  styleUrl: './flight-detail.component.css',
})
export class FlightDetailComponent implements OnInit {
  flight?: Flight = null;
  id: number;
  error: string = null;

  constructor(
    private flightService: FlightService,
    private route: ActivatedRoute,
    private router: Router,
    private titleService: Title
  ) {}

  ngOnInit(): void {
    this.route.paramMap
      .pipe(
        switchMap((params) => {
          this.id = Number(params.get('id'));
          return this.flightService.getFlightById(this.id);
        })
      )
      .subscribe((flight: Flight) => {
        this.flight = flight;
        this.titleService.setTitle(`${this.flight.numerLotu}: ${this.flight.miejsceWylotu} -> ${this.flight.miejscePrzylotu}`)
      });
  }

  fetchFlight() {
    this.flightService
      .getFlightById(this.id)
      .subscribe((flight: Flight) => (this.flight = flight));
  }

  onEditFlight() {
    this.router.navigate(['edit'], { relativeTo: this.route });
  }

  onDeleteFlight() {
    this.flightService.deleteFlight(this.flight).subscribe(() => {
      this.flightService.notifyOther({ refresh: true });
      this.router.navigate(['/flight']);
    },
    (errorMessage: Error) => {
      this.error = errorMessage.message;
    })
  }
}
