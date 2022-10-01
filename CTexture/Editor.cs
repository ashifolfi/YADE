using System;
using ImGuiNET;
using Vector2 = System.Numerics.Vector2;
using YADE.Resource;

namespace YADE.CTexture
{
    public class Editor
    {
        public CTDefResource currentFile;
        private String windowTitle = "Composite Texture Editor";

        public void drawWindow(bool open)
        {
            ImGui.SetNextWindowSize(new Vector2(300, 300), ImGuiCond.FirstUseEver);
            ImGui.Begin(windowTitle, ref open);

            // Change title to currently active file if one is open
            if (currentFile != null)
                windowTitle = "Composite Texture Editor - " + currentFile.resName;
            else
                windowTitle = "Composite Texture Editor";

            // the actual ui code

            ImGui.End();
        }

        // TODO: Implement texture view
        private void drawTextureView()
        {

        }

        // TODO: Implement patch list
        private void drawPatchList()
        {

        }

        // TODO: Implement texture list
        private void drawTextureList()
        {

        }
    }
}

