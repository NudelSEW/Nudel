using Nudel.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nudel.BusinessObjects
{
    public class Comment
    {
        public string Text { get; set; }
        public User Creator { get; set; }
        public DateTime Time { get; set; }
    }
}
