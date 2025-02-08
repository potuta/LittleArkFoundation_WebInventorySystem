using LittleArkFoundation_WebInventorySystem.Models;

namespace LittleArkFoundation_WebInventorySystem.Data.Repositories
{
    public class RolesRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionService _connectionService;

        public RolesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public RolesRepository(ConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public IEnumerable<RolesModel> GetRoles()
        {
            using (var context = new ApplicationDbContext(_connectionString))
            {
                return context.Roles.ToList();
            }
        }

        public IEnumerable<RolesModel> GetRoles(string dbType)
        {
            string connectionString = _connectionService.GetConnectionString(dbType);
            using (var context = new ApplicationDbContext(connectionString))
            {
                return context.Roles.ToList();
            }
        }
    }
}
