using Lisa.Common.WebApi;

namespace Lisa.RobotArm.Api
{
    public class LevelValidator : Validator
    {
        protected override void ValidateModel()
        {
            Ignore("id");
            Required("slug", NotEmpty, TypeOf(DataTypes.String));
            Required("contents", NotEmpty, TypeOf(DataTypes.String));
        }
    }
}