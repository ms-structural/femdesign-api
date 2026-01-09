using FemDesign.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.GenericClasses
{
    /// <summary>
    /// Defines the I Shell interface.
    /// </summary>
    public interface IShell
    {
        void UpdateMaterial(Materials.Material material);
        Region Region { get; }
    }
}
