import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { LocationModel } from '../Models/LocationModel';
import { ViewEncapsulation } from '@angular/core';
@Component({
  selector: 'app-ip-search',
  templateUrl: './ip-search.component.html',
  styleUrls: ['/ip-search.component.css'],
  encapsulation: ViewEncapsulation.None,

})
export class IpSearchComponent {
  geoInfo : LocationModel;
  public selectedIp:string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private location: Location, private router: Router, private route: ActivatedRoute) {
  }
  ngOnInit() {
  
  }

  public searchByIp() {
    var url = this.baseUrl + '/ip/location?ip='+ this.selectedIp;
    this.http.get<LocationModel>(url, { observe: 'response' }).subscribe((result) => {
      this.geoInfo = result.body ;
    }, error => console.error(error));

  }
}