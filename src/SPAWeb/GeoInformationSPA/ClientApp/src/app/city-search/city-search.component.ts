import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ViewEncapsulation } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
@Component({
  selector: 'app-city-search',
  templateUrl: './city-search.component.html',
  styleUrls: ['city-search.component.css'],
  encapsulation: ViewEncapsulation.None,

})
export class CitySearchComponent {
  private locations : LocationModel[];
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private location: Location, private router: Router, private route: ActivatedRoute) {

  
  }
  ngOnInit() {
  
  }

  public refreshData(ip:string) {
    this.http.get<LocationModel[]>(this.baseUrl + 'api/api/location?city='+ip, { observe: 'response' }).subscribe(result => {
      this.locations = result.body;
    }, error => console.error(error));

  }
}


interface LocationModel {
  id: number;
  name: string;
  priority: number;
  added: string;
  timeToComplete: number;
  actions: string;
}
