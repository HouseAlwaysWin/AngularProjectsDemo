using System.ComponentModel.DataAnnotations;
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
        public DbSet<UserFriend> UserFriends { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<MessageGroup> MessageGroup { get; set; }
        public DbSet<MessageConnection> MessageConnection { get; set; }
        public DbSet<MessageRecivedUser> MessageRecivedUser { get; set;}
        public DbSet<Notification> Notification { get; set; }

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
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                a.HasOne(u => u.Address)
                 .WithOne(u => u.AppUser)
                 .HasForeignKey<UserAddress>(a => a.AppUserId);

                
                a.HasMany(ur => ur.AppUserRoles)
                 .WithOne(u => u.User)
                 .HasForeignKey(ur => ur.UserId)
                 .IsRequired();

                a.HasMany(u => u.Notifications)
                 .WithOne(n => n.AppUser)
                 .HasForeignKey(n => n.AppUserId);


                //  a.HasMany(u => u.Friends)                
                //     .WithMany(u => u.FriendsReverse)
                //     .UsingEntity<UserFriend>("UserFriend",
                //         x => x.HasOne(uf => uf.AppUser).WithMany().HasForeignKey(uf => uf.AppUserId),
                //         x => x.HasOne(uf => uf.Friend).WithMany().HasForeignKey(uf => uf.FriendId),
                //         x => x.ToTable("UserFriend"));
                    
                 a.HasIndex(u => u.UserPublicId)
                    .IsUnique();

                    
            });

            builder.Entity<Notification>(n => {
                n.Property(n => n.CreatedDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd()
                    .IsRequired();
            });

            builder.Entity<UserFriend>(ur =>{
                ur.HasKey(ur => new {ur.FriendId,ur.AppUserId });
                ur.Property(a=>a.CreatedDate)
                    // .HasDefaultValueSql("GETDATE()")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                ur.HasOne<AppUser>(ur => ur.AppUser)
                    .WithMany(u => u.Friends)
                    .HasForeignKey(ur => ur.AppUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                ur.HasOne(ur => ur.Friend)
                    .WithMany(u => u.FriendsReverse)
                    .HasForeignKey(ur => ur.FriendId)
                    .OnDelete(DeleteBehavior.NoAction);

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

                b.Property(a=>a.CreatedDate)
                    // .HasDefaultValueSql("GETDATE()")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd()
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

            builder.Entity<AppUser_MessageGroup>(a =>{
                a.HasKey(am => new {am.AppUserId,am.MessageGroupId});
                a.ToTable("AppUser_MessageGroup");

                a.HasOne(a => a.AppUser)
                 .WithMany(a => a.MessageGroups)
                 .HasForeignKey(a => a.AppUserId)
                 .OnDelete(DeleteBehavior.NoAction);
                
                a.HasOne(m => m.MessageGroup)
                 .WithMany(m => m.AppUsers)
                 .HasForeignKey(m => m.MessageGroupId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Message>(m => {

              m.HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

              m.Property(a=>a.CreatedDate)
                    // .HasDefaultValueSql("GETDATE()")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd()
                    .IsRequired();

              m.HasMany(a => a.RecipientUsers)
                .WithOne()
                .HasForeignKey(a => a.Id)
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<MessageRecivedUser>(m => {
                m.HasOne(mr=> mr.Message)
                 .WithMany(m => m.RecipientUsers)
                 .HasForeignKey(mr => mr.Id);
            });

            builder.Entity<MessageGroup>(mg => {
                mg.HasAlternateKey(mg => mg.AlternateId);
                mg.HasMany(mg => mg.Connections)
                  .WithOne()
                  .OnDelete(DeleteBehavior.Cascade);
                
                mg.HasMany(mg => mg.Messages)
                  .WithOne()
                  .OnDelete(DeleteBehavior.Cascade);

            });


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