using MyPoints.Components.CarDealerPoint;
using MyPoints.Components.ShopPoint;
using MyPoints.Components.TeleportationPoint;
using MyPoints.Components.TextPoint;
using MyPoints.Components.OutfitPoint;
using MyPoints.Interfaces;
using System.Collections.Generic;
using MyPoints.Components.FuelPoint;
using MyPoints.Components.TrashPoint;
using MyPoints.Components.PollPoint;

public abstract class PointActionManager
{
    public enum PointActionKeys
    {
        Teleportation,
        Text,
        Shop,
        CarDealer,
        Outfit,
        Fuel,
        Trash,
        Poll
    }

    public static Dictionary<PointActionKeys, IPointAction> Actions = new Dictionary<PointActionKeys, IPointAction>
    {
        { PointActionKeys.Teleportation, new PTeleportation()},
        { PointActionKeys.Text, new PText()},
        { PointActionKeys.Shop, new PShop()},
        { PointActionKeys.CarDealer, new PCarDealer()},
        { PointActionKeys.Outfit, new POutfit()},
        { PointActionKeys.Fuel, new PFuel()},
        { PointActionKeys.Trash, new PTrash()},
        { PointActionKeys.Poll, new PPoll()},
    };

    public static IPointAction GetActionByKey(PointActionKeys key)
    {
        return Actions.ContainsKey(key) ? (IPointAction)Actions[key].Clone() : null;
    }
}