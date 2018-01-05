namespace MovieSpider.Data.IContentEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    [Serializable]
    [DataContract]
    [Table(nameof(Comment))]
    public partial class Comment
    {
        [DataMember]
        [Key]
        public int CommentId { get; set; }

        [DataMember]
        [StringLength(4000)]
        public string CommentContent { get; set; }

        [DataMember]
        [StringLength(20)]
        public string IP { get; set; }

        [DataMember]
        public int LikeCount { get; set; }
        [DataMember]
        public int HateCount { get; set; }
        [DataMember]
        public System.DateTime CreateTime { get; set; }
        [DataMember]
        public bool Visible { get; set; }

        [DataMember]
        [ForeignKey(nameof(Post))]
        public int PostId { get; set; }
        [DataMember]
        public virtual Post Post { get; set; }

        [DataMember]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [DataMember]
        public virtual User User { get; set; }
    }
}
