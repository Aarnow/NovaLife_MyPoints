using MyPoints.Components;
using MyPoints.Interfaces;
using System.Collections.Generic;

public abstract class PointActionManager
{
    private static Dictionary<string, IPointAction> _actions = new Dictionary<string, IPointAction>
    {
        { "Teleportation", new PTeleportation()},
        { "Shop", new PShop()},
        { "Text", new PText()}
    };

    public static IPointAction GetActionByKey(string key)
    {
        return _actions.ContainsKey(key) ? _actions[key] : null;
    }
}