using Lisa.Common.WebApi;

namespace Lisa.RobotArm.Api
{
    public class LevelValidator : Validator
    {
        protected override void ValidateModel()
        {
            Ignore("id");
            Required("Slug", NotEmpty, Length(5));
            Required("Contents", NotEmpty, Length(3));
        }
    }
}