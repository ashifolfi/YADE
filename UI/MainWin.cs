// fuck off I'm not adding xml comments to self describing classes and methods
#pragma warning disable 1591

using System;
using ImGuiNET;
using YADE;
using System.Numerics;

namespace YADE.UI
{
    /// <summary>
    /// Main Menu Bar component
    /// </summary>
    public class MainMenuBar
    {
        /// <summary>
        /// Draw the menu bar
        /// </summary>
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
                    Game1.environment.openEditors.Add(
                        new Archive.Editor(archive, Convert.ToString(Game1.environment.openEditors.Count()+1)));
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
                    Game1.editor1 = new CTexture.Editor("MRCE");
				}
				ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();
        }

        /// <summary>
        /// TODO: Add recent file loading/logging
        /// </summary>
        static public void drawRecentFiles()
        {

        }
    }
    public class MainToolBar {
        static public void draw() {
            if (ImGui.BeginMenuBar()) {
                ImGui.Button("New");
                ImGui.SameLine();
                ImGui.Button("Open Archive");
                ImGui.SameLine();
                ImGui.Button("Open Directory");
                ImGui.SameLine();
                ImGui.Button("Save");
                ImGui.SameLine();
                ImGui.Button("Save As");
                ImGui.SameLine();
                ImGui.Button("Save All");
                ImGui.SameLine();
                ImGui.Button("Close");
                ImGui.SameLine();
                ImGui.Button("Close All");
                
                ImGui.EndMenuBar();
            }
        }
    }

    /// <summary>
    /// The global dockspace class
    /// used to hold all open archives/editors/tools alongside the main toolbar
    /// </summary>
    public class YADEDockspace {
        public bool is_open = true;

        /// <summary>
        /// Creates a new global dockspace
        /// </summary>
        public YADEDockspace() {
        }

        private void setupDockSpaceNodes(uint dockspaceID)
        {
            // if (!ImGui::DockBuilderGetNode(dockspaceID)) {
            //     ImGui::DockBuilderRemoveNode(dockspaceID);
            //     ImGui::DockBuilderAddNode(dockspaceID, ImGuiDockNodeFlags_None);

            //     ImGuiID dock_main_id = dockspaceID;
            //     ImGuiID dock_up_id = ImGui::DockBuilderSplitNode(dock_main_id, ImGuiDir_Up, 0.05f, nullptr, &dock_main_id);
            //     ImGuiID dock_right_id = ImGui::DockBuilderSplitNode(dock_main_id, ImGuiDir_Right, 0.2f, nullptr, &dock_main_id);
            //     ImGuiID dock_left_id = ImGui::DockBuilderSplitNode(dock_main_id, ImGuiDir_Left, 0.2f, nullptr, &dock_main_id);
            //     ImGuiID dock_down_id = ImGui::DockBuilderSplitNode(dock_main_id, ImGuiDir_Down, 0.2f, nullptr, &dock_main_id);
            //     ImGuiID dock_down_right_id = ImGui::DockBuilderSplitNode(dock_down_id, ImGuiDir_Right, 0.6f, nullptr, &dock_down_id);

            //     ImGui::DockBuilderDockWindow("DockingTest", dock_left_id);

            //     ImGui::DockBuilderFinish(dock_main_id);
            // }
        }

        public void render() {
            ImGuiWindowFlags window_flags = ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoDocking;
            ImGuiViewportPtr viewport = ImGui.GetMainViewport();
            ImGui.SetNextWindowPos(viewport.WorkPos);
            ImGui.SetNextWindowSize(viewport.WorkSize);
            ImGui.SetNextWindowViewport(viewport.ID);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
            window_flags |= ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove;
            window_flags |= ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;

            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0.0f, 0.0f));
            ImGui.Begin("##DockSpace", ref is_open, window_flags);
            ImGui.PopStyleVar();
            ImGui.PopStyleVar(2);

            uint dockspace_id = ImGui.GetID("YADEDockSpace");
            //setupDockSpaceNodes(dockspace_id);
            ImGui.DockSpace(dockspace_id, new Vector2(0.0f, 0.0f), ImGuiDockNodeFlags.None);

            MainToolBar.draw();

            ImGui.End();
        }
    }
}

