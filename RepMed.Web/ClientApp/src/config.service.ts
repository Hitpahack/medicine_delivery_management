// src/app/config.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ConfigService {
  private configData: any;

  constructor(private http: HttpClient) {}

  loadConfig(): Promise<void> {
    return firstValueFrom(this.http.get('/assets/appconfig.json')).then(data => {
      this.configData = data;
    });
  }

  get settings() {
    return this.configData;
  }

  get apiUrl(): string {
    return this.configData?.apiUrl ?? '';
  }
  
  get admin_apiv1(): string {
    return this.configData?.admin_apiv1 ?? '';
  }

  get admin_api(): string {
    return this.configData?.admin_api ?? '';
  }
}
