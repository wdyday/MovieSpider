namespace MovieSpider.Data.IContentEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    [Serializable]
    [DataContract]
    [Table(nameof(Feedback))]
    public partial class Feedback
    {
        [DataMember]
        [Key]
        public int FeedbackId { get; set; }

        /// <summary>
        /// 反馈内容
        /// </summary>
        [DataMember]
        [StringLength(1000)]
        public string FeedbackInfo { get; set; }

        /// <summary>
        /// 答复内容
        /// </summary>
        [DataMember]
        [StringLength(1000)]
        public string Reply { get; set; }

        /// <summary>
        /// 反馈时间
        /// </summary>
        [DataMember]
        public System.DateTime FeedbackTime { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        [DataMember]
        public System.DateTime? ReplyTime { get; set; }

        /// <summary>
        /// 反馈人
        /// 未登录时保存成匿名用户
        /// </summary>
        [DataMember]
        [ForeignKey(nameof(User))]
        public int? UserId { get; set; }
        [DataMember]
        public virtual User User { get; set; }
    }
}
