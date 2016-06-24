using Lisa.Common.WebApi;
using System;

namespace Lisa.RobotArm.Api
{
    public class UserMapper
    {
        public static DynamicModel ToModel(dynamic entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Entity");
            }

            dynamic model = new DynamicModel();

            model.firstName = entity.firstName;
            model.lastName = entity.lastName;
            model.userName = entity.userName;
            model.email = entity.email;
            model.password = entity.password;

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