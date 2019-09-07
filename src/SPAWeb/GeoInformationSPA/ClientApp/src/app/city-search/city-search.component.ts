import { Component } from '@angular/core';

import { LocationModel } from '../models/LocationModel';
import { ViewEncapsulation } from '@angular/core';
import { LocationsAbstractService } from '../services/locations.service';
@Component({
  selector: 'app-city-search',
  templateUrl: './city-search.component.html',
  styleUrls: ['./city-search.component.css'],
  encapsulation: ViewEncapsulation.None,

})
export class CitySearchComponent {
  locations : LocationModel[];
  city:string;
  constructor(private locationsService: LocationsAbstractService) {
  }
  ngOnInit() {
  
  }

  public searchByCity() {
    this.locationsService.getLocationsByCity(this.city).then(res => this.locations = res).catch(e=>console.error(e));

  }
}
