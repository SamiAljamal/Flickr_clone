using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flickr_Clone.Models
{
    [Table("Images")]
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Path { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set;}
        public virtual ICollection<ImageTag> ImageTags { get; set; }
    }
}
