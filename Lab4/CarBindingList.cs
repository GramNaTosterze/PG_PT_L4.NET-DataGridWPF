using C__Lab3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab4
{
    class CarBindingList : BindingList<Car>
    {
        static private Dictionary<string, bool> sortDirection = new Dictionary<string, bool>()
        {
            { "model", false },
            { "motor", false },
            { "year", false }
        };
        private CarBindingList moddedList = null;
        public CarBindingList(List<Car> list) : base(list) { }
        public CarBindingList() : base() { }

        protected override bool SupportsSearchingCore { get { return true; } }
        protected override bool SupportsSortingCore { get { return true; } }
        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            if (property.PropertyType.GetInterface("IComparable") == null)
                return;

            List<Car> itemsList = (List<Car>)this.Items;

            itemsList.Sort(new Comparison<Car>(delegate (Car x, Car y)
            {
                // Compare x to y if x is not null. If x is, but y isn't, we compare y
                // to x and reverse the result. If both are null, they're equal.
                if (property.GetValue(x) != null)
                    return ((IComparable)property.GetValue(x)).CompareTo(property.GetValue(y)) * (direction == ListSortDirection.Descending ? -1 : 1);
                else if (property.GetValue(y) != null)
                    return ((IComparable)property.GetValue(y)).CompareTo(property.GetValue(x)) * (direction == ListSortDirection.Descending ? 1 : -1);
                else
                    return 0;
            }));

        }
        public void Sort(string colName)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Car));
            ApplySortCore(properties.Find(colName, true), (sortDirection[colName] ? ListSortDirection.Descending : ListSortDirection.Ascending));
            sortDirection[colName] = !sortDirection[colName];
        }
        protected override int FindCore(PropertyDescriptor prop, object key)
        {
            moddedList = new CarBindingList();
            PropertyInfo propInfo = typeof(Car).GetProperty(prop.Name);
            int found = -1;
            if (key == null)
                return found;

            for (int i = 0; i < Count; i++)
            {
                if (propInfo.GetValue(Items[i], null).Equals(key))
                {
                    moddedList.Add(Items[i]);
                    found++;
                }
                if (propInfo.Name == "motor")
                {
                    if (Items[i].motor.model.Equals(key) || Items[i].motor.displacement.ToString().Equals(key) || Items[i].motor.horsePower.ToString().Equals(key))
                    {
                        moddedList.Add(Items[i]);
                        found++;
                    }
                }
            }
            return found;
        }
        public CarBindingList Find(string str, int category)
        {
            List<Car> list = this.Items as List<Car>;
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(list[0]);
            switch (category)
            {
                case 0:
                    FindCore(properties.Find("model", false), str);
                    break;
                case 1:
                    FindCore(properties.Find("year", false), Int32.Parse(str));
                    break;
                case 2:
                    FindCore(properties.Find("motor", false), str);
                    break;
                default:
                    break;
            }

            return moddedList;                
        }
    }
}
