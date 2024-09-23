import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Flight } from '../flight.model';
import { FlightService, GetFlightResponse } from '../flight.service';
import { Router, ActivatedRoute } from '@angular/router';
import { SortDirectionEnum } from '../../app.component';

@Component({
  selector: 'app-flight-list',
  templateUrl: './flight-list.component.html',
  styleUrl: './flight-list.component.css',
})
export class FlightListComponent implements OnInit {
  @Input() flights: Flight[] = [];
  isLoading = false;

  SearchPhrase = '';
  PageNumber = 1;
  PageSize = 5;
  SortBy = 'Id';
  SortDirection: SortDirectionEnum = SortDirectionEnum.DESC;

  FlightResponse: GetFlightResponse;
  totalPagesArray: number[] = [];

  constructor(
    private flightService: FlightService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.fetchData();

    this.flightService.notifyObservable$.subscribe((res) => {
      if (res.refresh) {
        this.fetchData();
      }
    });
  }

  fetchData() {
    this.flightService
      .getFlights(
        this.SearchPhrase,
        this.PageNumber,
        this.PageSize,
        this.SortBy,
        this.SortDirection
      )
      .subscribe((res) => {
        this.flights = res.items;
        this.FlightResponse = res;
        this.isLoading = false;

        this.totalPagesArray = [];
        for (let i = 1; i <= this.FlightResponse.totalPages; i++) {
          this.totalPagesArray.push(i);
        }
      });
  }

  initSearch() {
    this.fetchData();
  }

  Sort(item) {
    if (this.SortBy == item) {
      if (this.SortDirection == SortDirectionEnum.ASC) {
        this.SortDirection = SortDirectionEnum.DESC;
      } else {
        this.SortDirection = SortDirectionEnum.ASC;
      }
    } else {
      this.SortBy = item;
      this.SortDirection = SortDirectionEnum.ASC;
    }

    this.onQueryChange();
  }

  initNewFlight() {
    this.router.navigate(['new'], { relativeTo: this.route });
  }

  onQueryChange() {
    this.fetchData();
  }
}
