//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Taxi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employees
    {
        public Employees()
        {
            this.Dispetchers = new HashSet<Dispetchers>();
            this.Drivers = new HashSet<Drivers>();
            this.Administators = new HashSet<Administators>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        public string CallSign { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<System.DateTime> DateOfBorn { get; set; }
        public Nullable<System.DateTime> DateOfHiring { get; set; }
        public string Sex { get; set; }
        public string Password { get; set; }
        public System.Guid IdRole { get; set; }
    
        public virtual ICollection<Dispetchers> Dispetchers { get; set; }
        public virtual ICollection<Drivers> Drivers { get; set; }
        public virtual RoleTable RoleTable { get; set; }
        public virtual ICollection<Administators> Administators { get; set; }
    }
}