﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        private static Entities _context;
        public static Entities GetContext()
        {
            if (_context == null) return _context = new Entities();
            return _context;
        }
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<Lesson> Lesson { get; set; }
        public virtual DbSet<Mark> Mark { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Specialty> Specialty { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<Teacher> Teacher { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }
    }
}
