// app-config.service.ts

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {
  private config: any;

  constructor(private http: HttpClient) { }

  loadConfig(): Observable<any> {
    return this.http.get('/assets/config.json'); // Adjust the path to your configuration file
  }

  setBaseUrl(baseUrl: string): void {
    debugger;
    this.config.remoteServiceBaseUrl = baseUrl + '/api/services/app/';
  }

  getConfig(url): any {
    debugger
    return this.config+url;
  }
  
}



