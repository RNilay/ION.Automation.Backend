using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IonFiltra.BagFilters.Core.Common;
using IonFiltra.BagFilters.Core.Interfaces.GenericView;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.GenericView
{
    public class GenericViewRepository : IGenericViewRepository
    {
        private readonly TransactionHelper _transactionHelper;

        public GenericViewRepository(TransactionHelper transactionHelper)
        {
            _transactionHelper = transactionHelper;
        }

        public async Task<List<Dictionary<string, object>>> GetViewData(string viewName)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var result = new List<Dictionary<string, object>>();
                var connection = dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                // MySQL uses `database`.`object` or simply `object`. Use backticks to be safe.
                var qualified = string.IsNullOrWhiteSpace(GlobalConstants.IONFILTRA_SCHEMA)
                    ? $"`{viewName}`"
                    : $"`{GlobalConstants.IONFILTRA_SCHEMA}`.`{viewName}`";

                var query = $"SELECT * FROM {qualified} WHERE IsDeleted = false";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    // Attach the current EF Core transaction to the command
                    var currentTransaction = dbContext.Database.CurrentTransaction;
                    if (currentTransaction != null)
                    {
                        command.Transaction = currentTransaction.GetDbTransaction();
                    }

                    try
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var row = new Dictionary<string, object>();
                                for (var i = 0; i < reader.FieldCount; i++)
                                {
                                    var columnName = reader.GetName(i).ToLower();
                                    var value = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
                                    row[columnName] = value;
                                }
                                result.Add(row);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // preserve original behavior but log/inspect as needed
                        Console.WriteLine(ex);
                    }
                }

                return result;
            });
        }

        public async Task<List<Dictionary<string, object>>> GetViewDataWithParam(string viewName, Dictionary<string, object> parameters)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var result = new List<Dictionary<string, object>>();
                var connection = dbContext.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                var qualified = string.IsNullOrWhiteSpace(GlobalConstants.IONFILTRA_SCHEMA)
                    ? $"`{viewName}`"
                    : $"`{GlobalConstants.IONFILTRA_SCHEMA}`.`{viewName}`";

                var query = $"SELECT * FROM {qualified}";
                int enquiryId = 0;

                if (parameters != null && parameters.Count > 0)
                {
                    var conditions = parameters.Select((param, index) => $"`{param.Key}` = @param{index}");
                    query += " WHERE " + string.Join(" AND ", conditions);

                    if (parameters.ContainsKey("enquiryId"))
                    {
                        enquiryId = Convert.ToInt32(parameters["enquiryId"]);
                    }
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    if (parameters != null)
                    {
                        int index = 0;
                        foreach (var param in parameters)
                        {
                            var parameter = command.CreateParameter();
                            parameter.ParameterName = $"@param{index}";
                            parameter.Value = param.Value ?? DBNull.Value;
                            // optionally set DbType if you know it: parameter.DbType = DbType.Int32; etc
                            command.Parameters.Add(parameter);
                            index++;
                        }
                    }

                    var currentTransaction = dbContext.Database.CurrentTransaction;
                    if (currentTransaction != null)
                    {
                        command.Transaction = currentTransaction.GetDbTransaction();
                    }

                    try
                    {
                        command.CommandTimeout = 120;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var row = new Dictionary<string, object>();
                                for (var i = 0; i < reader.FieldCount; i++)
                                {
                                    var columnName = reader.GetName(i);
                                    var value = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
                                    row[columnName] = value;
                                }
                                result.Add(row);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

                return result;
            });
        }

        public async Task<int> InsertAsync(string tableName, Dictionary<string, object> data)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var connection = dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                var columns = string.Join(", ", data.Keys.Select(k => $"`{k}`"));
                var parameters = string.Join(", ", data.Keys.Select((k, i) => $"@p{i}"));

                var query = $"INSERT INTO `{GlobalConstants.IONFILTRA_SCHEMA}`.`{tableName}` ({columns}) VALUES ({parameters})";

                using var command = connection.CreateCommand();
                command.CommandText = query;

                int index = 0;
                foreach (var kv in data)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = $"@p{index}";
                    parameter.Value = kv.Value ?? DBNull.Value;
                    command.Parameters.Add(parameter);
                    index++;
                }

                var transaction = dbContext.Database.CurrentTransaction;
                if (transaction != null)
                    command.Transaction = transaction.GetDbTransaction();

                await command.ExecuteNonQueryAsync();

                // Return inserted ID
                command.CommandText = "SELECT LAST_INSERT_ID();";
                var id = Convert.ToInt32(await command.ExecuteScalarAsync());

                return id;
            });
        }

        public async Task<int> UpdateAsync(string tableName, int id, Dictionary<string, object> data)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var connection = dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                var setters = string.Join(", ", data.Keys.Select((k, i) => $"`{k}` = @p{i}"));
                var query = $"UPDATE `{GlobalConstants.IONFILTRA_SCHEMA}`.`{tableName}` SET {setters} WHERE Id = @id";

                using var command = connection.CreateCommand();
                command.CommandText = query;

                int index = 0;
                foreach (var kv in data)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = $"@p{index}";
                    parameter.Value = kv.Value ?? DBNull.Value;
                    command.Parameters.Add(parameter);
                    index++;
                }

                var idParam = command.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = id;
                command.Parameters.Add(idParam);

                var transaction = dbContext.Database.CurrentTransaction;
                if (transaction != null)
                    command.Transaction = transaction.GetDbTransaction();

                return await command.ExecuteNonQueryAsync();
            });
        }

        public async Task<int> DeleteAsync(string tableName, int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var connection = dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                string query = $"UPDATE `{GlobalConstants.IONFILTRA_SCHEMA}`.`{tableName}` SET IsDeleted = 1 WHERE Id = @id";

                using var command = connection.CreateCommand();
                command.CommandText = query;

                var idParam = command.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = id;
                command.Parameters.Add(idParam);

                var transaction = dbContext.Database.CurrentTransaction;
                if (transaction != null)
                    command.Transaction = transaction.GetDbTransaction();

                return await command.ExecuteNonQueryAsync();
            });
        }

        public async Task ExecuteRawSqlAsync(string sql)
        {
            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var connection = dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sql;

                var transaction = dbContext.Database.CurrentTransaction;
                if (transaction != null)
                    command.Transaction = transaction.GetDbTransaction();

                await command.ExecuteNonQueryAsync();

                return 0; // dummy return because ExecuteAsync expects a return value
            });
        }

        public async Task ResetDefaultsInAllTablesAsync()
{
    await _transactionHelper.ExecuteAsync(async dbContext =>
    {
        var connection = dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        // 1️⃣ Get all tables that have IsDefault column
        string query = $@"
            SELECT TABLE_NAME 
            FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE COLUMN_NAME = 'IsDefault'
              AND TABLE_SCHEMA = '{GlobalConstants.IONFILTRA_SCHEMA}';
        ";

        var tables = new List<string>();

        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = query;
            var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                tables.Add(reader.GetString(0));
            }
            reader.Close();
        }

        // 2️⃣ Reset defaults in ALL tables
        foreach (var table in tables)
        {
            using var resetCmd = connection.CreateCommand();
            resetCmd.CommandText = $"UPDATE `{GlobalConstants.IONFILTRA_SCHEMA}`.`{table}` SET IsDefault = 0 WHERE IsDefault = 1;";
            await resetCmd.ExecuteNonQueryAsync();
        }

        return 0;
    });
}




    }
}
