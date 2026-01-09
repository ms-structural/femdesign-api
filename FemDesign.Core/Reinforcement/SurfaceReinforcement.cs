// https://strusoft.com/

using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;


namespace FemDesign.Reinforcement
{
    /// <summary>
    /// Represents a Surface Reinforcement.
    /// 
    /// Surface reinforcement
    /// </summary>
    [System.Serializable]
    public partial class SurfaceReinforcement: EntityBase
    {
        /// <summary>
        /// Gets or sets the base shell.
        /// </summary>
        [XmlElement("base_shell", Order=1)]
        public GuidListType BaseShell { get; set; } // guid_list_type // reference to slabPart of slab
        /// <summary>
        /// Gets or sets the surface reinforcement parameters guid.
        /// </summary>
        [XmlElement("surface_reinforcement_parameters", Order=2)]
        public FemDesign.GuidListType SurfaceReinforcementParametersGuid { get; set; } // guid_list_type
        /// <summary>
        /// Gets or sets the straight.
        /// </summary>
        [XmlElement("straight", Order=3)]
        public Straight Straight { get; set; } // choice
        /// <summary>
        /// Gets or sets the centric.
        /// </summary>
        [XmlElement("centric", Order=4)]
        public Centric Centric { get; set; } // next choice
        /// <summary>
        /// Gets or sets the wire.
        /// </summary>
        [XmlElement("wire", Order=5)] 
        public Wire Wire { get; set; } // rf_wire_type
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [XmlElement("region", Order=6)]
        public Geometry.Region Region { get; set; } // region_type

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private SurfaceReinforcement()
        {

        }

        /// <summary>
        /// Private constructor accessed by static methods.
        /// </summary>        
        private SurfaceReinforcement(Geometry.Region region, Straight straight, Centric centric, Wire wire)
        {
            // object information
            this.EntityCreated();

            // other properties
            this.Straight = straight;
            this.Centric = centric;
            this.Wire = wire;
            this.Region = region;
        }

        /// <summary>
        /// Create straight lay-out surface reinforcement.
        /// Internal static method used by GH components and Dynamo nodes.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="region">the region.</param>
        /// <param name="straight">the straight.</param>
        /// <param name="wire">the wire.</param>
        public static SurfaceReinforcement DefineStraightSurfaceReinforcement(Geometry.Region region, Straight straight, Wire wire)
        {
            // set straight (e.g. centric == null)
            Centric centric = null;

            // new surfaceReinforcement
            SurfaceReinforcement obj = new SurfaceReinforcement(region, straight, centric, wire);

            // return
            return obj;
        }

        /// <summary>
        /// Add SurfaceReinforcement to slab.
        /// Internal method use by GH components and Dynamo nodes.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="slab">the slab.</param>
        /// <param name="srfReinfs">the srf reinfs.</param>
        /// <param name="xDir">value for <paramref name="xDir"/>.</param>
        /// <param name="yDir">value for <paramref name="yDir"/>.</param>
        public static Shells.Slab AddReinforcementToSlab(Shells.Slab slab, List<SurfaceReinforcement> srfReinfs, Geometry.Vector3d xDir = null, Geometry.Vector3d yDir = null)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Shells.Slab clone = slab.DeepClone();

            // check if slab material is concrete
            if (clone.Material.Concrete == null)
            {
                throw new System.ArgumentException("Material of slab must be concrete");
            }

            // check if mixed layers
            if (SurfaceReinforcement.MixedLayers(srfReinfs))
            {
                throw new System.ArgumentException("Can't add top and bottom face reinforcement layout with mid face reinforcement layout to the same slab");
            }

            if (SurfaceReinforcement.AllStraight(srfReinfs))
            {
                return SurfaceReinforcement.AddStraightReinfToSlab(clone, srfReinfs, xDir, yDir);
            }
            else if (SurfaceReinforcement.AllCentric(srfReinfs))
            {
                return SurfaceReinforcement.AddCentricReinfToSlab(clone, srfReinfs);
            }
            else
            {
                throw new System.ArgumentException("Can't add mixed surface reinforcement layouts to the same slab.");
            }
        } 
        
        private static Shells.Slab AddStraightReinfToSlab(Shells.Slab slab, List<SurfaceReinforcement> srfReinfs, Geometry.Vector3d xDir = null, Geometry.Vector3d ydir = null)
        {
            // assert layout
            if (SurfaceReinforcement.AllStraight(srfReinfs))
            {
                
            }
            else
            {
                throw new System.ArgumentException("Not all passed surface reinforcement objects are of layout type straight");
            }

            // assert layers
            if (SurfaceReinforcement.MixedLayers(srfReinfs))
            {
                throw new System.ArgumentException("Can't add mixed layers to the same slab");
            }

            // single layer?
            var singleLayer = SurfaceReinforcement.AllSingleLayer(srfReinfs);

            // check if surface reinf parameters are set to slab
            SurfaceReinforcementParameters srfReinfParams;
            if (slab.SurfaceReinforcementParameters == null)
            {
                srfReinfParams = SurfaceReinforcementParameters.Straight(slab, singleLayer, xDir, ydir);
                slab.SurfaceReinforcementParameters = srfReinfParams;
            }

            // any surfaceReinforcementParameter set to slab will be overwritten
            // any surfaceReinforcement with option "centric" will be removed
            else if (slab.SurfaceReinforcementParameters.Center.PolarSystem == true)
            {
                srfReinfParams = SurfaceReinforcementParameters.Straight(slab, singleLayer, xDir, ydir);
                slab.SurfaceReinforcementParameters = srfReinfParams;

                foreach (SurfaceReinforcement item in slab.SurfaceReinforcement)
                {
                    if (item.Centric != null)
                    {
                        slab.SurfaceReinforcement.Remove(item);
                    }
                }
            }

            // use surface parameters already set to slab
            else
            {
                srfReinfParams = slab.SurfaceReinforcementParameters;
            }

            // add surface reinforcement
            GuidListType baseShell = new GuidListType(slab.SlabPart.Guid);
            FemDesign.GuidListType surfaceReinforcementParametersGuidReference = new FemDesign.GuidListType(slab.SurfaceReinforcementParameters.Guid);
            foreach (SurfaceReinforcement item in srfReinfs)
            {
                // add references to item
                item.BaseShell = baseShell;
                item.SurfaceReinforcementParametersGuid = surfaceReinforcementParametersGuidReference;

                // check if region item exists
                if (item.Region == null)
                {
                    item.Region = Geometry.Region.FromSlab(slab);
                }

                // add item to slab  
                slab.SurfaceReinforcement.Add(item);
            }

            // return
            return slab;
        }

        private static Shells.Slab AddCentricReinfToSlab(Shells.Slab slab, List<SurfaceReinforcement> srfReinfs)
        {
            if (SurfaceReinforcement.AllCentric(srfReinfs))
            {
                throw new System.ArgumentException("Method to add centric surface reinforcement is not implemented yet.");
            }
            else
            {
                throw new System.ArgumentException("Not all passed surface reinforcement objects are of layout type centric");
            }
        }


        /// <summary>
        /// Adds punching reinforcement.
        /// </summary>
        /// <param name="slab">the slab.</param>
        /// <param name="punchingReinforcements">the punching reinforcements.</param>
        /// <returns>The result.</returns>
        public static Shells.Slab AddPunchingReinforcement(Shells.Slab slab, List<PunchingReinforcement> punchingReinforcements)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Shells.Slab clone = slab.DeepClone();
            // deep clone punching reinforcements
            var punchingReinforcementsClone = punchingReinforcements.DeepClone();
            // check if slab material is concrete
            if (clone.Material.Concrete == null)
            {
                throw new System.ArgumentException("Material of slab must be concrete");
            }
            foreach (var item in punchingReinforcementsClone)
            {
                // add references to item
                item.BaseShell = new GuidListType(clone.SlabPart.Guid);
                item.PunchingArea.BaseShell = new GuidListType(clone.SlabPart.Guid);

                clone.PunchingReinforcement.Add(item);
            }
            return clone;
        }

        /// <summary>
        /// Adds shear control region to slab.
        /// </summary>
        /// <param name="slab">the slab.</param>
        /// <param name="shearControlRegions">the shear control regions.</param>
        /// <returns>The result.</returns>
        public static Shells.Slab AddShearControlRegionToSlab(Shells.Slab slab, List<ShearControlRegionType> shearControlRegions)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Shells.Slab clone = slab.DeepClone();

            // deep clone shear control regions
            var shearControlRegionsClone = shearControlRegions.DeepClone();

            // check if slab material is concrete
            if (clone.Material.Concrete == null)
            {
                throw new System.ArgumentException("Material of slab must be concrete");
            }

            foreach (var item in shearControlRegionsClone)
            {
                // add references to item
                item.BasePlate = clone.SlabPart.Guid;
                // add item to slab  
                clone.ShearControlRegions.Add(item);
            }

            return clone;
        }

        // check if surface reinforcement objects in list are mixed.
        private static bool MixedLayers(List<SurfaceReinforcement> srfReinfs)
        {
            return SurfaceReinforcement.AllMultiLayer(srfReinfs) == false && SurfaceReinforcement.AllSingleLayer(srfReinfs) == false;
        }
    
        // check if all surface reinforcement objects in list are multilayer
        private static bool AllMultiLayer(List<SurfaceReinforcement> srfReinfs)
        {
            var items = srfReinfs.Where(x => x.Straight != null).Where(x => x.Straight.MultiLayer == true).ToList();
            items.AddRange(srfReinfs.Where(x => x.Centric != null).Where(x => x.Centric.MultiLayer == true));
            return (items.Count() == srfReinfs.Count());
        }

        // check if all surface reinforcement objects in list are singlelayer
        private static bool AllSingleLayer(List<SurfaceReinforcement> srfReinfs)
        {
            var items = srfReinfs.Where(x => x.Straight != null).Where(x => x.Straight.SingleLayer == true).ToList();
            items.AddRange(srfReinfs.Where(x => x.Centric != null).Where(x => x.Centric.SingleLayer == true));
            return (items.Count() == srfReinfs.Count());
        }

        private static bool MixedLayout(List<SurfaceReinforcement> srfReinfs)
        {
            return srfReinfs.Any(x => x.Straight != null) && srfReinfs.Any(x => x.Centric != null);
        }

        // check if all surface reinforcement objects in list are of layout type straight
        private static bool AllStraight(List<SurfaceReinforcement> srfReinfs)
        {
            var items = srfReinfs.Where(x => x.Straight != null);
            return (items.Count() == srfReinfs.Count());
        }
        
        // check if all surface reinforcement objects in list are of layout type centric
        private static bool AllCentric(List<SurfaceReinforcement> srfReinfs)
        {
            var items = srfReinfs.Where(x => x.Centric != null);
            return (items.Count() == srfReinfs.Count());
        }
    }
}