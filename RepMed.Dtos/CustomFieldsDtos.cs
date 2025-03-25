using System;
using System.Collections.Generic;
using System.Text;

namespace RepMed.Dtos
{
    public class CustomFieldsDtos
    {
        
        public int ProviderCategory { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public bool? IsRequired { get; set; }
    }
    public class EntityCustomFieldsDtos : CustomFieldsDtos
    {
        public int Id { get; set; }
        public object FieldValue { get; set; }

    }
    public class AddCustomFieldsDtos : CustomFieldsDtos
    {
        
    }

    public class UpdateCustomFieldsDtos : CustomFieldsDtos
    {

    }
    public class UserCustomFieldsDtos
    {
        public string FieldName { get; set; }
        public object FieldValue { get; set; }
        public string FieldType { get; set; }
        public bool? IsRequired { get; set; }
        public Guid UserId { get; set; }


    }
    public class EntityUserCustomFieldsDtos : UserCustomFieldsDtos
    {
        public Guid Id { get; set; }
        //public Guid UserId { get; set; }
        public EntityUsersDto User { get; set; }

    }
    public class AddUserCustomFieldsDtos : UserCustomFieldsDtos
    {

    }
}
