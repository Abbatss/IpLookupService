import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IpSearchComponent } from './ip-search.component';
import { LocationDetailsComponentMock } from '../location-details/location-details.component.mock';
import { LocationModel } from '../Models/LocationModel';
import { FormsModule,  } from '@angular/forms';
import { LocationsAbstractService } from '../services/locations.service';
import { LocationsServiceMock } from '../services/locations.service.mock';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { DebugElement } from '@angular/core';

export var options: Partial<IConfig> | (() => Partial<IConfig>);


describe('IpSearchComponent', () => {
  let component: IpSearchComponent;
  let fixture: ComponentFixture<IpSearchComponent>;
  let debugElement: DebugElement;
  let locationsService: LocationsAbstractService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        FormsModule,
        NgxMaskModule.forRoot(options)
      ],
      declarations: [ IpSearchComponent,LocationDetailsComponentMock ],
      providers: [
        { provide: LocationsAbstractService, useClass: LocationsServiceMock },
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IpSearchComponent);
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
  it('should call getLocationByIp on button click', () => {
    const spy = spyOn(locationsService, 'getLocationByIp')
    const buttons = debugElement.nativeElement.querySelectorAll('button');
    buttons[0].click();
    expect(buttons.length).toBe(1);
    expect(buttons[0].innerText).toEqual('Search');
    expect(spy).toHaveBeenCalledTimes(1);
  });
});
