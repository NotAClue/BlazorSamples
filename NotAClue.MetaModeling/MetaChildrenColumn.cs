using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NotAClue.MetaModeling
{
    public class MetaChildrenColumn : MetaColumn, IMetaChildrenColumn, IMetaColumn
    {
        public MetaChildrenColumn(MetaTable table, ColumnProvider entityMember) : base(table, entityMember)
        {
        }

        public string GetChildrenListPath(object row)
        {
            return this.GetChildrenPath(PageAction.List, row);
        }

        public string GetChildrenPath(string action, object row)
        {
            if (row == null)
            {
                return string.Empty;
            }
            return this.ChildTable.GetActionPath(action, this.GetRouteValues(row));
        }

        public string GetChildrenPath(string action, object row, string path)
        {
            if (row == null)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(path))
            {
                return this.GetChildrenPath(action, row);
            }
            RouteValueDictionary routeValues = this.GetRouteValues(row);
            return QueryStringHandler.AddFiltersToPath(path, routeValues);
        }

        private RouteValueDictionary GetRouteValues(object row)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            IList<object> primaryKeyValues = base.Table.GetPrimaryKeyValues(row);
            MetaForeignKeyColumn columnInOtherTable = this.ColumnInOtherTable as MetaForeignKeyColumn;
            if (columnInOtherTable != null)
            {
                for (int i = 0; i < columnInOtherTable.ForeignKeyNames.Count; i++)
                {
                    dictionary.Add(columnInOtherTable.ForeignKeyNames[i], Misc.SanitizeQueryStringValue(primaryKeyValues[i]));
                }
            }
            return dictionary;
        }

        protected internal override void Initialize()
        {
            base.Initialize();
            AssociationProvider association = base.Provider.Association;
            this.ChildTable = base.Model.GetTable(association.ToTable.Name, base.Table.DataContextType);
            if (association.ToColumn != null)
            {
                this.ColumnInOtherTable = this.ChildTable.GetColumn(association.ToColumn.Name);
            }
        }

        public bool IsManyToMany
        {
            get
            {
                return ((base.Provider.Association != null) && (base.Provider.Association.Direction == AssociationDirection.ManyToMany));
            }
        }

        public MetaTable ChildTable { get; private set; }

        public MetaColumn ColumnInOtherTable { get; private set; }

        internal override string SortExpressionInternal
        {
            get
            {
                return string.Empty;
            }
        }

        internal override bool ScaffoldNoCache
        {
            get
            {
                return true;
            }
        }

        IMetaTable IMetaChildrenColumn.ChildTable
        {
            get
            {
                return this.ChildTable;
            }
        }

        IMetaColumn IMetaChildrenColumn.ColumnInOtherTable
        {
            get
            {
                return this.ColumnInOtherTable;
            }
        }
    }

}
