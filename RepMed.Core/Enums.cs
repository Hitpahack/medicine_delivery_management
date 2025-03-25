using System;
using System.Collections.Generic;
using System.Text;

namespace RepMed.Core
{
  public  class Enums
    {
        public enum Gender
        {
            Male,
            Female
        }

        public enum Roles
        {
            Provider,
            Owner,
            Admin,
            SuperAdmin,
            Facilities
        }

        public enum ProviderType
        {
            HealthCare,
            NonHealthCare
        }
        public enum FieldTyps
        {
            Text,
            Number,
            Bool,
            File,
            Select

        }
    }
}
