import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ViewEncapsulation } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
@Component({
  selector: 'app-ip-search',
  templateUrl: './ip-search.component.html',
  styleUrls: ['/ip-search.component.css'],
  encapsulation: ViewEncapsulation.None,

})
export class IpSearchComponent {
  geoInfo : LocationModel;
  selectedIp:string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private location: Location, private router: Router, private route: ActivatedRoute) {

  
  }
  ngOnInit() {
  
  }

  public searchByIp() {
    this.geoInfo =  new LocationModel();
      this.geoInfo.City="Moskow";
    this.http.get<LocationModel>(this.baseUrl + 'api/api/location?ip='+this.selectedIp, { observe: 'response' }).subscribe(result => {
      
    }, error => console.error(error));

  }
}

class LocationModel implements ILocationModel
{
  City: string;
  name: string;
  priority: number;
  added: string;
  timeToComplete: number;
  actions: string;
}

interface ILocationModel {
  City: string;
  name: string;
  priority: number;
  added: string;
  timeToComplete: number;
  actions: string;
}
