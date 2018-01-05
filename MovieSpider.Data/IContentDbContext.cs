using MovieSpider.Data.IContentEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Data
{
    public partial class IContentDbContext : DbContext
    {
        public IContentDbContext() : base("name=IContentDbConn")
        {
            //Database.SetInitializer<IContentDbContext>(null);
            Configuration.ProxyCreationEnabled = false;

#if TRACE
            Database.Log = LogSql;
#endif
        }

        #region Logging
        void LogSql(string sql)
        {
            System.Diagnostics.Trace.TraceInformation(sql);
        }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        #region Entities
        public virtual DbSet<AppConfig> AppConfig { get; set; }
        public virtual DbSet<BoardRole> BoardRole { get; set; }
        public virtual DbSet<Board> Board { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }

        //public virtual DbSet<Image> Image { get; set; }
        //public virtual DbSet<Log> Log { get; set; }
        //public virtual DbSet<Province> Province { get; set; }
        //public virtual DbSet<City> City { get; set; }

        #endregion
    }
}
