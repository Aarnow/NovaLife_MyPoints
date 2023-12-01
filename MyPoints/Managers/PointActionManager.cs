using MyPoints.Components.CarDealerPoint;
using MyPoints.Components.ShopPoint;
using MyPoints.Components.TeleportationPoint;
using MyPoints.Components.TextPoint;
using MyPoints.Components.TrashPoint;
using MyPoints.Interfaces;
using System.Collections.Generic;

public abstract class PointActionManager
{
    public enum PointActionKeys
    {
        Teleportation,
        Text,
        Shop,
        CarDealer,
        Trash,
    }

    public static Dictionary<PointActionKeys, IPointAction> Actions = new Dictionary<PointActionKeys, IPointAction>
    {
        { PointActionKeys.Teleportation, new PTeleportation()},
        { PointActionKeys.Text, new PText()},
        { PointActionKeys.Shop, new PShop()},
        { PointActionKeys.CarDealer, new PCarDealer()},
        { PointActionKeys.Trash, new PTrash()},
    };

    public static IPointAction GetActionByKey(PointActionKeys key)
    {
        return Actions.ContainsKey(key) ? (IPointAction)Actions[key].Clone() : null;
    }
}