import { enableProdMode } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { routes } from './app/app.routing';
import { environment } from './environments/environment';

if (environment.production) {
    enableProdMode();
}

bootstrapApplication(AppComponent, {
    providers: [
        provideRouter(routes),
        provideHttpClient(),
        {
            provide: 'BASE_URL',
            useFactory: getBaseUrl,
            deps: []
        }
    ],
}).catch(err => console.error(err));

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}

export function setTitle(title: string) {
    document.title = title;
}

export function loadScript(scriptpath: string) {
    const script = document.createElement('script');
    script.type = 'module';
    script.src = getBaseUrl() + scriptpath;
    script.async = true;
    script.onerror = () => {
        console.log('Error occurred while loading script');
    };
    document.body.appendChild(script);
}

export function loadScripts(scriptsPath: Array<string>) {
    scriptsPath.forEach(r => {
        loadScript(r);
    });
}

export function loadStylesheet(stylesheetPath: string) {
    const stylesheet = document.createElement('link');
    stylesheet.rel = 'stylesheet';
    stylesheet.type = 'text/css';
    stylesheet.href = getBaseUrl() + stylesheetPath;
    stylesheet.media = 'all';
    stylesheet.onerror = () => {
        console.log('Error occurred while loading stylesheet');
    };
    document.head.appendChild(stylesheet);
}

export function loadStylesheets(stylesheetsPath: Array<string>) {
    stylesheetsPath.forEach(s => {
        loadStylesheet(s);
    });
}
    