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
    
    public partial class Shifts
    {
        public System.Guid Id { get; set; }
        public System.Guid IdDriver { get; set; }
        public System.Guid IdCar { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual Cars Cars { get; set; }
        public virtual Drivers Drivers { get; set; }
    }
}