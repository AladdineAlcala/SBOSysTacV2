using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;
using Audit.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SBOSysTacV2.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middle { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
       // public virtual UserProfile UserProfileInformation { get; set; }

        public class ApplicationDbContext :IdentityDbContext<ApplicationUser>
        {
            public ApplicationDbContext()
                : base("PegasusEntities2", throwIfV1Schema: false)
            {
            }

          //  public DbSet<UserProfile> UserProfiles { get; set; }
            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<ApplicationUser>().ToTable("Users");
                modelBuilder.Entity<IdentityRole>().ToTable("Roles");
                modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
                modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
                modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
               
              // modelBuilder.Entity<UserProfile>().ToTable("UserProfile");

                //modelBuilder.Configurations.Add(new UserProfileMap());

            }


            public static ApplicationDbContext Create()
            {
                return new ApplicationDbContext();
            }
        }

    }

    public class UserProfile
    {
        [Key]
        public int No { get; set; }
        public string UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateofBirth { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser Users { get; set; }
    }

    //public class UserProfileMap:EntityTypeConfiguration<UserProfile>
    //{
    //    public UserProfileMap()
    //    {
    //        ToTable("UserProfile");
    //        HasKey(t => t.No);
    //        Property(t => t.No).HasColumnName("No");
    //        //Property(t => t.UserId).HasColumnName("UserId");
    //        Property(t => t.Firstname).HasColumnName("Firstname");
    //        Property(t => t.Lastname).HasColumnName("Lastname");
    //        Property(t => t.DateofBirth).HasColumnName("DateofBirth");

    //        HasRequired(x => x.Users).WithRequiredDependent(s => s.UserProfileInformation);
    //    }

     
    //}
}