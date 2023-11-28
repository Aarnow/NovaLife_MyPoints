using Life;
using Life.CheckpointSystem;
using Life.DB;
using Life.UI;
using Life.Network;
using Mirror;
using Newtonsoft.Json;
using System;
using System.IO;
using MyPoints.Components;
using MyPoints.Managers;
using MyPoints.Panels;

namespace MyPoints
{
    public class Main : Plugin
    {
        public static string directoryPath;
        public static string pointPath;
        public static string dataPath;

        public Main(IGameAPI api) : base(api)
        {
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            InitDirectory();

            new SChatCommand("/mypoints", "Permet d'ouvrir le panel du plugin MyPoints", "/mypoints", (player, arg) =>
                {
                    if (player.IsAdmin)
                    {
                        UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"MyPoints Menu");

                        panel.AddTabLine("Ajouter un point", (ui) => ui.selectedTab = 0);
                        panel.AddTabLine("Supprimer un point", (ui) => ui.selectedTab = 1);
                        panel.AddTabLine("Créer un jeu de données", (ui) => ui.selectedTab = 2);

                        panel.AddButton("Sélection", (ui) =>
                        {
                            if (ui.selectedTab == 0) UIPanelManager.NextPanel(player, ui, () => PointPanels.SetAction(player));
                            else if (ui.selectedTab == 1) UIPanelManager.NextPanel(player, ui, () => PointPanels.PointList(player));
                            else if (ui.selectedTab == 2) UIPanelManager.NextPanel(player, ui, () => DataPanels.Action(player));
                            else UIPanelManager.Notification(player, "Erreur", "Vous devez sélectionner un choix", NotificationManager.Type.Error);
                        });
                        panel.AddButton("Fermer", (ui) => UIPanelManager.Quit(ui, player));

                        player.ShowPanelUI(panel);
                    }
                    else UIPanelManager.Notification(player, "Avertissement", "Vous n'avez pas l'autorisation d'accéder à cette commande.", NotificationManager.Type.Error);
                    
                }).Register();
            
            Console.WriteLine($"Plugin \"MyPoints\" initialisé avec succès.");
        }

        public override void OnPlayerSpawnCharacter(Player player, NetworkConnection conn, Characters character)
        {
            base.OnPlayerSpawnCharacter(player, conn, character);

            try
            {
                string[] jsonFiles = Directory.GetFiles(pointPath, "*.json");
                foreach (string jsonFile in jsonFiles)
                {
                    string json = File.ReadAllText(jsonFile);
                    Point point = JsonConvert.DeserializeObject<Point>(json);
                    NCheckpoint newCheckpoint = point.Build(player);
                    player.CreateCheckpoint(newCheckpoint);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la lecture des fichiers JSON : " + ex.Message);
            }
        }

        public void InitDirectory()
        {
            directoryPath = pluginsPath + "/MyPoints";
            pointPath = directoryPath + "/Point";
            dataPath = directoryPath + "/Data";

            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            if (!Directory.Exists(pointPath)) Directory.CreateDirectory(pointPath);
            if (!Directory.Exists(dataPath)) Directory.CreateDirectory(dataPath);

            foreach (var pair in PointActionManager.Actions)
            {
                string dataActionPath = dataPath + "/" + pair.Key;
                if (!Directory.Exists(dataActionPath)) Directory.CreateDirectory(dataActionPath);
            }
        }
    }
}