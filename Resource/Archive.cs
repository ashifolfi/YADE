/// <summary>
/// Class definitions for Archive related resources
/// </summary>

using System;
using System.Xml.Linq;
using ImGuiNET;

namespace YADE.Resource
{
    /// <summary>
    /// Archive resoruce: contains all information related to the contents and info about an archive
    /// </summary>
    public class Archive : GenericResource
    {
        /// <summary>
        /// Create a new Archive resource object
        /// </summary>
        /// <param name="name">Name of the archive to look for</param>
        /// <param name="path">Path to where it is stored</param>
        public Archive(string name, string path) : base(name)
        {
            filePath = path;
        }

        /// <summary>
        /// Full path to where the file is stored
        /// </summary>
        public string filePath;
        /// <summary>
        /// Full list of an archive's contents
        /// </summary>
        public List<ArchiveItem> fileList;
    }

    /// <summary>
    /// Archive Item Definition
    /// Base class for archive items
    /// </summary>
    public class ArchiveItem
    {
        /// <summary>
        /// Create new archive item
        /// </summary>
        /// <param name="name">Name of archive item</param>
        /// <param name="path">Path to item in archive</param>
        /// <param name="type">Type of item</param>
        public ArchiveItem(string name, string path, string type)
        {
            fileName = name;
            fullPath = path;
            fileType = type;
        }

        /// <summary>
        /// Name of archive item
        /// </summary>
        public string fileName;
        /// <summary>
        /// Type of archive item
        /// </summary>
        public string fileType;
        /// <summary>
        /// Full path to archive item within archive
        /// </summary>
        public string fullPath;

        /// <summary>
        /// Used by the archive editor to draw the archive item in a list
        /// </summary>
        /// <returns>Boolean determining selection status</returns>
        public virtual Boolean drawNode()
        {
            bool isSelected = false;

            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            // TODO: Implement ID numbers
			ImGui.TreeNodeEx("", ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.Bullet | ImGuiTreeNodeFlags.NoTreePushOnOpen | ImGuiTreeNodeFlags.SpanFullWidth);
            ImGui.TableNextColumn();
            ImGui.Selectable(fileName, isSelected, ImGuiSelectableFlags.SpanAllColumns | ImGuiSelectableFlags.AllowItemOverlap);
            ImGui.TableNextColumn();
            ImGui.Text(fileType);
            return isSelected;
		}
    }

    /// <summary>
    /// Directory type. Requires a special class due to needing some specific items that regular items don't need
    /// </summary>
    public class ArchiveDirectory : ArchiveItem
    {
        /// <summary>
        /// Create a new Directory type
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="path">Path to item within archive</param>
        /// <param name="type">type</param>
        /// <param name="conts">Contents of the directory</param>
        public ArchiveDirectory(string name, string path, string type, List<ArchiveItem> conts) : base(name, path, type)
        {
            contents = conts;
        }

        /// <summary>
        /// Contents of the directory
        /// </summary>
        List<ArchiveItem> contents;

        /// <summary>
        /// Used by the archive editor to draw item into a list
        /// </summary>
        /// <returns>Boolean determining selection status</returns>
        public override Boolean drawNode()
        {
            bool isSelected = false; 
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
			bool open = ImGui.TreeNodeEx("", ImGuiTreeNodeFlags.SpanFullWidth);

            ImGui.TableNextColumn();
            ImGui.Selectable(fileName, isSelected, ImGuiSelectableFlags.SpanAllColumns | ImGuiSelectableFlags.AllowItemOverlap);

			ImGui.TableNextColumn();
            ImGui.Text(fileType);

            if (open)
            {
                foreach (ArchiveItem archiveItem in contents)
                {
                    // TODO: Figure out how to return that a subnode is selected
                    archiveItem.drawNode();
                }
                ImGui.TreePop();
            }
            return isSelected;
		}
    }

    /// <summary>
    /// Resource type Item. For the various resources we can load
    /// Images, sounds, scripts, etc.
    /// 
    /// Special class so we can have a resource pointer stored inside it
    /// </summary>
    public class ArchiveResource : ArchiveItem
    {
        /// <summary>
        /// Create new resource type archive item
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="path">Path to item in archive</param>
        /// <param name="type">Type of item</param>
        /// <param name="res">Resource data to store in pointer</param>
        public ArchiveResource(string name, string path, string type, GenericResource res) : base(name, path, type)
        {
            resource = res;
        }

        /// <summary>
        /// Resource data pointer
        /// </summary>
        GenericResource resource;
    }
}

