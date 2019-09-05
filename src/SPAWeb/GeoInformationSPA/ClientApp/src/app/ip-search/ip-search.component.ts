import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ViewEncapsulation } from '@angular/core';
@Component({
  selector: 'app-ip-search',
  templateUrl: './ip-search.component.html',
  styleUrls: ['/ip-search.component.css'],
  encapsulation: ViewEncapsulation.None,

})
export class IpSearchComponent {
  geoInfo : ILocationModel;
  selectedIp:string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private location: Location, private router: Router, private route: ActivatedRoute) {
  }
  ngOnInit() {
  
  }

  public searchByIp() {
    this.http.get<ILocationModel>(this.baseUrl + 'api/api/location?ip='+this.selectedIp, { observe: 'response' }).subscribe(result => {
      this.geoInfo = result;
    }, error => console.error(error));

  }
}
