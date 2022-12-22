using Microsoft.Xna.Framework.Graphics;
using SkiaSharp;
using Color = System.Drawing.Color;
using Vector3 = System.Numerics.Vector3;

namespace YADE.Resource
{

    /// <summary>
    /// Resource data class for Doom Graphics Format
    /// </summary>
    public class DPicture : GenericResource
    {
        /// <summary>
        /// Contains the header data of the patch file
        /// </summary>
        public struct DPHead
        {
            /// <summary>s: 2 o: 0</summary>
            public ushort width;
            /// <summary>s: 2 o: 2</summary>
            public ushort height;
            /// <summary>s: 2 o: 4</summary>
            public short leftoffset;
            /// <summary>s: 2 o: 6</summary>
            public short topoffset;
            /// <summary>s: 4*width o: 8</summary>
            public ushort[] columnofs;
        }

        /// <summary>
        /// Contains a singular column of patch data
        /// </summary>
        public struct DPPost
        {
            /// <summary>Length: 1 Offset: 0</summary>
            public byte topdelta;
            /// <summary>Length: 1 Offset: 1</summary>
            public byte length;
            /// <summary>Length: 1 Offset: 2</summary>
            public byte unused;
            /// <summary>array of pixels in post
            /// Length: length Offset: 3</summary>
            public byte[] data;
            /// <summary>size: 1 Offset: 3 + length</summary>
            public byte unused2;
        }

        /// <summary>The image header. contains the width, height, and graphic offsets</summary>
        public DPHead header;
        /// <summary>list of "posts" or columns of pixel data</summary>
        public List<DPPost> posts = new List<DPPost>();

        public Stream imgstream;

        /// <summary>
        /// Create a new Doom GFX Object from lump data
        /// 
        /// WARNING: THIS CLASS HAS NO ERROR HANDLING AT THIS MOMENT IN TIME!
        /// </summary>
        /// <param name="name">The name of the lump. used whenever a name needs to be shown</param>
        /// <param name="lump">FileStream of the graphics lump</param>
        public DPicture(string name, FileStream lump) : base(name)
        {
            //fslink = lump;

            byte[] pw = new byte[2];
            byte[] ph = new byte[2];
            byte[] plo = new byte[2];
            byte[] pto = new byte[2];
            lump.Read(pw, 0, 2);
            lump.Seek(2, SeekOrigin.Current);
            lump.Read(ph, 0, 2);
            lump.Seek(2, SeekOrigin.Current);
            lump.Read(plo, 0, 2);
            lump.Seek(2, SeekOrigin.Current);
            lump.Read(pto, 0, 2);
            
            // setup the header
            header = new DPHead();
            header.width = BitConverter.ToUInt16(pw);
            header.height = BitConverter.ToUInt16(ph);
            header.leftoffset = BitConverter.ToInt16(plo);
            header.topoffset = BitConverter.ToInt16(pto);

            // record this for later
            int headoffset = 4*BitConverter.ToUInt16(pw);

            // and now time to loop through the image data
            // length is 4*width
            byte[] pcos = new byte[4*BitConverter.ToUInt16(pw)];
            lump.Seek(8, SeekOrigin.Begin);
            lump.Read(pcos, 0, 4*BitConverter.ToUInt16(pw));

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
                lump.Seek(Convert.ToInt32(postoff), SeekOrigin.Begin);
                lump.Read(buffer, 0, 3);
                post.topdelta = buffer[0];
                post.length = buffer[2];
                post.unused = buffer[1];

                // time for a bit of fun
                post.data = new byte[Convert.ToInt32(post.length)];
                lump.Seek(3, SeekOrigin.Current);
                lump.Read(post.data, 0, post.length);

                buffer = new byte[1];
                lump.Seek(Convert.ToInt64(post.length), SeekOrigin.Current);
                lump.Read(buffer, 0, 1);
                post.unused2 = buffer[0];

                posts.Add(post);
            }

            // at the end of it all attempt to write a texture2d from the data so we can view it
            toTexture();
        }

        /// <summary>
        /// Updates the texture field data of the object when ran.
        /// </summary>
        private void toTexture()
        {
            // TODO: This should not be done here
            DPalette playpal = new DPalette("Sonic Robo Blast 2", "MRCE/PLAYPAL");

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

                    SKColor newcolor = SKColor.FromHsl(color.GetHue(), color.GetSaturation(), color.GetBrightness());

                    bmp.SetPixel(postcount-1, pixelcount-1, newcolor);
                    pixelcount++;
                }
                postcount++;
            }

            SKData encoded = SKImage.FromBitmap(bmp).Encode();
            imgstream = encoded.AsStream();
        }
    }
}