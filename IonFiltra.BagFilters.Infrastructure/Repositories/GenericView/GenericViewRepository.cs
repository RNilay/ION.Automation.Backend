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

                var query = $"SELECT * FROM {qualified}";

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
    }
}
