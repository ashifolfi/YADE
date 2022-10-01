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

        public virtual void drawNode()
        {
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.TreeNodeEx(fileName, ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.Bullet | ImGuiTreeNodeFlags.NoTreePushOnOpen | ImGuiTreeNodeFlags.SpanFullWidth);
            ImGui.TableNextColumn();
            ImGui.Text(fileType);
        }
    }

    public class ArchiveDirectory : ArchiveItem
    {
        public ArchiveDirectory(string name, string path, string type, List<ArchiveItem> conts) : base(name, path, type)
        {
            contents = conts;
        }

        List<ArchiveItem> contents;

        public override void drawNode()
        {
            ImGui.TableNextRow();
            ImGui.TableNextColumn();

            bool open = ImGui.TreeNodeEx(fileName, ImGuiTreeNodeFlags.SpanFullWidth);
            ImGui.TableNextColumn();
            ImGui.Text(fileType);

            if (open)
            {
                foreach (ArchiveItem archiveItem in contents)
                {
                    archiveItem.drawNode();
                }
                ImGui.TreePop();
            }
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

