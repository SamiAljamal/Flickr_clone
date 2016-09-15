using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flickr_Clone.Models
{
    [Table("ImageTags")]
    public class ImageTag
    {
        [Key]
        public int ImageTagId { get; set; }

        [ForeignKey("Tag")]
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
        [ForeignKey("Image")]
        public int ImageId { get; set; }
        public virtual Image Image { get; set; }
    }
}
