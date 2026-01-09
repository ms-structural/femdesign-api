using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Materials
{
    /// <summary>
    /// Defines the I Material interface.
    /// </summary>
    public interface IMaterial
    {
        string Name { get; set; }
        Guid Guid { get; set; }

    }
}
