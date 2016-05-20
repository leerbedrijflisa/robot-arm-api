using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using Lisa.Common.TableStorage;
using Microsoft.Extensions.OptionsModel;

namespace Lisa.RobotArm.Api
{
    public class TableStorage
    {
        public TableStorage(IOptions<TableStorageSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<IEnumerable<object>> GetLevels()
        {
            CloudTable table = await Connect("Levels");

            TableQuery<DynamicEntity> query = new TableQuery<DynamicEntity>();
            TableQuerySegment<DynamicEntity> levelsInformation = await table.ExecuteQuerySegmentedAsync(query, null);

            IEnumerable<object> result = levelsInformation.Results;
            var Levels = levelsInformation.Select(L => LevelMapper.ToModel(L, false));

            return Levels;
        }

        public async Task<object> GetLevel(string slug, bool GetRepository)
        {
            CloudTable table = await Connect("Levels");

            TableQuery<DynamicEntity> query = new TableQuery<DynamicEntity>().Where(TableQuery.GenerateFilterCondition("Slug", QueryComparisons.Equal, slug));
            TableQuerySegment<DynamicEntity> levelInformation = await table.ExecuteQuerySegmentedAsync(query, null);

            object result = levelInformation.SingleOrDefault();
            if (result == null)
            {
                return null;
            }

            dynamic Level = LevelMapper.ToModel(result, GetRepository);
            return Level;
        }

        public async Task<object> PostLevel(dynamic level, object url)
        {
            CloudTable table = await Connect("Levels");
            var NewLevel = LevelMapper.ToEntity(level, url);

            TableQuery<DynamicEntity> query = new TableQuery<DynamicEntity>().Where(TableQuery.GenerateFilterCondition("Slug", QueryComparisons.Equal, NewLevel.Slug));
            TableQuerySegment<DynamicEntity> levelInformation = await table.ExecuteQuerySegmentedAsync(query, null);

            if (levelInformation.Count() > 0)
            {
                return null;
            }

            TableOperation InsertLevel = TableOperation.Insert(NewLevel);

            await table.ExecuteAsync(InsertLevel);

            var ToModel = LevelMapper.ToModel(NewLevel, false);

            return ToModel;
        }

        public async Task<object> PutLevel(dynamic levelInput, object url, string oldSlug)
        {
            CloudTable table = await Connect("Levels");
            var InputLevel = LevelMapper.ToEntity(levelInput, url);

            TableQuery<DynamicEntity> query = new TableQuery<DynamicEntity>().Where(TableQuery.GenerateFilterCondition("Slug", QueryComparisons.Equal, oldSlug));
            TableQuerySegment<DynamicEntity> levelData = await table.ExecuteQuerySegmentedAsync(query, null);

            if (levelData.Count() == 0)
            {
                return null;
            }
            dynamic levelInformation = levelData.FirstOrDefault();
            levelInformation.Contents = InputLevel.Contents;
            levelInformation.Slug = InputLevel.Slug;
            levelInformation.Url = url;

            TableOperation InsertLevel = TableOperation.Replace(levelInformation);
            await table.ExecuteAsync(InsertLevel);

            var ToModel = LevelMapper.ToModel(levelInformation, false);

            return ToModel;
        }
        //public async Task<object> GetUser(string username, string password)
        //{
        //    CloudTable table = await Connect("Users");

        //    TableQuery<DynamicEntity> query = new TableQuery<DynamicEntity>().Where(TableQuery.GenerateFilterCondition("userName", QueryComparisons.Equal, username));
        //    TableQuerySegment<DynamicEntity> UserInformation = await table.ExecuteQuerySegmentedAsync(query, null);

        //    object result = UserInformation.SingleOrDefault();
        //    if (result == null)
        //    {
        //        return null;
        //    }

        //    dynamic data = result;
        //    if (data.password != password)
        //    {
        //        return null;
        //    }

        //    var User = UserMapper.ToModel(result);

        //    return User;
        //}

        private async Task<CloudTable> Connect(string tableName)
        {
            var account = CloudStorageAccount.Parse(_settings.ConnectionString);
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();

            return table;
        }

        private TableStorageSettings _settings;
    }
}