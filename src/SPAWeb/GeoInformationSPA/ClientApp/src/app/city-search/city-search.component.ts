import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ViewEncapsulation } from '@angular/core';
import { LocationModel } from '../Models/LocationModel';
@Component({
  selector: 'app-city-search',
  templateUrl: './city-search.component.html',
  styleUrls: ['city-search.component.css'],
  encapsulation: ViewEncapsulation.None,

})
export class CitySearchComponent {
  locations : LocationModel[];
  city:string;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private location: Location, private router: Router, private route: ActivatedRoute) {

  
  }
  ngOnInit() {
  
  }

  public searchByCity() {
    var url = this.baseUrl + '/ip/locations?city='+ this.city;
    this.http.get<LocationModel[]>(url, { observe: 'response' }).subscribe((result) => {
      this.locations = result.body ;
    }, error => console.error(error));

  }
}
