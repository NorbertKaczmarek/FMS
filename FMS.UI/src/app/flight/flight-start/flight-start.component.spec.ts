import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightStartComponent } from './flight-start.component';

describe('FlightStartComponent', () => {
  let component: FlightStartComponent;
  let fixture: ComponentFixture<FlightStartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FlightStartComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FlightStartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
