import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlightRoutingModule } from './flight-routing.module.';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { FlightDetailComponent } from './flight-detail/flight-detail.component';
import { FlightEditComponent } from './flight-edit/flight-edit.component';
import { FlightItemComponent } from './flight-list/flight-item/flight-item.component';
import { FlightListComponent } from './flight-list/flight-list.component';
import { FlightStartComponent } from './flight-start/flight-start.component';
import { FlightComponent } from './flight.component';

@NgModule({
  declarations: [
    FlightComponent,
    FlightEditComponent,
    FlightStartComponent,
    FlightListComponent,
    FlightDetailComponent,
    FlightItemComponent,
  ],
  imports: [
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    FlightRoutingModule,
    CommonModule,
  ],
})
export class FlightModule {}
