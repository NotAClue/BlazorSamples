using System;

namespace NotAClue.MetaModeling
{
    public abstract class TableProvider
    {
        private Type _rootEntityType;
        private string _dataContextPropertyName;

        internal TableProvider()
        {
        }

        protected TableProvider(DataModelProvider model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            this.DataModel = model;
        }

        public virtual bool CanDelete(IPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }
            return true;
        }

        public virtual bool CanInsert(IPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }
            return true;
        }

        public virtual bool CanRead(IPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }
            return true;
        }

        public virtual bool CanUpdate(IPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }
            return true;
        }

        public virtual object EvaluateForeignKey(object row, string foreignKeyName)
        {
            return DataBinder.GetPropertyValue(row, foreignKeyName);
        }

        public abstract IQueryable GetQuery(object context);
        public virtual ICustomTypeDescriptor GetTypeDescriptor()
        {
            return TypeDescriptor.GetProvider(this.EntityType).GetTypeDescriptor(this.EntityType);
        }

        public override string ToString()
        {
            return (this.Name ?? base.ToString());
        }

        public virtual System.ComponentModel.AttributeCollection Attributes
        {
            get
            {
                return this.GetTypeDescriptor().GetAttributes();
            }
        }

        public virtual string Name { get; protected set; }

        public virtual Type EntityType { get; protected set; }

        public abstract ReadOnlyCollection<ColumnProvider> Columns { get; }

        public DataModelProvider DataModel { get; internal set; }

        public virtual Type ParentEntityType { get; protected set; }

        public virtual Type RootEntityType
        {
            get
            {
                return (this._rootEntityType ?? this.EntityType);
            }
            protected set
            {
                this._rootEntityType = value;
            }
        }
    }
}