using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TPOS.Core.Interfaces.Services;
using Object = TPOS.Core.Entities.Object;

namespace TPOS.Infrastructure.Initialization
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public DbInitializer(AppDbContext context, ILogger<DbInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            ArgumentNullException.ThrowIfNull(_context, nameof(_context));
            //await _context.Database.MigrateAsync().ConfigureAwait(false);

            // migrations if they are not applied
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }

                await SeedDataAsync();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        private async Task SeedDataAsync()
        {
            try
            {
                _logger.LogInformation("Seeding Initial Data");

                DateTime _now = DateTime.UtcNow;

                #region Populate Objects

                #region Currency            
                await EnsureObjectAsync(new Object { ObjType = "CURRENCY", ObjKey = "MMK", ObjDesc = "Myanmar Kyat", ObjText1 = "1", IsSysObj = true, Active = true, CreatedBy = 1, UpdatedBy = 1, CreatedOn = _now, UpdatedOn = _now }, "CURRENCY", "MMK");
                await EnsureObjectAsync(new Object { ObjType = "CURRENCY", ObjKey = "USD", ObjDesc = "US Dollar", IsSysObj = true, Active = true, CreatedBy = 1, UpdatedBy = 1, CreatedOn = _now, UpdatedOn = _now }, "CURRENCY", "USD");
                #endregion

                #endregion

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        #region Helpers
        //private async Task EnsureAsync<T>(T data, params string[] id) where T : class
        //{
        //    if (await _context.Set<T>().FindAsync(id) == null)
        //    {
        //        await _context.Set<T>().AddAsync(data);
        //    }
        //}

        private async Task EnsureObjectAsync(Object data, string objType, string objKey)
        {
            var obj = await _context.Set<Object>().SingleOrDefaultAsync(x => x.ObjType == objType && x.ObjKey == objKey);
            if (obj == null)
            {
                await _context.Set<Object>().AddAsync(data);
            }
        }
        #endregion
    }
}
