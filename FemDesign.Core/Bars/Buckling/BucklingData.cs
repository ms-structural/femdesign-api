// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Bars.Buckling
{
    /// <summary>
    /// Represents a Buckling Data.
    /// </summary>
    [System.Serializable]
    public partial class BucklingData
    {
        /// <summary>
        /// Gets or sets the buckling length.
        /// </summary>
        [XmlElement("buckling_length")]
        public List<BucklingLength> BucklingLength = new List<BucklingLength>();
        private BucklingData()
        {

        }
        /// <summary>
        /// Set BucklingData on a Concrete bar-element.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="bar">Bar. Concrete bar-element.</param>
        /// <param name="flexuralStiff">BucklingLength definition in Flexural Stiff direction.</param>
        /// <param name="flexuralWeak">BucklingLength definition in Flexural Weak direction.</param>
        /// <returns></returns>
        public static Bar SetOnConcreteBar(Bar bar, BucklingLength flexuralStiff, BucklingLength flexuralWeak)
        {
            bar = bar.DeepClone();
            // assert input
            if (bar.BarPart.ComplexMaterialObj.Concrete == null)
            {
                throw new System.ArgumentException("Material of bar element must be concrete!");
            }
            if (flexuralStiff.Type != BucklingType.FlexuralStiff)
            {
                throw new System.ArgumentException("flexuralStiff is not of type FlexuralStiff!");
            }
            if (flexuralWeak.Type != BucklingType.FlexuralWeak)
            {
                throw new System.ArgumentException("flexuralWeak is not of type FlexuralWeak!");
            }
            
            // add input
            BucklingData bucklingData = new BucklingData();
            bucklingData.BucklingLength.Add(flexuralStiff);
            bucklingData.BucklingLength.Add(flexuralWeak);
            bar.BarPart.BucklingData = bucklingData;

            return bar;
        }
        /// <summary>
        /// Set BucklingData on a Steel bar-element.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="bar">Bar. Steel bar-element.</param>
        /// <param name="flexuralStiff">BucklingLength definition in Flexural Stiff direction.</param>
        /// <param name="flexuralWeak">BucklingLength definition in Flexural Weak direction.</param>
        /// <param name="pressuredFlange">BucklingLength definition for Pressured Flange.</param>
        /// <param name="pressuredBottomFlange">BucklingLength definition for Pressured Bottom Flange.</param>
        /// <returns></returns>
        public static Bar SetOnSteelBar(Bar bar, BucklingLength flexuralStiff, BucklingLength flexuralWeak, BucklingLength pressuredFlange, BucklingLength pressuredBottomFlange)
        {
            bar = bar.DeepClone();
            // assert input
            if (bar.BarPart.ComplexMaterialObj.Steel == null)
            {
                throw new System.ArgumentException("Material of bar element must be steel!");
            }
            if (flexuralStiff.Type != BucklingType.FlexuralStiff)
            {
                throw new System.ArgumentException("flexuralStiff is not of type FlexuralStiff!");
            }
            if (flexuralWeak.Type != BucklingType.FlexuralWeak)
            {
                throw new System.ArgumentException("flexuralWeak is not of type FlexuralWeak!");
            }
            if (pressuredFlange.Type != BucklingType.PressuredTopFlange)
            {
                throw new System.ArgumentException("pressuredFlange is not of type PressuredFlange!");
            }
            if (pressuredBottomFlange.Type != BucklingType.PressuredBottomFlange)
            {
                throw new System.ArgumentException("pressuredBottomFlange is not of type PressuredBottomFlange!");
            }

            // add input
            BucklingData bucklingData = new BucklingData();
            bucklingData.BucklingLength.Add(flexuralStiff);
            bucklingData.BucklingLength.Add(flexuralWeak);
            bucklingData.BucklingLength.Add(pressuredFlange);
            bucklingData.BucklingLength.Add(pressuredBottomFlange);
            bar.BarPart.BucklingData = bucklingData;

            return bar;
        }
        /// <summary>
        /// Set BucklingData on a Timber bar-element.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="bar">Bar. Timber bar-element.</param>
        /// <param name="flexuralStiff">BucklingLength definition in Flexural Stiff direction.</param>
        /// <param name="flexuralWeak">BucklingLength definition in Flexural Weak direction.</param>
        /// <param name="lateralTorsional">BucklingLength definition for Lateral Torsional Buckling.</param>
        /// <returns></returns>
        public static Bar SetOnTimberBar(Bar bar, BucklingLength flexuralStiff, BucklingLength flexuralWeak, BucklingLength lateralTorsional)
        {
            bar = bar.DeepClone();
            // assert input
            if (bar.BarPart.ComplexMaterialObj.Timber == null)
            {
                throw new System.ArgumentException("Material of bar element must be timber!");
            }
            if (flexuralStiff.Type != BucklingType.FlexuralStiff)
            {
                throw new System.ArgumentException("flexuralStiff is not of type FlexuralStiff!");
            }
            if (flexuralWeak.Type != BucklingType.FlexuralWeak)
            {
                throw new System.ArgumentException("flexuralWeak is not of type FlexuralWeak!");
            }
            if (lateralTorsional.Type != BucklingType.LateralTorsional)
            {
                throw new System.ArgumentException("lateralTorsional is not of type LateralTorsional!");
            }

            // add input.
            BucklingData bucklingData = new BucklingData();
            bucklingData.BucklingLength.Add(flexuralStiff);
            bucklingData.BucklingLength.Add(flexuralWeak);
            bucklingData.BucklingLength.Add(lateralTorsional);
            bar.BarPart.BucklingData = bucklingData;

            return bar;
        }

        /// <summary>
        /// Set BucklingData on a concealed bar-element.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="concealedBar">HiddenBar. Concealed bar-element.</param>
        /// <param name="flexuralStiff">BucklingLength definition in Flexural Stiff direction.</param>
        /// <param name="flexuralWeak">BucklingLength definition in Flexural Weak direction.</param>
        /// <returns></returns>
        public static Reinforcement.ConcealedBar SetOnHiddenBar(Reinforcement.ConcealedBar concealedBar, BucklingLength flexuralStiff, BucklingLength flexuralWeak)
        {
            concealedBar = concealedBar.DeepClone();
            
            if (flexuralStiff.Type != BucklingType.FlexuralStiff)
            {
                throw new System.ArgumentException("flexuralStiff is not of type FlexuralStiff!");
            }
            if (flexuralWeak.Type != BucklingType.FlexuralWeak)
            {
                throw new System.ArgumentException("flexuralWeak is not of type FlexuralWeak!");
            }

            // add input
            BucklingData bucklingData = new BucklingData();
            bucklingData.BucklingLength.Add(flexuralStiff);
            bucklingData.BucklingLength.Add(flexuralWeak);
            concealedBar.BucklingData = bucklingData;

            return concealedBar;
        }
    }
}