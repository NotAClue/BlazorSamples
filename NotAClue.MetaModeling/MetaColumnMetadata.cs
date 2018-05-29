using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NotAClue.MetaModeling
{
    internal class MetaColumnMetadata : MetaColumn.IMetaColumnMetadata
    {
        public MetaColumnMetadata(MetaColumn column)
        {
            this.Column = column;
            this.Attributes = this.Column.BuildAttributeCollection();
            this.DisplayAttribute = this.Attributes.FirstOrDefault<System.ComponentModel.DataAnnotations.DisplayAttribute>();
            this.DataTypeAttribute = this.Attributes.FirstOrDefault<System.ComponentModel.DataAnnotations.DataTypeAttribute>() ?? this.GetDefaultDataTypeAttribute();
            this.DescriptionAttribute = this.Attributes.FirstOrDefault<System.ComponentModel.DescriptionAttribute>();
            this.DefaultValueAttribute = this.Attributes.FirstOrDefault<System.ComponentModel.DefaultValueAttribute>();
            this.DisplayNameAttribute = this.Attributes.FirstOrDefault<System.ComponentModel.DisplayNameAttribute>();
            this.RequiredAttribute = this.Attributes.FirstOrDefault<System.ComponentModel.DataAnnotations.RequiredAttribute>();
            this.ScaffoldColumnAttribute = this.Attributes.FirstOrDefault<System.ComponentModel.DataAnnotations.ScaffoldColumnAttribute>();
            this.StringLengthAttribute = this.Attributes.FirstOrDefault<System.ComponentModel.DataAnnotations.StringLengthAttribute>();
            this.UIHint = this.GetHint<UIHintAttribute>(a => a.PresentationLayer, a => a.UIHint);
            this.FilterUIHint = this.GetHint<FilterUIHintAttribute>(a => a.PresentationLayer, a => a.FilterUIHint);
            this.EditableAttribute = this.Attributes.FirstOrDefault<System.ComponentModel.DataAnnotations.EditableAttribute>();
            this.IsReadOnly = this.Attributes.GetAttributePropertyValue<ReadOnlyAttribute, bool>(a => a.IsReadOnly, false);
            DisplayFormatAttribute attribute = this.Attributes.FirstOrDefault<DisplayFormatAttribute>() ?? ((this.DataTypeAttribute != null) ? this.DataTypeAttribute.DisplayFormat : null);
            this.ApplyFormatInEditMode = attribute.GetPropertyValue<DisplayFormatAttribute, bool>(a => a.ApplyFormatInEditMode, false);
            this.ConvertEmptyStringToNull = attribute.GetPropertyValue<DisplayFormatAttribute, bool>(a => a.ConvertEmptyStringToNull, true);
            this.DataFormatString = attribute.GetPropertyValue<DisplayFormatAttribute, string>(a => a.DataFormatString, string.Empty);
            this.NullDisplayText = attribute.GetPropertyValue<DisplayFormatAttribute, string>(a => a.NullDisplayText, string.Empty);
            this.HtmlEncode = attribute.GetPropertyValue<DisplayFormatAttribute, bool>(a => a.HtmlEncode, true);
        }

        private System.ComponentModel.DataAnnotations.DataTypeAttribute GetDefaultDataTypeAttribute()
        {
            if (!this.Column.IsString)
            {
                return null;
            }
            if (this.Column.IsLongString)
            {
                return new System.ComponentModel.DataAnnotations.DataTypeAttribute(DataType.MultilineText);
            }
            return new System.ComponentModel.DataAnnotations.DataTypeAttribute(DataType.Text);
        }

        private string GetHint<T>(Func<T, string> presentationLayerPropertyAccessor, Func<T, string> hintPropertyAccessor) where T : Attribute
        {
            T local;
            IEnumerable<T> enumerable = this.Attributes.OfType<T>();
            IEnumerable<T> source = from a in enumerable
                                    where string.IsNullOrEmpty(presentationLayerPropertyAccessor(a))
                                    select a;
            T local1 = (from a in enumerable
                        where !string.IsNullOrEmpty(presentationLayerPropertyAccessor(a))
                        select a).FirstOrDefault<T>(delegate (T a)
                        {
                            if (presentationLayerPropertyAccessor(a).ToLower(CultureInfo.InvariantCulture) != "webforms")
                            {
                                return presentationLayerPropertyAccessor(a).ToLower(CultureInfo.InvariantCulture) == "mvc";
                            }
                            return true;
                        });
            if (local1 != null)
            {
                local = local1;
            }
            else
            {
                local = source.FirstOrDefault<T>();
            }
            return local.GetPropertyValue<T, string>(hintPropertyAccessor);
        }

        private MetaColumn Column { get; set; }

        public AttributeCollection Attributes { get; private set; }

        public System.ComponentModel.DataAnnotations.DisplayAttribute DisplayAttribute { get; private set; }

        public bool ApplyFormatInEditMode { get; private set; }

        public bool ConvertEmptyStringToNull { get; private set; }

        public string DataFormatString { get; private set; }

        public System.ComponentModel.DataAnnotations.DataTypeAttribute DataTypeAttribute { get; private set; }

        public object DefaultValue
        {
            get
            {
                return this.DefaultValueAttribute.GetPropertyValue<System.ComponentModel.DefaultValueAttribute, object>(a => a.Value, null);
            }
        }

        private System.ComponentModel.DefaultValueAttribute DefaultValueAttribute { get; set; }

        public string Description
        {
            get
            {
                return (this.DisplayAttribute.GetLocalizedDescription() ?? this.DescriptionAttribute.GetPropertyValue<System.ComponentModel.DescriptionAttribute, string>(a => a.Description, null));
            }
        }

        private System.ComponentModel.DescriptionAttribute DescriptionAttribute { get; set; }

        public string DisplayName
        {
            get
            {
                return (this.DisplayAttribute.GetLocalizedName() ?? this.DisplayNameAttribute.GetPropertyValue<System.ComponentModel.DisplayNameAttribute, string>(a => a.DisplayName, null));
            }
        }

        public string ShortDisplayName
        {
            get
            {
                return this.DisplayAttribute.GetLocalizedShortName();
            }
        }

        private System.ComponentModel.DisplayNameAttribute DisplayNameAttribute { get; set; }

        public string FilterUIHint { get; private set; }

        public System.ComponentModel.DataAnnotations.EditableAttribute EditableAttribute { get; private set; }

        public bool IsReadOnly { get; private set; }

        public string NullDisplayText { get; private set; }

        public string Prompt
        {
            get
            {
                return this.DisplayAttribute.GetLocalizedPrompt();
            }
        }

        public System.ComponentModel.DataAnnotations.RequiredAttribute RequiredAttribute { get; private set; }

        public System.ComponentModel.DataAnnotations.ScaffoldColumnAttribute ScaffoldColumnAttribute { get; private set; }

        public System.ComponentModel.DataAnnotations.StringLengthAttribute StringLengthAttribute { get; private set; }

        public string UIHint { get; private set; }

        public bool HtmlEncode { get; set; }

        [Serializable, CompilerGenerated]
        private sealed class <>c
            {
                public static readonly MetaColumn.MetaColumnMetadata.<>c<>9 = new MetaColumn.MetaColumnMetadata.<>c();
        public static Func<UIHintAttribute, string> <>9__8_0;
                public static Func<UIHintAttribute, string> <>9__8_1;
                public static Func<FilterUIHintAttribute, string> <>9__8_2;
                public static Func<FilterUIHintAttribute, string> <>9__8_3;
                public static Func<ReadOnlyAttribute, bool> <>9__8_4;
                public static Func<DisplayFormatAttribute, bool> <>9__8_5;
                public static Func<DisplayFormatAttribute, bool> <>9__8_6;
                public static Func<DisplayFormatAttribute, string> <>9__8_7;
                public static Func<DisplayFormatAttribute, string> <>9__8_8;
                public static Func<DisplayFormatAttribute, bool> <>9__8_9;
                public static Func<DefaultValueAttribute, object> <>9__30_0;
                public static Func<DescriptionAttribute, string> <>9__36_0;
                public static Func<DisplayNameAttribute, string> <>9__42_0;
                
                internal string <.ctor>b__8_0(UIHintAttribute a)
        {
            return a.PresentationLayer;
        }

        internal string <.ctor>b__8_1(UIHintAttribute a)
        {
            return a.UIHint;
        }

        internal string <.ctor>b__8_2(FilterUIHintAttribute a)
        {
            return a.PresentationLayer;
        }

        internal string <.ctor>b__8_3(FilterUIHintAttribute a)
        {
            return a.FilterUIHint;
        }

        internal bool <.ctor>b__8_4(ReadOnlyAttribute a)
        {
            return a.IsReadOnly;
        }

        internal bool <.ctor>b__8_5(DisplayFormatAttribute a)
        {
            return a.ApplyFormatInEditMode;
        }

        internal bool <.ctor>b__8_6(DisplayFormatAttribute a)
        {
            return a.ConvertEmptyStringToNull;
        }

        internal string <.ctor>b__8_7(DisplayFormatAttribute a)
        {
            return a.DataFormatString;
        }

        internal string <.ctor>b__8_8(DisplayFormatAttribute a)
        {
            return a.NullDisplayText;
        }

        internal bool <.ctor>b__8_9(DisplayFormatAttribute a)
        {
            return a.HtmlEncode;
        }

        internal object <get_DefaultValue>b__30_0(DefaultValueAttribute a)
        {
            return a.Value;
        }

        internal string <get_Description>b__36_0(DescriptionAttribute a)
        {
            return a.Description;
        }

        internal string <get_DisplayName>b__42_0(DisplayNameAttribute a)
        {
            return a.DisplayName;
        }
    }

}
