using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace C__Lab3
{
    [XmlRoot(ElementName = "engine")]
    public class Engine : IComparable
    {
        public double displacement { get; set; }
        public double horsePower { get; set; }
        [XmlAttribute]
        public string model { get; set; }
        public Engine(double displacement, double horsePower, string model)
        {
            this.displacement = displacement;
            this.horsePower = horsePower;
            this.model = model;
        }
        public Engine() { }

        public int CompareTo(Object obj)
        {
            if (obj == null)
                return 1;
            Engine en = (Engine)obj;
            return this.horsePower.CompareTo(en.horsePower);
        }
        public override string ToString()
        {
            return model + " " + displacement + " (" + horsePower + " hp)";
        }
    }
}
