using System.Numerics;

namespace YADE.Resource
{
    /// <summary>
    /// Doom Palette resource.
    /// Used by DPicture to display colors based on indexes
    /// </summary>
    public class DPalette : GenericResource
    {
        /// <summary>
        /// Creates a new DPalette resource
        /// </summary>
        /// <param name="name">Unique recognizable name for palette chooser</param>
        /// <param name="palpath">Path to palette file</param>
        public DPalette(string name, string palpath) : base(name)
        {
            filepath = palpath;
        }

        private string filepath;

        /// <summary>
        /// Pulls the RGB values that correspond to the given index's 3 byte slot
        /// </summary>
        /// <param name="index">Palette index to pull the color from</param>
        /// <returns>RGB format colors stored in a Vector3</returns>
        public Vector3 indexToRGB(byte index)
        {
            FileStream fslink = File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            // attempt to read from palette 1 at given byte index
            byte[] colorbuffer = new byte[3];
            fslink.Seek(Convert.ToInt32(index) * 3, SeekOrigin.Begin);
            fslink.Read(colorbuffer, 0, 3);

            Vector3 color = new Vector3(
                Convert.ToInt32(colorbuffer[0]),
                Convert.ToInt32(colorbuffer[1]),
                Convert.ToInt32(colorbuffer[2])
            );

            fslink.Close();

            return color;
        }
    }
}
