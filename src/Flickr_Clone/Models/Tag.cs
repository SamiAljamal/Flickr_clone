using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Flickr_Clone.Models
{
    [Table("Tags")]
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        public string description { get; set; }
        public virtual ICollection<ImageTag> ImageTags { get; set; }
    }
}
