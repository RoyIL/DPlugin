using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorPlugin.Commands
{
    public class RemoveDoorCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "RemoveDoor";

        public string Help => "/RemoveDoor";

        public string Syntax => null;

        public List<string> Aliases => new List<string> { "removedoor", "Rdoor" };

        public List<string> Permissions => new List<string> { "D.Removedoor" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
           Core.DeleteData(RocketLib.Raycast(caller), command, caller);
        }
    }
}
