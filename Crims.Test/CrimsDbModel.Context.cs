﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Crims.Test
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class crims_dbEntities : DbContext
    {
        public crims_dbEntities()
            : base("name=crims_dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<crims_base_data> crims_base_data { get; set; }
        public virtual DbSet<crims_custom_field_types> crims_custom_field_types { get; set; }
        public virtual DbSet<crims_custom_fields> crims_custom_fields { get; set; }
        public virtual DbSet<crims_custom_list> crims_custom_list { get; set; }
        public virtual DbSet<crims_custom_list_data> crims_custom_list_data { get; set; }
        public virtual DbSet<crims_finger_images> crims_finger_images { get; set; }
        public virtual DbSet<crims_finger_reason> crims_finger_reason { get; set; }
        public virtual DbSet<crims_finger_template> crims_finger_template { get; set; }
        public virtual DbSet<crims_photograph> crims_photograph { get; set; }
        public virtual DbSet<crims_project_custom_field> crims_project_custom_field { get; set; }
        public virtual DbSet<crims_project_custom_group> crims_project_custom_group { get; set; }
        public virtual DbSet<crims_project_custom_list> crims_project_custom_list { get; set; }
        public virtual DbSet<crims_project_custom_list_data> crims_project_custom_list_data { get; set; }
        public virtual DbSet<crims_projects> crims_projects { get; set; }
        public virtual DbSet<crims_signature> crims_signature { get; set; }
        public virtual DbSet<crims_systems_features> crims_systems_features { get; set; }
        public virtual DbSet<crims_user_access_rights> crims_user_access_rights { get; set; }
        public virtual DbSet<crims_users> crims_users { get; set; }
        public virtual DbSet<crims_custom_group> crims_custom_group { get; set; }
    }
}
