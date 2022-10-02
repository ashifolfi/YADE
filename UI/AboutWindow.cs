﻿using System;
using ImGuiNET;

namespace YADE.UI
{
    public class AboutWindow
    {
        public static bool is_open = false;

        public static void drawWindow()
        {
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(300, 150), ImGuiCond.Always);
            if (ImGui.Begin("About YADE", ref is_open, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDocking))
            {
                ImGui.Text("Yet Another Doom Editor");
                ImGui.Text("Version 1.0");
                ImGui.Separator();
                ImGui.Text("Created by Ashifolfi in C# and ImGui");
                ImGui.Separator();
                if (ImGui.Button("Close"))
                {
                    is_open = false;
                }

                ImGui.End();
            }
        }
    }
}

