import { HttpClient } from "@angular/common/http";
import { APP_DI_CONTAINER } from "../../common/app.di.container";
import { AdminApiConfigService } from "../admin.endpoints";
export class adminBaseService {
    public apiConfig: AdminApiConfigService;
    public http: HttpClient;
    constructor(http: HttpClient = null) {
        this.apiConfig = APP_DI_CONTAINER.getInjector().get(AdminApiConfigService);
        if (this.http == null)
            this.http = APP_DI_CONTAINER.getInjector().get(HttpClient);
    }

    
}
