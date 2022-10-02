using System;
using Vector2 = System.Numerics.Vector2;

namespace YADE.Resource
{

    public class CTDefResource : GenericResource // Inherit from generic resource
    {
        public CTDefResource(string name, Dictionary<String, CTexture> texlist) : base(name)
        {
            ctexList = texlist;
        }

        public Dictionary<String, CTexture> ctexList;
    }

    // Custom resource classes for the internal components of a CTexture
    public class CTexture
    {
        public CTexture(string name, List<CTexPatch> plist)
        {
            ctexName = name;
            patchList = plist;
        }

        public string ctexName;
        public String type;
        public System.Numerics.Vector2 size;
        public List<CTexPatch> patchList;
    }

    public class CTexPatch
    {
        public CTexPatch(string name, Vector2 pos)
        {
            patchName = name;
            position = pos;
        }

        // base info
        public string patchName;
        public Vector2 position;

        // Extra info
        public bool flipX;
        public bool flipY;
        public bool useOffsets;

        // public TYPE rotation;
        // public TYPE translation;

        // public TYPE blend;
        public float alpha;
        public string style;
    }
}

