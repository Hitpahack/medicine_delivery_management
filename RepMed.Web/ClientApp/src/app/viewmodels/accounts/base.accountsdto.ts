import { EntityRoleDto } from "../roles.dto";
import { BasicPersonsDto } from "../User/Person.add.dto";

export class BaseAccountsDto {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    emailConfirmed: boolean | null;
    isLocked: boolean;
    isDeleted: boolean;
    lastLoginDate: Date | null;
    roles:EntityRoleDto[]|null;
    person:BasicPersonsDto|null;
    constructor(values: Object = {}) {
        Object.assign(this, values);
      }
}

export class JwtTokenDto {
    tokenId: number;
    token: string;
    tokenValidTill: Date;

    constructor(values: Object = {}) {
        Object.assign(this, values);
      }
}

export class LoginResponse extends BaseAccountsDto {

    token: JwtTokenDto;

    constructor(values: Object = {}) {
        super(values);
        Object.assign(this, values);
      }
}
