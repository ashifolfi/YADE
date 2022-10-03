using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using ImGuiNET;
using Num = System.Numerics;

namespace YADE
{
    /// <summary>
    /// Main class that handles the entire editor
    /// </summary>
    public class Game1 : Game
    {
        // I need this for things
        // probably dangerous to do this though
        public static GraphicsDeviceManager _graphics;
        public static ImGuiRenderer _imGuiRenderer;

        static public List<Archive.Editor> openEditors = new List<Archive.Editor>();

        static public uint dockid;

        static public CTexture.Editor editor1;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            //_graphics.PreferMultiSampling = true;
            Window.AllowUserResizing = true;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _imGuiRenderer = new ImGuiRenderer(this);
            _imGuiRenderer.RebuildFontAtlas();

            ImGui.GetIO().ConfigFlags = ImGuiConfigFlags.DockingEnable;

            // Colors!!
            var colors = ImGui.GetStyle().Colors;
			colors[(int)ImGuiCol.FrameBg]               = new Num.Vector4(0.48f, 0.28f, 0.16f, 0.54f);
			colors[(int)ImGuiCol.FrameBgHovered]        = new Num.Vector4(0.98f, 0.82f, 0.26f, 0.40f);
			colors[(int)ImGuiCol.FrameBgActive]         = new Num.Vector4(0.98f, 0.82f, 0.26f, 0.40f);
			colors[(int)ImGuiCol.TitleBgActive]         = new Num.Vector4(0.48f, 0.28f, 0.16f, 1.00f);
			colors[(int)ImGuiCol.CheckMark]             = new Num.Vector4(0.98f, 0.49f, 0.26f, 1.00f);
			colors[(int)ImGuiCol.SliderGrab]            = new Num.Vector4(0.98f, 0.49f, 0.26f, 1.00f);
			colors[(int)ImGuiCol.SliderGrabActive]      = new Num.Vector4(0.98f, 0.66f, 0.26f, 1.00f);
			colors[(int)ImGuiCol.Button]                = new Num.Vector4(0.98f, 0.66f, 0.26f, 0.40f);
			colors[(int)ImGuiCol.ButtonHovered]         = new Num.Vector4(0.98f, 0.66f, 0.26f, 1.00f);
			colors[(int)ImGuiCol.ButtonActive]          = new Num.Vector4(0.98f, 0.66f, 0.26f, 1.00f);
			colors[(int)ImGuiCol.Header]                = new Num.Vector4(0.98f, 0.49f, 0.26f, 1.00f);
			colors[(int)ImGuiCol.HeaderHovered]         = new Num.Vector4(0.98f, 0.49f, 0.26f, 1.00f);
			colors[(int)ImGuiCol.HeaderActive]          = new Num.Vector4(0.98f, 0.66f, 0.26f, 1.00f);
			colors[(int)ImGuiCol.SeparatorHovered]      = new Num.Vector4(0.75f, 0.46f, 0.10f, 0.78f);
			colors[(int)ImGuiCol.SeparatorActive]       = new Num.Vector4(0.75f, 0.37f, 0.10f, 1.00f);
			colors[(int)ImGuiCol.ResizeGrip]            = new Num.Vector4(0.98f, 0.59f, 0.26f, 0.20f);
			colors[(int)ImGuiCol.ResizeGripHovered]     = new Num.Vector4(0.98f, 0.52f, 0.26f, 0.67f);
			colors[(int)ImGuiCol.ResizeGripActive]      = new Num.Vector4(0.98f, 0.63f, 0.26f, 0.95f);
			colors[(int)ImGuiCol.Tab]                   = new Num.Vector4(0.58f, 0.35f, 0.18f, 0.86f);
			colors[(int)ImGuiCol.TabHovered]            = new Num.Vector4(0.98f, 0.61f, 0.26f, 0.80f);
			colors[(int)ImGuiCol.TabActive]             = new Num.Vector4(0.98f, 0.52f, 0.26f, 0.67f);
			colors[(int)ImGuiCol.TabUnfocused]          = new Num.Vector4(0.15f, 0.10f, 0.07f, 0.97f);
			colors[(int)ImGuiCol.TabUnfocusedActive]    = new Num.Vector4(0.42f, 0.27f, 0.14f, 1.00f);
			colors[(int)ImGuiCol.DockingPreview]        = new Num.Vector4(0.98f, 0.54f, 0.26f, 0.70f);
			colors[(int)ImGuiCol.TextSelectedBg]        = new Num.Vector4(0.98f, 0.56f, 0.26f, 0.35f);
			colors[(int)ImGuiCol.NavHighlight]          = new Num.Vector4(0.98f, 0.54f, 0.26f, 1.00f);

            ImGui.GetStyle().FrameRounding = 2;
            ImGui.GetStyle().FramePadding = new Num.Vector2(2, 4);
            ImGui.GetStyle().WindowRounding = 4;

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
            if (editor1 != null)
                editor1.drawWindow(true);

            ImGui.ShowDemoWindow();

            // Call AfterLayout now to finish up and draw all the things
            _imGuiRenderer.AfterLayout();

            base.Draw(gameTime);
        }
    }
}