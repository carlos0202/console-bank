using Simple_Banking_System.Data;
using Simple_Banking_System.Interfaces;
using Simple_Banking_System.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Banking_System
{
    public class Program
    {
        public static void Main(string[] args)
        {

            IApplication application = new Application(Store.Instance);

            application.MainMenu();
        }
    }
}
