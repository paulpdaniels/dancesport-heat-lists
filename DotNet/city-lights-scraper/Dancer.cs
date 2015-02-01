using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DancingDuck
{
    public class Dancer
    {

        public Dancer()
        {
            this.Events = new List<string>();
        }

        public string Name { get; set; }

        public string Uri { get; set; }

        public List<string> Events { get; private set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(String.Format("Name: {0}, Uri: {1}", Name, Uri));

            builder.AppendLine();

            foreach (var item in Events)
            {
                builder.AppendLine(item);
            }

            builder.AppendLine();

            return builder.ToString();
        }

    }
}
