using Life.Network;
using MyPoints.Common;
using Newtonsoft.Json;
using static PointActionManager;

namespace MyPoints.Components.TrashPoint
{
    public class PTrash : PointAction
    {
        public override PointActionKeys ActionKeys { get; }
        public override string Slug { get; set; }

        public PTrash()
        {
            ActionKeys = PointActionKeys.Trash;
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
    }
}
