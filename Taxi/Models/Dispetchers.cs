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
    
    public partial class Dispetchers
    {
        public Dispetchers()
        {
            this.Order = new HashSet<Order>();
        }
    
        public System.Guid Id { get; set; }
        public System.Guid IdEmployee { get; set; }
    
        public virtual Employees Employees { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
