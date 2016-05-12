using Lisa.Common.WebApi;

namespace Lisa.RobotArm.Api
{
    public class LevelValidator : Validator
    {
        protected override void ValidateModel()
        {
            Ignore("id");
            Required("Slug", NotEmpty, TypeOf(DataTypes.String));
            Required("Contents", NotEmpty);
        }
    }
}