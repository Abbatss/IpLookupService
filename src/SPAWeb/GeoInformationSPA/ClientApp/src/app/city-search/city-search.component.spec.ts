import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CitySearchComponent } from './city-search.component';
import { LocationDetailsComponentMock } from './../location-details/location-details.component.mock';
import { LocationModel } from '../Models/LocationModel';
import { NgModule } from '@angular/core';
import {NgxMaskModule, IConfig} from 'ngx-mask'
import { FormsModule, Validators, FormControl } from '@angular/forms';
describe('CitySearchComponent', () => {
  let component: CitySearchComponent;
  let fixture: ComponentFixture<CitySearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        FormsModule,
        NgxMaskModule
      ],
      declarations: [ CitySearchComponent,
        LocationDetailsComponentMock ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CitySearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
