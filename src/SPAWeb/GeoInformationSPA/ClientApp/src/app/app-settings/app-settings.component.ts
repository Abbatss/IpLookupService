import { Component, OnInit } from '@angular/core';
import { AppConfig } from '../app.config';

@Component({
  selector: 'app-app-settings',
  templateUrl: './app-settings.component.html',
  styleUrls: ['./app-settings.component.css']
})
export class AppSettingsComponent implements OnInit {

 public  baseUrl:string;
  constructor() { }

  ngOnInit() {
  }
  public setUrl()
  {
    AppConfig.settings.apiUrl = this.baseUrl;
  }

}
