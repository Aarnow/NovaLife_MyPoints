using Life.Network;
using static PointActionManager;

namespace MyPoints.Interfaces
{
    public interface IPointAction
    {
        PointActionKeys ActionKeys { get; set; }
        void OnPlayerTrigger(Player player);
        void CreateData(Player player);
        void UpdateProps(string json);
    }
}
