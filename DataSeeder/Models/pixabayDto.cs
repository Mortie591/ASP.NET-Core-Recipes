using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSeeder.Models
{
    public class pixabayDto
    {
        public pixabayDto()
        {
            
        }
        public int total { get; set; }
            public int totalHits { get; set; }
            public ImageDto[] hits { get; set; }

    }
}
