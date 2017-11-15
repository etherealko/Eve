using eth.Eve.Storage.Model;
using Microsoft.EntityFrameworkCore;

namespace eth.Eve.Storage
{
    public class EveDb : DbContext
    {
        public virtual DbSet<EveSpace> EveSpaces { get; set; }

        public virtual DbSet<PluginStoreString> PluginStoreStrings { get; set; }
        public virtual DbSet<PluginStoreBinary> PluginStoreBinaries { get; set; }
        
        public EveDb(DbContextOptions<EveDb> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PluginStoreString>().HasKey(e => new { e.SpaceId, e.PluginGuid, e.Key });
            modelBuilder.Entity<PluginStoreBinary>().HasKey(e => new { e.SpaceId, e.PluginGuid, e.Key });
            modelBuilder.Entity<EveSpace>().HasKey(e => new { e.Id });

            modelBuilder.Entity<EveSpace>()
                .HasMany(e => e.PluginStoreStrings)
                .WithOne()
                .HasForeignKey(e => e.SpaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EveSpace>()
                .HasMany(e => e.PluginStoreBinaries)
                .WithOne()
                .HasForeignKey(e => e.SpaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PluginStoreBinary>().Property(e => e.Value).IsRequired();

            modelBuilder.Entity<EveSpace>().Property(e => e.BotApiAccessToken).HasMaxLength(50);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}