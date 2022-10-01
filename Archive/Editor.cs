using System;
using ImGuiNET;

namespace YADE.Archive
{
    // This editor works differently. the close button actually just closes the file entirely
    public class Editor
    {
        public Editor(Resource.Archive file, string id)
        {
            archive = file;
            strid = id;
        }

        public Resource.Archive archive;
        public bool is_open = true;

        private string strid;

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
                if (ImGui.BeginTable("filelist", 2,
                    ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable | ImGuiTableFlags.NoBordersInBody))
                {
                    ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.NoHide);
                    ImGui.TableSetupColumn("Type", ImGuiTableColumnFlags.WidthFixed, 20);

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

