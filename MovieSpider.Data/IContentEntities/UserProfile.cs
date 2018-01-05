namespace MovieSpider.Data.IContentEntities
{
    using MovieSpider.Data.IContentEnums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    [Serializable]
    [DataContract]
    [Table(nameof(UserProfile))]
    public partial class UserProfile
    {
        [DataMember]
        [Key]
        public int UserId { get; set; }

        [DataMember]
        [StringLength(300)]
        public string Headshot { get; set; }

        [DataMember]
        public GenderEnum Gender { get; set; }

        [DataMember]
        [StringLength(4000)]
        public string Description { get; set; }

        [DataMember]
        [Required]
        public virtual User User { get; set; }
    }
}
