using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class Social:BaseModel
    {
        public string Icon { get; set; }
        public string Name { get; set; }
        public int? SettingId { get; set; }
        public Setting Setting { get; set; }
        public List<AuthorSocial> AuthorSocials { get; set; }
        [NotMapped]
        public IFormFile? file { get; set; }

    }
}
