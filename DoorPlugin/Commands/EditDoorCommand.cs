using Rocket.API;
using Rocket.Unturned.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DoorPlugin.Commands
{
    public class EditDoorCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "EditDoor";

        public string Help => "/EditDoor [Perms]";

        public string Syntax => "";

        public List<string> Aliases => new List<string> {"editdoor", "Dedit" };

        public List<string> Permissions => new List<string> { "D.Edit" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            EditData(RocketLib.Raycast(caller), command,caller);
        }
        public void EditData(Transform transform, string[] Perms, IRocketPlayer caller)
        {
            var Exsists = Core.Insta.Configuration.Instance.conf.Exists(c => new Vector3 { x = c.transform.x, y = c.transform.y, z = c.transform.z } == transform.position);
            if (Exsists == true)
            {
                Core.DeleteData(transform, Perms, caller);
                Core.Insta.Configuration.Instance.SaveData(transform, Perms, caller);
            } else
            {
                UnturnedChat.Say (caller,Core.Insta.Translations.Instance.Translate("NoExists"));
            }
        }
    }
}
