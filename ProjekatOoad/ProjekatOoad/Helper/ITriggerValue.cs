using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatOoad.Helper
{
    //Moze i bez interface, ovako je lakse okupiti sve StateTriggere
    public interface ITriggerValue
    {
        bool IsActive { get; }
        event EventHandler IsActiveChanged;
    }
}
