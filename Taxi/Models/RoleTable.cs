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
    
    public partial class RoleTable
    {
        public RoleTable()
        {
            this.Employees = new HashSet<Employees>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    
        public virtual ICollection<Employees> Employees { get; set; }
    }
}
