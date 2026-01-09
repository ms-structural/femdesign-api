// https://strusoft.com/


namespace FemDesign.Materials
{
    /// <summary>
    /// material_type --> custom
    /// </summary>
    [System.Serializable]
    public partial class Custom: MaterialBase
    {
        internal Custom()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Custom"/> class.
        /// </summary>
        /// <param name="mass">the mass.</param>
        /// <param name="e_0">value for <paramref name="e_0"/>.</param>
        /// <param name="e_1">value for <paramref name="e_1"/>.</param>
        /// <param name="e_2">value for <paramref name="e_2"/>.</param>
        /// <param name="nu_0">the nu 0.</param>
        /// <param name="nu_1">the nu 1.</param>
        /// <param name="nu_2">the nu 2.</param>
        /// <param name="alfa_0">the alfa 0.</param>
        /// <param name="alfa_1">the alfa 1.</param>
        /// <param name="alfa_2">the alfa 2.</param>
        /// <param name="G_0">value for <paramref name="G_0"/>.</param>
        /// <param name="G_1">value for <paramref name="G_1"/>.</param>
        /// <param name="G_2">value for <paramref name="G_2"/>.</param>
        public Custom(double mass, double e_0, double e_1, double e_2, double nu_0, double nu_1, double nu_2, double alfa_0, double alfa_1, double alfa_2, double G_0, double G_1, double G_2)
        {
            this.Mass = mass;
            this.E_0 = e_0;
            this.E_1 = e_1;
            this.E_2 = e_2;
            this.nu_0 = nu_0;
            this.nu_1 = nu_1;
            this.nu_2 = nu_2;
            this.alfa_0 = alfa_0;
            this.alfa_1 = alfa_1;
            this.alfa_2 = alfa_2;
            this.G_0 = G_0;
            this.G_1 = G_1;
            this.G_2 = G_2;
        }
        /// <summary>
        /// Create an Uniaxial Custom Material
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="e_0"></param>
        /// <param name="nu_0"></param>
        /// <param name="alfa_0"></param>
        public Custom(double mass, double e_0, double nu_0, double alfa_0)
        {
            this.Mass = mass;
            this.E_0 = e_0;
            this.E_1 = 0.0;
            this.E_2 = 0.0;
            this.nu_0 = nu_0;
            this.nu_1 = 0.0;
            this.nu_2 = 0.0;
            this.alfa_0 = alfa_0;
            this.alfa_1 = 0.0;
            this.alfa_2 = 0.0;
            this.G_0 = 0.0;
            this.G_1 = 0.0;
            this.G_2 = 0.0;
        }

    }
}