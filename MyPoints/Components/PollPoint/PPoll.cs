using Life;
using Life.Network;
using Mirror;
using MyPoints.Common;
using MyPoints.Panels.ActionPanels;
using MyPoints.Panels.DPanels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UIPanelManager;
using static PointActionManager;

namespace MyPoints.Components.PollPoint
{
    public class PPoll : PointAction
    {
        public override PointActionKeys ActionKeys { get; set; }
        public override string Slug { get; set; }
        public List<string> Choices = new List<string>();
        public bool IsSingleVote;
        public bool IsInProgress;
        public int BizOwner;
        public int CharacterIdClosedPoll = 0; //0 == null
        public Dictionary<int, List<int>> characterChoices = new Dictionary<int, List<int>>();

        public PPoll() : base(PointActionKeys.Poll, "default_poll")
        {
        }

        public override void OnPlayerTrigger(Player player)
        {
            UpdateProps();

            if (player.character.BizId == BizOwner || player.IsAdmin)
            {
                PollActionPanels.PollConfig(player, this);
            } else
            {
                if (characterChoices.ContainsKey(player.character.Id))
                {
                    if (IsInProgress) PollActionPanels.WaitPollResult(player, this);
                    else PollActionPanels.ShowPollResult(player, this);
                }
                else
                {
                    if(!IsInProgress) PollActionPanels.ShowPollClosed(player, this);
                    else PollActionPanels.ShowPoll(player, this, new List<int>());
                } 
            }
        }

        public override void CreateData(Player player)
        {
            if (Nova.biz.bizs.Count > 0)
            {
                PPoll pPoll = new PPoll();
                PollDataPanels.SetChoicesList(player, pPoll);
            }
            else PanelManager.Notification(player, "Erreur", "Vous devez avoir au minimum une société sur votre serveur.", NotificationManager.Type.Error);
        }

        public override void UpdateProps()
        {
            Choices = new List<string>();
            JsonConvert.PopulateObject(File.ReadAllText(Main.dataPath + "/" + ActionKeys + "/" + $"{ActionKeys}_{Slug}.json"), this);
        }

        public override object Clone()
        {
            return new PPoll();
        }

        public void UpdateData()
        {
            string filePath = Path.Combine(Main.dataPath + "/" + ActionKeys, $"{ActionKeys}_{Slug}.json");
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
