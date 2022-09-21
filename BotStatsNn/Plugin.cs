using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using BotStatsNn.Windows;
using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using System.Collections.Generic;
using Dalamud.Utility;

namespace BotStatsNn
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "BotStatsNn";
        private const string CommandName = "/botStats";

        private DalamudPluginInterface PluginInterface { get; init; }
        public CommandManager CommandManager { get; init; }
        public ChatGui ChatGui { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("SamplePlugin");

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager,
            [RequiredVersion("1.0")] ChatGui chatGui)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;
            this.ChatGui = chatGui;
            

            

            ChatGui.ChatMessage += Chat_OnChatMessage;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            WindowSystem.AddWindow(new ConfigWindow(this));

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
        }

        public void Dispose()
        {
            this.Configuration.Save();
            this.WindowSystem.RemoveAllWindows();
            this.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
                // in response to the slash command, just display our main ui
                WindowSystem.GetWindow("BotStats").IsOpen = true;

        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }


        private void Chat_OnChatMessage (XivChatType type, uint senderId, ref SeString sender, ref SeString cmessage, ref bool isHandled)
        {
            if(cmessage.TextValue.Contains("has been kicked"))
            {
                Configuration.uncatched.Add(type.ToString());
            }

            if (type.ToString().Equals("4427") || type.ToString().Equals("8523") || type.ToString().Equals("8523") || type.ToString().Equals("2635") || type.ToString().Equals("8779") || type.ToString().Equals("4683"))
            {
                string[] kickerArray = cmessage.TextValue.Split(' ');
                string kicker = kickerArray[11] + " " + kickerArray[12].Split('.')[0];
                if(Configuration.kickers.ContainsKey(kicker))
                {
                    Configuration.kickers[kicker]+=1;
                } else
                {
                    Configuration.kickers.Add(kicker, 1);
                }
            }
            if (cmessage.TextValue.StartsWith("0") && cmessage.TextValue.EndsWith("0") && type.Equals(XivChatType.NoviceNetwork))
            {
                Configuration.botList.Add(sender.TextValue);
                Configuration.messageCount++;
                isHandled = true;
            }
            else
            {
                if (cmessage.TextValue.Contains("bot") && type.Equals(XivChatType.NoviceNetwork))
                {
                    Configuration.messageWithBot++;
                }
                if(Configuration.botList.Contains(sender.TextValue))
                {
                    Configuration.botList.Remove(sender.TextValue);
                }
            }
            
        }

    }
}
