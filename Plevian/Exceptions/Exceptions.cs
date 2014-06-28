using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Exceptions
{
    public class AlreadyBuiltException : System.Exception { }
    public class NotBuildYetException : System.Exception { }
    public class NotEnoughResourcesException : System.Exception { }
}
