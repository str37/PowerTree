using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui.Model
{
    [Serializable]
    public class XamlItem
    {
        public string Key { get; set; }
        public int ItemId { get; set; }

        public byte[]? Icon { get; set; }
    }
    [Serializable]
    public class XamlItemGroup
    {
        public List<XamlItemGroup> Children { get; } = new();
        public List<XamlItem> XamlItems { get; } = new();

        public string Name { get; set; }
        public int GroupId { get; set; }
    }
}
