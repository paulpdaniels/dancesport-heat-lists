using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DancingDuck
{
    public class Event
    {
        public IList<Dancer> Dancers { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(String.Format("Name: {0}", Name));

            builder.AppendLine();

            foreach (var item in Dancers)
            {
                builder.AppendLine(item.Name);
            }

            builder.AppendLine();

            return builder.ToString();
        }

    }
}
