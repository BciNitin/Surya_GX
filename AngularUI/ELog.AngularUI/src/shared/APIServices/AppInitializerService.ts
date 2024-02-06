// app-initializer.service.ts

import { Injectable } from '@angular/core';
import { AppConfigService } from './AppConfigService ';
// app-initializer.service.ts
import { Observable, from } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppInitializerService {

  constructor(private appConfigService: AppConfigService) { }

  initializeApp(): Observable<any> {
    return from(this.appConfigService.loadConfig());
  }
}
