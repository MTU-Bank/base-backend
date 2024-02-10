using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.Config
{
    public abstract class ConfigBase<T> where T : ConfigBase<T>, new()
    {
        private string filename;

        public static T Load(string filepath)
        {
            Directory.CreateDirectory("conf");
            if (File.Exists($"conf/{filepath}"))
            {
                var tmp = JsonConvert.DeserializeObject<T>(File.ReadAllText($"conf/{filepath}"));
                tmp.filename = filepath;
                return tmp;
            }
            else
            {
                T cfg = new T();
                cfg.filename = filepath;
                cfg.Save();
                return cfg;
            }
        }

        public void Save()
        {
            File.WriteAllText($"conf/{filename}", JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
