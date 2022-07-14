using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeonStream.Models
{
    public class Armlet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LinkedSlots { get; set; }
        public int SingleSlots { get; set; }
        public int Growth { get; set; }
    }
}
