import { Injectable,Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocationModel } from '../models/LocationModel';
import { environment } from './../../environments/environment';

export abstract class LocationsAbstractService {
  abstract getLocationByIp(ip: string): Promise<LocationModel>;
  abstract getLocationsByCity(city: string): Promise<LocationModel[]>;
}

@Injectable({
  providedIn: 'root'
})
export class LocationsService implements LocationsAbstractService {
  constructor(
    private http: HttpClient,
  ) { }

  async getLocationByIp(ip: string): Promise<LocationModel> {
    environment.baseUrl 
    var url = environment.baseUrl + '/ip/location?ip='+ ip;
   const res = await this.http.get<LocationModel>(url, { observe: 'response' }).toPromise();
      return res.body;
  }
  async getLocationsByCity(city: string): Promise<LocationModel[]> {
    var url = environment.baseUrl + '/ip/locations?city='+ city;
   const res = await this.http.get<LocationModel[]>(url, { observe: 'response' }).toPromise();
      return res.body;
  }
}
