using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using System;
using Lisa.Common.TableStorage;
using Lisa.Common.WebApi;

namespace Lisa.RobotArm.Api.Database
{
    public class TableStorage
    {
        public static async Task<IEnumerable<object>> GetLevels()
        {
            var account = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            var client = account.CreateCloudTableClient();
            var levels = client.GetTableReference("Levels");


            await levels.CreateIfNotExistsAsync();

            TableQuery<DynamicEntity> query = new TableQuery<DynamicEntity>();
            TableQuerySegment<DynamicEntity> levelsInformation = await levels.ExecuteQuerySegmentedAsync(query, null);

            IEnumerable<object> result = levelsInformation.Results;
            var Levels = levelsInformation.Select(L => LevelMapper.ToModel(L));

            return Levels;
        }
        public static async Task<object> GetLevel(String Slug)
        {
            var account = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            var client = account.CreateCloudTableClient();
            var level = client.GetTableReference("Levels");

            await level.CreateIfNotExistsAsync();

            TableQuery<DynamicEntity> query = new TableQuery<DynamicEntity>().Where(TableQuery.GenerateFilterCondition("Slug", QueryComparisons.Equal, Slug));
            TableQuerySegment<DynamicEntity> levelInformation = await level.ExecuteQuerySegmentedAsync(query, null);

            object result = levelInformation.SingleOrDefault();
            var Level = levelInformation.Select(L => LevelMapper.ToModel(L));

            return Level;
        }

        public static async Task<object> PostLevel(dynamic levels)
        {
            var account = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            var client = account.CreateCloudTableClient();
            var level = client.GetTableReference("Levels");

            await level.CreateIfNotExistsAsync();

            var NewLevel = LevelMapper.ToEntity(levels);

            TableOperation InsertLevel = TableOperation.Insert(NewLevel);

            await level.ExecuteAsync(InsertLevel);

            var ToModel = LevelMapper.ToModel(NewLevel);

            return ToModel;
        }
    }
}