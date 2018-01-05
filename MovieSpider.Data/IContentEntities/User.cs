namespace MovieSpider.Data.IContentEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    [Serializable]
    [DataContract]
    [Table(nameof(User))]
    public partial class User
    {
        [DataMember]
        [Key]
        public int UserId { get; set; }

        [DataMember]
        [StringLength(300)]
        public string UserName { get; set; }

        [DataMember]
        [StringLength(300)]
        public string Email { get; set; }

        [DataMember]
        [StringLength(300)]
        public string Password { get; set; }

        [DataMember]
        public bool Enabled { get; set; }
        [DataMember]
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        [DataMember]
        public bool IsLocked { get; set; }
        [DataMember]
        public Nullable<System.DateTime> LockedDate { get; set; }
        [DataMember]
        public int FailedPasswordAttemptCount { get; set; }
        [DataMember]
        public System.DateTime FailedPasswordAttemptWindowStart { get; set; }
        [DataMember]
        public System.DateTime CreateDate { get; set; }

        [DataMember]
        public virtual UserProfile UserProfile { get; set; }


        [DataMember]
        [InverseProperty(nameof(User))]
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
