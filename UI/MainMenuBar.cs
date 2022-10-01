using System;
using ImGuiNET;

namespace YADE.UI
{
    public class MainMenuBar
    {
        static public void drawMenuBar()
        {
            ImGui.BeginMainMenuBar();

            if (ImGui.BeginMenu("File"))
            {
                ImGui.MenuItem("New");
                ImGui.Separator();
                ImGui.MenuItem("Open Archive");
                ImGui.MenuItem("Open Directory");
                ImGui.Separator();
                ImGui.MenuItem("Save");
                ImGui.MenuItem("Save As");
                ImGui.MenuItem("Save All");
                ImGui.Separator();
                ImGui.MenuItem("Close");
                ImGui.MenuItem("Close All");
                ImGui.Separator();
                drawRecentFiles();
                ImGui.Text("TODO: Implement recent files");
                ImGui.Separator();
                if (ImGui.MenuItem("Exit"))
                {
                    Environment.Exit(0);
                }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Help"))
            {
                if (ImGui.MenuItem("About"))
                {
                    UI.AboutWindow.is_open = true;
                }
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();
        }

        static public void drawRecentFiles()
        {

        }
    }
}

