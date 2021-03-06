import { Component, OnInit, Input} from '@angular/core';
import { LocationModel } from '../models/LocationModel';

@Component({
  selector: 'app-location-details',
  template: ''
})
export class LocationDetailsComponentMock implements OnInit {
  @Input() geoInfo: LocationModel = new LocationModel();

  constructor() {
  }

  ngOnInit() {
  }
}
