/// <summary>
/// Contains all definitions related to the CTexture Editor GUI component
/// </summary>

using ImGuiNET;
using Vector2 = System.Numerics.Vector2;
using YADE.Resource;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace YADE.CTexture
{
	/// <summary>
	/// Main CTexture Editor Class
	/// </summary>
	public class Editor
    {
		/// <summary>
		/// Constructor for CTexture Editor
		/// </summary>
		/// <param name="path">Path to search for definitions inside of</param>
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

		private string curArchivePath;
		private string windowTitle = "Texture Editor";
		private List<EditorTab> defTabs = new List<EditorTab>();

		/// <summary>
		/// Draw main CTexture Editor window
		/// </summary>
		/// <param name="open">boolean for window open status</param>
        public void drawWindow(bool open)
        {
            ImGui.SetNextWindowSize(new Vector2(300, 300), ImGuiCond.FirstUseEver);
			ImGui.Begin(windowTitle, ref open);

            // Call each tab for the composite texture definitions
			if (ImGui.BeginTabBar("comptexedit", ImGuiTabBarFlags.FittingPolicyScroll))
            {
				foreach (EditorTab tab in defTabs)
				{
					tab.draw(curArchivePath);
				}
				ImGui.EndTabBar();
			}

			ImGui.End();
        }

		/// <summary>
		/// Load a definition file into a new tab
		/// </summary>
		/// <param name="resfile">CTexture Definition data</param>
        public void loadDefinitions(CTDefResource resfile)
        {
			defTabs.Add(new EditorTab(resfile));
        }
    }

	/// <summary>
	/// Class for CTexture Editor Tabs
	/// </summary>
    public class EditorTab
    {
		/// <summary>
		/// Constructor for EditorTabs
		/// </summary>
		/// <param name="resfile">CTexture Definition data</param>
        public EditorTab(CTDefResource resfile)
        {
            currentFile = resfile;
        }

		/// <summary>
		/// the file for this tab
		/// </summary>
	    public CTDefResource currentFile;
		/// <summary>
		/// is the editor open
		/// </summary>
		public bool is_open = false;

		private List<patchNode> curPatchList = new List<patchNode>();

		/// <summary>
		/// Draw the Composite Texture Editor Tab
		/// </summary>
		/// <param name="path">Directory path for our archive</param>
		public void draw(string path)
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
					drawTextureList(path);
					ImGui.TableNextColumn();
					drawTextureView();
					ImGui.TableNextColumn();
					drawPatchList();

					ImGui.EndTable();
				}
				ImGui.EndTabItem();
            }
        }

		/// <summary>
		/// function to load a texture from a patch list
		/// </summary>
		/// <param name="plist">patch list</param>
		/// <param name="path">TODO: We should pull from all open archives/directories</param>
		public void loadTexture(List<CTexPatch> plist, string path)
		{
			curPatchList = new List<patchNode>();
			foreach (CTexPatch patch in plist)
			{
				curPatchList.Add(new patchNode(patch, path));
			}
		}

		// TODO: Implement texture view
		private void drawTextureView()
		{
			if (ImGui.BeginChild("texeditview", new System.Numerics.Vector2(500, ImGui.GetWindowHeight() - 50)))
			{
				// draw each patch in a loop
				Vector2 startPos = ImGui.GetCursorPos();
                foreach (patchNode patch in curPatchList)
                {
					ImGui.SetCursorPos(startPos
                        + new Vector2(patch.patch.position.X, patch.patch.position.Y));
                    ImGui.Image(patch.patchPtr, 
						new Vector2(patch.patchTex.Width, patch.patchTex.Height));
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

		private void drawTextureList(string path)
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
							loadTexture(tex.patchList, path);
                        }
					}

					ImGui.EndTable();
				}
				ImGui.EndChild();
			}
		}
	}

    /// <summary>
    /// Class for the CTexture editor texture list
    /// </summary>
    public class texNode
	{
        /// <summary>
        /// Used by main editor to draw the node into the texture list
        /// </summary>
        /// <returns>Boolean corresponding to selection status</returns>
        public static bool drawNode(string name, string size, string type)
		{
			bool isSelected = false;

			ImGui.TableNextRow();
			ImGui.TableNextColumn();
			isSelected = ImGui.Button(name);
			ImGui.TableNextColumn();
			ImGui.Text(size);
			ImGui.TableNextColumn();
			ImGui.Text(type);
			return isSelected;
		}
	}

	/// <summary>
	/// Class for the CTexture editor patch list
	/// </summary>
	public class patchNode
    {
		/// <summary>
		/// Constructor for patch list entries
		/// </summary>
		/// <param name="res">resource pointer</param>
		/// <param name="path">path to patch</param>
        public patchNode(Resource.CTexPatch res, string path)
        {
            patch = res;
			patchTex = CTexture.FileSystem.locatePatchGraphic(res.patchName, path);
			patchPtr = Game1._imGuiRenderer.BindTexture(patchTex);
        }

		/// <summary>
		/// resource pointer
		/// </summary>
        public Resource.CTexPatch patch;
		/// <summary>
		/// patch graphic pointer
		/// </summary>
		public Texture2D patchTex;
		/// <summary>
		/// integer pointer for ImGui
		/// </summary>
		public IntPtr patchPtr;

		/// <summary>
		/// Used by main editor to draw the node into the patch list
		/// </summary>
		/// <returns>Boolean corresponding to selection status</returns>
        public virtual bool drawNode()
		{
			bool isSelected = false;

			ImGui.TableNextRow();
			ImGui.TableNextColumn();
			ImGui.Selectable(patch.patchName, isSelected, ImGuiSelectableFlags.SpanAllColumns | ImGuiSelectableFlags.AllowItemOverlap);
			return isSelected;
		}
	}
}

