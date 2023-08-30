using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class Setting:BaseModel
    {
        public string Address { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Logo { get; set; }

        public string CopyText { get; set; }
        public string CopyImage { get; set; }

        public string WhatLearImage { get; set; }
        public List<WhatLearn> whatLearns { get; set; }

        public List<Social>? Socials { get; set; }
        [NotMapped]
        public IFormFile? file { get; set; }

    }
}
