using Icarus_Item_Calculator.Models;
using Icarus_Item_Calculator.SeedData;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ItemContext : IdentityDbContext
{
    public ItemContext(DbContextOptions<ItemContext> options) : base(options) { }

    public DbSet<Item> Items { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<RecipeItem> RecipeItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Item>()
            .Property(i => i.ItemId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.Item)
            .WithMany(i => i.Recipes)
            .HasForeignKey(r => r.ItemId)
            .OnDelete(DeleteBehavior.Cascade); // Delete Recipes when Item is deleted

        modelBuilder.Entity<RecipeItem>()
            .HasOne(ri => ri.Item)
            .WithMany()
            .HasForeignKey(ri => ri.ItemId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of ingredients used in recipes

        modelBuilder.Entity<RecipeItem>()
            .HasOne(ri => ri.Recipe)
            .WithMany(r => r.Ingredients)
            .HasForeignKey(ri => ri.RecipeId)
            .OnDelete(DeleteBehavior.Cascade); // Delete RecipeItems when Recipe is deleted

        // Apply seed data from separate files
        ItemSeedData.Seed(modelBuilder);
        RecipeSeedData.Seed(modelBuilder);
        RecipeItemSeedData.Seed(modelBuilder);
    }
}
