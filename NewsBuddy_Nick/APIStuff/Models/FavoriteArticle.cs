using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsBuddy_Nick.APIStuff.Models
{
    public class FavoriteArticle
    {
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public string Description { get; set; } = "";
        public string Url { get; set; } = "";
    }
}
