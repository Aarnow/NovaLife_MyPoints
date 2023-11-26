using Life.Network;
using Life.UI;
using MyPoints.Interfaces;
using MyPoints.Managers;
using MyPoints.Panels.PanelsData;
using Newtonsoft.Json;
using System;
using System.IO;
using static PointActionManager;

namespace MyPoints.Components
{
    public class PText : IPointAction
    {
        public PointActionKeys ActionKeys { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }

        public PText()
        {
            ActionKeys = PointActionKeys.Text;
        }

        public void OnPlayerTrigger(Player player)
        {
            UIPanel panel = new UIPanel("MyPoints Panel", UIPanel.PanelType.Text).SetTitle($"{Slug}");

            panel.text = $"{Content}";

            panel.AddButton("Fermer", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public void CreateData(Player player)
        {
            PText pText = new PText();
            TextDataPanels.SetTextSlug(player, pText);
        }

        public void UpdateProps(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }

        public void Save()
        {
            int number = 0;
            string filePath;

            do
            {
                filePath = Path.Combine(Main.dataPath + "/" + PointActionKeys.Text, $"text_{Slug}_{number}.json");
                number++;
            } while (File.Exists(filePath));

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
