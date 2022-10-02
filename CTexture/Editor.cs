using System;
using ImGuiNET;
using Vector2 = System.Numerics.Vector2;
using YADE.Resource;
using Microsoft.VisualBasic.FileIO;

namespace YADE.CTexture
{
    public class Editor
    {
        private String windowTitle = "Composite Texture Editor";

		private List<EditorTab> defTabs = new List<EditorTab>();

        public void drawWindow(bool open)
        {
            ImGui.SetNextWindowSize(new Vector2(300, 300), ImGuiCond.FirstUseEver);
			ImGui.Begin(windowTitle, ref open);

            // Call each tab for the composite texture definitions
			if (ImGui.BeginTabBar("comptexedit"))
            {
				foreach (EditorTab tab in defTabs)
				{
					tab.draw();
				}
				ImGui.EndTabBar();
			}

			ImGui.End();
        }

        public void loadDefinitions(CTDefResource resfile)
        {
			defTabs.Add(new EditorTab(resfile));
        }
    }

    public class EditorTab
    {
        public EditorTab(CTDefResource resfile)
        {
            currentFile = resfile;
        }

	    public CTDefResource currentFile;
		public bool is_open = false;

		private List<patchNode> curPatchList = new List<patchNode>();

		public void draw()
        {
			// and here we assemble the ui
			if (ImGui.BeginTabItem(currentFile.resName))
            {
				// tables are used for the layout
				if (ImGui.BeginTable("compmain", 3))
                {
					ImGui.TableSetupColumn("", ImGuiTableColumnFlags.None);
					ImGui.TableSetupColumn("", ImGuiTableColumnFlags.None);
					ImGui.TableSetupColumn("", ImGuiTableColumnFlags.None);

					ImGui.TableNextColumn();
					drawTextureList();
					ImGui.TableNextColumn();
					ImGui.TableNextColumn();
					drawPatchList();

					ImGui.EndTable();
				}
				ImGui.EndTabItem();
            }
        }

		public void loadTexture(List<CTexPatch> plist)
		{
			curPatchList = new List<patchNode>();
			foreach (CTexPatch patch in plist)
			{
				curPatchList.Add(new patchNode(patch));
			}
		}

		// TODO: Implement texture view
		private void drawTextureView()
		{
		}

		private void drawPatchList()
		{
			if (ImGui.BeginChild("patview", new System.Numerics.Vector2(200, ImGui.GetWindowHeight() - 4)))
			{
				if (ImGui.BeginTable("patlist", 1,
					ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable | ImGuiTableFlags.NoBordersInBody))
				{
					ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.NoHide);

					ImGui.TableHeadersRow();

					foreach (patchNode patch in curPatchList)
					{
						patch.drawNode();
					}

					ImGui.EndTable();
				}
				ImGui.EndChild();
			}
		}

		private void drawTextureList()
		{
			if (ImGui.BeginChild("texview", new System.Numerics.Vector2(200, ImGui.GetWindowHeight() - 4)))
			{
				if (ImGui.BeginTable("texlist", 3,
					ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable | ImGuiTableFlags.NoBordersInBody))
				{
					ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.NoHide);
					ImGui.TableSetupColumn("Size", ImGuiTableColumnFlags.NoHide);
					ImGui.TableSetupColumn("Type", ImGuiTableColumnFlags.NoHide);

					ImGui.TableHeadersRow();

					foreach (Resource.CTexture tex in currentFile.ctexList.Values)
					{
						if (texNode.drawNode(tex.ctexName, "", ""))
                        {
							loadTexture(tex.patchList);
                        }
					}

					ImGui.EndTable();
				}
				ImGui.EndChild();
			}
		}
	}

	public class texNode
	{
		public static Boolean drawNode(string name, string size, string type)
		{
			bool isSelected = false;

			ImGui.TableNextRow();
			ImGui.TableNextColumn();
			//ImGui.Selectable(name, isSelected, ImGuiSelectableFlags.SpanAllColumns | ImGuiSelectableFlags.AllowItemOverlap);
			isSelected = ImGui.Button(name);
			ImGui.TableNextColumn();
			ImGui.Text("WxH");
			ImGui.TableNextColumn();
			ImGui.Text("TYPE");
			return isSelected;
		}
	}

	public class patchNode
    {
        public patchNode(Resource.CTexPatch res)
        {
            patch = res;
        }

        public Resource.CTexPatch patch;

		public virtual Boolean drawNode()
		{
			bool isSelected = false;

			ImGui.TableNextRow();
			ImGui.TableNextColumn();
			ImGui.Selectable(patch.patchName, isSelected, ImGuiSelectableFlags.SpanAllColumns | ImGuiSelectableFlags.AllowItemOverlap);
			return isSelected;
		}
	}
}

