using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using Malison.Core;

namespace Malison.ExampleSFML
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            SFMLApp app = new SFMLApp();

            app.Run();
        }
    }
}
