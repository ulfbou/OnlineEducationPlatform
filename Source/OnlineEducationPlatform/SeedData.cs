using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Persistence.Data;
using OnlineEducationPlatform.Shared.Entities;

namespace OnlineEducationPlatform
{
    internal class SeedData
    {
        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;
        private ApplicationDbContext _context;
        private ILogger<SeedData> _logger;

        private SeedData(UserManager<User> userManager, RoleManager<Role> roleManager, ApplicationDbContext context, ILogger<SeedData> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public static async Task Initialize(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<Role>>();
                var logger = services.GetRequiredService<ILogger<SeedData>>();
                var seed = new SeedData(userManager, roleManager, context, logger);

                await seed.SeedMigration();
                await seed.SeedRoles();
            }
        }

        internal static async Task InitializeAsync(WebApplication app)
        {
            throw new NotImplementedException();
        }

        private async Task SeedMigration()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while migrating the database.");
                throw new InvalidOperationException("An error occurred while migrating the database.", ex);
            }
        }

        private async Task SeedRoles()
        {
            IdentityResult? result = null;
            try
            {
                var roles = new List<string>()
                {
                    Role.SuperAdmin,
                    Role.TenantAdmin,
                    Role.CourseCreator,
                    Role.CourseEditor,
                    Role.CourseViewer,
                    Role.Instructor,
                    Role.Student,
                    Role.User
                };

                foreach (var role in roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        result = await _roleManager.CreateAsync(new Role
                        {
                            Name = role,
                            CreatedAt = DateTime.UtcNow
                        });
                        if (!result.Succeeded)
                        {
                            _logger.LogError($"Failed to seed a role: {role}");
                            throw new InvalidOperationException($"Failed to seed a role: {role}");
                        }

                        _logger.LogInformation($"Created {role} role.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding roles.");
            }
        }
    }
}