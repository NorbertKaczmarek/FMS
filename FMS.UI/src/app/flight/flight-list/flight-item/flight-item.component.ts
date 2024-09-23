import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Flight } from '../../flight.model';
import { FlightService } from '../../flight.service';

@Component({
  selector: '[app-flight-item]',
  templateUrl: './flight-item.component.html',
  styleUrl: './flight-item.component.css'
})
export class FlightItemComponent {
  @Input() flight: Flight;
  @Input() index: number;
  
  @Output() flightToUpdate = new EventEmitter<Flight>();

  constructor(private flightService: FlightService){}

  editFlight(flight: Flight) {
    this.flightService
      .getFlightById(this.index)
      .subscribe((flight: Flight) => this.flightToUpdate.emit(flight));
  }
}
