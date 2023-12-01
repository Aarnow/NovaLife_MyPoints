using Life.Network;
using MyPoints.Common;
using Newtonsoft.Json;
using static PointActionManager;

namespace MyPoints.Components.JobPoint
{
    public class PJob : PointAction
    {
        public override PointActionKeys ActionKeys { get; set; }
        public override string Slug { get; set; }

        public PJob():base(PointActionKeys.Job, "default_job")
        {
        }

        public override void OnPlayerTrigger(Player player)
        {
        }

        public override void CreateData(Player player)
        {
        }

        public override void UpdateProps(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }

        public override object Clone()
        {
            return new PJob();
        }
    }
}
