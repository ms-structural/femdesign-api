using FemDesign.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.GenericClasses
{
    /// <summary>
    /// Defines the I Bar interface.
    /// </summary>
    public interface IBar
    {
        Edge Edge { get; }
    }
}