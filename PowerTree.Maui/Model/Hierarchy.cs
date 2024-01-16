using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui.Model
{
    // =======================   Each Hierarchy has a Name and may be used by different subsystems for tracking Links, Documents, Menus, or anything, using a hierarchy that may be displayed and navigated by the user
    // =======================   Each Hierarchy, when created will have a single top level node created to hold all the items and nodes below it.

    [Table("pthierarchy")]
    public class PTHierarchy
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HierarchyId { get; set; }


        [Required]
        [MaxLength(50), MinLength(2)]
        public string? HierarchyName { get; set; }

        [Required]
        [MaxLength(50), MinLength(2)]
        public string? Subsystem { get; set; }

    }
    // =======================   Each Node is related to a specific hierarchy; Except for the single top level node of the hierarchy(RootNode), all nodes will have and point to a parent Node
    // =======================   Each Node has zero or one ParentNode;
    // =======================   Each Node has zero to many Child nodes

    [Table("ptnode")]
    public class PTNode
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int NodeId { get; set; }

        public int HierarchyId { get; set; }

        public int NodeOrder { get; set; }

        [Required]
        [MaxLength(50), MinLength(2)]
        public string NodeName { get; set; }

        public int? ParentNodeId { get; set; }

        [ForeignKey(nameof(ParentNodeId))]
        public PTNode ParentNode { get; set; }

        public PTHierarchy Hierarchy { get; set; }

        public virtual ICollection<PTNodeItem> NodeItems { get; set; }

    }

    // =======================  Each NodeItem belongs to a single Node; Nodes may have many NodeItems 
    // =======================  The NodeItem contains a FK linkid to the actual item 

    [Table("ptnodeitem")]
    public class PTNodeItem
    {
        [Key]
        public int NodeItemId { get; set; }
        public string NodeItemName { get; set; }

        /// <summary>
        /// This EntityId represents some external entity to the PowerTree Library.
        /// It may be a LinkId, DocumentId, ContactId, or anything the consumer of the control desires
        /// </summary>
        public int EntityId { get; set; }

        public int NodeId { get; set; }

        public PTNode Node { get; set; }

        //public Link Link { get; set; }

        //
        //public Contact Contact { get; set; }

        //[DataDictionary(Description = "Navigation Property to Document")]
        //public Document1 Document { get; set; }

        public int Order { get; set; }

        public byte[]? NodeImage { get; set; }
    }
}
