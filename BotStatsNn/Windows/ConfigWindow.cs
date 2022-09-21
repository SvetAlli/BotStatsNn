using System;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace BotStatsNn.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;
    private Plugin plugin;

    public ConfigWindow(Plugin plugin) : base(
        "BotStats")
    {
        this.Size = new Vector2(500, 100);
        this.SizeCondition = ImGuiCond.Once;
        this.plugin = plugin;

        this.Configuration = plugin.Configuration;
    }

    public void Dispose() {
        this.Configuration.Save();
    }

    public override void Draw()
    {
        ImGui.Text("From date : " + Configuration.date);
        ImGui.Spacing();
        ImGui.Spacing();
        ImGui.Spacing();

        ImGui.TextColored(new Vector4(150,0,0,1), "Number of possible bot : " + this.Configuration.botList.Count);
        ImGui.TextColored(new Vector4(150,0,0,1), "Number of messages from bots or people mimicking : " + this.Configuration.messageCount);
        ImGui.TextColored(new Vector4(150, 0, 0, 1), "Number of messages containing the word \"bot\" : " + this.Configuration.messageWithBot);

        ImGui.Spacing();
        ImGui.Spacing();


        if (Configuration.kickers.Count == 0)
        {
            ImGui.TextColored(new Vector4(144, 238, 144, 1), "No kickers yet, such a peaceful NN");
        }
        else
        {
            var list = Configuration.kickers.ToList();
            list.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            ImGui.TextColored(new Vector4(144, 255, 144, 1), "Top 10 Trigger happy (kicks) : ");

            ImGui.Columns(2);
            ImGui.SetColumnWidth(0, 150);
            foreach (var kicker in list.Take(10))
            {
                ImGui.TextUnformatted(kicker.Key);
                ImGui.NextColumn();
                ImGui.TextUnformatted(kicker.Value + "");
                ImGui.NextColumn();
            }
            ImGui.Columns();
        }

        ImGui.Spacing();
        ImGui.Spacing();
        ImGui.Spacing();
        ImGui.Spacing();


        if (ImGui.Button("Reset Stats"))
        {
            Configuration.resetStats();
        }
    }
}
