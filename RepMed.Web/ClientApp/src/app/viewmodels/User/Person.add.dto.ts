export interface BasePerson {
    firstName?: string;
    lastName?: string;
    email: string;
    id?: number;
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
  }
  
  export interface AddAddressDto {
    // Add address properties here
    addressline?:string
    street?: string;
    city?: string;
    state?: string;
    postalCode?: string;
    country?: string;
    pincode?: string
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
  }
  