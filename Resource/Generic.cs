using System;
namespace YADE.Resource
{
    /// <summary>
    /// Generic Resource class
    /// </summary>
    public class GenericResource
    {
        /// <summary>
        /// Create generic resource
        /// </summary>
        /// <param name="name">Name of resource</param>
        public GenericResource(string name)
        {
            resName = name;
        }

        /// <summary>
        /// Name of resource
        /// </summary>
        public string resName;
    }
}

