namespace MovieSpider.Data.IContentEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    [Serializable]
    [DataContract]
    [Table(nameof(BoardRole))]
    public partial class BoardRole
    {
        [DataMember]
        [Key]
        public int BoardRoleId { get; set; }

        [DataMember]
        [ForeignKey(nameof(Board))]
        public int BoardId { get; set; }
        [DataMember]
        public virtual Board Board { get; set; }

        [DataMember]
        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }
        [DataMember]
        public virtual Role Role { get; set; }
    }
}
