using System;

namespace NotAClue.MetaModeling
{
    public abstract class ColumnProvider
    {
        private bool? _isReadOnly;

        protected ColumnProvider(TableProvider table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            this.Table = table;
        }

        protected static AttributeCollection AddDefaultAttributes(ColumnProvider columnProvider, AttributeCollection attributes)
        {
            List<Attribute> list = new List<Attribute>();
            if ((attributes.FirstOrDefault<RequiredAttribute>() == null) && !columnProvider.Nullable)
            {
                list.Add(new RequiredAttribute());
            }
            StringLengthAttribute attribute2 = attributes.FirstOrDefault<StringLengthAttribute>();
            int maxLength = columnProvider.MaxLength;
            if (((attribute2 == null) && (columnProvider.ColumnType == typeof(string))) && (maxLength > 0))
            {
                list.Add(new StringLengthAttribute(maxLength));
            }
            if (list.Count > 0)
            {
                attributes = AttributeCollection.FromExisting(attributes, list.ToArray());
            }
            return attributes;
        }

        public override string ToString()
        {
            return (this.Name ?? base.ToString());
        }

        internal virtual System.ComponentModel.PropertyDescriptor PropertyDescriptor
        {
            get
            {
                return this.Table.GetTypeDescriptor().GetProperties().Find(this.Name, true);
            }
        }

        public virtual AttributeCollection Attributes
        {
            get
            {
                System.ComponentModel.PropertyDescriptor propertyDescriptor = this.PropertyDescriptor;
                AttributeCollection attributes = (propertyDescriptor != null) ? propertyDescriptor.Attributes : AttributeCollection.Empty;
                return AddDefaultAttributes(this, attributes);
            }
        }

        public virtual string Name { get; protected set; }

        public virtual Type ColumnType { get; protected set; }

        public virtual bool IsPrimaryKey { get; protected set; }

        public virtual bool IsReadOnly
        {
            get
            {
                if (!this._isReadOnly.HasValue)
                {
                    System.ComponentModel.PropertyDescriptor propertyDescriptor = this.PropertyDescriptor;
                    this._isReadOnly = new bool?((propertyDescriptor != null) ? propertyDescriptor.IsReadOnly : false);
                }
                return this._isReadOnly.Value;
            }
            protected set
            {
                this._isReadOnly = new bool?(value);
            }
        }

        public virtual bool IsGenerated { get; protected set; }

        public virtual bool IsSortable { get; protected set; }

        public virtual int MaxLength { get; protected set; }

        public virtual bool Nullable { get; protected set; }

        public virtual bool IsCustomProperty { get; protected set; }

        public virtual AssociationProvider Association { get; protected set; }

        public TableProvider Table { get; private set; }

        public virtual PropertyInfo EntityTypeProperty { get; protected set; }

        public virtual bool IsForeignKeyComponent { get; protected set; }
    }
}