using System;

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
        public CTexture(string name, Dictionary<string, CTexPatch> plist)
        {
            ctexName = name;
            patchList = plist;
        }

        public string ctexName;
        public Dictionary<String, CTexPatch> patchList;
    }

    public class CTexPatch
    {
        public CTexPatch(string name, Vector2 pos)
        {
            patchName = name;
            position = pos;
        }

        public string patchName;
        public Vector2 position;
    }
}

