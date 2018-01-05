namespace MovieSpider.Data.IContentEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    [Serializable]
    [DataContract]
    [Table(nameof(AppConfig))]
    public partial class AppConfig
    {
        [DataMember]
        [Key]
        public int AppConfigId { get; set; }

        [DataMember]
        [StringLength(300)]
        public string Name { get; set; }

        [DataMember]
        [StringLength(1000)]
        public string Value { get; set; }
    }
}
