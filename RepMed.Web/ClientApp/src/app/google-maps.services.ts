import { Injectable } from '@angular/core';

declare global {
  interface Window { google: any; }
}

@Injectable({
  providedIn: 'root'
})
export class GoogleMapsService {
  private scriptLoaded = false;

  loadGoogleMapsScript(): Promise<void> {
    return new Promise((resolve, reject) => {
      if (this.scriptLoaded) {
        resolve();
        return;
      }

      // Check if the Google Maps script is already in the page
      const existingScript = document.querySelector('script[src*="maps.googleapis.com"]');
      if (existingScript) {
        resolve();
        return;
      }

      // Create and append script tag for Google Maps API
      const script = document.createElement('script');
      script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyDgOLtRDzjHtYVShbj9d1Z5qeBl6A6oFec&libraries=places';
      script.async = true;
      script.defer = true;

      script.onload = () => {
        this.scriptLoaded = true;
        resolve();
      };

      script.onerror = () => {
        reject('Google Maps script failed to load');
      };

      document.head.appendChild(script);
    });
  }
}
