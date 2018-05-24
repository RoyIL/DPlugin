using Rocket.API;
using Rocket.Core.Plugins;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;
using Rocket.Unturned.Player;
using Rocket.API.Collections;

namespace DoorPlugin
{
    public class Core : RocketPlugin<Config>
    {
        public static Core Insta;
        #region Loading/Unloading
        protected override void Load()
        {
            base.Load();
            Insta = this;
             RocketLib.ULogger("DoorPlugin Loaded ❤️ Joosep", System.ConsoleColor.Blue);
            Rocket.Unturned.Events.UnturnedPlayerEvents.OnPlayerUpdateGesture += UnturnedPlayerEvents_OnPlayerUpdateGesture;
        }
        protected override void Unload()
        {
            base.Unload();
            Insta = null;
            RocketLib.ULogger("DoorPlugin Unloaded ❤️ Joosep", System.ConsoleColor.Blue);
            Rocket.Unturned.Events.UnturnedPlayerEvents.OnPlayerUpdateGesture -= UnturnedPlayerEvents_OnPlayerUpdateGesture;
        }
        public override TranslationList DefaultTranslations => new TranslationList
        {
            {"NoPerms", "You Don't Have Enough Permissions" },
            {"AExsists", "The Door Already Exists" },
            {"NoExists", "The Door Doesn't Exist" }

        };
        #endregion

        private void UnturnedPlayerEvents_OnPlayerUpdateGesture(UnturnedPlayer player, Rocket.Unturned.Events.UnturnedPlayerEvents.PlayerGesture gesture)
        {
            if (gesture.Equals(Rocket.Unturned.Events.UnturnedPlayerEvents.PlayerGesture.PunchLeft) && RocketLib.Raycast(player) != null)
            {
                Main(player);
            }
        }
        public static bool CheckPerms(IRocketPlayer caller, List<string> perms)
        {
            if (perms.Count > 0)
            {
                foreach (var i in perms)
                {
                    foreach (var t in caller.GetPermissions())
                    {
                        if (t.Name == i)
                        {
                            return true;
                        }
                    }
                }
            } else { return true; }
            return false;
        }


        public static void Main(IRocketPlayer caller)
        {
            var RaycastPos = RocketLib.Raycast(caller).transform.position;
            var Exsists = Insta.Configuration.Instance.conf.Exists(c => new Vector3 { x = c.transform.x, y = c.transform.y, z = c.transform.z } == RaycastPos);
            if (Exsists != false)
            {
                var Item = Insta.Configuration.Instance.conf.Find(c => new Vector3 { x = c.transform.x, y = c.transform.y, z = c.transform.z } == RaycastPos);
                if (CheckPerms(caller, Item.Permissions))
                {
                    RocketLib.OpenDoor(RocketLib.Raycast(caller), ShouldOpen(RocketLib.RaycastForDoorOnly(caller)));
                }
                else
                {
                    RocketLib.UMessage(Insta.Translations.Instance.Translate("NoPerms"), caller);
                }
            }
        }


        public static void DeleteData(Transform transform, string[] permissions, IRocketPlayer rocketPlayer)
        {
            var i = Insta.Configuration.Instance.conf.FindIndex(c => new Vector3 { x = c.transform.x, y = c.transform.y, z = c.transform.z } == RocketLib.Raycast(rocketPlayer).transform.position);
            Insta.Configuration.Instance.conf.RemoveAt(i);
            Insta.Configuration.Save();
        }


        public static bool ShouldOpen(Transform transform)
        {
            if (transform.GetComponent<InteractableDoorHinge>() != null)
            {
                transform = transform.parent.parent;
                if (transform.GetComponent<InteractableDoor>().isOpen)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }
    }   
}
