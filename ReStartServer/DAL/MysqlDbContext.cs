using ReStartServer.Model;
using log4net;
using MySql.Data.Entity;
using System.Data.Entity;
using System.Reflection;
using ReStartServer.DAL;

namespace ReStartServer.DAL
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public partial class MysqlDbContext : DbContext
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static MysqlDbContext()
        {
        }

        public MysqlDbContext()
            : base("name=MysqlDbConn")
        {
            //add log for EF gen SQL by xlg
            Database.Log = message => logger.DebugFormat(message.Replace("\n", " "));

            //Configuration.ProxyCreationEnabled = true;
            //Configuration.LazyLoadingEnabled = true;

            //Enable-Migrations
            //Enable-Migrations -Force
            //Update-Database
        }

        //以下是数据库上下文对象，以后对数据库的访问就用下面对象
        public DbSet<UploadData> Datas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //指定单数形式的表名
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<UploadData>();
        }
    }
}