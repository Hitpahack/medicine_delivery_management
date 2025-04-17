export interface PharmacyDto {
    user: AddPersonDto;
    pharmacy: {
      id?: number; // JsonIgnore in C#
      userId: number;
      storeName?: string;
      businessName?: string;
      licenseNumber?: string;
      licenseExpiry?: Date;
      gstnumber?: string;
      ownerName?: string;
      registeredMobile?: string;
      officialEmail?: string;
      storeMobile1?: string;
      storeEmail1?: string;
      storeEmail2?: string;
      mobileVerified?: boolean;
      emailVerified?: boolean;
      address1?: string;
      address2?: string;
      cityId?: number;
      stateId?: number;
      countryId?: number;
      otpverified?: boolean;
      status?: string;
      createdAt?: Date;
      latitude?: number;
      longitude?: number;
    };
    person: {
        firstName?: string;
        lastName?: string;
        email?: string;
        mobile?: string;
        dateOfBirth?: Date;
        gender?: string;
        picture?: string;
        password?: string;
        confirmPassword?: string;
    }
    pharmacyBankDetails: {
      id?: number; // JsonIgnore in C#
      pharmacyId: number;
      bankName: string;
      accountHolderName: string;
      accountNumber: string;
      ifsccode: string;
      branchName: string;
      upiId?: string;
    };
    
  }
  
  // You should also define AddPersonDto separately like this:
  export interface AddPersonDto {
    // AddPersonDto properties here
  }
  