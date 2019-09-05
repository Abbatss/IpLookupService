import { Component, OnInit,Input } from '@angular/core';
import { LocationModel } from '../Models/LocationModel';
@Component({
  selector: 'app-location-details',
  templateUrl: './location-details.component.html',
  styleUrls: ['./location-details.component.css']
})
export class LocationDetailsComponent implements OnInit {
  @Input() geoInfo:LocationModel
  constructor() { }

  ngOnInit() {
  }

}
