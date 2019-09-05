import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { IpSearchComponent } from './ip-search/ip-search.component';
import { CitySearchComponent } from './city-search/city-search.component';
import {NgxMaskModule, IConfig} from 'ngx-mask'
import {APP_BASE_HREF} from '@angular/common';

export var options: Partial<IConfig> | (() => Partial<IConfig>);

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    IpSearchComponent,
    CitySearchComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgxMaskModule.forRoot(options),
    RouterModule.forRoot([
      { path: '', component: IpSearchComponent, pathMatch: 'full' },

      { path: 'ipsearch', component: IpSearchComponent },
      { path: 'citysearch', component: CitySearchComponent }
    ])
  ],
  providers: [{provide: APP_BASE_HREF, useValue : '/' },
  {provide: 'BASE_URL', useValue : 'https://localhost:44365/api'}
],
  bootstrap: [AppComponent]
})
export class AppModule { }
