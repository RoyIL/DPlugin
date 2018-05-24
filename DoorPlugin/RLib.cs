
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace DoorPlugin
{
    #region ExtensionMethods
    public static class ExtensionMethods
    {
        public static UnturnedPlayer RocketPlayerToU(this RocketPlayer rocket)
        {
            return UnturnedPlayer.FromName(rocket.DisplayName);
        }
        public static UnturnedPlayer SteamPlayerToU(this SteamPlayer player)
        {
            return UnturnedPlayer.FromSteamPlayer(player);
        }
        public static UnturnedPlayer SteamIDToU(this CSteamID steamID)
        {
            return UnturnedPlayer.FromCSteamID(steamID);
        }
        public static UnturnedPlayer LongToUntrunedPlayer(this ulong steamID)
        {
            return UnturnedPlayer.FromCSteamID((CSteamID)steamID);
        }

    }
    #endregion


    public class RocketLib
    {
        private static ushort plant;
        private static ushort index;
        private static BarricadeRegion r;

        #region IsPlayerOnline
        public static bool IsPlayerOnline(UnturnedPlayer unturnedPlayer)
        {
            foreach (var player in Provider.clients)
            {
                if (UnturnedPlayer.FromSteamPlayer(player).CSteamID == unturnedPlayer.CSteamID)
                {
                    return true;
                }
            }
            return false;

        }
        public static bool IsPlayerOnline(RocketPlayer rocketPlayer)
        {
            foreach (var player in Provider.clients)
            {
                UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromName(rocketPlayer.DisplayName);
                if (UnturnedPlayer.FromSteamPlayer(player).CSteamID == unturnedPlayer.CSteamID)
                {
                    return true;
                }
            }
            return false;

        }
        public static bool IsPlayerOnline(CSteamID cSteam)
        {
            foreach (var player in Provider.clients)
            {
                if (UnturnedPlayer.FromSteamPlayer(player).CSteamID == cSteam)
                {
                    return true;
                }
            }
            return false;

        }
        public static bool IsPlayerOnline(ulong cSteam)
        {
            foreach (var player in Provider.clients)
            {
                if (UnturnedPlayer.FromSteamPlayer(player).CSteamID == (CSteamID)cSteam)
                {
                    return true;
                }
            }
            return false;

        }
        public static bool IsPlayerOnline(String Name, out UnturnedPlayer unturnedPlayer)
        {
            foreach (var player in Provider.clients)
            {
                if (UnturnedPlayer.FromSteamPlayer(player).DisplayName.ToLower() == Name.ToLower())
                {
                    unturnedPlayer = UnturnedPlayer.FromName(Name);
                    return true;

                }
            }
            unturnedPlayer = null;
            return false;

        }
        #endregion
        #region PlayerSearch
        public static void FindPlayer(string Name, out List<UnturnedPlayer> FoundPlayers)
        {
            UnturnedPlayer player;
            IsPlayerOnline(Name, out player);

            List<UnturnedPlayer> _FoundPlayers = new List<UnturnedPlayer>();
            if (player == null)
            {
                foreach (var P in Provider.clients)
                {
                    UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(P);

                    if (unturnedPlayer.DisplayName.Substring(0, 4).ToLower() == Name.Substring(0, 4).ToLower())
                    {
                        _FoundPlayers.Clear();
                        _FoundPlayers.Add(unturnedPlayer);
                    }
                }

            }
            else
            {
                _FoundPlayers.Clear();
                _FoundPlayers.Add(player);

            }
            FoundPlayers = _FoundPlayers;
        }
        #endregion
        #region Logger
        public static void ULogger(string Input)
        {
            Rocket.Core.Logging.Logger.Log(Input);
        }
        public static void ULogger(string Input, ConsoleColor color)
        {
            Rocket.Core.Logging.Logger.Log(Input, color);
        }
        #endregion
        #region InGameMessage
        public static void UMessage(string Message)
        {
            UnturnedChat.Say(Message);
        }
        public static void UMessage(string Message, IRocketPlayer rocketPlayer)
        {
            UnturnedChat.Say(rocketPlayer, Message);
        }
        public static void UMessage(string Message, IRocketPlayer rocketPlayer, Color color)
        {
            UnturnedChat.Say(rocketPlayer, Message, color);
        }
        #endregion
        #region Door
        public static void OpenDoor(Transform transform, bool ShouldOpen)
        {
            byte x;
            byte y;

            if (BarricadeManager.tryGetInfo(transform, out x, out y, out plant, out index, out r))
            {

                BarricadeManager.instance.channel.send("askToggleDoor", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[] {
                        x,
                        y,
                        plant,
                        index,
                        ShouldOpen

                    });
                BarricadeManager.instance.channel.send("tellToggleDoor", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[] {
                        x,
                        y,
                        plant,
                        index,
                        ShouldOpen

                    });

            }
                
        }
        #endregion
        #region Raycast
        public static Transform Raycast(IRocketPlayer rocketPlayer)
        {
            RaycastHit hit;
            UnturnedPlayer player = (UnturnedPlayer)rocketPlayer;
            if (Physics.Raycast(player.Player.look.aim.position, player.Player.look.aim.forward, out hit, 5, RayMasks.BARRICADE_INTERACT))
            {
                byte x;
                byte y;

                ushort plant;
                ushort index;

                BarricadeRegion r;
                StructureRegion s;


                Transform transform = hit.transform;
                InteractableVehicle vehicle = transform.gameObject.GetComponent<InteractableVehicle>();

                if (transform.GetComponent<InteractableDoorHinge>() != null)
                {
                    transform = transform.parent.parent;
                    return transform;
                }
                return transform;
            }
            return null;
        }
        public static Transform RaycastForDoorOnly(IRocketPlayer rocketPlayer)
        {
            RaycastHit hit;
            UnturnedPlayer player = (UnturnedPlayer)rocketPlayer;
            if (Physics.Raycast(player.Player.look.aim.position, player.Player.look.aim.forward, out hit, 5, RayMasks.BARRICADE_INTERACT))
            {
                byte x;
                byte y;

                ushort plant;
                ushort index;

                BarricadeRegion r;
                StructureRegion s;


                Transform transform = hit.transform;
                InteractableVehicle vehicle = transform.gameObject.GetComponent<InteractableVehicle>();

                if (transform.GetComponent<InteractableDoorHinge>() != null)
                {
                    transform = transform;
                    return transform;
                }
                return transform;
            }
            return null;
        }
        public static Transform Raycast(UnturnedPlayer unturnedPlayer)
        {
            RaycastHit hit;
            UnturnedPlayer player = unturnedPlayer;
            if (Physics.Raycast(player.Player.look.aim.position, player.Player.look.aim.forward, out hit, 5, RayMasks.BARRICADE_INTERACT))
            {
                byte x;
                byte y;

                ushort plant;
                ushort index;

                BarricadeRegion r;
                StructureRegion s;


                Transform transform = hit.transform;
                InteractableVehicle vehicle = transform.gameObject.GetComponent<InteractableVehicle>();

                if (transform.GetComponent<InteractableDoorHinge>() != null)
                {
                    transform = transform.parent.parent;
                    return transform;
                }
                return transform;
            }
            return null;
        }

        #endregion


    }
    #region RESTful
    public class Restful
    {
        #region Get
        public static string SimpleHTTPGet(string Adress)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://" + Adress);

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }
        public static string SimpleHTTPSGet(string Adress)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://" + Adress);

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }
        #endregion
        #region Post
        public static string SimpleHTTPpost(string adress)
        {
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["thing1"] = "hello";
                values["thing2"] = "world";

                var response = client.UploadValues("http://" + adress, values);

                var responseString = Encoding.Default.GetString(response);
                return responseString;
            }
        }
        public static void HTTPPost(string url, string json)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }


            #endregion
        }
        public static void HTTPSPost(string url, string json)
        {
            ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
            #endregion

        }
    }
    #region Timer
    public delegate void TimeUp(object sender);
    public delegate void TimerChange(object sender, int Time);
    public class UTimer
    {
        private int _Delay;
        private DateTime LastCalled;
        public int Timeleft;
        private bool Enabled = false;
        public UTimer(int Delay)
        {
            _Delay = Delay;
            LastCalled = DateTime.Now;
        }
        public void Stop()
        {
            Enabled = false;
        }
        public void Start()
        {
            Enabled = true;
        }
        public event TimeUp timeUp;
        public event TimerChange timeChange;
        public void RaiseEvent()
        {
            timeUp(this); 
        }
        
        public void Update()
        {
            if (Enabled == true)
            {
                if((DateTime.Now  - LastCalled).TotalSeconds >= _Delay)
                {
                    RaiseEvent();
                    LastCalled = DateTime.Now;
                }
                else
                {
                    Timeleft = (int)(DateTime.Now - LastCalled).TotalSeconds;
                    timeChange(this,Timeleft);
                }

            }

        }

    }
    #endregion
}

