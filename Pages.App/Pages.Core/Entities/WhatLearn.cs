using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class WhatLearn:BaseModel
    {
        public string Text { get; set; }
        public int SettingId { get; set; }
        public Setting? Setting { get; set; }
    }
}
 