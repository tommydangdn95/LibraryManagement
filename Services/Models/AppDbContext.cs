using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Services.Dtos;
using Services.Models._Users;
using Services.Utils;
using System.Data;
using System.Data.SqlTypes;

namespace Services.Models
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid,
        IdentityUserClaim<Guid>, IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
    {
        private SqlConnection _sqlConn;
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Branch> Branchs { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<BorrowRequest> BorrowRequest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public async Task<PagedResult<T>> ExecutePagedListRead<T>(String cmdText, List<SqlParameter> sqlParams, Func<SqlDataReader, T> mapper)
        {
            T e;
            PagedResult<T> pagedResult = PagedResult<T>.Empty();

            using (SqlCommand SQLCmd = new SqlCommand())
            {
                SQLCmd.CommandText = cmdText;
                SQLCmd.Connection = GetSqlConnection();
                SQLCmd.CommandType = CommandType.StoredProcedure;

                foreach (SqlParameter param in sqlParams)
                {
                    SQLCmd.Parameters.Add(param);
                }

                await EnsureConnectionOpenAsync(SQLCmd.Connection);

                using (var reader = await SQLCmd.ExecuteReaderAsync())
                {
                    while ( await reader.ReadAsync())
                    {
                        e = mapper(reader);
                        pagedResult.TotalCount = reader.GetValue<int>("TotalCount");
                        pagedResult.PageNumber = reader.GetValue<int>("CurrentPage");
                        pagedResult.PageSize = reader.GetValue<int>("RowsPerPage");
                        pagedResult.Items.Add(e);
                    }
                }
            }

            return pagedResult;
        }

        private SqlConnection GetSqlConnection()
        {
            if (_sqlConn == null)
            {
                var connection = Database.GetDbConnection() as SqlConnection;
                if (connection == null)
                    throw new InvalidOperationException("Connection is not SqlConnection");
                _sqlConn = connection;
            }
            return _sqlConn;
        }

        private async Task EnsureConnectionOpenAsync(SqlConnection connection)
        {
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();
        }

    }
}
