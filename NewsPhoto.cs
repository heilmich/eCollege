//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eCollege
{
    using System;
    using System.Collections.Generic;
    
    public partial class NewsPhoto
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public string Photo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    
        public virtual News News { get; set; }
    }
}
