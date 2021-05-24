using System;
using BackendApi.Core.Entities.Identity;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Core.Data.Identity
{
    public class UserContext:
        IdentityDbContext<AppUser,AppRole,int,
        IdentityUserClaim<int>,AppUserRole,IdentityUserLogin<int>,
        IdentityRoleClaim<int>,IdentityUserToken<int>>
    {
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<UserPhoto> UserPhoto { get; set; }
        public DbSet<Message> Message { get; set; }
        public UserContext(DbContextOptions<UserContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

                            
            builder.Entity<AppUser>(a => {
                a.ToTable("AppUsers");
                a.Property(a=>a.CreatedDate)
                    // .HasDefaultValueSql("GETDATE()")
                    .IsRequired();
                a.HasOne(u => u.Address)
                 .WithOne(u => u.AppUser)
                 .HasForeignKey<UserAddress>(a => a.AppUserId);
                
                a.HasMany(ur => ur.AppUserRoles)
                 .WithOne(u => u.User)
                 .HasForeignKey(ur => ur.UserId)
                 .IsRequired();

                 a.HasIndex(u => u.UserPublicId)
                    .IsUnique();
                    
            });

            builder.Entity<AppRole>(b =>
            {
                b.ToTable("AppRoles");
                b.Property(b=>b.CreatedDate)
                    // .HasDefaultValueSql("GETDATE()")
                    .IsRequired();
                b.HasMany(ur => ur.AppUserRoles)
                 .WithOne(u => u.Role)
                 .HasForeignKey(ur => ur.RoleId)
                 .IsRequired();
            });

            builder.Entity<AppUserRole>(b =>
            {
                b.ToTable("AppUserRoles");
                b.HasOne(ur => ur.Role)
                 .WithMany(r => r.AppUserRoles)
                 .HasForeignKey(ur => ur.RoleId)
                 .IsRequired();
                
                b.HasOne(ur => ur.User)
                 .WithMany(u => u.AppUserRoles)
                 .HasForeignKey(ur => ur.UserId)
                 .IsRequired();
            }); 


            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            

           builder.Entity<IdentityUserClaim<int>>(b =>
            {
                b.ToTable("AppUserClaims");
            });

            builder.Entity<IdentityUserLogin<int>>(b =>
            {
                b.ToTable("AppUserLogin");
            });

            builder.Entity<IdentityUserToken<int>>(b =>
            {
                b.ToTable("AppUserTokens");
            });

            

            builder.Entity<IdentityRoleClaim<int>>(b =>
            {
                b.ToTable("AppRoleClaims");
            });

    

        }
    }

    
}