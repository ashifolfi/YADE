using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using ImGuiNET;
using Num = System.Numerics;

namespace YADE
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private ImGuiRenderer _imGuiRenderer;

        private Texture2D _xnaTexture;
        private IntPtr _imGuiTexture;

        static public List<Archive.Editor> openEditors = new List<Archive.Editor>();

        static public uint dockid;

        static public CTexture.Editor editor1 = new CTexture.Editor();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.PreferMultiSampling = true;
            Window.AllowUserResizing = true;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _imGuiRenderer = new ImGuiRenderer(this);
            _imGuiRenderer.RebuildFontAtlas();

            ImGui.GetIO().ConfigFlags = ImGuiConfigFlags.DockingEnable;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(10, 10, 10));

            // Call BeforeLayout first to set things up
            _imGuiRenderer.BeforeLayout(gameTime);
            dockid = ImGui.DockSpaceOverViewport(ImGui.GetMainViewport());

            // call all of our draw calls we need
            UI.MainMenuBar.drawMenuBar();
            if (UI.AboutWindow.is_open)
                UI.AboutWindow.drawWindow();

            // draw all open editors
            foreach (Archive.Editor editor in openEditors) {
                // put this here for now
                // destroy the editor on close as we've closed the file.
                if (!editor.is_open)
                {
                    openEditors.Remove(editor);
                    break;
                }
                else
                    editor.drawWindow();
            }

            editor1.drawWindow(true);

            ImGui.ShowDemoWindow();

            // Call AfterLayout now to finish up and draw all the things
            _imGuiRenderer.AfterLayout();

            base.Draw(gameTime);
        }
    }
}