using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using GymManagement.DbContexts;
using GymManagement.Models;
using Microsoft.Extensions.Logging;

namespace GymManagement.DAL.DateSeeding
{
    public static class GymDataSeeding
    {
        public static async Task SeedAsync(GymDbcontext context , string seedFolderPath, ILogger logger , CancellationToken ct = default)
        {
            try
            {
                if(!context.Plans.Any() )
                {

                   var plans = LoadDataFromJsonFile<Plan>(seedFolderPath, "Plans.json");
                    //Add to DataBase
                    if (plans.Any())
                    {
                        await context.Plans.AddRangeAsync(plans);
                        await context.SaveChangesAsync(ct);
                        logger.LogInformation($"Seeding plans with Count {plans.Count}");
                    }

                }
            }
            catch (Exception ex) {
                logger.LogError(ex.Message);
            }
       
        
        }

        public static List<T> LoadDataFromJsonFile<T>(string folderPath, string fileName)
        {
            var filePath = Path.Combine(folderPath, "Plans.json");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Seed Data File not found at path: {filePath}");

            //Read Data from Json file plans.json as jsonstring (string)
            var data = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            //convert jsonstring to List<Plan> using Deserialization
            var result = JsonSerializer.Deserialize<List<T>>(data, options) ?? [];
            return result;
        }

    }
}
