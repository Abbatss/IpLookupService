import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IpSearchComponent } from './ip-search.component';
import { LocationDetailsComponentMock } from '../location-details/location-details.component.mock';
import { LocationModel } from '../Models/LocationModel';
import { FormsModule, Validators, FormControl } from '@angular/forms';
describe('IpSearchComponent', () => {
  let component: IpSearchComponent;
  let fixture: ComponentFixture<IpSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        FormsModule,
      ],
      declarations: [ IpSearchComponent,LocationDetailsComponentMock ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IpSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
