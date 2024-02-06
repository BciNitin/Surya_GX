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
    debugger
    return this.http.get('/assets/config.json'); // Adjust the path to your configuration file
  }

  setBaseUrl(baseUrl: string): void {
    this.config.remoteServiceBaseUrl = baseUrl;
  }

  getConfig(): any {
    return this.config;
  }
}



