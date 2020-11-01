using System.IO;
using Newtonsoft.Json;

namespace ShortURL.App
{
	public class ZavorConfig
    {
        public Config Config;
        public ZavorConfig()
        {
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("App/Config.json"));
        }
    }
}
