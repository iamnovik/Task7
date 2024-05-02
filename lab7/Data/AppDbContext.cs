using lab7.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace lab7.Data;

public class AppDbContext : IdentityDbContext<Contact>
{

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ContactRelationship> ContactRelationships { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ContactRelationship>()
            .HasKey(c => new { c.SourceContactId, c.TargetContactId });

        builder.Entity<ContactRelationship>()
            .HasOne(c => c.SourceContact)
            .WithMany(c => c.ContactRelationships)
            .HasForeignKey(c => c.SourceContactId);

        builder.Entity<ContactRelationship>()
            .HasOne(c => c.TargetContact)
            .WithMany()
            .HasForeignKey(c => c.TargetContactId)
            .OnDelete(DeleteBehavior.Restrict);

            
    }
}