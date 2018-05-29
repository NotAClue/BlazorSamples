using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;

namespace NotAClue.MetaModeling
{
    internal interface IMetaColumnMetadata
    {
        AttributeCollection Attributes { get; }

        System.ComponentModel.DataAnnotations.DisplayAttribute DisplayAttribute { get; }

        bool ApplyFormatInEditMode { get; }

        bool ConvertEmptyStringToNull { get; }

        bool HtmlEncode { get; }

        string DataFormatString { get; }

        System.ComponentModel.DataAnnotations.DataTypeAttribute DataTypeAttribute { get; }

        object DefaultValue { get; }

        string Description { get; }

        string DisplayName { get; }

        string FilterUIHint { get; }

        string ShortDisplayName { get; }

        string NullDisplayText { get; }

        string Prompt { get; }

        System.ComponentModel.DataAnnotations.RequiredAttribute RequiredAttribute { get; }

        System.ComponentModel.DataAnnotations.ScaffoldColumnAttribute ScaffoldColumnAttribute { get; }

        System.ComponentModel.DataAnnotations.StringLengthAttribute StringLengthAttribute { get; }

        string UIHint { get; }

        bool IsReadOnly { get; }

        System.ComponentModel.DataAnnotations.EditableAttribute EditableAttribute { get; }
    }

}
