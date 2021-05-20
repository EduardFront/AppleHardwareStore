using System.Linq;
using AppleHardwareStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AppleHardwareStore.Data
{
    public class AppleHardwareStoreDbContext : DbContext
    {
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Position> Positions { get; set; }


        public AppleHardwareStoreDbContext(DbContextOptions<AppleHardwareStoreDbContext> options) : base(options)
        {
            Database.EnsureCreated();
            if (!ProductTypes.Any())
            {
                ProductTypes.Add(new ProductType
                {
                    Name = "Phone"
                });
                ProductTypes.Add(new ProductType
                {
                    Name = "Laptop"
                });
                SaveChanges();
            }
            if (!Products.Any())
            {
                Products.Add(new Product
                {
                    Name = "IPhone6",
                    ProductTypeId = ProductTypes.FirstOrDefault(x => x.Name == "Phone").Id,
                    Price = 20000,
                    Description = "128gb, Processor A9, Color SpaceGray, Camera 9mp"
                });
                Products.Add(new Product
                {
                    Name = "IPhoneXR",
                    ProductTypeId = ProductTypes.FirstOrDefault(x => x.Name == "Phone").Id,
                    Price = 65000,
                    Description = "250gb, Processor A13, Color White, Camera 15mp"
                });
                Products.Add(new Product
                {
                    Name = "MacBook Pro",
                    ProductTypeId = ProductTypes.FirstOrDefault(x => x.Name == "Laptop").Id,
                    Price = 65000,
                    Description = "SSD - 250gb, RAM - 8gb, Processor - M9, Color - SpaceGray, GPU - AMD Radeon"
                });
                SaveChanges();
            }
        }

        /// <summary>
        /// Database models schema
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.ProductType>(entity =>
            {
                entity.ToTable("product_type");
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.HasKey(e => e.Id).HasName("product_type_pk");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Models.Product>(entity =>
            {
                entity.ToTable(name: "product");
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.HasKey(e => e.Id).HasName("product_pk");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(225);

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.Price)
                    .HasColumnName("price");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ProductTypeId).HasColumnName("product_type_id");
                entity.HasIndex(e => e.ProductTypeId).HasName("idx_product_type_id");
                entity.HasOne(e => e.ProductType)
                    .WithMany(pt => pt.Products)
                    .HasForeignKey(e => e.ProductTypeId);
            });

            modelBuilder.Entity<Models.OrderStatus>(entity =>
            {
                entity.ToTable(name: "order_status");
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.HasKey(e => e.Id).HasName("order_status_pk");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(225);
            });

            modelBuilder.Entity<Models.Order>(entity =>
            {
                entity.ToTable(name: "order");
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.HasKey(e => e.Id).HasName("order_pk");


                entity.Property(e => e.ClientName).HasColumnName("client_name");
                entity.Property(e => e.ClientAddress).HasColumnName("client_address");
                entity.Property(e => e.ClientPhone).HasColumnName("client_phone");
                entity.Property(e => e.ClientCardNumber).HasColumnName("client_card_number");

                entity.Property(e => e.TotalCost)
                    .HasColumnName("total_cost");

                entity.Property(e => e.OrderStatusId).HasColumnName("order_status_id");
                entity.HasIndex(e => e.OrderStatusId).HasName("idx_order_status_id");
                entity.HasOne(e => e.OrderStatus)
                    .WithMany(pt => pt.Orders)
                    .HasForeignKey(e => e.OrderStatusId);
            });

            modelBuilder.Entity<Models.Position>(entity =>
            {
                entity.ToTable(name: "position");
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.HasKey(e => e.Id).HasName("position_pk");


                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasColumnType("int");
                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.HasIndex(e => e.ProductId).HasName("idx_product_id");
                entity.HasOne(e => e.Product)
                    .WithMany(pt => pt.Positions)
                    .HasForeignKey(e => e.ProductId);

                entity.Property(e => e.OrderId).HasColumnName("order_id");
                entity.HasIndex(e => e.OrderId).HasName("idx_order_id");
                entity.HasOne(e => e.Order)
                    .WithMany(pt => pt.Positions)
                    .HasForeignKey(e => e.OrderId);
            });
        }


        /// <summary>
        /// Connecting configuration
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Startup.Configuration.GetConnectionString("DefaultConnection"));
        }
    }
}