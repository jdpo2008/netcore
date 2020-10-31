using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tgs.Entities.Seguridad;

namespace Tgs.DataAccess.Core.Context
{
    public class ApplicationDbContext : IdentityDbContext<
        ApplicationUser, ApplicationRole, int,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.HasDefaultSchema("SEGURIDAD");
            modelBuilder.Entity<ApplicationUser>(b =>
            {
            // Each User can have many UserClaims
            b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

            // Each User can have many UserLogins
            b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

            // Each User can have many UserTokens
            b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

            // Set Max Length 
            b.Property(u => u.FirstName).HasMaxLength(50);
                b.Property(u => u.LastName).HasMaxLength(50);
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

            // Each Role can have many associated RoleClaims
            b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();

            // Set Max Length 
            b.Property(u => u.Description).HasMaxLength(25);
            });

            modelBuilder.Entity<ApplicationUser>().ToTable("Users", "SEGURIDAD");

            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaims", "SEGURIDAD");

            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogins", "SEGURIDAD");

            modelBuilder.Entity<ApplicationUserToken>().ToTable("UserTokens", "SEGURIDAD");

            modelBuilder.Entity<ApplicationRole>().ToTable("Roles", "SEGURIDAD");

            modelBuilder.Entity<ApplicationRoleClaim>().ToTable("RoleClaims", "SEGURIDAD");

            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRoles", "SEGURIDAD");

        }
    }
}
