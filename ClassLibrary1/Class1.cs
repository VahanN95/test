using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1
    {
        public  string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }

        }

        public void F(string s)
        {
            Console.WriteLine(s);
        }
    }
}
