/// <summary>
/// Class definition for Doom Palette type resources
/// </summary>

using System.Numerics;

namespace YADE.Resource
{
    public class DPalette
    {
        public DPalette(string palpath)
        {
            fslink = File.Open(palpath, FileMode.Open);
        }

        private FileStream fslink;

        /// <summary>
        /// Pulls the RGB values that correspond to the given index's 3 byte slot
        /// </summary>
        /// <param name="index">Palette index to pull the color from</param>
        /// <returns>RGB format colors stored in a Vector3</returns>
        public Vector3 indexToRGB(byte index)
        {
            // attempt to read from palette 1 at given byte index
            byte[] colorbuffer = new byte[3];
            fslink.Read(colorbuffer, Convert.ToInt32(index) * 3, 3);

            Vector3 color = new Vector3(
                Convert.ToInt32(colorbuffer[1]),
                Convert.ToInt32(colorbuffer[2]),
                Convert.ToInt32(colorbuffer[3])
            );

            return color;
        }
    }
}
