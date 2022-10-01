using System;

namespace YADE.CTexture
{
    public class CTextureFile
    {
        public string fileName;
        public Dictionary<String, CTextureResource> ctexList;
    }

    public class CTextureResource
    {
        public string ctexName;
        public Dictionary<String, CTexPatch> patchList;
    }

    public class CTexPatch
    {
        public string patchName;
        public Vector2 position;
    }
}

