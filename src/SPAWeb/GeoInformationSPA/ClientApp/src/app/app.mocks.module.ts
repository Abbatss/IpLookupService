import { NgModule } from '@angular/core';

import { LocationDetailsComponentMock } from './location-details/location-details.component.mock';


@NgModule({
  declarations: [
    LocationDetailsComponentMock,
  ],
  exports: [
    LocationDetailsComponentMock,
  ],
})
export class AppMocksModule { }