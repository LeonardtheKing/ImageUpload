using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    
    public class Photo
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        [ForeignKey("AppUser")]
        public Guid AppUserId { get; set; }
        public AppUser AppUser{get;set;}
    }
}