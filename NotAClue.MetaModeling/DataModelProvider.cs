using System;

namespace NotAClue.MetaModeling
{
    public abstract class DataModelProvider
    {
        protected DataModelProvider()
        {
        }

        public abstract object CreateContext();

        public abstract ReadOnlyCollection<TableProvider> Tables { get; }

        public virtual Type ContextType { get; protected set; }
    }
}