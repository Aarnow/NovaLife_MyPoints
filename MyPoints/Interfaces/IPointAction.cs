using Life.Network;
using static PointActionManager;

namespace MyPoints.Interfaces
{
    public interface IPointAction
    {
        PointActionKeys ActionKeys { get; set; }
        void OnPlayerTrigger();
        void CreateData(Player player);
    }
}
