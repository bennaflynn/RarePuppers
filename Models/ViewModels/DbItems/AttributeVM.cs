using RarePuppers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RarePuppers.Models.ViewModels.DbItems
{
    public class AttributeVM
    {
        public string name { get; set; }
        public decimal rarity { get; set; }
        public string image_src { get; set; }
        public int attribute_type_id { get; set; }

        public AttributeType attribute { get; set; }
    }
}
