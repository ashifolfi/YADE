/*
    Envrionment Manager
    
    Handles the entire environment used globally
*/
using System;
using YADE.Resource;

namespace YADE
{
    /// <summary>
    /// Environment Manager
    /// </summary>
    public class EnvManager 
    {
        // private variables
        // These are private either because they have special systems for editing 
        // or to protect them from being modified in places they shouldn't be.

        /// <summary> the current global doom graphics palette </summary>
        private DPalette globalPalette;
        /// <summary>
        /// List of all open editors with no sorting 
        /// </summary>
        public List<Archive.Editor> openEditors = new List<Archive.Editor>();
        /// <summary> The current "base archive" or IWAD </summary>
        private Resource.Archive baseArchive;

        /// <summary> create a new envrionment manager.
        /// ***YOU SHOULD ONLY EVER CREATE ONE OF THESE!*** </summary>
        public EnvManager() {
            // TODO: Add config file reading to load default settings and palette and whatnot
        }

        /// <summary>
        /// Set's the global palette to the given palette
        /// </summary>
        /// <param name="newpal">DPalette resource to use</param>
        public void setPalette(DPalette newpal) {
            globalPalette = newpal;
        }

        /// <summary>
        /// Returns the current global palette
        /// </summary>
        /// <returns>DPalette type resource of the global palette</returns>
        public DPalette getPalette() {
            return globalPalette;
        }
    }
}