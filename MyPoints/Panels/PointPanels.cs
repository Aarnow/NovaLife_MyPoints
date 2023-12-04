using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Life;
using Life.DB;
using Life.Network;
using Life.UI;
using MyPoints.Common;
using MyPoints.DTO;
using MyPoints.Interfaces;
using Newtonsoft.Json;
using UIPanelManager;
using UnityEngine;
using static PointActionManager;

namespace MyPoints.Panels
{
    abstract class PointPanels
    {
        public static void SetAction(Player player)
        {
            PointDto pointDto = new PointDto();
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"Sélectionner une action");

            foreach ((KeyValuePair<PointActionKeys, IPointAction> pair, int index) in PointActionManager.Actions.Select((pair, key) => (pair, key)))
                panel.AddTabLine($"{pair.Key}", (ui) => ui.selectedTab = index);

            panel.AddButton("Sélectionner", (ui) =>
            {
                PanelManager.NextPanel(player, ui, () =>
                {
                    if (Enum.TryParse(ui.lines[ui.selectedTab].name, out PointActionKeys actionKey)) pointDto.ActionKey = actionKey;                  
                    else PanelManager.Notification(player, "Erreur", "Nous n'avons pas pu valider cette action", NotificationManager.Type.Error);
                    SetData(player, pointDto);
                });
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, ()=> MainPanel.OpenMyPointsMenu(player)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetData(Player player, PointDto pointDto)
        {
            string path = Main.dataPath + "/" + Actions[pointDto.ActionKey].ActionKeys;
            string[] jsonFiles = Directory.GetFiles(path, "*.json");

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"Sélectionner vos données");

            foreach (string jsonFile in jsonFiles)
            {
                int index = Array.IndexOf(jsonFiles, jsonFile);
                panel.AddTabLine($"{Path.GetFileNameWithoutExtension(jsonFile)}", (ui) => ui.selectedTab = index);               
            }

            panel.AddButton("Sélectionner", (ui) =>
            {
                PanelManager.NextPanel(player, ui, () =>
                {
                    pointDto.DataFilePath = path + "/" + ui.lines[ui.selectedTab].name + ".json";
                    SetAllowedBizs(player, pointDto);
                });
            });
            panel.AddButton("Retour", (ui) =>
            {
                PanelManager.NextPanel(player, ui, () =>
                {
                    SetAction(player);
                });
            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetAllowedBizs(Player player, PointDto pointDto)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"Ajouter les sociétés autorisées");

            foreach ((Bizs biz, int index) in Nova.biz.bizs.Select((value, index) => (value, index)))
                panel.AddTabLine(pointDto.AllowedBizs.Contains(biz.Id) ? $"<color={PanelManager.Colors[NotificationManager.Type.Success]}>{biz.BizName}</color>" : $"{biz.BizName}", (ui) => ui.selectedTab = index);
            
            panel.AddButton("Ajouter/Retirer", (ui) =>
            {
                PanelManager.NextPanel(player, ui, () =>
                {
                    if (pointDto.AllowedBizs.Contains(Nova.biz.bizs[ui.selectedTab].Id)) pointDto.AllowedBizs.Remove(Nova.biz.bizs[ui.selectedTab].Id);               
                    else pointDto.AllowedBizs.Add(Nova.biz.bizs[ui.selectedTab].Id);
                    SetAllowedBizs(player, pointDto);
                });
            });
            panel.AddButton("Confirmer", (ui) => PanelManager.NextPanel(player, ui, () => SetIsOpen(player, pointDto)));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetData(player, pointDto)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetIsOpen(Player player, PointDto pointDto)
        {

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Text).SetTitle($"Renseigner l'état du point");

            panel.text = "Voulez-vous que votre point soit ouvert ou fermer à sa création ?";

            panel.AddButton("Ouvert", (ui) =>
            {
                PanelManager.NextPanel(player, ui, () =>
                {
                    pointDto.IsOpen = true;
                    SetName(player, pointDto);
                });
            });
            panel.AddButton("Fermer", (ui) =>
            {
                PanelManager.NextPanel(player, ui, () =>
                {
                    pointDto.IsOpen = false;
                    SetName(player, pointDto);
                });
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetAllowedBizs(player, pointDto)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetName(Player player, PointDto pointDto)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Enregistrer votre point");

            panel.inputPlaceholder = "Nom du point";

            panel.AddButton("Enregistrer", (ui) =>
            {
                PanelManager.NextPanel(player, ui, () =>
                {
                    if (ui.inputText.Length != 0)
                    {
                        pointDto.Name = ui.inputText;
                        Vector3 position = player.setup.transform.position;
                        Point newPoint = new Point(player.netId, pointDto.ActionKey+"_"+pointDto.Name, pointDto.Name, pointDto.DataFilePath, pointDto.ActionKey, pointDto.IsOpen, pointDto.AllowedBizs, new float[] { position.x, position.y, position.z });
                        newPoint.Create(player);
                        PanelManager.Notification(player, "Succès", "Votre point est prêt.", NotificationManager.Type.Success);
                    } else PanelManager.Notification(player, "Erreur", "Veuillez nommer votre point.", NotificationManager.Type.Error);
                });
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetIsOpen(player, pointDto)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void PointList(Player player)
        {
            string[] jsonFiles = Directory.GetFiles(Main.pointPath, "*.json");
           

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle("Supprimer un point");

            foreach ((string jsonFile, int index) in jsonFiles.Select((jsonFile, index) => (jsonFile, index)))
            {
                panel.AddTabLine($"{Path.GetFileNameWithoutExtension(jsonFile)}", (ui) => ui.selectedTab = index);
            }


            panel.AddButton("Téléportation", (ui) =>
            {
                string json = File.ReadAllText(jsonFiles[ui.selectedTab]);
                Point point = JsonConvert.DeserializeObject<Point>(json);
                player.setup.TargetSetPosition(new Vector3(point.Position.x, point.Position.y, point.Position.z));
            });
            panel.AddButton("Supprimer", (ui) =>
            {
                string json = File.ReadAllText(jsonFiles[ui.selectedTab]);
                Point point = JsonConvert.DeserializeObject<Point>(json);
                if(point != null) point.Delete(player);
                PanelManager.NextPanel(player, ui, () => PointList(player));
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => MainPanel.OpenMyPointsMenu(player)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
