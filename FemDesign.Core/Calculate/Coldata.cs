using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    [System.Serializable]
    public class Coldata
    {
        /// <summary>
        /// Column number to set the properties of.
        /// </summary>
        [XmlElement("num")]
        public int Num { get; set; }

        
        [XmlElement("flags")]
        public int Flags { get; set; } = 0;

        /// <summary>
        /// Width of column
        /// </summary>
        [XmlElement("width")]
        public int Width { get; set; } = 0;

        private bool ShouldSerializeWidth()
        {
            return Width != 0;
        }

        /// <summary>
        /// %s for string, %d for integer, %.3f for float with 3 digits
        /// </summary>
        [XmlElement("format")]
        public string Format { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Coldata()
        {
        }


        public Coldata(int num, int flags)
        {
            this.Num = num;
            this.Flags = flags;
        }

        public Coldata(int num, int flags, int width = 50, string format = "%s") : this(num, flags)
        {
            this.Width = width;
            this.Format = format;
        }


        public static List<Coldata> Default()
        {
            List<Coldata> coldata = new List<Coldata>();
            for (int i = 0; i < 64; i++)
            {
                coldata.Add(new Coldata(i, 0));
            }
            return coldata;
        }
    }
}