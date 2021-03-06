import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface IAppConfig {
  apiBaseUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class AppConfig {
  static settings: IAppConfig;
  constructor(private http: HttpClient) { }
  load() {
    const jsonFile = 'clientapp/dist/assets/config.json';
    return new Promise<void>((resolve, reject) => {
      this.http.get(jsonFile).toPromise().then((response: IAppConfig) => {
        AppConfig.settings = response as AppConfigSettngs;
        resolve();
      }).catch((response: any) => {
        resolve();
        var appConfig = new AppConfigSettngs();
        appConfig.apiBaseUrl="http://localhost:44365/api"
        AppConfig.settings =  appConfig;
      });
    });
  }
}
export class AppConfigSettngs implements IAppConfig
{
  public apiBaseUrl: string;
  
}

