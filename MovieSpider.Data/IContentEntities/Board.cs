
namespace MovieSpider.Data.IContentEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    [Serializable]
    [DataContract]
    [Table(nameof(Board))]
    public partial class Board
    {
        [DataMember]
        [Key]
        public int BoardId { get; set; }

        [DataMember]
        [StringLength(300)]
        public string BoardName { get; set; }

        [DataMember]
        [StringLength(1000)]
        public string Description { get; set; }
        [DataMember]
        public int ParentId { get; set; }
        [DataMember]
        public string ParentIdList { get; set; }
        [DataMember]
        public int DisplayOrder { get; set; }
        [DataMember]
        public int Layer { get; set; }
        [DataMember]
        public int SubBoardCount { get; set; }
        [DataMember]
        public bool IsMenu { get; set; }
        [DataMember]
        public bool IsMultiple { get; set; }


        [DataMember]
        [StringLength(300)]
        public string ExternalUrl { get; set; }

        [DataMember]
        public virtual ICollection<BoardRole> BoardRoles { get; set; }
        [DataMember]
        public virtual ICollection<Post> Posts { get; set; }
    }
}
