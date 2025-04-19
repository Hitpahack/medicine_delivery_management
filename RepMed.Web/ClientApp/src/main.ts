import { APP_INITIALIZER, enableProdMode, Injector } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { routes } from './app/app.routing';
import { ConfigService } from './config.service';
import { APP_DI_CONTAINER } from './app/common/app.di.container';
import { HttpClient, HttpXhrBackend } from '@angular/common/http';

// Immediately Invoked Async Function
(async () => {
  // Step 1: Manually create HttpClient before DI
  const httpClient = new HttpClient(new HttpXhrBackend({ build: () => new XMLHttpRequest() }));
  const configService = new ConfigService(httpClient);

  // Step 2: Load config from /assets/appsettings.json
  await configService.loadConfig();

  // Step 3: Bootstrap Angular App
  bootstrapApplication(AppComponent, {
    providers: [
      provideRouter(routes),
      provideHttpClient(),

      // Step 4: Provide ConfigService as useValue (manually created one)
      {
        provide: ConfigService,
        useValue: configService,
      },

      // Step 5: Register global injector AFTER bootstrap starts
      {
        provide: APP_INITIALIZER,
        useFactory: (injector: Injector) => () => APP_DI_CONTAINER.setInjector(injector),
        deps: [Injector],
        multi: true,
      },

      // Step 6: Provide BASE_URL from loaded config
      {
        provide: 'BASE_URL',
        useFactory: () => configService.apiUrl,
      }
    ]
  }).catch(err => console.error(err));
})();
export function getBaseUrl(): string {
  return document.getElementsByTagName('base')[0].href;
}

export function setTitle(title: string) {
  document.title = title;
}

export function loadScript(scriptPath: string) {
  const script = document.createElement('script');
  script.type = 'module';
  script.src = getBaseUrl() + scriptPath;
  if (scriptPath.startsWith('http'))
    script.src = scriptPath;
  script.async = true;
  script.onerror = () => console.error('Error loading script:', scriptPath);
  document.body.appendChild(script);
}

export function loadScripts(scriptsPath: string[]) {
  scriptsPath.forEach(loadScript);
}

export function loadStylesheet(stylesheetPath: string) {
  const link = document.createElement('link');
  link.rel = 'stylesheet';
  link.href = getBaseUrl() + stylesheetPath;

  if (stylesheetPath.startsWith('http'))
    link.href = stylesheetPath;

  
  link.onerror = () => console.error('Error loading stylesheet:', stylesheetPath);
  document.head.appendChild(link);
}

export function loadStylesheets(stylesheetsPath: string[]) {
  stylesheetsPath.forEach(loadStylesheet);
}
