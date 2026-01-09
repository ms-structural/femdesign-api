using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

#if ISDYNAMO
using Autodesk.DesignScript.Runtime;
#endif

namespace FemDesign.Calculate
{
    /// <summary>
    /// Defines the List Proc enumeration.
    /// </summary>
    [System.Serializable]
#if ISDYNAMO
	[IsVisibleInDynamoLibrary(false)]
#endif
    public enum ListProc
    {
        #region STRUCTURE
        /* STRUCTURE */
        /// <summary>
        /// Section properties
        /// </summary>
        [XmlEnum("StrucSection_ListProc")]
        SectionProperties,
        #endregion

        #region FINITE ELEMENTS
        /* FINITE ELEMENTS */
        /// <summary>
        /// Fea Nodes
        /// </summary>
        [XmlEnum("femNode_ListProc")]
        FemNode,

        /// <summary>
        /// Fea Bars
        /// </summary>
        [XmlEnum("stBarElem_ListProc")]
        FemBar,

        /// <summary>
        /// Fea Shell
        /// </summary>
        [XmlEnum("stShellElem_ListProc")]
        FemShell,

        /// <summary>
        /// Point Connection Element
        /// </summary>
        [XmlEnum("stConnPtElem_ListProc")]
        FemPointConnection,

        /// <summary>
        /// Line Connection Element
        /// </summary>
        [XmlEnum("stConnLnElem_ListProc")]
        FemLineConnection,

        /// <summary>
        /// Point support Element
        /// </summary>
        [XmlEnum("stSuppPtElem_ListProc")]
        FemPointSupport,

        /// <summary>
        /// Line support Element
        /// </summary>
        [XmlEnum("stSuppLnElem_ListProc")]
        FemLineSupport,



        #endregion

        #region QUANTITY ESTIMATION
        /* QUANTITY ESTIMATION */
        /// <summary>
        /// Quantity estimation, Concrete
        /// </summary>
        [XmlEnum("ccQuantityConc_ListProc")]
        QuantityEstimationConcrete,
        /// <summary>
        /// Quantity estimation, Reinforcement
        /// </summary>
        [XmlEnum("ccQuantityReinf_ListProc")]
        QuantityEstimationReinforcement,
        /// <summary>
        /// Quantity estimation, Steel
        /// </summary>
        [XmlEnum("ccQuantitySteel_ListProc")]
        QuantityEstimationSteel,
        /// <summary>
        /// Quantity estimation, Timber
        /// </summary>
        [XmlEnum("ccQuantityTimber_ListProc")]
        QuantityEstimationTimber,
        /// <summary>
        /// Quantity estimation, Timber panel
        /// </summary>
        [XmlEnum("ccQuantityTmPanel_ListProc")]
        QuantityEstimationTimberPanel,
        /// <summary>
        /// Quantity estimation, Masonry
        /// </summary>
        [XmlEnum("ccQuantityMasonry_ListProc")]
        QuantityEstimationMasonry,
        /// <summary>
        /// Quantity estimation, General
        /// </summary>
        [XmlEnum("ccQuantityGeneral_ListProc")]
        QuantityEstimationGeneral,
        /// <summary>
        /// Quantity estimation, Profiled panel
        /// </summary>
        [XmlEnum("ccQuantityProfiledPanel_ListProc")]
        QuantityEstimationProfiledPanel,
        #endregion

        #region LOADCASES
        /* LOAD CASES */
        /// <summary>
        /// Load case: Nodal displacements
        /// </summary>
        [XmlEnum("frCaseDispNodal_ListProc")]
        NodalDisplacementsLoadCase,
        /// <summary>
        /// Load case: Bars, internal forces
        /// </summary>
        [XmlEnum("frCaseIntfBar_ListProc")]
        BarsInternalForcesLoadCase,
        /// <summary>
        /// Load case: Bars, stresses
        /// </summary>
        [XmlEnum("frCaseStrsBar_ListProc")]
        BarsStressesLoadCase,
        /// <summary>
        /// Load case: Bars, End Displacement
        /// </summary>
        [XmlEnum("frCaseDispBar_ListProc")]
        BarsDisplacementsLoadCase,
        /// <summary>
        /// Load case: Labelled sections, internal forces
        /// </summary>
        [XmlEnum("frCaseIntfResSection_ListProc")]
        LabelledSectionsInternalForcesLoadCase,
        /// <summary>
        /// Load case: Labelled sections, Resultants
        /// </summary>
        [XmlEnum("frCaseResResSection_ListProc")]
        LabelledSectionsResultantsLoadCase,
        /// <summary>
        /// Load case: Point connection forces
        /// </summary>
        [XmlEnum("frCaseConnPt_ListProc")]
        PointConnectionForceLoadCase,
        /// <summary>
        /// Load case: Line connection forces
        /// </summary>
        [XmlEnum("frCaseConnLn_ListProc")]
        LineConnectionForceLoadCase,
        /// <summary>
        /// Load case: Line connection  resultants
        /// </summary>
        [XmlEnum("frCaseConnLnRes_ListProc")]
        LineConnectionResultantLoadCase,
        /// <summary>
        /// Load case: Point support group, Reactions
        /// </summary>
        [XmlEnum("frCaseReacPtGroup_ListProc")]
        PointSupportReactionsLoadCase,
        /// <summary>
        /// Load case: Line support group, Reactions
        /// </summary>
        [XmlEnum("frCaseReacLnGroup_ListProc")]
        LineSupportReactionsLoadCase,
        /// <summary>
        /// Load case: Line support group, Resultants
        /// </summary>
        [XmlEnum("frCaseReacLnGroupRes_ListProc")]
        LineSupportResultantsLoadCase,
        /// <summary>
        /// Load case: Surface support, Reactions
        /// </summary>
        [XmlEnum("frCaseReacSf_ListProc")]
        SurfaceSupportReactionsLoadCase,
        /// <summary>
        /// Load case: Surface support, Resultants
        /// </summary>
        [XmlEnum("frCaseReacSfRes_ListProc")]
        SurfaceSupportResultantsLoadCase,
        /// <summary>
        /// Load case: Shells, Displacements
        /// </summary>
        [XmlEnum("frCaseDispShell_ListProc")]
        ShellDisplacementLoadCase,
        /// <summary>
        /// Load case: Shells, Displacements (Extract)
        /// </summary>
        [XmlEnum("frCaseDispShellExtract_ListProc")]
        ShellDisplacementExtractLoadCase,
        /// <summary>
        /// Load case: Shells, Internal Forces
        /// </summary>
        [XmlEnum("frCaseIntfShell_ListProc")]
        ShellInternalForceLoadCase,
        /// <summary>
        /// Load case: Shells, Internal Forces (Extract)
        /// </summary>
        [XmlEnum("frCaseIntfShellExtract_ListProc")]
        ShellInternalForceExtractLoadCase,
        /// <summary>
        /// Load case: Shells, Derived Forces
        /// </summary>
        [XmlEnum("frCaseIntfDerShell_ListProc")]
        ShellDerivedForceLoadCase,
        /// <summary>
        /// Load case: Shells, Derived Forces (Extract)
        /// </summary>
        [XmlEnum("frCaseIntfDerShellExtract_ListProc")]
        ShellDerivedForceExtractLoadCase,
        /// <summary>
        /// Load case: Shells, Stresses Top Side
        /// </summary>
        [XmlEnum("frCaseStrsTopShell_ListProc")]
        ShellStressesTopLoadCase,
        /// <summary>
        /// Load case: Shells, Stresses Top Side (Extract)
        /// </summary>
        [XmlEnum("frCaseStrsTopShellExtract_ListProc")]
        ShellStressesTopExtractLoadCase,
        /// <summary>
        /// Load case: Shells, Stresses Mid Axis
        /// </summary>
        [XmlEnum("frCaseStrsMembShell_ListProc")]
        ShellStressesMembraneLoadCase,
        /// <summary>
        /// Load case: Shells, Stresses Mid Axis (Extract)
        /// </summary>
        [XmlEnum("frCaseStrsMembShellExtract_ListProc")]
        ShellStressesMembraneExtractLoadCase,
        /// <summary>
        /// Load case: Shells, Stresses Bottom Side
        /// </summary>
        [XmlEnum("frCaseStrsBotShell_ListProc")]
        ShellStressesBottomLoadCase,
        /// <summary>
        /// Load case: Shells, Stresses Bottom Side (Extract)
        /// </summary>
        [XmlEnum("frCaseStrsBotShellExtract_ListProc")]
        ShellStressesBottomExtractLoadCase,
        /// <summary>
        /// Load case: Equilibrium
        /// </summary>
        [XmlEnum("frCaseEqui_ListProc")]
        EquilibriumLoadCase,
        #endregion

        #region LOAD COMBINATIONS
        /* LOAD COMBINATIONS */
        /// <summary>
        /// Load combination: Nodal displacements
        /// </summary>
        [XmlEnum("frCombDispNodal_ListProc")]
        NodalDisplacementsLoadCombination,
        /// <summary>
        /// Load combination: Bars, internal forces
        /// </summary>
        [XmlEnum("frCombIntfBar_ListProc")]
        BarsInternalForcesLoadCombination,
        /// <summary>
        /// Load combination: Shells, Derived Forces
        /// </summary>
        [XmlEnum("frCombIntfDerShell_ListProc")]
        ShellDerivedForceLoadCombination,
        /// <summary>
        /// Load combination: Shells, Derived Forces (Extract)
        /// </summary>
        [XmlEnum("frCombIntfDerShellExtract_ListProc")]
        ShellDerivedForceExtractLoadCombination,
        /// <summary>
        /// Load combination: Bars, Stresses
        /// </summary>
        [XmlEnum("frCombStrsBar_ListProc")]
        BarsStressesLoadCombination,
        /// <summary>
        /// Load combination: Bars, End Displacement
        /// </summary>
        [XmlEnum("frCombDispBar_ListProc")]
        BarsDisplacementsLoadCombination,
        /// <summary>
        /// Load combination: Bars, End forces
        /// </summary>
        [XmlEnum("frCombIntfBarEnd_ListProc")]
        BarsEndForcesLoadCombination,
        /// <summary>
        /// Load combination: Labelled sections, internal forces
        /// </summary>
        [XmlEnum("frCombIntfResSection_ListProc")]
        LabelledSectionsInternalForcesLoadCombination,
        /// <summary>
        /// Load combination: Labelled sections, Resultants
        /// </summary>
        [XmlEnum("frCombResResSection")]
        LabelledSectionsResultantsLoadCombination,
        /// <summary>
        /// Load combination: Point support group, Reactions
        /// </summary>
        [XmlEnum("frCombReacPtGroup_ListProc")]
        PointSupportReactionsLoadCombination,
        /// <summary>
        /// Load combination: Point connection forces
        /// </summary>
        [XmlEnum("frCombConnPt_ListProc")]
        PointConnectionForceLoadCombination,
        /// <summary>
        /// Load combination: Line connection forces
        /// </summary>
        [XmlEnum("frCombConnLn_ListProc")]
        LineConnectionForceLoadCombination,
        /// <summary>
        /// Load combination: Line connection resultants
        /// </summary>
        [XmlEnum("frCombConnLnRes_ListProc")]
        LineConnectionResultantLoadCombination,
        /// <summary>
        /// Load combination: Line support group, Reactions
        /// </summary>
        [XmlEnum("frCombReacLnGroup_ListProc")]
        LineSupportReactionsLoadCombination,
        /// <summary>
        /// Load combination: Line support group, Resultants
        /// </summary>
        [XmlEnum("frCombReacLnGroupRes_ListProc")]
        LineSupportResultantsLoadCombination,
        /// <summary>
        /// Load combination: Surface support, Reactions
        /// </summary>
        [XmlEnum("frCombReacSf_ListProc")]
        SurfaceSupportReactionsLoadCombination,
        /// <summary>
        /// Load combination: Surface support, Resultants
        /// </summary>
        [XmlEnum("frCombReacSfRes_ListProc")]
        SurfaceSupportResultantsLoadCombination,
        /// <summary>
        /// Load combination: Shells, Displacements
        /// </summary>
        [XmlEnum("frCombDispShell_ListProc")]
        ShellDisplacementLoadCombination,
        /// <summary>
        /// Load combination: Shells, Displacements (Extract)
        /// </summary>
        [XmlEnum("frCombDispShellExtract_ListProc")]
        ShellDisplacementExtractLoadCombination,
        /// <summary>
        /// Load combination: Shells, Internal Forces
        /// </summary>
        [XmlEnum("frCombIntfShell_ListProc")]
        ShellInternalForceLoadCombination,
        /// <summary>
        /// Load combination: Shells, Internal Forces (Extract)
        /// </summary>
        [XmlEnum("frCombIntfShellExtract_ListProc")]
        ShellInternalForceExtractLoadCombination,

        /// <summary>
        /// Load combination: Shells, Stresses Top Side
        /// </summary>
        [XmlEnum("frCombStrsTopShell_ListProc")]
        ShellStressesTopLoadCombination,
        /// <summary>
        /// Load combination: Shells, Stresses Top Side (Extract)
        /// </summary>
        [XmlEnum("frCombStrsTopShellExtract_ListProc")]
        ShellStressesTopExtractLoadCombination,
        /// <summary>
        /// Load combination: Shells, Stresses Mid Axis
        /// </summary>
        [XmlEnum("frCombStrsMembShell_ListProc")]
        ShellStressesMembraneLoadCombination,
        /// <summary>
        /// Load combination: Shells, Stresses Mid Axis (Extract)
        /// </summary>
        [XmlEnum("frCombStrsMembShellExtract_ListProc")]
        ShellStressesMembraneExtractLoadCombination,
        /// <summary>
        /// Load combination: Shells, Stresses Bottom Side
        /// </summary>
        [XmlEnum("frCombStrsBotShell_ListProc")]
        ShellStressesBottomLoadCombination,
        /// <summary>
        /// Load combination: Shells, Stresses Bottom Side (Extract)
        /// </summary>
        [XmlEnum("frCombStrsBotShellExtract_ListProc")]
        ShellStressesBottomExtractLoadCombination,
        /// <summary>
        /// Load combination: Equilibrium
        /// </summary>
        [XmlEnum("frCombEqui_ListProc")]
        EquilibriumLoadCombination,
        #endregion

        #region MAX OF LOAD COMBINATION
        /* MAX OF LOAD COMBINATION */
        #region NODE
        [XmlEnum("frCmaxDispNodalMax_ListProc")]
        NodalDisplacementMaxOfLoadCombinationMax,

        [XmlEnum("frCmaxDispNodalMin_ListProc")]
        NodalDisplacementMaxOfLoadCombinationMin,

        [XmlEnum("frCmaxReacPtGroupMax_ListProc")]
        PointSupportReactionsMaxOfLoadCombinationMax,

        [XmlEnum("frCmaxReacPtGroupMin_ListProc")]
        PointSupportReactionsMaxOfLoadCombinationMin,

        [XmlEnum("frCmaxReacPtGroupExtract_ListProc")]
        PointSupportReactionsMaxOfLoadCombinationMinMax,

        #endregion

        #region SHELL
        [XmlEnum("frCmaxDispShellMax_ListProc")]
        ShellDisplacementMaxOfLoadCombinationMax,

        [XmlEnum("frCmaxDispShellMin_ListProc")]
        ShellDisplacementMaxOfLoadCombinationMin,

        [XmlEnum("frCmaxIntfShellMax_ListProc")]
        ShellInternalForceMaxOfLoadCombinationMax,

        [XmlEnum("frCmaxIntfShellMin_ListProc")]
        ShellInternalForceMaxOfLoadCombinationMin,

        [XmlEnum("frCmaxStrsTopShellMax_ListProc")]
        ShellStressesTopMaxOfLoadCombinationMax,

        [XmlEnum("frCmaxStrsTopShellMin_ListProc")]
        ShellStressesTopMaxOfLoadCombinationMin,

        [XmlEnum("frCmaxStrsMembraneShellMax_ListProc")]
        ShellStressesMembraneMaxOfLoadCombinationMax,

        [XmlEnum("frCmaxStrsMembraneShellMin_ListProc")]
        ShellStressesMembraneMaxOfLoadCombinationMin,

        [XmlEnum("frCmaxStrsBottomShellMax_ListProc")]
        ShellStressesBottomMaxOfLoadCombinationMax,

        [XmlEnum("frCmaxStrsBottomShellMin_ListProc")]
        ShellStressesBottomMaxOfLoadCombinationMin,

        [XmlEnum("frCmaxIntfDerShellMax_ListProc")]
        ShellDerivedForceMaxOfLoadCombinationMax,

        [XmlEnum("frCmaxIntfDerShellMin_ListProc")]
        ShellDerivedForceMaxOfLoadCombinationMin,
        #endregion
        #endregion

        #region STABILITY
        /* STABILITY */
        /// <summary>
        /// Critical Parameters (only positive)
        /// </summary>
        [XmlEnum("frStabCritParamOnlyPositive_ListProc")]
        CriticalParameters,

        /// <summary>
        /// Nodal Buckling Shapes: Structural Model
        /// </summary>
        [XmlEnum("frStabDispNodal_ListProc")]
        NodalBucklingShape,
        #endregion

        #region IMPERFECTIONS
        /*IMPERFECTIONS*/
        /// <summary>
        /// Imperfection Factors (only positive)
        /// </summary>
        [XmlEnum("frImpfFactorsOnlyPositive_ListProc")]
        ImperfectionFactors,
        #endregion

        #region EIGEN FREQUENCIES
        /* EIGEN FREQUENCIES */
        /// <summary>
        /// Eigen Frequencies: Structural Model
        /// </summary>
        [XmlEnum("frFreqEigen_ListProc")]
        EigenFrequencies,

        /// <summary>
        /// Eigen Frequencies: Structural Model
        /// </summary>
        [XmlEnum("frFreqDispNodal_ListProc")]
        NodalVibrationShape,
        #endregion

        #region FOOTFALL
        /* FOOTFALL */
        /// <summary>
        /// Eigen Frequencies: FootFall
        /// </summary>
        [XmlEnum("frFootEigen_ListProc")]
        EigenFrequenciesFootFall,

        /// <summary>
        /// Nodal Response Factor: FootFall
        /// </summary>
        [XmlEnum("frFootRespNodal_ListProc")]
        NodalResponseFactor,

        /// <summary>
        /// Nodal Acceleration: FootFall
        /// </summary>
        [XmlEnum("frFootAccNodal_ListProc")]
        NodalAcceleration,

        #endregion

        #region STEEL DESIGN
        /// <summary>
        /// Steel design: Bar, Utilisation
        /// </summary>
        [XmlEnum("StUtilizationComb_ListProc")]
        SteelDesignBarUtilizationLoadCombination,
        #endregion

        #region RC DESIGN
        /* RC design */
        /// <summary>
        /// RC design: Shell, Crack width
        /// </summary>
        [XmlEnum("RCShellCrackWidthCmax_ListProc")]
        RCDesignShellCrackWidthMaxComb,

        /// <summary>
        /// RC design: Shell, Utilization.
        /// </summary>
        [XmlEnum("RCShellUtilizationComb_ListProc")]
        RCDesignShellUtilizationLoadCombination,

        /// <summary>
        /// RC design: Shell, Utilization
        /// </summary>
        [XmlEnum("RCShellUtilizationCmax_ListProc")]
        RCDesignShellUtilizationMaxComb,

        /// <summary>
        /// RC design: Shell, Utilization
        /// </summary>
        [XmlEnum("RCShellUtilizationGmax_ListProc")]
        RCDesignShellUtilizationMaxLoadGroup,

        /// <summary>
        /// RC design: Shell, Crack width
        /// </summary>
        [XmlEnum("RCShellCrackWidthComb_ListProc")]
        RCDesignShellCrackWidthLoadCombination,


        /// <summary>
        /// RC design: Shell, Design Forces.
        /// </summary>
        [XmlEnum("RCShellDesForcesComb_ListProc")]
        RCDesignShellDesignForcesLoadCombination,

        /// <summary>
        /// RC design: Shell, Required Reinforcement.
        /// </summary>
        [XmlEnum("RCShellReqReinfComb_ListProc")]
        RCDesignShellRequiredReinforcementLoadCombination,

        /// <summary>
        /// RC design: Shell, Shear Utilization, load combination.
        /// </summary>
        [XmlEnum("RCShellShearUtilizationComb_ListProc")]
        RCDesignShellShearUtilizationLoadCombination,

        /// <summary>
        /// RC design: Shell, Shear Utilization, max of combinations.
        /// </summary>
        [XmlEnum("RCShellShearUtilizationCmax_ListProc")]
        RCDesignShellShearUtilizationMaxComb,

        /// <summary>
        /// RC design: Shell, Shear Utilization, max of load groups.
        /// </summary>
        [XmlEnum("RCShellShearUtilizationGmax_ListProc")]
        RCDesignShellShearUtilizationMaxLoadGroup,

        /// <summary>
        /// RC design: Shell, Shear Capacity, load combination.
        /// </summary>
        [XmlEnum("RCShellShearCapacityComb_ListProc")]
        RCDesignShellShearCapacityLoadCombination,

        /// <summary>
        /// RC design: Shell, Shear Capacity, max of combinations.
        /// </summary>
        [XmlEnum("RCShellShearCapacityCmax_ListProc")]
        RCDesignShellShearCapacityMaxComb,

        /// <summary>
        /// RC design: Shell, Shear Capacity, max of load groups.
        /// </summary>
        [XmlEnum("RCShellShCapGmax_ListProc")]
        RCDesignShellShearCapacityMaxLoadGroup,

        /// <summary>
        /// RC design: Bar, Utilization.
        /// </summary>
        [XmlEnum("RCBarUtilizationComb_ListProc")]
        RCDesignBarUtilizationLoadCombination,

        /// <summary>
        /// RC design: Bar, Utilization, max of combinations.
        /// </summary>
        [XmlEnum("RCBarUtilizationCmax_ListProc")]
        RCDesignBarUtilizationMaxComb,

        /// <summary>
        /// RC design: Bar, Utilization, max of load groups.
        /// </summary>
        [XmlEnum("RCBarUtilizationGmax_ListProc")]
        RCDesignBarUtilizationMaxLoadGroup,

        #endregion

        #region TIMBER

        #region BEAM

        /// <summary>
        /// Timber: Utilization - Load Combination
        /// </summary>
        [XmlEnum("TmbUtilizationComb_ListProc")]
        TimberBarUtilizationLoadCombination,

        /// <summary>
        /// Timber: Utilization - max of combinations.
        /// </summary>
        [XmlEnum("TmbUtilizationCmax_ListProc")]
        TimberBarUtilizationMaxComb,

        /// <summary>
        /// Timber: Utilization - max of load groups.
        /// </summary>
        [XmlEnum("TmbUtilizationGmax_ListProc")]
        TimberBarUtilizationMaxLoadGroup,


        #endregion

        #region  CLT DESIGN
        /* CLT Design */
        /// <summary>
        /// CLT Panel: Utilization - Load Combination
        /// </summary>
        [XmlEnum("CltPanelUtilizationComb_ListProc")]
        CLTPanelUtilizationLoadCombination,
        /// <summary>
        /// CLT Panel: Utilization - Max of Load Combination
        /// </summary>
        [XmlEnum("CltPanelUtilizationCmax_ListProc")]
        CLTPanelUtilizationMaxComb,

        /// <summary>
        /// CLT Panel: Fire Utilization - Load Combination
        /// </summary>
        [XmlEnum("CltFireUtilizationComb_ListProc")]
        CLTPanelFireUtilizationLoadCombination,

        /// <summary>
        /// CLT Panel: Fire Utilization - Max of Load Combination
        /// </summary>
        [XmlEnum("CltFireUtilizationCmax_ListProc")]
        CLTPanelFireUtilizationMaxComb,

        /// <summary>
        /// Orthotropic Panel: Utilization - Load Combination
        /// </summary>
        [XmlEnum("TmPanelUtilizationComb_ListProc")]
        OrthotropicPanelUtilizationLoadCombination,

        /// <summary>
        /// Orthotropic Panel: Utilization - Max of Load Combination
        /// </summary>
        [XmlEnum("TmPanelUtilizationCmax_ListProc")]
        OrthotropicPanelUtilizationMaxComb,
        #endregion

        #endregion
    }

    public static class ListProcExtension
    {
        /// <summary>
        /// Determines whether load case.
        /// </summary>
        /// <param name="listProc">the list proc.</param>
        /// <returns>The result.</returns>
        public static bool IsLoadCase(this ListProc listProc)
        {
            {
                string r = listProc.ToString();
                if (r.EndsWith("LoadCase"))
                    return true;
                else return false;
            }
        }

        /// <summary>
        /// Determines whether load combination.
        /// </summary>
        /// <param name="listProc">the list proc.</param>
        /// <returns>The result.</returns>
        public static bool IsLoadCombination(this ListProc listProc)
        {
            {
                string r = listProc.ToString();
                if (r.EndsWith("LoadCombination"))
                    return true;
                else if(r == ListProc.NodalBucklingShape.ToString() || r == ListProc.ImperfectionFactors.ToString() || r == ListProc.CriticalParameters.ToString())
                    return true;
                else return false;
            }
        }

        /// <summary>
        /// Determines whether quantity estimation.
        /// </summary>
        /// <param name="listProc">the list proc.</param>
        /// <returns>The result.</returns>
        public static bool IsQuantityEstimation(this ListProc listProc)
        {
            {
                string r = listProc.ToString();
                if (r.StartsWith("QuantityEstimation"))
                    return true;
                else return false;
            }
        }

        /// <summary>
        /// Determines whether max comb.
        /// </summary>
        /// <param name="listProc">the list proc.</param>
        /// <returns>The result.</returns>
        public static bool IsMaxComb(this ListProc listProc)
        {
            {
                string r = listProc.ToString();
                if (r.EndsWith("MaxComb"))
                    return true;
                else return false;
            }
        }

        /// <summary>
        /// Determines whether max load group.
        /// </summary>
        /// <param name="listProc">the list proc.</param>
        /// <returns>The result.</returns>
        public static bool IsMaxLoadGroup(this ListProc listProc)
        {
            {
                string r = listProc.ToString();
                if (r.EndsWith("MaxLoadGroup"))
                    return true;
                else return false;
            }
        }

    }
}