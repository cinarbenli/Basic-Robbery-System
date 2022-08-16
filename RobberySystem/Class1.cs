using Rocket.API;
using Rocket.Core.Commands;
using Rocket.Core.Plugins;
using Rocket.Core.Serialization;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RobberySystem
{
    public class Class1 : RocketPlugin<Config>
    {
        protected override void Load()
        {
            base.Load();
        }
        [RocketCommand("soy", "Bir Oyuncuyu Soyarsınız", "/soy", AllowedCaller.Player)]
        [RocketCommandPermission("plovv.soy")]
        public void soy(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer pl = caller as UnturnedPlayer;
            if (Physics.Raycast(pl.Player.look.aim.position, pl.Player.look.aim.forward, out RaycastHit hit, 10, RayMasks.PLAYER) && hit.transform.TryGetComponent(out Player player))
            {
                UnturnedPlayer pls = UnturnedPlayer.FromPlayer(player);
                if (pls == null) return;
                if (pls.Player.animator.gesture != EPlayerGesture.SURRENDER_START) return;
                pls.Player.equipment.dequip();
                DropAll(pls);
                ChatManager.serverSendMessage($"<color=green>Soygun Başarılı |</color> Başarılı Bir Şekilde <color=orange>{pls.CharacterName}</color> Adlı Oyuncuyu Soydun.", Color.white, pl.SteamPlayer(), pl.SteamPlayer(), EChatMode.SAY, Configuration.Instance.Logo, true);
            }
        }
        public  void DropAll(UnturnedPlayer pls)
        {
            for (byte page = 0; page < 8; page++)
            {
                byte count = pls.Player.inventory.getItemCount(page);
                for (byte index = 0; index < count; index++)
                {
                    try
                    {
                        pls.Player.inventory.askDropItem(pls.CSteamID, page, pls.Player.inventory.getItem(page, index).x, pls.Player.inventory.getItem(page, index).y);
                    }
                    catch (Exception e)
                    {
                        ChatManager.serverSendMessage($"<color=red>HATA |</color> {e.Message}!", Color.white, null, pls.SteamPlayer(), (EChatMode)4, Configuration.Instance.Logo, true);
                    }
                }
            }
        }
    }
    public class Config : IRocketPluginConfiguration
    {

        public string Logo;
        public void LoadDefaults()
        {
            Logo = "HTTP";
        }
    }
}
