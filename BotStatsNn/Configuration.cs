using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace SamplePlugin
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public HashSet<string> botList = new HashSet<string>();

        public int messageCount = 0;

        public int messageWithBot = 0;

        public HashSet<string> uncatched = new HashSet<string>();

        public DateTime date = DateTime.Now;

        public Dictionary<string, int> kickers = new Dictionary<string, int>();

        public string botname { get; set; } = "";

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.PluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.PluginInterface!.SavePluginConfig(this);
        }

        public void resetStats()
        {
            this.botList.Clear();
            this.date = DateTime.Now;
            this.kickers.Clear();
            this.messageCount = 0;
            this.messageWithBot = 0;
            this.uncatched.Clear();
        }
    }
}
