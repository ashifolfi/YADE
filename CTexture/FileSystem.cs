using System;
using System.Text.RegularExpressions;
using YADE.Resource;

namespace YADE.CTexture
{
    public class FileSystem
    {
        public CTDefResource parseFile()
        {
            return new CTDefResource("lmao", new Dictionary<string, Resource.CTexture>());
        }

        public void saveCTexDefinitions(CTDefResource tosave)
        {

        }
    }
}

