using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System.ComponentModel.DataAnnotations;

namespace NotAClue.MetaModeling
{
    public class MetaForeignKeyColumn : MetaColumn, IMetaForeignKeyColumn, IMetaColumn
    {
        private Dictionary<string, string> _foreignKeyFilterMapping;

        public MetaForeignKeyColumn(MetaTable table, ColumnProvider entityMember) : base(table, entityMember)
        {
        }

        internal void CreateForeignKeyFilterMapping(IList<string> foreignKeyNames, IList<string> primaryKeyNames, Func<string, bool> propertyExists)
        {
            if (foreignKeyNames != null)
            {
                int num = 0;
                foreach (string str in foreignKeyNames)
                {
                    if (!propertyExists(str))
                    {
                        if (this._foreignKeyFilterMapping == null)
                        {
                            this._foreignKeyFilterMapping = new Dictionary<string, string>();
                        }
                        this._foreignKeyFilterMapping[str] = base.Name + "." + primaryKeyNames[num];
                    }
                    num++;
                }
            }
        }

        public void ExtractForeignKey(IDictionary dictionary, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                foreach (string str in this.ForeignKeyNames)
                {
                    dictionary[str] = null;
                }
            }
            else
            {
                string[] strArray = Misc.ParseCommaSeparatedString(value);
                for (int i = 0; i < strArray.Length; i++)
                {
                    dictionary[this.ForeignKeyNames[i]] = strArray[i];
                }
            }
        }

        public string GetFilterExpression(string foreignKeyName)
        {
            string str;
            if ((this._foreignKeyFilterMapping != null) && this._foreignKeyFilterMapping.TryGetValue(foreignKeyName, out str))
            {
                return str;
            }
            return foreignKeyName;
        }

        public string GetForeignKeyDetailsPath(object row)
        {
            return this.GetForeignKeyPath(PageAction.Details, row);
        }

        internal MetaTable GetForeignKeyMetaTable(object row)
        {
            object propertyValue = DataBinder.GetPropertyValue(row, base.Name);
            if (propertyValue != null)
            {
                MetaTable tableFromTypeHierarchy = Misc.GetTableFromTypeHierarchy(propertyValue.GetType());
                if (tableFromTypeHierarchy != null)
                {
                    return tableFromTypeHierarchy;
                }
            }
            return this.ParentTable;
        }

        public string GetForeignKeyPath(string action, object row)
        {
            return this.GetForeignKeyPath(action, row, null);
        }

        public string GetForeignKeyPath(string action, object row, string path)
        {
            if (row == null)
            {
                return string.Empty;
            }
            IList<object> foreignKeyValues = this.GetForeignKeyValues(row);
            if (foreignKeyValues == null)
            {
                return string.Empty;
            }
            return this.GetForeignKeyMetaTable(row).GetActionPath(action, foreignKeyValues, path);
        }

        public string GetForeignKeyString(object row)
        {
            if (row == null)
            {
                return string.Empty;
            }
            return Misc.PersistListToCommaSeparatedString(this.GetForeignKeyValues(row));
        }

        public IList<object> GetForeignKeyValues(object row)
        {
            object[] objArray = new object[this.ForeignKeyNames.Count];
            int num = 0;
            bool flag = false;
            foreach (string str in this.ForeignKeyNames)
            {
                object obj2 = base.Table.Provider.EvaluateForeignKey(row, str);
                if (obj2 != null)
                {
                    flag = true;
                }
                objArray[num++] = obj2;
            }
            if (!flag)
            {
                return null;
            }
            return objArray;
        }

        protected internal override void Initialize()
        {
            base.Initialize();
            this.ParentTable = base.Model.GetTable(base.Provider.Association.ToTable.Name, base.Table.DataContextType);
            this.CreateForeignKeyFilterMapping(this.ForeignKeyNames, this.ParentTable.PrimaryKeyNames, foreignKey => base.Table.EntityType.GetProperty(foreignKey) != null);
        }

        public MetaTable ParentTable { get; internal set; }

        public bool IsPrimaryKeyInThisTable
        {
            get
            {
                return base.Provider.Association.IsPrimaryKeyInThisTable;
            }
        }

        internal override string SortExpressionInternal
        {
            get
            {
                MetaColumn displayColumn = this.ParentTable.DisplayColumn;
                return (base.Provider.Association.GetSortExpression(displayColumn.Provider) ?? string.Empty);
            }
        }

        internal override bool ScaffoldNoCache
        {
            get
            {
                return true;
            }
        }

        public ReadOnlyCollection<string> ForeignKeyNames
        {
            get
            {
                return base.Provider.Association.ForeignKeyNames;
            }
        }

        IMetaTable IMetaForeignKeyColumn.ParentTable
        {
            get
            {
                return this.ParentTable;
            }
        }
    }

}
