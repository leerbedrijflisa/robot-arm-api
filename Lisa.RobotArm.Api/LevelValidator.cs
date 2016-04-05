using Lisa.Common.WebApi;

namespace Lisa.RobotArm.Api
{
    public class LevelValidator : Validator
    {
        protected override void ValidateModel()
        {
            Ignore("id");
            Required("Slug", NotEmpty);
            Required("Contents", NotEmpty);
        }
    }
}