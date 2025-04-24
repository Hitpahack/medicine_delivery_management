export interface BasePerson {
    firstName?: string;
    lastName?: string;
    email: string;
    id?: number;
    name: string;
  }
  
  export interface BasePersonDto extends BasePerson {
    gender?: string;
    dateOfBirth?: Date;
    mobile: string;
  }
  
  export interface BasicPersonsDto extends BasePersonDto {
    picture?: string;
  }
  
  export interface EntityPersonsDto extends BasicPersonsDto {
    id: number;
    firstName?: string;
    lastName?: string;
    email: string;
    mobile: string;
    mobileVerified?: boolean;
    emailVerified?: boolean;
    dateOfBirth?: Date;
    gender?: string;
    bloodGroup?: string;
    picture?: string;
    signature?: string;
    motherName?: string;
    qualification?: string;
    createdAt?: Date;
    updatedAt?: Date;
    persionId?:number;
  }
  
  export interface AddAddressDto {
    // Add address properties here
    addressLine?:string
    street?: string;
    cityId?: string;
    stateId?: string;
    postalCode?: string;
    countryId?: string;
    pincode?: string;
    personId:number;
  }
  
  export interface AddPersonDto extends BasicPersonsDto {
    id: number;
    password: string;
    confirmPassword: string;
    createdBy?: number;
    createdOn?: Date;
    isActive?: boolean;
    address?: AddAddressDto;
    Role?: string;
    personId:number;    
  }
  