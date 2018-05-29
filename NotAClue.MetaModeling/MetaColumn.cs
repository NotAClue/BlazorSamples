using System;
using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;

namespace NotAClue.MetaModeling
{
    public class MetaColumn : IFieldFormattingOptions, IMetaColumn
    {
        private System.TypeCode _typeCode;
        private Type _type;
        private object _metadataCacheLock = new object();
        private static readonly int s_longStringLengthCutoff = 0x3ffffffb;
        private IMetaColumnMetadata _metadata;
        private bool? _scaffoldValueManual;
        private bool? _scaffoldValueDefault;

        public MetaColumn(MetaTable table, ColumnProvider columnProvider)
        {
            this.Table = table;
            this.Provider = columnProvider;
        }

        protected virtual AttributeCollection BuildAttributeCollection()
        {
            return this.Provider.Attributes;
        }

        protected internal virtual void Initialize()
        {
        }

        public void ResetMetadata()
        {
            this._metadata = null;
        }

        public override string ToString()
        {
            return (base.GetType().Name + " " + this.Name);
        }

        public AttributeCollection Attributes
        {
            get
            {
                return this.Metadata.Attributes;
            }
        }

        public Type ColumnType
        {
            get
            {
                if (this._type == null)
                {
                    this._type = Misc.RemoveNullableFromType(this.Provider.ColumnType);
                }
                return this._type;
            }
        }

        public System.ComponentModel.DataAnnotations.DataTypeAttribute DataTypeAttribute
        {
            get
            {
                return this.Metadata.DataTypeAttribute;
            }
        }

        public object DefaultValue
        {
            get
            {
                return this.Metadata.DefaultValue;
            }
        }

        public virtual string Description
        {
            get
            {
                return this.Metadata.Description;
            }
        }

        public virtual string DisplayName
        {
            get
            {
                return (this.Metadata.DisplayName ?? this.Name);
            }
        }

        public PropertyInfo EntityTypeProperty
        {
            get
            {
                return this.Provider.EntityTypeProperty;
            }
        }

        public string FilterUIHint
        {
            get
            {
                return this.Metadata.FilterUIHint;
            }
        }

        public bool IsBinaryData
        {
            get
            {
                return (this.ColumnType == typeof(byte[]));
            }
        }

        public bool IsCustomProperty
        {
            get
            {
                return this.Provider.IsCustomProperty;
            }
        }

        public bool IsFloatingPoint
        {
            get
            {
                if (!(this.ColumnType == typeof(float)) && !(this.ColumnType == typeof(double)))
                {
                    return (this.ColumnType == typeof(decimal));
                }
                return true;
            }
        }

        public bool IsForeignKeyComponent
        {
            get
            {
                return this.Provider.IsForeignKeyComponent;
            }
        }

        public bool IsGenerated
        {
            get
            {
                return this.Provider.IsGenerated;
            }
        }

        public bool IsInteger
        {
            get
            {
                if ((!(this.ColumnType == typeof(byte)) && !(this.ColumnType == typeof(short))) && !(this.ColumnType == typeof(int)))
                {
                    return (this.ColumnType == typeof(long));
                }
                return true;
            }
        }

        public bool IsLongString
        {
            get
            {
                return (this.IsString && (this.Provider.MaxLength >= s_longStringLengthCutoff));
            }
        }

        public bool IsPrimaryKey
        {
            get
            {
                return this.Provider.IsPrimaryKey;
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                return ((this.Provider.IsReadOnly || this.Metadata.IsReadOnly) || ((this.Metadata.EditableAttribute != null) && !this.Metadata.EditableAttribute.AllowEdit));
            }
        }

        public bool AllowInitialValue
        {
            get
            {
                if (this.IsGenerated)
                {
                    return false;
                }
                return this.Metadata.EditableAttribute.GetPropertyValue<EditableAttribute, bool>(a => a.AllowInitialValue, !this.IsReadOnly);
            }
        }

        public bool IsRequired
        {
            get
            {
                return (this.Metadata.RequiredAttribute > null);
            }
        }

        public bool IsString
        {
            get
            {
                return (this.ColumnType == typeof(string));
            }
        }

        public int MaxLength
        {
            get
            {
                StringLengthAttribute stringLengthAttribute = this.Metadata.StringLengthAttribute;
                if (stringLengthAttribute == null)
                {
                    return this.Provider.MaxLength;
                }
                return stringLengthAttribute.MaximumLength;
            }
        }

        public MetaModel Model
        {
            get
            {
                return this.Table.Model;
            }
        }

        public string Name
        {
            get
            {
                return this.Provider.Name;
            }
        }

        public virtual string Prompt
        {
            get
            {
                return this.Metadata.Prompt;
            }
        }

        public ColumnProvider Provider { get; private set; }

        public string RequiredErrorMessage
        {
            get
            {
                RequiredAttribute requiredAttribute = this.Metadata.RequiredAttribute;
                if (requiredAttribute == null)
                {
                    return string.Empty;
                }
                return StringLocalizerUtil.GetLocalizedString(requiredAttribute, this.DisplayName);
            }
        }

        public virtual bool Scaffold
        {
            get
            {
                if (this._scaffoldValueManual.HasValue)
                {
                    return this._scaffoldValueManual.Value;
                }
                DisplayAttribute displayAttribute = this.Metadata.DisplayAttribute;
                if ((displayAttribute != null) && displayAttribute.GetAutoGenerateField().HasValue)
                {
                    return displayAttribute.GetAutoGenerateField().Value;
                }
                ScaffoldColumnAttribute scaffoldColumnAttribute = this.Metadata.ScaffoldColumnAttribute;
                if (scaffoldColumnAttribute != null)
                {
                    return scaffoldColumnAttribute.Scaffold;
                }
                if (!this._scaffoldValueDefault.HasValue)
                {
                    this._scaffoldValueDefault = new bool?(this.ScaffoldNoCache);
                }
                return this._scaffoldValueDefault.Value;
            }
            set
            {
                this._scaffoldValueManual = new bool?(value);
            }
        }

        internal virtual bool ScaffoldNoCache
        {
            get
            {
                Type type;
                if (!string.IsNullOrEmpty(this.UIHint))
                {
                    return true;
                }
                if (this.IsForeignKeyComponent)
                {
                    return false;
                }
                if (this.IsGenerated)
                {
                    return false;
                }
                if (this.IsPrimaryKey)
                {
                    return true;
                }
                if (this.IsCustomProperty)
                {
                    return false;
                }
                return (this.IsString || ((this.ColumnType == typeof(char)) || (this.IsInteger || (this.IsFloatingPoint || ((this.ColumnType == typeof(DateTime)) || ((this.ColumnType == typeof(TimeSpan)) || ((this.ColumnType == typeof(DateTimeOffset)) || ((this.ColumnType == typeof(bool)) || (this.IsEnumType(out type) || ((this.ColumnType == typeof(DbGeography)) || (this.ColumnType == typeof(DbGeometry))))))))))));
            }
        }

        public virtual string ShortDisplayName
        {
            get
            {
                return (this.Metadata.ShortDisplayName ?? this.DisplayName);
            }
        }

        public string SortExpression
        {
            get
            {
                return this.SortExpressionInternal;
            }
        }

        internal virtual string SortExpressionInternal
        {
            get
            {
                if (!this.Provider.IsSortable)
                {
                    return string.Empty;
                }
                return this.Name;
            }
        }

        public MetaTable Table { get; private set; }

        public System.TypeCode TypeCode
        {
            get
            {
                if (this._typeCode == System.TypeCode.Empty)
                {
                    this._typeCode = DataSourceUtil.TypeCodeFromType(this.ColumnType);
                }
                return this._typeCode;
            }
        }

        public virtual string UIHint
        {
            get
            {
                return this.Metadata.UIHint;
            }
        }

        public bool ApplyFormatInEditMode
        {
            get
            {
                return this.Metadata.ApplyFormatInEditMode;
            }
        }

        public bool ConvertEmptyStringToNull
        {
            get
            {
                return this.Metadata.ConvertEmptyStringToNull;
            }
        }

        public string DataFormatString
        {
            get
            {
                return this.Metadata.DataFormatString;
            }
        }

        public bool HtmlEncode
        {
            get
            {
                return this.Metadata.HtmlEncode;
            }
        }

        public string NullDisplayText
        {
            get
            {
                return this.Metadata.NullDisplayText;
            }
        }

        internal IMetaColumnMetadata Metadata
        {
            get
            {
                IMetaColumnMetadata metadata = this._metadata;
                if (metadata == null)
                {
                    metadata = new MetaColumnMetadata(this);
                    this._metadata = metadata;
                }
                return metadata;
            }
            set
            {
                this._metadata = value;
            }
        }

        string IMetaColumn.Description
        {
            get
            {
                return this.Description;
            }
        }

        string IMetaColumn.DisplayName
        {
            get
            {
                return this.DisplayName;
            }
        }

        string IMetaColumn.Prompt
        {
            get
            {
                return this.Prompt;
            }
        }

        string IMetaColumn.ShortDisplayName
        {
            get
            {
                return this.ShortDisplayName;
            }
        }

        IMetaTable IMetaColumn.Table
        {
            get
            {
                return this.Table;
            }
        }

        IMetaModel IMetaColumn.Model
        {
            get
            {
                return this.Model;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly MetaColumn.<>c<>9 = new MetaColumn.<>c();
        public static Func<EditableAttribute, bool> <>9__43_0;
            
            internal bool <get_AllowInitialValue>b__43_0(EditableAttribute a)
        {
            return a.AllowInitialValue;
        }
    }

}
