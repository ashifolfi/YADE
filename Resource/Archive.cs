using System;
using System.Xml.Linq;
using ImGuiNET;

namespace YADE.Resource
{
    public class Archive : GenericResource
    {
        public Archive(string name, string path) : base(name)
        {
            filePath = path;
        }

        public string filePath;
        public List<ArchiveItem> fileList;
    }

    public class ArchiveItem
    {
        public ArchiveItem(string name, string path, string type)
        {
            fileName = name;
            fullPath = path;
            fileType = type;
        }

        public string fileName;
        public string fileType;
        public string fullPath;

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

    public class ArchiveDirectory : ArchiveItem
    {
        public ArchiveDirectory(string name, string path, string type, List<ArchiveItem> conts) : base(name, path, type)
        {
            contents = conts;
        }

        List<ArchiveItem> contents;

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

    public class ArchiveResource : ArchiveItem
    {
        public ArchiveResource(string name, string path, string type, GenericResource res) : base(name, path, type)
        {
            resource = res;
        }

        GenericResource resource;
    }
}

