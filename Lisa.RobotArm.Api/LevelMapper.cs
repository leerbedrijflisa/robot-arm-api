using Lisa.Common.TableStorage;
using Lisa.Common.WebApi;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Lisa.RobotArm.Api
{
    public class LevelMapper
    {
        public static ITableEntity ToEntity(dynamic model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Model");
            }

            dynamic entity = new DynamicEntity();
            entity.Slug = model.Slug;
            entity.Contents = model.Contents;
            entity.Url = "localhost:31415/repository/" + model.Slug;

            dynamic metadata = model.GetMetadata();
            if (metadata == null)
            {
                entity.Id = Guid.NewGuid();
                entity.RowKey = entity.Id.ToString();
                entity.PartitionKey = entity.Slug.ToString();
            }
            //else
            //{
            //    entity.Id = model.Id;
            //    entity.Observations = JsonConvert.SerializeObject(model.Observations);
            //    entity.PartitionKey = metadata.PartitionKey;
            //    entity.RowKey = metadata.RowKey;
            //}

            return entity;
        }

        public static DynamicModel ToModel(dynamic entity, bool k)
        {
            if (entity == null)
            {
                throw new ArgumentException("Entity");
            }

            dynamic model = new DynamicModel();
            if (k)
            {
                model.Url = entity.Contents;
            } else {
                model.Slug = entity.Slug;
                model.Contents = entity.Contents;
                model.Url = entity.Url;
            }
            var metadata = new
            {
                PartitionKey = entity.PartitionKey,
                RowKey = entity.RowKey,
            };
            model.SetMetadata(metadata);

            return model;
        }
    }
}