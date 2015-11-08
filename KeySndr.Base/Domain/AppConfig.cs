using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KeySndr.Base.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AppConfig
    {
        [JsonProperty("lastProcessName")]
        public string LastProcessName { get; set; }

        [JsonProperty("lastPath")]
        public string LastPath { get; set; }

        [JsonProperty("lastPort")]
        public int LastPort { get; set; }

        [JsonProperty("lastIp")]
        public string LastIp { get; set; }

        [JsonProperty("useForegroundWindow")]
        public bool UseForegroundWindow { get; set; }

        [JsonProperty("updateVersionCheckUrl")]
        public string UpdateVersionCheckUrl { get; set; }

        [JsonProperty("configFolder")]
        public string ConfigFolder { get; set; }

        [JsonProperty("scriptsFolder")]
        public string ScriptsFolder { get; set; }

        [JsonProperty("webFolder")]
        public string WebRoot { get; set; }

        [JsonProperty("checkUpdateOnStart")]
        public bool CheckUpdateOnStart { get; set; }

        [JsonProperty("broadcastIdentifier")]
        public string BroadcastIdentifier { get; set; }

        [JsonProperty("enableKeyboardAndMouse")]
        public bool EnableKeyboardAndMouse { get; set; }

        public IntPtr ProcessNumber { get; set; }

        public AppConfig()
        {
            LastProcessName = string.Empty;
            ProcessNumber = IntPtr.Zero;
            LastPath = "c:\\";
            UseForegroundWindow = false;
            LastPort = 8888;
            LastIp = string.Empty;
            CheckUpdateOnStart = false;
            BroadcastIdentifier = "broadcast_id_" + Guid.NewGuid().ToString("n");
            UpdateVersionCheckUrl = "http://www.keysndr.win/Content/v/keysndr/version.json";
            ConfigFolder = string.Empty;
            ScriptsFolder = string.Empty;
            WebRoot = string.Empty;
            EnableKeyboardAndMouse = false;
        }

        public AppConfig CopyFrom(AppConfig c)
        {
            LastProcessName = c.LastProcessName;
            LastPath = c.LastPath;
            LastPort = c.LastPort;
            LastIp = c.LastIp;
            UseForegroundWindow = c.UseForegroundWindow;
            UpdateVersionCheckUrl = c.UpdateVersionCheckUrl;
            ConfigFolder = c.ConfigFolder;
            ProcessNumber = c.ProcessNumber;
            ScriptsFolder = c.ScriptsFolder;
            WebRoot = c.WebRoot;
            EnableKeyboardAndMouse = c.EnableKeyboardAndMouse;
            return this;
        }
    }
}
