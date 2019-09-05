import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, Validators, FormControl } from '@angular/forms';
import { LocationDetailsComponent } from './location-details.component';
import { LocationModel } from '../Models/LocationModel';
import { NgModule } from '@angular/core';
describe('LocationDetailsComponent', () => {
  let component: LocationDetailsComponent;
  let fixture: ComponentFixture<LocationDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        FormsModule,
      ],
      declarations: [ LocationDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LocationDetailsComponent);
    component = fixture.componentInstance;
    component.geoInfo = new LocationModel();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
