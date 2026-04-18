using LotTrace_MES.src.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace LotTrace_MES.src.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Lot> Lots { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<LogHistories> LogHistories { get; set; }
        public DbSet<Line> Lines { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // LogHistories와 Lot의 관계에서 연쇄 삭제 방지
            modelBuilder.Entity<LogHistories>()
                .HasOne(lh => lh.Lot)
                .WithMany()
                .HasForeignKey(lh => lh.LotId)
                .OnDelete(DeleteBehavior.Restrict);

            // LogHistories와 Worker의 관계에서 연쇄 삭제 방지
            modelBuilder.Entity<LogHistories>()
                .HasOne(lh => lh.Worker)
                .WithMany()
                .HasForeignKey(lh => lh.WorkerId)
                .OnDelete(DeleteBehavior.Restrict);
            // Lot 테이블 내의 외래키들도 안전하게 설정
            modelBuilder.Entity<Lot>()
                .HasOne(l => l.Worker)
                .WithMany()
                .HasForeignKey(l => l.WorkerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
