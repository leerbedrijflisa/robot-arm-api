using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using Lisa.Common.TableStorage;

namespace Lisa.RobotArm.Api
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
            var Levels = levelsInformation.Select(L => LevelMapper.ToModel(L, false));

            return Levels;
        }
        public static async Task<object> GetLevel(string slug, bool k)
        {
            var account = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            var client = account.CreateCloudTableClient();
            var level = client.GetTableReference("Levels");

            await level.CreateIfNotExistsAsync();

            TableQuery<DynamicEntity> query = new TableQuery<DynamicEntity>().Where(TableQuery.GenerateFilterCondition("Slug", QueryComparisons.Equal, slug));
            TableQuerySegment<DynamicEntity> levelInformation = await level.ExecuteQuerySegmentedAsync(query, null);

            object result = levelInformation.SingleOrDefault();
            if (result == null)
            {
                return null;
            }

            var Level = LevelMapper.ToModel(result, k);
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

            var ToModel = LevelMapper.ToModel(NewLevel, false);

            return ToModel;
        }
    }
}