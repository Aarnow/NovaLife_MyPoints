using Life.Network;
using MyPoints.Interfaces;
using Newtonsoft.Json;
using System.IO;
using static PointActionManager;

namespace MyPoints.Common
{
    public abstract class PointAction : IPointAction
    {
        public abstract PointActionKeys ActionKeys { get; set; }
        public virtual string Slug { get; set; }
        protected PointAction(PointActionKeys actionKeys, string slug)
        {
            ActionKeys = actionKeys;
            Slug = slug;
        }

        public abstract void OnPlayerTrigger(Player player);
        public abstract void CreateData(Player player);
        public abstract void UpdateProps(string json);
        public abstract object Clone();
        public void Save()
        {
            int number = 0;
            string filePath;

            do
            {
                filePath = Path.Combine(Main.dataPath + "/" + ActionKeys, $"{ActionKeys}_{Slug}_{number}.json");
                number++;
            } while (File.Exists(filePath));

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }  
    }
}
