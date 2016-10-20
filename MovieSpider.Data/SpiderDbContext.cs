using MovieSpider.Core.Extentions;
using MovieSpider.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Data
{
    public partial class SpiderDbContext : DbContext
    {
        public SpiderDbContext() : base("name=MovieDbConn")
        {
            Database.SetInitializer<SpiderDbContext>(null);
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
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        #region Entities
        public virtual DbSet<Movie> Movie { get; set; }

        #endregion
    }
}
