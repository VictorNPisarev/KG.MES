// KG.MES.Server/Data/AppDbContext.cs
using KG.MES.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KG.MES.Server.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<Workplace> Workplaces { get; set; }
	public DbSet<WorkplaceTransition> WorkplaceTransitions { get; set; }
	public DbSet<Order> Orders { get; set; }
	public DbSet<ProductionOrder> ProductionOrders { get; set; }
	public DbSet<OrderFootprint> OrderFootprints { get; set; }
	public DbSet<OperationLog> OperationLogs { get; set; }
	public DbSet<OrderBlock> OrderBlocks { get; set; }
	public DbSet<SupplyType> SupplyTypes { get; set; }
	public DbSet<SupplyCondition> SupplyConditions { get; set; }
	public DbSet<OrderSupply> OrderSupplies { get; set; }
	public DbSet<SupplyItem> SupplyItems { get; set; }
	public DbSet<Comment> Comments { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<Role> Roles { get; set; }
	public DbSet<UserWorkplace> UserWorkplaces { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Настройка связей и индексов
		modelBuilder.Entity<Workplace>()
			.HasIndex(w => w.LegacyId);

		modelBuilder.Entity<Order>()
			.HasIndex(o => o.OrderNumber);

		modelBuilder.Entity<User>()
			.HasIndex(u => u.Email)
			.IsUnique();

		// Массивы UUID
		modelBuilder.Entity<Order>()
			.Property(o => o.CommentIds)
			.HasColumnType("uuid[]");

		modelBuilder.Entity<ProductionOrder>()
			.Property(p => p.CommentIds)
			.HasColumnType("uuid[]");

		modelBuilder.Entity<OrderSupply>()
			.Property(o => o.CommentIds)
			.HasColumnType("uuid[]");

		// Уникальные ограничения
		modelBuilder.Entity<WorkplaceTransition>()
			.HasIndex(wt => new { wt.FromWorkplaceId, wt.ToWorkplaceId })
			.IsUnique();

		modelBuilder.Entity<UserWorkplace>()
			.HasIndex(uw => new { uw.UserId, uw.WorkplaceId })
			.IsUnique();

		base.OnModelCreating(modelBuilder);
	}
}