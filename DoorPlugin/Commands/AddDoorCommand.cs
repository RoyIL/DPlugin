using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorPlugin.Commands
{
    public class AddDoorCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "AddDoor";

        public string Help => "/AddDoor";

        public string Syntax => null;

        public List<string> Aliases => new List<string> { "adddoor", "AD" };

        public List<string> Permissions => new List<string> { "D.Adddoor" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            Core.Insta.Configuration.Instance.SaveData(RocketLib.Raycast(caller), command, caller);
        }
    }
}
