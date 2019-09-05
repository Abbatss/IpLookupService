import { Injectable,Inject } from '@angular/core';
import { LocationModel } from '../models/LocationModel';
import { LocationsAbstractService } from './locations.service';

@Injectable({
  providedIn: 'root'
})
export class LocationsServiceMock implements LocationsAbstractService {
  getLocationByIp(ip: string): Promise<LocationModel> {
    throw new Error("Method not implemented.");
  }
  getLocationsByCity(city: string): Promise<LocationModel[]> {
    throw new Error("Method not implemented.");
  }
  constructor(
   
  ) { }

}
