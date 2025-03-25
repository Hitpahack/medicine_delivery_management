import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { APP_DI_CONTAINER } from './app/common/app.di.container';
import { environment } from './environments/environment';


export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

export function setTitle(title:string) {
  document.title = title;
}

export function loadScript(scriptpath:string) {
  
 
    const script = document.createElement('script');
    script.type = 'module';
    script.src = getBaseUrl()+scriptpath;  
    script.async = true;
    script.onerror = () => {
      console.log('Error occurred while loading script');
    };

    document.body.appendChild(script);

   
  
}
export function loadScripts(scriptsPath:Array<string>) {
  
  scriptsPath.forEach(r=> {
    loadScript(r);
    })
}

export function loadStylesheet(stylesheetPath:string) {
  
 
  const stylesheet = document.createElement('link');
  stylesheet.rel  = 'stylesheet';
  stylesheet.type = 'text/css';
  stylesheet.href = getBaseUrl()+stylesheetPath;
  stylesheet.media = 'all';  

  stylesheet.onerror = () => {
    console.log('Error occurred while loading stylesheet');
  };

  document.head.appendChild(stylesheet);

 

}

export function loadStylesheets(stylesheetsPath:Array<string>) {

  stylesheetsPath.forEach(s=> {
    loadStylesheet(s);
  })
}

const providers = [
    {
        provide: 'BASE_URL',
        useFactory: getBaseUrl,
        deps: []
    }
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule).then((moduleRef) => {
  APP_DI_CONTAINER.setInjector(moduleRef.injector);
}).catch(err => console.error(err)); 
