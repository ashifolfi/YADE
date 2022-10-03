using Microsoft.Xna.Framework.Graphics;
using ImGuiNET;

namespace YADE
{
    /// <summary>
    /// UNKNOWN: Provided by the ImGuiNET XNA Example
    /// </summary>
    public static class DrawVertDeclaration
    {
        /// <summary>
        /// UNKNOWN: Provided by the ImGuiNET XNA Example
        /// </summary>
        public static readonly VertexDeclaration Declaration;

        /// <summary>
        /// UNKNOWN: Provided by the ImGuiNET XNA Example
        /// </summary>
        public static readonly int Size;

        static DrawVertDeclaration()
        {
            unsafe { Size = sizeof(ImDrawVert); }

            Declaration = new VertexDeclaration(
                Size,

                // Position
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),

                // UV
                new VertexElement(8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),

                // Color
                new VertexElement(16, VertexElementFormat.Color, VertexElementUsage.Color, 0)
            );
        }
    }
}