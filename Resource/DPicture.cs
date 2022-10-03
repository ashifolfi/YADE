using System;
using SkiaSharp;

namespace YADE.Resource
{
    // TODO: Move this out of here.
    // PLAYPAL loading. used by DPicture to properly display colors from picture lumps
    public class DPalette {
        public DPalette(string palpath) {
            fslink = File.Open(palpath, FileMode.Open);
        }

        private FileStream fslink;

        // colors returned in vector3 format for ease of conversion
        public Vector3 indexToRGB(byte index) 
        {
            // attempt to read from palette 1 at given byte index
            byte[] colorbuffer = new byte[3];
            fslink.Read(colorbuffer, Convert.ToInt32(index)*3, 3);

            Vector3 color = new Vector3(
                Convert.ToInt32(colorbuffer[1]),
                Convert.ToInt32(colorbuffer[2]),
                Convert.ToInt32(colorbuffer[3])
            );

            return color;
        }
    }
    public struct DPHead
    {
        public UInt16 width; // s: 2 o: 0
        public UInt16 height; // s: 2 o: 2
        public Int16 leftoffset; // s: 2 o: 4
        public Int16 topoffset; // s: 2 o: 6
        public UInt32[] columnofs; // s: 4*width o: 8
    }

    public struct DPPost
    {
        public byte topdelta; // Length: 1 Offset: 0
        public byte length; // Length: 1 Offset: 1
        public byte unused; // Length: 1 Offset: 2
        public byte[] data; // array of pixels in post
                              // Length: length Offset: 3
        public byte unused2; // size: 1 Offset: 3 + length
    }

    public class DPicture 
    {
        public DPHead header;
        public List<DPPost> posts;
        public FileStream fslink;

        public Texture2D texture;

        // Create new DPicture resource from a lump
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
        public void toTexture()
        {
            // I couldn't tell you how most of this shit works only that it does.
            texture = new Texture2D(Game1._graphics.GraphicsDevice, header.width, header.height);

            // based on the doom wiki conversion algorithm
            SKBitmap bmp = new SKBitmap(header.width, header.height);

            int postcount = 1;
            foreach (DPPost column in posts)
            {
                int pixelcount = 1;
                foreach (byte pixel in column.data) {
                    bmp.SetPixel(postcount, pixelcount, SKColor.Empty);
                    pixelcount++;
                }
                postcount++;
            }

            //
        }
    }
}