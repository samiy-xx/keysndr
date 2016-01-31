using System;
using Newtonsoft.Json;

namespace KeySndr.Base.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AppConfig
    {
        [JsonProperty("lastPath")]
        public string LastPath { get; set; }

        [JsonProperty("lastPort")]
        public int LastPort { get; set; }

        [JsonProperty("lastIp")]
        public string LastIp { get; set; }

        [JsonProperty("updateVersionCheckUrl")]
        public string UpdateVersionCheckUrl { get; set; }

        [JsonProperty("dataFolder")]
        public string DataFolder { get; set; }
        
        [JsonProperty("checkUpdateOnStart")]
        public bool CheckUpdateOnStart { get; set; }

        [JsonProperty("broadcastIdentifier")]
        public string BroadcastIdentifier { get; set; }

        [JsonProperty("enableKeyboardAndMouse")]
        public bool EnableKeyboardAndMouse { get; set; }

        [JsonProperty("firstTimeRunning")]
        public bool FirstTimeRunning { get; set; }

        [JsonProperty("defaultKeyDownMs")]
        public int DefaultKeyDownMs { get; set; }

        public IntPtr ProcessNumber { get; set; }
        public string ConfigFolder => $@"{DataFolder}\{KeySndrApp.ConfigurationsFolderName}";
        public string ScriptsFolder => $@"{DataFolder}\{KeySndrApp.ScriptsFolderName}";
        public string WebRoot => $@"{DataFolder}\{KeySndrApp.WebFolderName}";
        public string MediaRoot => $@"{DataFolder}\{KeySndrApp.WebFolderName}\{KeySndrApp.MediaFolderName}";
        public string ViewsRoot => $@"{DataFolder}\{KeySndrApp.WebFolderName}\{KeySndrApp.ViewsFolderName}";
        public AppConfig()
        {
            ProcessNumber = IntPtr.Zero;
            LastPath = "c:\\";
            LastPort = 45889;
            LastIp = "+";
            CheckUpdateOnStart = false;
            BroadcastIdentifier = "broadcast_id_" + Guid.NewGuid().ToString("n");
            UpdateVersionCheckUrl = "http://www.keysndr.win/Content/v/keysndr/version.json";
            DataFolder = string.Empty;
            EnableKeyboardAndMouse = false;
            FirstTimeRunning = true;
            DefaultKeyDownMs = 200;
        }

        public AppConfig CopyFrom(AppConfig c)
        {
            LastPath = c.LastPath;
            LastPort = c.LastPort;
            LastIp = c.LastIp;
            UpdateVersionCheckUrl = c.UpdateVersionCheckUrl;
            ProcessNumber = c.ProcessNumber;
            DataFolder = c.DataFolder;
            EnableKeyboardAndMouse = c.EnableKeyboardAndMouse;
            FirstTimeRunning = c.FirstTimeRunning;
            DefaultKeyDownMs = c.DefaultKeyDownMs;
            return this;
        }
    }
}
