using System;
using ImGuiNET;
using YADE;

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
                if (ImGui.MenuItem("Open Archive"))
                {
                    // Testing archive file
                    Resource.Archive archive = new Resource.Archive("Test archive", "Test path");
                    archive.fileList = new List<Resource.ArchiveItem>() {
                        new Resource.ArchiveResource("LUA_FILE", "/LUA_FILE", "Script", new Resource.GenericResource("LUA_FILE")),
                        new Resource.ArchiveResource("Graphic.png", "/Graphic.png", "Graphic", new Resource.GenericResource("Graphic.png")),
                        new Resource.ArchiveDirectory("Test", "/Test", "Directory", new List<Resource.ArchiveItem>()
                        {
                            new Resource.ArchiveResource("LUA_FILE", "/Test/LUA_FILE", "Script", new Resource.GenericResource("LUA_FILE")),
                            new Resource.ArchiveResource("Graphic.png", "/Test/Graphic.png", "Graphic", new Resource.GenericResource("Graphic.png")),
                        }),
                    };
                    Game1.openEditors.Add(new Archive.Editor(archive, Convert.ToString(Game1.openEditors.Count()+1)));
                }
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
            if (ImGui.BeginMenu("DEBUG"))
            {
				if (ImGui.MenuItem("Test CTexture Editor"))
				{
                    Game1.editor1.loadDefinitions(CTexture.FileSystem.parseFile("TEXTURES.jcz", "TEXTURES.jcz"));
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

