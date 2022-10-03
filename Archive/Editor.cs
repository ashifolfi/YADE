using System;
using ImGuiNET;

namespace YADE.Archive
{
    /// <summary>
	/// Main Archive Editor Class
	/// </summary>
    public class Editor
    {
        /// <summary>
        /// Constructor for Main Archive Editor Class
        /// </summary>
        /// <param name="file">Archive data to load</param>
        /// <param name="id">id for ImGui tabs</param>
        public Editor(Resource.Archive file, string id)
        {
            archive = file;
            strid = id;
        }

        /// <summary>
        /// Archive data pointer
        /// </summary>
        public Resource.Archive archive;
        /// <summary>
        /// Window open statuc
        /// </summary>
        public bool is_open = true;

        private string strid;

        /// <summary>
		/// Draw main Archive Editor Window
		/// </summary>
        public void drawWindow()
        {
            ImGui.SetWindowSize(new System.Numerics.Vector2(500, 500), ImGuiCond.Appearing);
            ImGui.SetNextWindowDockID(Game1.dockid, ImGuiCond.Always);
            if (ImGui.Begin(archive.resName+"#"+strid, ref is_open))
            {
                drawMenuBar();

                // split view system
                ImGui.Columns(2);
                drawFileList();
                ImGui.NextColumn();
                drawEditorPanel();

                ImGui.End();
            }
        }

        private void drawMenuBar()
        {
            ImGui.Button("New");
            ImGui.SameLine();
            ImGui.Button("Rename");
            ImGui.SameLine();
            ImGui.Button("Delete");
            ImGui.SameLine();
            ImGui.Button("Import");
            ImGui.SameLine();
            ImGui.Button("Export");
        }

        private void drawFileList()
        {
            if (ImGui.BeginChild("aclist", new System.Numerics.Vector2(500, ImGui.GetWindowHeight() - 4)))
            {
                if (ImGui.BeginTable("filelist", 3,
                    ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable | ImGuiTableFlags.NoBordersInBody))
                {
					ImGui.TableSetupColumn("ID", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHide
                        | ImGuiTableColumnFlags.NoResize, 15);
					ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.NoHide);
                    ImGui.TableSetupColumn("Type", ImGuiTableColumnFlags.NoHide);

                    ImGui.TableHeadersRow();

                    foreach (Resource.ArchiveItem archiveItem in archive.fileList)
                    {
                        archiveItem.drawNode();
                    }

                    ImGui.EndTable();
                }
                ImGui.EndChild();
            }
        }

        private void drawEditorPanel()
        {
            // file has no editor we can use for it
            // display some basic info
            if (ImGui.BeginChild("editorpane", new System.Numerics.Vector2(500, ImGui.GetWindowHeight() - 4)))
            {
                ImGui.Text("TODO: Implement editor panes");
                ImGui.EndChild();
            }
        }
    }
}

