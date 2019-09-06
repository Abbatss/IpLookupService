import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface IAppConfig {
  apiUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class AppConfig {
  static settings: IAppConfig;
  constructor(private http: HttpClient) { }
  load() {
    const jsonFile = 'clientapp/dist/assets/config/config.json';
    return new Promise<void>((resolve, reject) => {
      this.http.get(jsonFile).toPromise().then((response: IAppConfig) => {
        AppConfig.settings = response as IAppConfig;
        resolve();
      }).catch((response: any) => {
        resolve();
        AppConfig.settings =  new AppConfigSettngs();
      });
    });
  }
}
export class AppConfigSettngs implements IAppConfig
{
  apiUrl: string;
  
}

