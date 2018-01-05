namespace MovieSpider.Data.IContentEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    [Serializable]
    [DataContract]
    [Table(nameof(UserRole))]
    public partial class UserRole
    {
        [DataMember]
        [Key]
        public int UserRoleId { get; set; }

        [DataMember]
        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }
        [DataMember]
        public virtual Role Role { get; set; }

        [DataMember]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [DataMember]
        public virtual User User { get; set; }
    }
}
