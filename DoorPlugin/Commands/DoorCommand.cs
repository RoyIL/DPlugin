using Rocket.API;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DoorPlugin.Commands
{
    public class Door : IRocketCommand
    {
        public static Rocket.Unturned.Permissions.UnturnedPermissions UPerms;
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "Door";

        public string Help => "/Door";

        public string Syntax => null;

        public List<string> Aliases => new List<string> { "/D", "door", "/d" };

        public List<string> Permissions => new List<string> { "D.Door" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
           Core.Main(caller);
        }

        
        //Checks If The Player Has One Of The Required Perms.
       
    }
}
