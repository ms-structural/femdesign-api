// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.ModellingTools
{
    /// <summary>
    /// Represents a Cover Reference List.
    /// </summary>
    [System.Serializable]
    public partial class CoverReferenceList
    {
        /// <summary>
        /// Gets or sets the ref guid.
        /// </summary>
        [XmlElement("ref")]
        public List<GuidListType> RefGuid = new List<GuidListType>(); // guidtype

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CoverReferenceList()
        {
            
        }

        /// <summary>
        /// Constructor Cover Reference List
        /// </summary>
        /// <param name="guids">BarPart or SlabPart guid from objects (only bars and slabs supported) in list</param>
        public CoverReferenceList(List<Guid> guids)
        {
            foreach (var guid in guids)
            {
                this.RefGuid.Add(new GuidListType(guid));
            }
        }

        /// <summary>
        /// Get guid from objects (only bars and slabs supported) in list.
        /// </summary>
        /// <param name="objs">Bars and/or Slabs</param>
        internal static CoverReferenceList FromObjectList(List<object> objs)
        {
            if (objs == null)
            {
                return null;
            }

            if (objs.Count == 0)
            {
                return null;
            }

            CoverReferenceList refList = new CoverReferenceList();
            foreach (object elem in objs)
            {
                if (IsSameOrSubclass(typeof(Bars.Bar), elem.GetType()))
                {
                    refList.RefGuid.Add(new GuidListType(((Bars.Bar)elem).BarPart.Guid));
                }
                else if (IsSameOrSubclass(typeof(Shells.Slab), elem.GetType()))
                {
                    refList.RefGuid.Add(new GuidListType(((Shells.Slab)elem).SlabPart.Guid));
                }
                else
                {
                    throw new System.ArgumentException("Type of supporting structure is not supported. Can be bar or slab.");
                }
            }
            return refList;
        }

        internal static CoverReferenceList FromObjectList(List<FemDesign.GenericClasses.IStructureElement> objs)
        {
            if (objs == null)
            {
                return null;
            }

            if (objs.Count == 0)
            {
                return null;
            }

            CoverReferenceList refList = new CoverReferenceList();
            foreach (object elem in objs)
            {
                if (IsSameOrSubclass(typeof(Bars.Bar), elem.GetType()))
                {
                    refList.RefGuid.Add(new GuidListType(((Bars.Bar)elem).BarPart.Guid));
                }
                else if (IsSameOrSubclass(typeof(Shells.Slab), elem.GetType()))
                {
                    refList.RefGuid.Add(new GuidListType(((Shells.Slab)elem).SlabPart.Guid));
                }
                else
                {
                    throw new System.ArgumentException("Type of supporting structure is not supported. Can be bar or slab.");
                }
            }
            return refList;
        }

        private static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }

    }
}