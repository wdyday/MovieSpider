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
        /// ��������
        /// </summary>
        [DataMember]
        [StringLength(1000)]
        public string FeedbackInfo { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [DataMember]
        [StringLength(1000)]
        public string Reply { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [DataMember]
        public System.DateTime FeedbackTime { get; set; }

        /// <summary>
        /// �ظ�ʱ��
        /// </summary>
        [DataMember]
        public System.DateTime? ReplyTime { get; set; }

        /// <summary>
        /// ������
        /// δ��¼ʱ����������û�
        /// </summary>
        [DataMember]
        [ForeignKey(nameof(User))]
        public int? UserId { get; set; }
        [DataMember]
        public virtual User User { get; set; }
    }
}
