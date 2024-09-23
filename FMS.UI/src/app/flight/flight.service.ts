import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, throwError } from 'rxjs';
import { Flight } from './flight.model';
import { environment } from '../../environments/environment';
import { SortDirectionEnum } from '../app.component';

export interface GetFlightResponse {
  items: Flight[];
  itemsFrom: number;
  itemsTo: number;
  totalItemsCount: number;
  totalPages: number;
}

@Injectable({
  providedIn: 'root',
})
export class FlightService {
  private url = 'flight';

  public notify = new BehaviorSubject<any>('');
  notifyObservable$ = this.notify.asObservable();
  public notifyOther(data: any) {
    if (data) {
      this.notify.next(data);
    }
  }

  constructor(private http: HttpClient) {}

  public getFlights(
    SearchPhrase = "",
    PageNumber = 1,
    PageSize = 5,
    SortBy = "",
    SortDirection: SortDirectionEnum = SortDirectionEnum.ASC
  ): Observable<GetFlightResponse> {
    let params = new HttpParams()
    .set("SearchPhrase", SearchPhrase)
    .set("PageNumber", PageNumber)  // .toString()
    .set("PageSize", PageSize)  // .toString()
    .set("SortBy", SortBy)
    .set("SortDirection", SortDirection.toString())
    return this.http.get<GetFlightResponse>(`${environment.apiUrl}/${this.url}`, {params});
  }

  public getFlightById(id: number): Observable<Flight> {
    return this.http.get<Flight>(`${environment.apiUrl}/${this.url}/${id}`);
  }

  public updateFlight(flight: Flight): Observable<Flight[]> {
    return this.http.post<any>(
      `${environment.apiUrl}/${this.url}/${flight.id}`,
      flight
    );
  }

  public updateFlightWithId(id: number, flight: Flight): Observable<Flight[]> {
    return this.http
      .post<any>(`${environment.apiUrl}/${this.url}/${id}`, flight)
      .pipe(catchError(this.handleError));
  }

  public createFlight(flight: Flight): Observable<any> {
    return this.http
      .post<any>(`${environment.apiUrl}/${this.url}`, flight, {
        observe: 'response',
      })
      .pipe(catchError(this.handleError));
  }

  public deleteFlight(flight: Flight): Observable<Flight[]> {
    return this.http
      .delete<any>(`${environment.apiUrl}/${this.url}/${flight.id}`)
      .pipe(catchError(this.handleError));
  }

  private handleError(errorRes: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred!';

    if (!errorRes.status) {
      return throwError(() => new Error(errorMessage));
    }

    switch (errorRes.status) {
      case 401:
        errorMessage = 'You must be logged in to do it.';
    }

    if (errorRes.error) { 
      if (errorRes.error.errors) {
        errorMessage = '';
        let errors = Object.values(errorRes.error.errors);
        errors.forEach((error) => {
          errorMessage += error;
        });
      } else {
        errorMessage = errorRes.error;
      }
    }

    return throwError(() => new Error(errorMessage));
  }
}
