using System;
using ImGuiNET;
using Vector2 = System.Numerics.Vector2;
using YADE.Resource;
using System.IO;

namespace YADE.CTexture
{
    public class Editor
    {
		public Editor(string path)
        {
			curArchivePath = path;

			// Initialize every definition file we find
			// Look for all TEXTURE.* files
			DirectoryInfo dirtosearch = new DirectoryInfo(curArchivePath);
			FileInfo[] filesInDir = dirtosearch.GetFiles("TEXTURES.*", SearchOption.TopDirectoryOnly);
			foreach (FileInfo foundFile in filesInDir)
            {
				loadDefinitions(CTexture.FileSystem.parseFile(foundFile.Name, curArchivePath + "/" + foundFile.Name));
			}
			windowTitle += " - " + curArchivePath;
		}

		private String curArchivePath;
		private String windowTitle = "Texture Editor";
		private List<EditorTab> defTabs = new List<EditorTab>();

        public void drawWindow(bool open)
        {
            ImGui.SetNextWindowSize(new Vector2(300, 300), ImGuiCond.FirstUseEver);
			ImGui.Begin(windowTitle, ref open);

            // Call each tab for the composite texture definitions
			if (ImGui.BeginTabBar("comptexedit", ImGuiTabBarFlags.FittingPolicyScroll))
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
					ImGui.TableSetupColumn("", ImGuiTableColumnFlags.None | ImGuiTableColumnFlags.WidthFixed, 200);
					ImGui.TableSetupColumn("", ImGuiTableColumnFlags.None | ImGuiTableColumnFlags.WidthStretch);
					ImGui.TableSetupColumn("", ImGuiTableColumnFlags.None | ImGuiTableColumnFlags.WidthFixed, 200);

					ImGui.TableNextColumn();
					drawTextureList();
					ImGui.TableNextColumn();
					drawTextureView();
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
			if (ImGui.BeginChild("texeditview", new System.Numerics.Vector2(400, ImGui.GetWindowHeight() - 50)))
			{
				// draw each patch in a loop

				foreach (patchNode patch in curPatchList)
                {
					ImGui.Text(patch.patch.patchName + Convert.ToString(patch.patch.position));
                }

				ImGui.EndChild();
			}
		}

		private void drawPatchList()
		{
			if (ImGui.BeginChild("patview", new System.Numerics.Vector2(200, ImGui.GetWindowHeight() - 50)))
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
			if (ImGui.BeginChild("texview", new System.Numerics.Vector2(200, ImGui.GetWindowHeight() - 50)))
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
						if (texNode.drawNode(tex.ctexName, Convert.ToString(tex.size.X + "x" + tex.size.Y), tex.type))
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
			ImGui.Text(size);
			ImGui.TableNextColumn();
			ImGui.Text(type);
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

