using MyPoints.Components;
using MyPoints.Components.CarDealerPoint;
using MyPoints.Components.ShopPoint;
using MyPoints.Components.TeleportationPoint;
using MyPoints.Components.TextPoint;
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
    }

    public static Dictionary<PointActionKeys, IPointAction> Actions = new Dictionary<PointActionKeys, IPointAction>
    {
        { PointActionKeys.Teleportation, new PTeleportation()},
        { PointActionKeys.Text, new PText()},
        { PointActionKeys.Shop, new PShop()},
        { PointActionKeys.CarDealer, new PCarDealer()},
    };

    public static IPointAction GetActionByKey(PointActionKeys key)
    {
        return Actions.ContainsKey(key) ? (IPointAction)Actions[key].Clone() : null;
    }
}