import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FlightDetailComponent } from './flight-detail/flight-detail.component';
import { FlightEditComponent } from './flight-edit/flight-edit.component';
import { FlightStartComponent } from './flight-start/flight-start.component';
import { FlightComponent } from './flight.component';

const routes: Routes = [
  {
    path: '',
    title: 'flight',
    component: FlightComponent,
    children: [
      { path: '', component: FlightStartComponent },
      { path: 'new', component: FlightEditComponent },
      {
        path: ':id',
        component: FlightDetailComponent,
        // resolve: [FlightsResolverService],
      },
      {
        path: ':id/edit',
        component: FlightEditComponent,
        // resolve: [FlightsResolverService],
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FlightRoutingModule { }
