export class ApiResponse<T> {
    isSuccess: boolean;
    message: string;
    data: T;
    status: number;
    extraData: any;
    constructor(values: Object = {}) {
        Object.assign(this, values);
      }
}