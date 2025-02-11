﻿using LittleArkFoundation_WebInventorySystem.Models;
using Microsoft.Data.SqlClient;

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

        public async Task<IEnumerable<RolesModel>> GetRolesAsync(string dbType)
        {
            string connectionString = _connectionService.GetConnectionString(dbType);
            using (var context = new ApplicationDbContext(connectionString))
            {
                return await Task.Run(() => context.Roles.ToList());
            }
        }

        public async Task<string> GetRoleNameByRoleID(int roleID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = $"SELECT RoleName FROM Roles WHERE RoleID = @RoleID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleID", roleID);
                    await connection.OpenAsync();
                    var result = await command.ExecuteScalarAsync();
                    return (string)result;
                }
            }
        }
    }
}
