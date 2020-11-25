using Rocket.API;
using Rocket.API.Collections;
using Rocket.API.Extensions;
using Rocket.Core;
using Rocket.Core.Commands;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPChat.Core
{
    public class RPChatPlugin : RocketPlugin<RPChatConfig>
    {
        public static RPChatPlugin Instance;


        protected override void Load()
        {
            Instance = this;
            UnturnedPlayerEvents.OnPlayerChatted += onPlayerChatted;
        }
        protected override void Unload()
        {
            Instance = null;
            UnturnedPlayerEvents.OnPlayerChatted -= onPlayerChatted;
        }


        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "global_chat_is_disabled",    "Глобальный чат отключен, используйте /nrp;" }, // -
            { "local_chat_is_disabled",     "Локальный чат отключен!" },                    // -
            { "group_chat_is_disabled",     "Групповой чат отключен!" },                    // -

            { "command_nrp_message", "[NRP] {0}: {1}" },    // {0} - player name, {1} - message

            { "command_sms_player_notfound",        "Игрок с таким именем не найден" },         // -
            { "command_sms_to_myself",              "Нет смысла писать sms самому себе" },      // -
            { "command_sms_syntax",                 "Используйте: /sms (игрок) [текст]" },      // -
            { "command_sms_dont_have_item",         "Купите телефон чтобы отправлять SMS" },    // {0} - item id
            { "command_sms_not_enough_currency",    "На вашем балансе недостаточно средств" },  // {0} - corrent currency, {1} - sms cost, {2} - how much is missing
            { "command_sms_sended",                 "SMS отправлено!" },                        // {0} - to player name, {1} - message
            { "command_sms_message",                "[SMS от {0}]: {1}" },                      // {0} - from player name, {1} - message

            { "command_ad_syntax",              "Используйте: /ad [текст]" },                   // -
            { "command_ad_dont_have_item",      "Купите телефон чтобы отправить объявление" },  // {0} - item id
            { "command_ad_not_enough_currency", "На вашем балансе недостаточно средств" },      // {0} - corrent currency, {1} - ad cost, {2} - how much is missing
            { "command_ad_message",             "[Объявление] {0}: {1}" },                      // {0} - player name, {1} - announcement

            { "command_dn_syntax",              "Используйте: /dn [текст]" },                               // -
            { "command_dn_dont_have_item",      "Купите телефон чтобы отправить объявление в DarkNet" },    // {0} - item id
            { "command_dn_not_enough_currency", "На вашем балансе недостаточно средств" },                  // {0} - corrent currency, {1} - dn cost, {2} - how much is missing
            { "command_dn_message",             "[DarkNet Объявление] {0}: {1}" },                          // {0} - player name, {1} - announcement
            

            { "command_me_message", "[Действие] {0}: {1}" },    // {0} - player name, {1} - message
            { "command_s_message", "{0} Крикнул: {1}" },        // {0} - player name, {1} - message
            { "command_w_message", "{0} Прошептал: {1}" },      //  {0} - player name, {1} - message
            
            { "command_try_successfully", "удачно" },               // -
            { "command_try_unsuccessfully", "неудачно" },           // - 
            { "command_try_message", "[Дейтвие] {0}: {1} ({2})" },  // {0} - player name, {1} - message, {2} - result 
        };


        public string IgnoreDisabledChatsPermission => "rpchat.ignore.disable.chats"; // Permission for ignore disabled chats

        private void onPlayerChatted(UnturnedPlayer player, ref Color color, string message, EChatMode chatMode, ref bool cancel)
        {
            if (chatMode == EChatMode.GLOBAL && Configuration.Instance.DisableGlobalChat && !PlayerHasPermission(player, IgnoreDisabledChatsPermission))
            {
                UnturnedChat.Say(player, Instance.Translate("global_chat_is_disabled"), Color.red);
                cancel = true;
            }
            else if (chatMode == EChatMode.LOCAL && Configuration.Instance.DisableLocalChat && !PlayerHasPermission(player, IgnoreDisabledChatsPermission))
            {
                UnturnedChat.Say(player, Instance.Translate("local_chat_is_disabled"), Color.red);
                cancel = true;
            }
            else if (chatMode == EChatMode.GROUP && Configuration.Instance.DisableGroupChat && !PlayerHasPermission(player, IgnoreDisabledChatsPermission))
            {
                UnturnedChat.Say(player, Instance.Translate("group_chat_is_disabled"), Color.red);
                cancel = true;
            }
        }


        public List<SteamPlayer> GetSteamPlayersInRadius(Vector3 point, float radius, Predicate<SteamPlayer> predicate = null)
        {
            List<SteamPlayer> players = new List<SteamPlayer>();
            foreach (SteamPlayer player in Provider.clients)
            {
                if (Vector3.Distance(point, player.player.transform.position) <= (radius == 0 ? float.MaxValue : radius))
                {
                    if (predicate?.Invoke(player) == false)
                    {
                        continue;
                    }
                    else
                    {
                        players.Add(player);
                    }
                }
            }
            return players;
        }

        public bool PlayerHasPermission(UnturnedPlayer player, string permission)
        {
            return (player.IsAdmin && !Configuration.Instance.IgnoreAdmin) || player.GetPermissions().Any(p => p.Name == permission);
        }


        #region Commands:


        [RocketCommand("nrp", "", "/nrp [text]", AllowedCaller.Player)]
        [RocketCommandPermission("rpchat.nrp")]
        public void ExecuteNrpCommand(IRocketPlayer caller, string[] command) // /nrp (text: 0)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            string text = command.GetParameterString(0);
            
            if (text == null)
            {
                return;
            }

            ChatManager.serverSendMessage(
                text:       Instance.Translate("command_nrp_message", player.CharacterName, text),
                color:      UnturnedChat.GetColorFromName(Configuration.Instance.NRPChatColor, Color.green),
                fromPlayer: player.SteamPlayer(),
                toPlayer:   null,
                mode:       EChatMode.SAY,
                iconURL:    null);

        }



        [RocketCommand("sms", "", "/sms (player) [text]", AllowedCaller.Player)]
        [RocketCommandPermission("rpchat.sms")]
        public void ExecuteSmsCommand(IRocketPlayer caller, string[] command) // /sms (player: 0) [text: 1]
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            UnturnedPlayer toPlayer = UnturnedPlayer.FromName(command.GetStringParameter(0));
            string text = command.GetParameterString(1);

            if (command.Length <= 1)
            {
                UnturnedChat.Say(player, Instance.Translate("command_sms_syntax"), Color.yellow);
                return;
            }

            if (toPlayer == null)
            {
                UnturnedChat.Say(player, Instance.Translate("command_sms_player_notfound"), Color.red);
                return;
            }
            else if (toPlayer == player)
            {
                UnturnedChat.Say(player, Instance.Translate("command_sms_to_myself"), Color.red);
                return;
            }


            if (Configuration.Instance.SMSChatItemId != 0 && player.Player.inventory.search(Configuration.Instance.SMSChatItemId, true, true).Count <= 0)
            {
                UnturnedChat.Say(player, Instance.Translate("command_sms_dont_have_item", Configuration.Instance.SMSChatItemId), Color.red);
                return;
            }

            if (player.Experience < Configuration.Instance.SMSChatCost)
            {
                UnturnedChat.Say(player, Instance.Translate("command_sms_not_enough_currency", player.Experience, Configuration.Instance.SMSChatCost,
                    Configuration.Instance.SMSChatCost - player.Experience), Color.red);
                return;
            }

            player.Experience -= Configuration.Instance.SMSChatCost;


            UnturnedChat.Say(player, Instance.Translate("command_sms_sended", toPlayer.CharacterName, text), UnturnedChat.GetColorFromName(Configuration.Instance.SMSChatColor, Color.green));


            ChatManager.serverSendMessage(
                text:       Instance.Translate("command_sms_message", player.CharacterName, text),
                color:      UnturnedChat.GetColorFromName(Configuration.Instance.SMSChatColor, Color.green),
                fromPlayer: player.SteamPlayer(),
                toPlayer:   toPlayer.SteamPlayer(),
                mode:       EChatMode.SAY,
                iconURL:    string.IsNullOrEmpty(Configuration.Instance.SMSChatIcon) ? null : Configuration.Instance.SMSChatIcon);
        }



        public string ADChatLintenerPermission => "rpchat.ad.listener";
        [RocketCommand("ad", "", "/ad [text]", AllowedCaller.Player)]
        [RocketCommandPermission("rpchat.ad")]
        public void ExecuteAdCommand(IRocketPlayer caller, string[] command) // /ad (text: 0)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (Configuration.Instance.ADChatItemId != 0 && player.Player.inventory.search(Configuration.Instance.ADChatItemId, true, true).Count <= 0)
            {
                UnturnedChat.Say(player, Instance.Translate("command_ad_dont_have_item", Configuration.Instance.ADChatItemId), Color.red);
                return;
            }

            if (player.Experience < Configuration.Instance.ADChatCost)
            {
                UnturnedChat.Say(player, Instance.Translate("command_ad_not_enough_currency", player.Experience, Configuration.Instance.ADChatCost,
                    Configuration.Instance.ADChatCost - player.Experience), Color.red);
                return;
            }

            string text = command.GetParameterString(0);
            if (text == null)
            {
                UnturnedChat.Say(player, Instance.Translate("command_ad_syntax"), Color.yellow);
                return;
            }


            player.Experience -= Configuration.Instance.ADChatCost;


            GetSteamPlayersInRadius(player.Position, Configuration.Instance.ADChatRadius, (p) => PlayerHasPermission(UnturnedPlayer.FromSteamPlayer(p), ADChatLintenerPermission)).ForEach((steamPlayer) =>
            {
                ChatManager.serverSendMessage(
                    text: Instance.Translate("command_ad_message", player.CharacterName, text),
                    color: UnturnedChat.GetColorFromName(Configuration.Instance.ADChatColor, Color.green),
                    fromPlayer: player.SteamPlayer(),
                    toPlayer: steamPlayer,
                    mode: EChatMode.SAY,
                    iconURL: string.IsNullOrEmpty(Configuration.Instance.ADChatIcon) ? null : Configuration.Instance.ADChatIcon);
            });
        }



        public string DNChatListenerPermission => "rpchat.dn.listener";
        [RocketCommand("dn", "", "/dn [text]", AllowedCaller.Player)]
        [RocketCommandPermission("rpchat.dn")]
        public void ExecuteDnCommand(IRocketPlayer caller, string[] command) // /dn (text: 0)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (Configuration.Instance.DNChatItemId != 0 && player.Player.inventory.search(Configuration.Instance.DNChatItemId, true, true).Count <= 0)
            {
                UnturnedChat.Say(player, Instance.Translate("command_dn_dont_have_item", Configuration.Instance.DNChatItemId), Color.red);
                return;
            }

            if (player.Experience < Configuration.Instance.DNChatCost)
            {
                UnturnedChat.Say(player, Instance.Translate("command_dn_not_enough_currency", player.Experience, Configuration.Instance.DNChatCost,
                    Configuration.Instance.ADChatCost - player.Experience), Color.red);
                return;
            }

            string text = command.GetParameterString(0);
            if (text == null)
            {
                UnturnedChat.Say(player, Instance.Translate("command_dn_syntax"), Color.yellow);
                return;
            }


            player.Experience -= Configuration.Instance.DNChatCost;


            GetSteamPlayersInRadius(player.Position, Configuration.Instance.DNChatRadius, (p) => PlayerHasPermission(UnturnedPlayer.FromSteamPlayer(p), DNChatListenerPermission)).ForEach((steamPlayer) => 
            {
                ChatManager.serverSendMessage(
                    text: Instance.Translate("command_dn_message", player.CharacterName, text),
                    color: UnturnedChat.GetColorFromName(Configuration.Instance.DNChatColor, Color.green),
                    fromPlayer: player.SteamPlayer(),
                    toPlayer: steamPlayer,
                    mode: EChatMode.SAY,
                    iconURL: string.IsNullOrEmpty(Configuration.Instance.DNChatIcon) ? null : Configuration.Instance.DNChatIcon);
            });
        }



        [RocketCommand("me", "", "/me [text]", AllowedCaller.Player)]
        [RocketCommandPermission("rpchat.me")]
        public void ExecuteMeCommand(IRocketPlayer caller, string[] command) // /me (text: 0)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            string text = command.GetParameterString(0);

            if (text == null)
            {
                return;
            }

            GetSteamPlayersInRadius(player.Position, Configuration.Instance.MEChatRadius).ForEach((steamPlayer) => 
            {
                ChatManager.serverSendMessage(
                    text:       Instance.Translate("command_me_message", player.CharacterName, text),
                    color:      UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MEChatColor, Color.green),
                    fromPlayer: player.SteamPlayer(),
                    toPlayer:   steamPlayer,
                    mode:       EChatMode.SAY,
                    iconURL:    null);
            });
            
            
        }



        [RocketCommand("s", "", "/s [text]", AllowedCaller.Player)]
        [RocketCommandPermission("rpchat.s")]
        public void ExecuteSCommand(IRocketPlayer caller, string[] command) // /s (text: 0)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            string text = command.GetParameterString(0);

            if (text == null)
            {
                return;
            }

            GetSteamPlayersInRadius(player.Position, Configuration.Instance.SChatRadius).ForEach((steamPlayer) =>
            {
                ChatManager.serverSendMessage(
                    text:       Instance.Translate("command_s_message", player.CharacterName, text),
                    color:      UnturnedChat.GetColorFromName(Configuration.Instance.SChatColor, Color.green),
                    fromPlayer: player.SteamPlayer(),
                    toPlayer:   steamPlayer,
                    mode:       EChatMode.SAY,
                    iconURL:    null);
            });
        }



        [RocketCommand("w", "", "/w [text]", AllowedCaller.Player)]
        [RocketCommandPermission("rpchat.w")]
        public void ExecuteWCommand(IRocketPlayer caller, string[] command) // /w (text: 0)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            string text = command.GetParameterString(0);

            if (text == null)
            {
                return;
            }

            GetSteamPlayersInRadius(player.Position, Configuration.Instance.WChatRadius).ForEach((steamPlayer) => 
            {
                ChatManager.serverSendMessage(
                    text:       Instance.Translate("command_w_message", player.CharacterName, text),
                    color:      UnturnedChat.GetColorFromName(Configuration.Instance.WChatColor, Color.green),
                    fromPlayer: player.SteamPlayer(),
                    toPlayer:   steamPlayer,
                    mode:       EChatMode.SAY,
                    iconURL:    null);
            });
        }



        [RocketCommand("try", "", "/try [text]", AllowedCaller.Player)]
        [RocketCommandPermission("rpchat.try")]
        public void ExecuteTryCommand(IRocketPlayer caller, string[] command) // /try (text: 0)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            string text = command.GetParameterString(0);

            if (text == null)
            {
                return;
            }

            string res = UnityEngine.Random.Range(0f, 1f) > 0.5f ? Instance.Translate("command_try_successfully") : Instance.Translate("command_try_unsuccessfully");

            GetSteamPlayersInRadius(player.Position, Configuration.Instance.TRYChatRadius).ForEach((steamPlayer) =>
            {
                ChatManager.serverSendMessage(
                    text:       Instance.Translate("command_try_message", player.CharacterName, text, res),
                    color:      UnturnedChat.GetColorFromName(Configuration.Instance.TRYChatColor, Color.green),
                    fromPlayer: player.SteamPlayer(),
                    toPlayer:   steamPlayer,
                    mode:       EChatMode.SAY,
                    iconURL:    null);
            });
        }


        #endregion
    }
}
