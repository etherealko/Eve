using eth.Eve.Storage.Model;
using System.Data.Entity;

namespace eth.Eve.Storage
{
    public class EveDb : DbContext
    {
        static EveDb()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<EveDb>());
            //Database.SetInitializer(new DropCreateDatabaseAlways<EveDb>());
        }

        public virtual DbSet<EveSpace> EveSpaces { get; set; }

        public virtual DbSet<PluginStoreString> PluginStoreStrings { get; set; }
        public virtual DbSet<PluginStoreBinary> PluginStoreBinaries { get; set; }

        public EveDb() : this("name=EveDb") { }

        public EveDb(string nameOrConnectionString) : base(nameOrConnectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PluginStoreString>().HasKey(e => new { e.SpaceId, e.PluginGuid, e.Key });
            modelBuilder.Entity<PluginStoreBinary>().HasKey(e => new { e.SpaceId, e.PluginGuid, e.Key });
            modelBuilder.Entity<EveSpace>().HasKey(e => new { e.Id });

            modelBuilder.Entity<EveSpace>()
                .HasMany(e => e.PluginStoreStrings)
                .WithRequired()
                .HasForeignKey(e => e.SpaceId);

            modelBuilder.Entity<EveSpace>()
                .HasMany(e => e.PluginStoreBinaries)
                .WithRequired()
                .HasForeignKey(e => e.SpaceId);

            modelBuilder.Entity<PluginStoreBinary>().Property(e => e.Value).IsRequired();
        }
    }
}