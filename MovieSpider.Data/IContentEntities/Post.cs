namespace MovieSpider.Data.IContentEntities
{
    using Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    [Serializable]
    [DataContract]
    [Table(nameof(Post))]
    public partial class Post
    {
        [DataMember]
        [Key]
        public int PostId { get; set; }

        [DataMember]
        [StringLength(1000)]
        public string Title { get; set; }

        [DataMember]
        public string PostContent { get; set; }

        /// <summary>
        /// ����Ա׷������
        /// </summary>
        [DataMember]
        [StringLength(4000)]
        public string PostContentExtra { get; set; }

        [DataMember]
        public System.DateTime CreateTime { get; set; }

        [DataMember]
        [StringLength(300)]
        public string DefaultImagePath { get; set; }
        [DataMember]
        public int LikeCount { get; set; }
        [DataMember]
        public int HateCount { get; set; }
        [DataMember]
        public int HitCount { get; set; }

        /// <summary>
        /// ״̬:0-����, 1-ͨ��, 2-����
        /// </summary>
        [DataMember]
        public int Status { get; set; }

        [DataMember]
        [StringLength(20)]
        public string IP { get; set; }

        #region Movie

        /// <summary>
        /// ����: 1-�й�,2-ŷ��,3-�պ�
        /// </summary>
        [DataMember]
        public RegionEnum Region { get; set; }

        /// <summary>
        /// ��ӳ����: ȡ��С������, �� 2016-10-21(�й���½) / 2016-08-26(����) / 2016-08-31(����), ȡ 2016-08-26(����)
        /// </summary>
        [DataMember]
        public DateTime? PremiereDate { get; set; }

        /// <summary>
        /// ��ӳ����: 2016-10-21(�й���½) / 2016-08-26(����) / 2016-08-31(����)
        /// </summary>
        [DataMember]
        [StringLength(1000)]
        public string PremiereDateMulti { get; set; }

        [DataMember]
        [StringLength(300)]
        public string CnName { get; set; }

        /// <summary>
        /// ��������: ����ʱ��5���Ǽ���ײ/��ԭ���ռ�5��Ц��ײ����(̨)/��������5/����ʱ��5����ײ����/�������ͣ���ʯײ����(��)
        /// </summary>
        [DataMember]
        [StringLength(1000)]
        public string OtherCnNames { get; set; }

        [DataMember]
        [StringLength(300)]
        public string EnName { get; set; }

        /// <summary>
        /// ժҪ
        /// </summary>
        [DataMember]
        public string Summary { get; set; }

        [DataMember]
        [StringLength(300)]
        public string FromUrl { get; set; }

        /// <summary>
        /// ý������
        /// 0:ȫ��, 1: ��Ӱ, 2: ���Ӿ�, 3: ����, 4: ����
        /// </summary>
        [DataMember]
        public MediaTypeEnum? MediaType { get; set; }

        /// <summary>
        /// ͬ��״̬
        /// 1. Job ͬ������ʱΪ NULL,
        /// 2. ����Ա��̨�ֶ����º�Ϊ false, 
        /// 3. ͬ��������ץȡ���Ϊ true 
        /// </summary>
        [DataMember]
        public bool? IsSyncDone { get; set; }

        #endregion

        [DataMember]
        [ForeignKey(nameof(Board))]
        public int BoardId { get; set; }
        [DataMember]
        public virtual Board Board { get; set; }

        [DataMember]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [DataMember]
        public virtual User User { get; set; }

        [DataMember]
        [InverseProperty(nameof(Post))]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
