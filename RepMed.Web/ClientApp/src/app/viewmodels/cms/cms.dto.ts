export interface CMSDto {
    title: string;
    slug: string;
    content: string;
    metaTitle?: string;
    metaContext?: string;
    isActive?: boolean;
    createdDate?: Date;
    updatedDate?: Date;
  }