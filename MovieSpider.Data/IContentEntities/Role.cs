namespace MovieSpider.Data.IContentEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract]
    [Table(nameof(Role))]
    public partial class Role
    {
        [DataMember]
        [Key]
        public int RoleId { get; set; }

        [DataMember]
        [StringLength(300)]
        public string RoleName { get; set; }

        [DataMember]
        [InverseProperty(nameof(Role))]
        public virtual ICollection<BoardRole> BoardRoles { get; set; }

        [DataMember]
        [InverseProperty(nameof(Role))]
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
