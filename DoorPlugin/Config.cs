using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DoorPlugin
{
    public class Config : IRocketPluginConfiguration
    {
        //Config
        public List<Data> conf = new List<Data>();

        public void SaveData(Transform transform, string[] permissions, IRocketPlayer caller)
        {
            var find = conf.Find(c => new Vector3 { x = c.transform.x, y = c.transform.y, z = c.transform.z } == transform.position);
            if (find == null)
            {
                conf.Add(new Data { Permissions = new List<string>(permissions), transform = transform.position });
                Core.Insta.Configuration.Save();
            }
            else
            {
                RocketLib.UMessage(Core.Insta.Translations.Instance.Translate("AExsists"), caller);
            }
        }


        public void LoadDefaults()
        {
            conf.Add(new Data { Permissions = null, transform = new Vector3 { x = 0, y = 0, z = 0 } });
        }


        public class Data
        {
            public Vector3 transform;
            public List<string> Permissions;
        }
    }
}
