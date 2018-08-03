using RarePuppers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RarePuppers.Services
{
    public class GenerateCat
    {
        ApplicationDbContext context;

        public GenerateCat(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Data.Attribute GetRandomAttribute(int attr_type)
        {
            var query = context.Attributes.Where(a => a.attribute_type_id == attr_type);

            decimal sum = 0;

            foreach(var q in query)
            {
                sum += q.rarity;
            }

            return context.Attributes.Where(a => a.attribute_id == 0).FirstOrDefault();
        }
    }
}
