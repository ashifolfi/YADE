/// <summary>
/// Class definitions for the Doom Graphics format resource
/// 
/// Also includes the structs for the header and post data
/// </summary>

using Microsoft.Xna.Framework.Graphics;
using SkiaSharp;
using Color = System.Drawing.Color;
using Vector3 = System.Numerics.Vector3;

namespace YADE.Resource
{
    /// <summary>
    /// Contains the header data of the patch file
    /// </summary>
    public struct DPHead
    {
        public UInt16 width; // s: 2 o: 0
        public UInt16 height; // s: 2 o: 2
        public Int16 leftoffset; // s: 2 o: 4
        public Int16 topoffset; // s: 2 o: 6
        public UInt32[] columnofs; // s: 4*width o: 8
    }

    /// <summary>
    /// Contains a singular column of patch data
    /// </summary>
    public struct DPPost
    {
        public byte topdelta; // Length: 1 Offset: 0
        public byte length; // Length: 1 Offset: 1
        public byte unused; // Length: 1 Offset: 2
        public byte[] data; // array of pixels in post
                              // Length: length Offset: 3
        public byte unused2; // size: 1 Offset: 3 + length
    }

    /// <summary>
    /// Resource data class for Doom Graphics Format
    /// </summary>
    public class DPicture 
    {
        public DPHead header;
        public List<DPPost> posts;
        public FileStream fslink;

        public Texture2D texture;

        /// <summary>
        /// Create a new Doom GFX Object from lump data
        /// 
        /// WARNING: THIS CLASS HAS NO ERROR HANDLING AT THIS MOMENT IN TIME!
        /// </summary>
        /// <param name="lump">FileStream of the graphics lump</param>
        public DPicture(FileStream lump)
        {
            fslink = lump;

            byte[] pw = new byte[2];
            byte[] ph = new byte[2];
            byte[] plo = new byte[2];
            byte[] pto = new byte[2];
            lump.Read(pw, 0, 2);
            lump.Read(ph, 2, 2);
            lump.Read(plo, 4, 2);
            lump.Read(pto, 6, 2);
            
            // setup the header
            header = new DPHead();
            header.width = BitConverter.ToUInt16(pw);
            header.height = BitConverter.ToUInt16(ph);
            header.leftoffset = BitConverter.ToInt16(plo);
            header.topoffset = BitConverter.ToInt16(pto);

            // record this for later
            int headoffset = 4*BitConverter.ToInt32(pw);

            // and now time to loop through the image data
            // length is 4*width
            byte[] pcos = new byte[4*BitConverter.ToUInt16(pw)];
            lump.Read(pcos, 8, 4*BitConverter.ToUInt16(pw));

            // and now convert this set of bytes into a uint32 array
            UInt32[] poffa = new UInt32[BitConverter.ToUInt16(pw)];
            byte[] offbuffer = new byte[4];
            foreach (byte data in pcos)
            {
                if (offbuffer.Length != 4)
                    offbuffer.Append(data);
                else
                {
                    poffa.Append(BitConverter.ToUInt32(offbuffer));
                    offbuffer = new byte[4];
                }
            }

            foreach (UInt32 postoff in poffa)
            {
                DPPost post = new DPPost();

                byte[] buffer = new byte[3];
                lump.Read(buffer, Convert.ToInt32(postoff), 3);
                post.topdelta = buffer[1];
                post.length = buffer[2];
                post.unused = buffer[3];

                // time for a bit of fun
                post.data = new byte[post.length];
                lump.Read(post.data, Convert.ToInt32(postoff)+3, post.length);

                buffer = new byte[1];
                lump.Read(buffer, Convert.ToInt32(postoff)+3+post.length, 1);
                post.unused2 = buffer[1];

                posts.Add(post);
            }

            // at the end of it all attempt to write a texture2d from the data so we can view it
            toTexture();
        }

        /// <summary>
        /// Updates the texture field data of the object when ran.
        /// </summary>
        public void toTexture()
        {
            // I couldn't tell you how most of this shit works only that it does.
            texture = new Texture2D(Game1._graphics.GraphicsDevice, header.width, header.height);

            // TODO: This should not be done here
            DPalette playpal = new DPalette("MRCE/PLAYPAL");

            // based on the doom wiki conversion algorithm
            SKBitmap bmp = new SKBitmap(header.width, header.height);

            int postcount = 1;
            foreach (DPPost column in posts)
            {
                int pixelcount = 1;
                foreach (byte pixel in column.data) {
                    // call for the playpal index corresponding to this pixel
                    Vector3 rgbnum = playpal.indexToRGB(pixel);
                    Color color = Color.FromArgb(255, (int)rgbnum.X, (int)rgbnum.Y, (int)rgbnum.Z);
                    SKColor.FromHsl(color.GetHue(), color.GetSaturation(), color.GetBrightness());

                    bmp.SetPixel(postcount, pixelcount, SKColor.Empty);
                    pixelcount++;
                }
                postcount++;
            }

            //
        }
    }
}