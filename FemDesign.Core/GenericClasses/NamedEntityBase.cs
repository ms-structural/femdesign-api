// https://strusoft.com/
using System;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;

namespace FemDesign
{
    [System.Serializable]
    public abstract partial class NamedEntityBase : EntityBase, INamedEntity
    {
        /// <summary>
        /// Gets or sets the xml name.
        /// </summary>
        [XmlAttribute("name")]
        public string _xmlName; // identifier for serialisation purposes
        [XmlIgnore]
        internal string _name // identifier;  !It must be locked otherwise FEM-Design overrides the instance index. GitHub case: https://github.com/strusoft/femdesign-api/issues/850
        {
            get
            {
                if (_xmlName.StartsWith("@"))
                    return this._xmlName;
                else
                    return "@" + this._xmlName;
            }
            set
            {
                if (value.StartsWith("@"))
                    this._xmlName = value;
                else
                    this._xmlName = "@" + value;
            }
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlIgnore]
        public virtual string Name => _namePattern.Match(this._name).Groups["name"].Value;
        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        [XmlIgnore]
        public virtual int Instance => int.Parse(_namePattern.Match(this._name).Groups["instance"].Value);

        [XmlIgnore]
        public virtual string Identifier
        {
            get => _namePattern.Match(this._name).Groups["identifier"].Value;
            set
            {
                this._name = $"{value}.{GetUniqueInstanceCount()}";

                if (string.IsNullOrEmpty(value) || _namePattern.IsMatch(this._name) == false)
                    throw new ArgumentException($"'{value}' is not a valid Identifier.");
            }
        }

        /// <summary>
        /// All objects are locked automatically, otherwise FEM-Design overrides the object names.
        /// </summary>
        [XmlIgnore]
        public virtual bool LockedIdentifier    // Each object will be locked in the _name field
        {
            get => _name.StartsWith("@");
            set
            {
                if (value && !LockedIdentifier)
                    _name = "@" + _name;
                else if (!value && LockedIdentifier)
                    _name = _name.Substring(1);
            }
        }

        protected static readonly Regex _namePattern = new Regex(@"^@{0,1}(?'name'(?'identifier'[ -#%'-;=?A-\ufffd;]{0,50})\.(?'instance'[0-9]{1,6}){1})$");

        protected NamedEntityBase()
        {
            this._name = "";
        }

        /// <summary>
        /// This value will be used to set number (like ".1") part of the entity name (like "B.1"). Typically this is a counter starting at 1 and incrementing for each new instance of this class that has been created.
        /// </summary>
        /// <returns>A unique number.</returns>
        protected abstract int GetUniqueInstanceCount();
    }

    [System.Serializable]
    public abstract partial class NamedEntityPartBase : NamedEntityBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public override string Name => _namePattern.Match(base._name).Groups["name"].Value;
        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        public override int Instance => int.Parse(_namePattern.Match(base._name).Groups["instance"].Value);
        [XmlIgnore]
        public override string Identifier
        {
            get => _namePattern.Match(base._name).Groups["identifier"].Value;
            set
            {
                base.Identifier = value;
                base._name += ".1";
            }
        }
        protected new static readonly Regex _namePattern = new Regex(@"@{0,1}(?'name'(?'identifier'[ -#%'-;=?A-\ufffd;]{0,50})\.(?'instance'[0-9]{1,6}){1}\.[0-9]{0,4})");
    }
}