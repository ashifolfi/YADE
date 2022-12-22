using System;
using Vector2 = System.Numerics.Vector2;

namespace YADE.Resource
{

    /// <summary>
    /// Composite Texture Definition Resource
    /// 
    /// A resource containing a dictionary of all Composite Textures found inside of a Composite Texture Definition
    /// </summary>
    public class CTDefResource : GenericResource // Inherit from generic resource
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">File/Res Name: Used whenever a name needs to be displayed</param>
        /// <param name="texlist">The dictionary of CTexture entries from a definition file</param>
        public CTDefResource(string name, Dictionary<String, CTexture> texlist) : base(name)
        {
            ctexList = texlist;
        }

        /// <summary>
        /// The dictionary of CTexture entries from a definition file
        /// </summary>
        public Dictionary<String, CTexture> ctexList;
    }

    /// <summary>
    /// Composite texture resource
    /// 
    /// Contains the patch list and name of the texture alongside a vec2 size of the texture
    /// </summary>
    public class CTexture : GenericResource
    {
        /// <summary>
        /// Creates a new CTexture resource
        /// </summary>
        /// <param name="name">Name of the texture</param>
        /// <param name="plist">CTexPatch list</param>
        public CTexture(string name, List<CTexPatch> plist) : base(name)
        {
            patchList = plist;
            ctexName = resName;
        }

        /// <summary>Duplicate of resName</summary>
        public string ctexName;
        /// <summary>Texture type</summary>
        public String type;
        /// <summary>The width(x) and height(y) of the texture</summary>
        public System.Numerics.Vector2 size;
        /// <summary>The list of CTexPatch es that make up the texture</summary>
        public List<CTexPatch> patchList;
    }

    /// <summary>
    /// Composite Texture Patch entry
    /// 
    /// Contains the position and name of patch alongside any patch flags/info
    /// </summary>
    public class CTexPatch : GenericResource
    {
        /// <summary>
        /// Create a new Composite Texture Patch entry
        /// </summary>
        /// <param name="name">name of the patch</param>
        /// <param name="pos">Vector2 position of the patch</param>
        public CTexPatch(string name, Vector2 pos) : base (name)
        {
            patchName = resName;
            position = pos;
        }

        /// <summary>
        /// Duplicate of resName
        /// </summary>
        public string patchName;
        /// <summary>Contains the X and Y coordinates</summary>
        public Vector2 position;

        // Extra info
        /// <summary>NOT IMPLEMENTED</summary>
        public bool flipX;
        /// <summary>NOT IMPLEMENTED</summary>
        public bool flipY;
        /// <summary>NOT IMPLEMENTED</summary>
        public bool useOffsets;

        // <summary>NOT IMPLEMENTED</summary>
        // public TYPE rotation;
        // <summary>NOT IMPLEMENTED</summary>
        // public TYPE translation;

        // <summary>NOT IMPLEMENTED</summary>
        // public TYPE blend;
        /// <summary>NOT IMPLEMENTED</summary>
        public float alpha;
        /// <summary>NOT IMPLEMENTED</summary>
        public string style;
    }
}

