import { Component } from '@angular/core';

import { LocationModel } from '../Models/LocationModel';
import { ViewEncapsulation } from '@angular/core';
import { LocationsAbstractService } from '../services/locations.service';
@Component({
  selector: 'app-ip-search',
  templateUrl: './ip-search.component.html',
  styleUrls: ['./ip-search.component.css'],
  encapsulation: ViewEncapsulation.None,

})
export class IpSearchComponent {
  geoInfo : LocationModel;
  public selectedIp:string;
  constructor(private locationsService: LocationsAbstractService) {
  }
  ngOnInit() {
  
  }

  public searchByIp() {
    this.locationsService.getLocationByIp(this.selectedIp).then(res => this.geoInfo = res).catch(e=>console.error(e));
  }
}