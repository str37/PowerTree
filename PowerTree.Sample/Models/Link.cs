
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PowerTree.Maui.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;


namespace PowerTree.Sample.Models
{

    [Table("link")]
    public class Link
    {
        public int LinkId { get; set; }

        public string LinkName { get; set; }

        public string LinkURL { get; set; }
        public virtual ICollection<LinkIcon> LinkIcons { get; set; }


    }

    [Table("linkicon")]
    public class LinkIcon
    {
        public int LinkIconId { get; set; }

        public int LinkId { get; set; }

        public string Rel { get; set; }

        public string Href { get; set; }

        public string MimeType { get; set; }

        public byte[] IconImage { get; set; }

        public string Size { get; set; }

        public virtual Link Link { get; set; }

    }

}
