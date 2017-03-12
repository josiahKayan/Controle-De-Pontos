
using Apassos.TeamWork.JsonObject;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Apassos.Models
{
    /// <summary>
    /// Class TimesheetContext.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public partial class TimesheetContext : DbContext
   {
        /// <summary>
        /// Interface IDescribableEntity
        /// </summary>
        public interface IDescribableEntity
      {
            // Override this method to provide a description of the entity for audit purposes
            /// <summary>
            /// Describes this instance.
            /// </summary>
            /// <returns>System.String.</returns>
            string Describe();
      }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimesheetContext"/> class.
        /// </summary>
        public TimesheetContext()
          : base("TimesheetContext")
      {

      }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.</remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
         //modelBuilder.Entity<aspnet_UsersInRoles>().HasMany(i => i.Users).WithRequired().WillCascadeOnDelete(false);
         modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
      }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>The users.</value>
        public DbSet<Users> Users { get; set; }
        /// <summary>
        /// Gets or sets the partners.
        /// </summary>
        /// <value>The partners.</value>
        public DbSet<Partners> Partners { get; set; }
        /// <summary>
        /// Gets or sets the periods.
        /// </summary>
        /// <value>The periods.</value>
        public DbSet<Period> Periods { get; set; }
        /// <summary>
        /// Gets or sets the projects.
        /// </summary>
        /// <value>The projects.</value>
        public DbSet<Project> Projects { get; set; }
        /// <summary>
        /// Gets or sets the project users.
        /// </summary>
        /// <value>The project users.</value>
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        /// <summary>
        /// Gets or sets the timesheet headers.
        /// </summary>
        /// <value>The timesheet headers.</value>
        public DbSet<TimesheetHeader> TimesheetHeaders { get; set; }
        /// <summary>
        /// Gets or sets the timesheet items.
        /// </summary>
        /// <value>The timesheet items.</value>
        public DbSet<TimesheetItem> TimesheetItems { get; set; }
        /// <summary>
        /// Gets or sets the countries.
        /// </summary>
        /// <value>The countries.</value>
        public DbSet<Country> Countries { get; set; }
        /// <summary>
        /// Gets or sets the state of the brazil.
        /// </summary>
        /// <value>The state of the brazil.</value>
        public DbSet<BrazilState> BrazilState { get; set; }
        /// <summary>
        /// Gets or sets the brazil city.
        /// </summary>
        /// <value>The brazil city.</value>
        public DbSet<BrazilCity> BrazilCity { get; set; }
        /// <summary>
        /// Gets or sets the perfils.
        /// </summary>
        /// <value>The perfils.</value>
        public DbSet<Perfil> Perfils { get; set; }
        /// <summary>
        /// Gets or sets the user perfils.
        /// </summary>
        /// <value>The user perfils.</value>
        public DbSet<UserPerfil> UserPerfils { get; set; }
        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public DbSet<Permission> Permissions { get; set; }
        /// <summary>
        /// Gets or sets the perfil permissions.
        /// </summary>
        /// <value>The perfil permissions.</value>
        public DbSet<PerfilPermissions> PerfilPermissions { get; set; }
        /// <summary>
        /// Gets or sets the timesheet team work items.
        /// </summary>
        /// <value>The timesheet team work items.</value>
        public DbSet<TimesheetTeamWorkItem> TimesheetTeamWorkItems { get; set; }
        /// <summary>
        /// Gets or sets the teamwork root cause problems.
        /// </summary>
        /// <value>The teamwork root cause problems.</value>
        public DbSet<TeamworkRootCauseProblems> TeamworkRootCauseProblems { get; set; }
        /// <summary>
        /// Gets or sets the teamwork log traces.
        /// </summary>
        /// <value>The teamwork log traces.</value>
        public DbSet<TeamworkLogTraces> TeamworkLogTraces { get; set; }
        /// <summary>
        /// Gets or sets the checkins.
        /// </summary>
        /// <value>The checkins.</value>
        public DbSet<Checkins> Checkins { get; set; }
        /// <summary>
        /// Gets or sets the cities.
        /// </summary>
        /// <value>The cities.</value>
        public DbSet<Cities> Cities { get; set; }
        /// <summary>
        /// Gets or sets the feriados.
        /// </summary>
        /// <value>The feriados.</value>
        public DbSet<Feriados> Feriados { get; set; }

        public DbSet<InfoObjects> InfoObjects { get; set; }
    }
}