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
        /// 管理员追加内容
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
        /// 状态:0-待审, 1-通过, 2-驳回
        /// </summary>
        [DataMember]
        public int Status { get; set; }

        [DataMember]
        [StringLength(20)]
        public string IP { get; set; }

        #region Movie

        /// <summary>
        /// 地区: 1-中国,2-欧美,3-日韩
        /// </summary>
        [DataMember]
        public RegionEnum Region { get; set; }

        /// <summary>
        /// 首映日期: 取最小的日期, 如 2016-10-21(中国大陆) / 2016-08-26(美国) / 2016-08-31(法国), 取 2016-08-26(美国)
        /// </summary>
        [DataMember]
        public DateTime? PremiereDate { get; set; }

        /// <summary>
        /// 首映日期: 2016-10-21(中国大陆) / 2016-08-26(美国) / 2016-08-31(法国)
        /// </summary>
        [DataMember]
        [StringLength(1000)]
        public string PremiereDateMulti { get; set; }

        [DataMember]
        [StringLength(300)]
        public string CnName { get; set; }

        /// <summary>
        /// 其他名字: 冰川时代5：星际碰撞/冰原历险记5：笑星撞地球(台)/冰河世纪5/冰川时代5：碰撞航向/冰河世纪：陨石撞地球(港)
        /// </summary>
        [DataMember]
        [StringLength(1000)]
        public string OtherCnNames { get; set; }

        [DataMember]
        [StringLength(300)]
        public string EnName { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [DataMember]
        public string Summary { get; set; }

        [DataMember]
        [StringLength(300)]
        public string FromUrl { get; set; }

        /// <summary>
        /// 媒体类型
        /// 0:全部, 1: 电影, 2: 电视剧, 3: 动漫, 4: 综艺
        /// </summary>
        [DataMember]
        public MediaTypeEnum? MediaType { get; set; }

        /// <summary>
        /// 同步状态
        /// 1. Job 同步过来时为 NULL,
        /// 2. 管理员后台手动更新后为 false, 
        /// 3. 同步给本地抓取库后为 true 
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
