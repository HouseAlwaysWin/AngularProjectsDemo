using System;
using EcommerceApi.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Core.Data.Identity
{
    public class ECIdentityDbContext:IdentityDbContext<ECUser,ECRole,int>
    {
        public ECIdentityDbContext(DbContextOptions<ECIdentityDbContext> options ):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ECUser>()
                .HasOne(u => u.Address)
                .WithOne(u => u.ECUser)
                .HasForeignKey<UserAddress>(a => a.ECUserId);
            
            builder.Entity<ECUser>(a => {
                a.ToTable("ECUsers");
                a.Property(a=>a.CreatedDate)
                    .HasDefaultValueSql("GETDATE()")
                    .IsRequired();
            });

            builder.Entity<ECRole>(b =>
            {
                b.ToTable("ECRoles");
                b.Property(b=>b.CreatedDate)
                    .HasDefaultValueSql("GETDATE()")
                    .IsRequired();


            });

            

           builder.Entity<IdentityUserClaim<int>>(b =>
            {
                b.ToTable("ECUserClaims");
            });

            builder.Entity<IdentityUserLogin<int>>(b =>
            {
                b.ToTable("ECUserLogin");
            });

            builder.Entity<IdentityUserToken<int>>(b =>
            {
                b.ToTable("ECUserTokens");
            });

            

            builder.Entity<IdentityRoleClaim<int>>(b =>
            {
                b.ToTable("ECRoleClaims");
            });

            builder.Entity<IdentityUserRole<int>>(b =>
            {
                b.ToTable("ECUserRoles");
            }); 

        }
    }

    
}