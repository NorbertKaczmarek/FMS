import { Component } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FlightService } from '../flight.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Flight, PlaneType } from '../flight.model';
import { Observable, switchMap } from 'rxjs';
import { Title } from '@angular/platform-browser';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-flight-edit',
  templateUrl: './flight-edit.component.html',
  styleUrl: './flight-edit.component.css',
})
export class FlightEditComponent {
  id: number;
  flight?: Flight = null;
  editMode = false;
  flightForm: FormGroup;
  error: string = null;

  obs: Observable<any>;

  constructor(
    private route: ActivatedRoute,
    private flightService: FlightService,
    private router: Router,
    private titleService: Title
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.id = +params['id'];
      this.editMode = params['id'] != null;
      this.initForm();
      if (this.editMode) {
        this.flightService
          .getFlightById(this.id)
          .subscribe((flight: Flight) => {
            this.flight = flight;
            this.titleService.setTitle(
              `${this.flight.numerLotu}: ${this.flight.miejsceWylotu} -> ${this.flight.miejscePrzylotu}`
            );
            this.initForm();
          });
      } else {
        this.titleService.setTitle(`New Flight`);
      }
    });
  }

  onSubmit() {
    const dataWylotu = this.flightForm.value['dataWylotu'];
    const czasWylotu = this.flightForm.value['czasWylotu'];

    let editedFlight = new Flight(
      this.flightForm.value['numerLotu'],
      new Date(`${dataWylotu}T${czasWylotu}`),
      this.flightForm.value['miejsceWylotu'],
      this.flightForm.value['miejscePrzylotu'],
      this.flightForm.value['typSamolotu']
    );

    if (this.editMode) {
      this.flightService.updateFlightWithId(this.id, editedFlight).subscribe(
        () => {
          this.flightService.notifyOther({ refresh: true });
          this.onCancel();
        },
        (errorMessage: Error) => {
          this.error = errorMessage.message;
        }
      );
    } else {
      this.flightService.createFlight(editedFlight).subscribe(
        (res: HttpResponse<any>) => {
          let createdIdString: string = res.headers.get('location');
          this.router.navigate([createdIdString]);
          this.flightService.notifyOther({ refresh: true });
        },
        (errorMessage: Error) => {
          this.error = errorMessage.message;
        }
      );
    }
  }

  refreshGridInAnotherComponent() {
    this.flightService.notifyOther({ refresh: true });
  }

  onCancel() {
    this.router.navigate(['../'], { relativeTo: this.route });
  }

  private getDateFromDate(initialDate: Date) {
    return initialDate.toISOString().slice(0, 10); // "YYYY-MM-DD"
  }

  private getTimeFromDate(initialDate: Date) {
    return initialDate.toTimeString().slice(0, 5); // "HH:mm"
  }

  private initForm() {
    let numerLotu = null;
    let dataWylotu = this.getDateFromDate(new Date());
    let czasWylotu = this.getTimeFromDate(new Date());
    let miejsceWylotu = '';
    let miejscePrzylotu = '';
    let typSamolotu = null;

    if (this.editMode) {
      this.fetchFlight();
      if (this.flight) {
        numerLotu = this.flight.numerLotu;
        dataWylotu = this.getDateFromDate(new Date(this.flight.dataWylotu));
        czasWylotu = this.getTimeFromDate(new Date(this.flight.dataWylotu));
        miejsceWylotu = this.flight.miejsceWylotu;
        miejscePrzylotu = this.flight.miejscePrzylotu;
        typSamolotu = this.flight.typSamolotu;
      }
    }

    this.flightForm = new FormGroup({
      numerLotu: new FormControl(numerLotu, Validators.required),
      dataWylotu: new FormControl(dataWylotu, Validators.required),
      czasWylotu: new FormControl(czasWylotu, Validators.required),
      miejsceWylotu: new FormControl(miejsceWylotu, Validators.required),
      miejscePrzylotu: new FormControl(miejscePrzylotu, Validators.required),
      typSamolotu: new FormControl(typSamolotu, Validators.required),
    });
  }

  fetchFlight() {
    this.flightService
      .getFlightById(this.id)
      .subscribe((flight: Flight) => (this.flight = flight));
  }

  onFeedTestData() {
    this.flightForm = new FormGroup({
      numerLotu: new FormControl(Math.floor(Math.random() * 10 * Math.random() * 100)),
      dataWylotu: new FormControl(
        this.getDateFromDate(
          new Date(
            2010 + Math.floor(Math.random() * 14),
            Math.floor(Math.random() * 12),
            Math.floor(Math.random() * 30)
          )
        )
      ),
      czasWylotu: new FormControl(
        this.getTimeFromDate(
          new Date(
            2024,
            1,
            1,
            Math.floor(Math.random() * 24),
            Math.floor(Math.random() * 60)
          )
        )
      ),
      miejsceWylotu: new FormControl(
        polishCitiesWithAirports[
          Math.floor(Math.random() * polishCitiesWithAirports.length)
        ]
      ),
      miejscePrzylotu: new FormControl(
        polishCitiesWithAirports[
          Math.floor(Math.random() * polishCitiesWithAirports.length)
        ]
      ),
      typSamolotu: new FormControl(PlaneType[Math.floor(Math.random() * 3)]),
    });
  }
}

const polishCitiesWithAirports = [
  'Warszawa',
  'Kraków',
  'Gdańsk',
  'Wrocław',
  'Poznań',
  'Szczecin',
  'Łódź',
  'Katowice',
  'Gdynia',
  'Częstochowa',
];
