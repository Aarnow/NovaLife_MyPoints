using Life.Network;
using Newtonsoft.Json;
using static PointActionManager;

namespace MyPoints.Interfaces
{
    public interface IPointAction
    {
        PointActionKeys ActionKeys { get; set; }
        string Slug { get; set; }
        void OnPlayerTrigger(Player player);
        void CreateData(Player player);
        void UpdateProps(string json);
        void Save();
    }
}
