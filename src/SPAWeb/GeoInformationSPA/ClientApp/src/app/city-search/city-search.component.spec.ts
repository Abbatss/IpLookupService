import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CitySearchComponent } from './city-search.component';
import { LocationDetailsComponentMock } from './../location-details/location-details.component.mock';
import {NgxMaskModule, IConfig} from 'ngx-mask'
import { FormsModule, Validators, FormControl } from '@angular/forms';
import { LocationsAbstractService } from '../services/locations.service';
import { LocationsServiceMock } from '../services/locations.service.mock';
import { DebugElement } from '@angular/core';
describe('CitySearchComponent', () => {
  let component: CitySearchComponent;
  let fixture: ComponentFixture<CitySearchComponent>;
  let locationsService: LocationsAbstractService;
  let debugElement: DebugElement;
  
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        FormsModule,
        NgxMaskModule
      ],
      declarations: [ CitySearchComponent,
        LocationDetailsComponentMock ],
        providers: [
          { provide: LocationsAbstractService, useClass: LocationsServiceMock },
        ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CitySearchComponent);
    component = fixture.componentInstance;
    debugElement = fixture.debugElement;
    locationsService = TestBed.get(LocationsAbstractService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should have search button', () => {
    const buttons = debugElement.nativeElement.querySelectorAll('button');
    expect(buttons.length).toBe(1);
    expect(buttons[0].innerText).toEqual('Search');
  });
  it('should call getLocationsByCity on button click', () => {
    const spy = spyOn(locationsService, 'getLocationsByCity')
    const buttons = debugElement.nativeElement.querySelectorAll('button');
    buttons[0].click();
    expect(buttons.length).toBe(1);
    expect(buttons[0].innerText).toEqual('Search');
    expect(spy).toHaveBeenCalledTimes(1);
  });
});
