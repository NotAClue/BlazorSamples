namespace NotAClue.MetaModeling
{
    public class MetaTable : IMetaTable
    {
        private const int DefaultColumnOrder = 0x2710;
        private Dictionary<string, MetaColumn> _columnsByName;
        private MetaColumn _displayColumn;
        private string _foreignKeyColumnsNames;
        private bool? _hasToStringOverride;
        private MetaTableMetadata _metadata;
        private ReadOnlyCollection<MetaColumn> _primaryKeyColumns;
        private bool _scaffoldDefaultValue;
        private MetaColumn _sortColumn;
        private bool _sortColumnProcessed;
        private TableProvider _tableProvider;
        private string _listActionPath;

        public MetaTable(MetaModel metaModel, TableProvider tableProvider)
        {
            this._tableProvider = tableProvider;
            this.Model = metaModel;
        }

        protected virtual System.ComponentModel.AttributeCollection BuildAttributeCollection()
        {
            return this.Provider.Attributes;
        }

        public virtual bool CanDelete(IPrincipal principal)
        {
            return this.Provider.CanDelete(principal);
        }

        public virtual bool CanInsert(IPrincipal principal)
        {
            return this.Provider.CanInsert(principal);
        }

        public virtual bool CanRead(IPrincipal principal)
        {
            return this.Provider.CanRead(principal);
        }

        public virtual bool CanUpdate(IPrincipal principal)
        {
            return this.Provider.CanUpdate(principal);
        }

        protected virtual MetaChildrenColumn CreateChildrenColumn(ColumnProvider columnProvider)
        {
            return new MetaChildrenColumn(this, columnProvider);
        }

        protected virtual MetaColumn CreateColumn(ColumnProvider columnProvider)
        {
            return new MetaColumn(this, columnProvider);
        }

        private MetaColumn CreateColumnInternal(ColumnProvider columnProvider)
        {
            if (columnProvider.Association != null)
            {
                switch (columnProvider.Association.Direction)
                {
                    case AssociationDirection.OneToOne:
                    case AssociationDirection.ManyToOne:
                        return this.CreateForeignKeyColumn(columnProvider);

                    case AssociationDirection.OneToMany:
                    case AssociationDirection.ManyToMany:
                        return this.CreateChildrenColumn(columnProvider);
                }
            }
            return this.CreateColumn(columnProvider);
        }

        internal void CreateColumns()
        {
            List<MetaColumn> list = new List<MetaColumn>();
            this._columnsByName = new Dictionary<string, MetaColumn>(StringComparer.OrdinalIgnoreCase);
            foreach (ColumnProvider provider in this.Provider.Columns)
            {
                MetaColumn item = this.CreateColumnInternal(provider);
                list.Add(item);
                if (this._columnsByName.ContainsKey(item.Name))
                {
                    object[] args = new object[] { item.Name, this.Provider.Name };
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, DynamicDataResources.MetaTable_ColumnNameConflict, args));
                }
                this._columnsByName.Add(item.Name, item);
            }
            this.Columns = new ReadOnlyCollection<MetaColumn>(list);
        }

        public virtual object CreateContext()
        {
            return this.Provider.DataModel.CreateContext();
        }

        protected virtual MetaForeignKeyColumn CreateForeignKeyColumn(ColumnProvider columnProvider)
        {
            return new MetaForeignKeyColumn(this, columnProvider);
        }

        public static MetaTable CreateTable(ICustomTypeDescriptor typeDescriptor)
        {
            return MetaModel.CreateSimpleModel(typeDescriptor).Tables.First<MetaTable>();
        }

        public static MetaTable CreateTable(Type entityType)
        {
            return MetaModel.CreateSimpleModel(entityType).Tables.First<MetaTable>();
        }

        public string GetActionPath(string action)
        {
            return this.GetActionPath(action, (IList<object>)null);
        }

        public string GetActionPath(string action, IList<object> primaryKeyValues)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary {
                {
                    "Table",
                    this.Name
                },
                {
                    "Action",
                    action
                }
            };
            this.GetRouteValuesFromPK(routeValues, primaryKeyValues);
            return this.GetActionPathFromRoutes(routeValues);
        }

        public string GetActionPath(string action, object row)
        {
            return this.GetActionPath(action, this.GetPrimaryKeyValues(row));
        }

        public string GetActionPath(string action, RouteValueDictionary routeValues)
        {
            routeValues.Add("Table", this.Name);
            routeValues.Add("Action", action);
            return this.GetActionPathFromRoutes(routeValues);
        }

        public string GetActionPath(string action, IList<object> primaryKeyValues, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return this.GetActionPath(action, primaryKeyValues);
            }
            RouteValueDictionary routeValues = new RouteValueDictionary();
            this.GetRouteValuesFromPK(routeValues, primaryKeyValues);
            return QueryStringHandler.AddFiltersToPath(path, routeValues);
        }

        public string GetActionPath(string action, object row, string path)
        {
            return this.GetActionPath(action, this.GetPrimaryKeyValues(row), path);
        }

        private string GetActionPathFromRoutes(RouteValueDictionary routeValues)
        {
            RequestContext requestContext = DynamicDataRouteHandler.GetRequestContext(this.Context);
            string str = null;
            if (requestContext != null)
            {
                routeValues.Add("__Model", this.Model);
                VirtualPathData virtualPath = RouteTable.Routes.GetVirtualPath(requestContext, routeValues);
                if (virtualPath != null)
                {
                    str = virtualPath.VirtualPath;
                }
            }
            return (str ?? string.Empty);
        }

        public MetaColumn GetColumn(string columnName)
        {
            MetaColumn column;
            if (!this.TryGetColumn(columnName, out column))
            {
                object[] args = new object[] { this.Name, columnName };
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, DynamicDataResources.MetaTable_NoSuchColumn, args));
            }
            return column;
        }

        private IDictionary<string, int> GetColumnGroupingOrder()
        {
            return (from c in this.Columns
                    where (c.Metadata.DisplayAttribute != null) && !string.IsNullOrEmpty(c.Metadata.DisplayAttribute.GetGroupName())
                    group c by c.Metadata.DisplayAttribute.GetGroupName()).ToDictionary<IGrouping<string, MetaColumn>, string, int>(cg => cg.Key, cg => cg.Min<MetaColumn>(c => GetColumnOrder(c)));
        }

        private static int GetColumnOrder(MetaColumn column)
        {
            DisplayAttribute displayAttribute = column.Metadata.DisplayAttribute;
            if ((displayAttribute != null) && displayAttribute.GetOrder().HasValue)
            {
                return displayAttribute.GetOrder().Value;
            }
            return 0x2710;
        }

        private static int GetColumnOrder(MetaColumn column, IDictionary<string, int> groupings)
        {
            DisplayAttribute displayAttribute = column.Metadata.DisplayAttribute;
            if (displayAttribute != null)
            {
                int num;
                string groupName = displayAttribute.GetGroupName();
                if (!string.IsNullOrEmpty(groupName) && groupings.TryGetValue(groupName, out num))
                {
                    return num;
                }
            }
            return GetColumnOrder(column);
        }

        public IDictionary<string, object> GetColumnValuesFromRoute(HttpContext context)
        {
            return this.GetColumnValuesFromRoute(context.ToWrapper());
        }

        internal IDictionary<string, object> GetColumnValuesFromRoute(HttpContextBase context)
        {
            RouteValueDictionary values = DynamicDataRouteHandler.GetRequestContext(context).RouteData.Values;
            Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
            foreach (MetaColumn column in this.Columns)
            {
                if (Misc.IsColumnInDictionary(column, values))
                {
                    MetaForeignKeyColumn column2 = column as MetaForeignKeyColumn;
                    if (column2 != null)
                    {
                        foreach (string str in column2.ForeignKeyNames)
                        {
                            dictionary2[str] = values[str];
                        }
                    }
                    else
                    {
                        dictionary2[column.Name] = Misc.ChangeType(values[column.Name], column.ColumnType);
                    }
                }
            }
            return dictionary2;
        }

        public DataKey GetDataKeyFromRoute()
        {
            OrderedDictionary keyTable = new OrderedDictionary(this.PrimaryKeyNames.Length);
            foreach (MetaColumn column in this.PrimaryKeyColumns)
            {
                string routeValue = Misc.GetRouteValue(column.Name);
                if (string.IsNullOrEmpty(routeValue))
                {
                    return null;
                }
                keyTable[column.Name] = Misc.ChangeType(routeValue, column.ColumnType);
            }
            return new DataKey(keyTable, this.PrimaryKeyNames);
        }

        private MetaColumn GetDisplayColumnFromHeuristic()
        {
            List<MetaColumn> source = (from c in this.Columns
                                       where !c.IsCustomProperty
                                       select c).ToList<MetaColumn>();
            return (source.FirstOrDefault<MetaColumn>(c => (c.IsString && !c.IsPrimaryKey)) ?? (source.FirstOrDefault<MetaColumn>(c => c.IsString) ?? (source.FirstOrDefault<MetaColumn>(c => c.IsPrimaryKey) ?? this.Columns.First<MetaColumn>())));
        }

        private MetaColumn GetDisplayColumnFromMetadata()
        {
            DisplayColumnAttribute displayColumnAttribute = this.Metadata.DisplayColumnAttribute;
            if (displayColumnAttribute == null)
            {
                return null;
            }
            MetaColumn column = null;
            if (!this.TryGetColumn(displayColumnAttribute.DisplayColumn, out column))
            {
                object[] args = new object[] { displayColumnAttribute.DisplayColumn, this.Name };
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, DynamicDataResources.MetaTable_CantFindDisplayColumn, args));
            }
            return column;
        }

        public virtual string GetDisplayString(object row)
        {
            if (row != null)
            {
                row = this.PreprocessRowObject(row);
                if (this.HasToStringOverride)
                {
                    return row.ToString();
                }
                object propertyValue = DataBinder.GetPropertyValue(row, this.DisplayColumn.Name);
                if (propertyValue != null)
                {
                    return propertyValue.ToString();
                }
            }
            return string.Empty;
        }

        public virtual IEnumerable<MetaColumn> GetFilteredColumns()
        {
            IDictionary<string, int> columnGroupOrder = this.GetColumnGroupingOrder();
            return (from c in this.Columns
                    where IsFilterableColumn(c, this.Context.User)
                    orderby GetColumnOrder(c, columnGroupOrder), GetColumnOrder(c)
                    select c);
        }

        public IDictionary<string, object> GetPrimaryKeyDictionary(object row)
        {
            row = this.PreprocessRowObject(row);
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (MetaColumn column in this.PrimaryKeyColumns)
            {
                dictionary.Add(column.Name, DataBinder.GetPropertyValue(row, column.Name));
            }
            return dictionary;
        }

        public string GetPrimaryKeyString(IList<object> primaryKeyValues)
        {
            return Misc.PersistListToCommaSeparatedString(primaryKeyValues);
        }

        public string GetPrimaryKeyString(object row)
        {
            row = this.PreprocessRowObject(row);
            return this.GetPrimaryKeyString(this.GetPrimaryKeyValues(row));
        }

        public IList<object> GetPrimaryKeyValues(object row)
        {
            if (row == null)
            {
                return null;
            }
            row = this.PreprocessRowObject(row);
            return Misc.GetKeyValues(this.PrimaryKeyColumns, row);
        }

        public IQueryable GetQuery()
        {
            return this.GetQuery(null);
        }

        public virtual IQueryable GetQuery(object context)
        {
            if (context == null)
            {
                context = this.CreateContext();
            }
            IQueryable query = this.Provider.GetQuery(context);
            if (this.EntityType != this.RootEntityType)
            {
                Type[] typeArguments = new Type[] { this.EntityType };
                System.Linq.Expressions.Expression[] arguments = new System.Linq.Expressions.Expression[] { query.Expression };
                System.Linq.Expressions.Expression expression = System.Linq.Expressions.Expression.Call(typeof(Queryable), "OfType", typeArguments, arguments);
                query = query.Provider.CreateQuery(expression);
            }
            if (this.SortColumn != null)
            {
                return Misc.BuildSortQueryable(query, this);
            }
            return query;
        }

        private void GetRouteValuesFromPK(RouteValueDictionary routeValues, IList<object> primaryKeyValues)
        {
            if (primaryKeyValues != null)
            {
                for (int i = 0; i < this.PrimaryKeyNames.Length; i++)
                {
                    routeValues.Add(this.PrimaryKeyNames[i], Misc.SanitizeQueryStringValue(primaryKeyValues[i]));
                }
            }
        }

        public virtual IEnumerable<MetaColumn> GetScaffoldColumns(DataBoundControlMode mode, ContainerType containerType)
        {
            IDictionary<string, int> columnGroupOrder = this.GetColumnGroupingOrder();
            return (from c in this.Columns
                    where this.IsScaffoldColumn(c, mode, containerType)
                    orderby GetColumnOrder(c, columnGroupOrder), GetColumnOrder(c)
                    select c);
        }

        public static MetaTable GetTable(Type entityType)
        {
            MetaTable table;
            if (!TryGetTable(entityType, out table))
            {
                object[] args = new object[] { entityType.FullName };
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, DynamicDataResources.MetaModel_EntityTypeDoesNotBelongToModel, args));
            }
            return table;
        }

        protected internal virtual void Initialize()
        {
            foreach (MetaColumn column in this.Columns)
            {
                column.Initialize();
            }
        }

        internal static bool IsFilterableColumn(IMetaColumn column, IPrincipal user)
        {
            DisplayAttribute attribute = column.Attributes.FirstOrDefault<DisplayAttribute>();
            if ((attribute != null) && attribute.GetAutoGenerateFilter().HasValue)
            {
                return attribute.GetAutoGenerateFilter().Value;
            }
            if (!string.IsNullOrEmpty(column.FilterUIHint))
            {
                return true;
            }
            if (!column.Scaffold)
            {
                return false;
            }
            if (column.IsCustomProperty)
            {
                return false;
            }
            IMetaForeignKeyColumn column2 = column as IMetaForeignKeyColumn;
            if (column2 != null)
            {
                return column2.ParentTable.CanRead(user);
            }
            return ((column.ColumnType == typeof(bool)) || (column.GetEnumType() != null));
        }

        private bool IsScaffoldColumn(IMetaColumn column, DataBoundControlMode mode, ContainerType containerType)
        {
            if (!column.Scaffold)
            {
                return false;
            }
            if (mode == DataBoundControlMode.Insert)
            {
                IMetaChildrenColumn column3 = column as IMetaChildrenColumn;
                if ((column3 != null) && !column3.IsManyToMany)
                {
                    return false;
                }
            }
            IMetaForeignKeyColumn column2 = column as IMetaForeignKeyColumn;
            if ((column2 != null) && !column2.ParentTable.CanRead(this.Context.User))
            {
                return false;
            }
            return true;
        }

        private object PreprocessRowObject(object row)
        {
            if (row == null)
            {
                return null;
            }
            if (!this.EntityType.IsAssignableFrom(row.GetType()))
            {
                IList list = row as IList;
                if ((list != null) && (list.Count >= 1))
                {
                    return this.PreprocessRowObject(list[0]);
                }
            }
            return row;
        }

        public void ResetMetadata()
        {
            this._metadata = null;
            this._displayColumn = null;
            this._sortColumnProcessed = false;
            foreach (MetaColumn column in this.Columns)
            {
                column.ResetMetadata();
            }
        }

        internal void SetScaffoldAndName(bool scaffoldDefaultValue, string nameOverride)
        {
            if (!string.IsNullOrEmpty(nameOverride))
            {
                this.Name = nameOverride;
            }
            else if (this.Provider != null)
            {
                this.Name = this.Provider.Name;
            }
            this._scaffoldDefaultValue = scaffoldDefaultValue;
        }

        bool IMetaTable.CanDelete(IPrincipal principal)
        {
            return this.CanDelete(principal);
        }

        bool IMetaTable.CanInsert(IPrincipal principal)
        {
            return this.CanInsert(principal);
        }

        bool IMetaTable.CanRead(IPrincipal principal)
        {
            return this.CanRead(principal);
        }

        bool IMetaTable.CanUpdate(IPrincipal principal)
        {
            return this.CanUpdate(principal);
        }

        object IMetaTable.CreateContext()
        {
            return this.CreateContext();
        }

        IMetaColumn IMetaTable.GetColumn(string columnName)
        {
            return this.GetColumn(columnName);
        }

        string IMetaTable.GetDisplayString(object row)
        {
            return this.GetDisplayString(row);
        }

        IEnumerable<IMetaColumn> IMetaTable.GetFilteredColumns()
        {
            return this.GetFilteredColumns().OfType<IMetaColumn>();
        }

        IQueryable IMetaTable.GetQuery(object context)
        {
            return this.GetQuery(context);
        }

        IEnumerable<IMetaColumn> IMetaTable.GetScaffoldColumns(DataBoundControlMode mode, ContainerType containerType)
        {
            return this.GetScaffoldColumns(mode, containerType).OfType<IMetaColumn>();
        }

        bool IMetaTable.TryGetColumn(string columnName, out IMetaColumn column)
        {
            MetaColumn column2;
            column = null;
            if (this.TryGetColumn(columnName, out column2))
            {
                column = column2;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public bool TryGetColumn(string columnName, out MetaColumn column)
        {
            if (columnName == null)
            {
                throw new ArgumentNullException("columnName");
            }
            return this._columnsByName.TryGetValue(columnName, out column);
        }

        public static bool TryGetTable(Type entityType, out MetaTable table)
        {
            MetaModel.CheckForRegistrationException();
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }
            return MetaModel.MetaModelManager.TryGetTable(entityType, out table);
        }

        public System.ComponentModel.AttributeCollection Attributes
        {
            get
            {
                return this.Metadata.Attributes;
            }
        }

        public ReadOnlyCollection<MetaColumn> Columns { get; internal set; }

        internal HttpContextBase Context
        {
            private get
            {
                return (this._context ?? new HttpContextWrapper(HttpContext.Current));
            }
            set
            {
                this._context = value;
            }
        }

        public string DataContextPropertyName
        {
            get
            {
                return this._tableProvider.DataContextPropertyName;
            }
        }

        public Type DataContextType
        {
            get
            {
                return this.Provider.DataModel.ContextType;
            }
        }

        public virtual MetaColumn DisplayColumn
        {
            get
            {
                MetaColumn column = this._displayColumn;
                if (column == null)
                {
                    column = this.GetDisplayColumnFromMetadata() ?? this.GetDisplayColumnFromHeuristic();
                    this._displayColumn = column;
                }
                return column;
            }
        }

        public virtual string DisplayName
        {
            get
            {
                return (this.Metadata.DisplayName ?? this.Name);
            }
        }

        public Type EntityType
        {
            get
            {
                return this.Provider.EntityType;
            }
        }

        public string ForeignKeyColumnsNames
        {
            get
            {
                if (this._foreignKeyColumnsNames == null)
                {
                    string[] strArray = (from column in this.Columns.OfType<MetaForeignKeyColumn>() select column.Name).ToArray<string>();
                    this._foreignKeyColumnsNames = string.Join(",", strArray);
                }
                return this._foreignKeyColumnsNames;
            }
        }

        public bool HasPrimaryKey
        {
            get
            {
                return (this.PrimaryKeyColumns.Count > 0);
            }
        }

        private bool HasToStringOverride
        {
            get
            {
                if (!this._hasToStringOverride.HasValue)
                {
                    MethodInfo method = this.EntityType.GetMethod("ToString");
                    this._hasToStringOverride = new bool?(method.DeclaringType != typeof(object));
                }
                return this._hasToStringOverride.Value;
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                if (!this.Metadata.IsReadOnly)
                {
                    return !this.HasPrimaryKey;
                }
                return true;
            }
        }

        public string ListActionPath
        {
            get
            {
                return (this._listActionPath ?? this.GetActionPath(PageAction.List));
            }
            internal set
            {
                this._listActionPath = value;
            }
        }

        private MetaTableMetadata Metadata
        {
            get
            {
                MetaTableMetadata metadata = this._metadata;
                if (metadata == null)
                {
                    metadata = new MetaTableMetadata(this);
                    this._metadata = metadata;
                }
                return metadata;
            }
        }

        public MetaModel Model { get; private set; }

        public string Name { get; private set; }

        public ReadOnlyCollection<MetaColumn> PrimaryKeyColumns
        {
            get
            {
                if (this._primaryKeyColumns == null)
                {
                    this._primaryKeyColumns = (from c in this.Columns
                                               where c.IsPrimaryKey
                                               select c).ToList<MetaColumn>().AsReadOnly();
                }
                return this._primaryKeyColumns;
            }
        }

        internal string[] PrimaryKeyNames
        {
            get
            {
                if (this._primaryKeyColumnNames == null)
                {
                    this._primaryKeyColumnNames = (from c in this.PrimaryKeyColumns select c.Name).ToArray<string>();
                }
                return this._primaryKeyColumnNames;
            }
        }

        public TableProvider Provider
        {
            get
            {
                return this._tableProvider;
            }
        }

        public Type RootEntityType
        {
            get
            {
                return this.Provider.RootEntityType;
            }
        }

        public virtual bool Scaffold
        {
            get
            {
                bool? scaffoldTable = this.Metadata.ScaffoldTable;
                if (!scaffoldTable.HasValue)
                {
                    return this._scaffoldDefaultValue;
                }
                return scaffoldTable.GetValueOrDefault();
            }
        }

        public virtual MetaColumn SortColumn
        {
            get
            {
                if (!this._sortColumnProcessed)
                {
                    DisplayColumnAttribute displayColumnAttribute = this.Metadata.DisplayColumnAttribute;
                    if ((displayColumnAttribute != null) && !string.IsNullOrEmpty(displayColumnAttribute.SortColumn))
                    {
                        if (!this.TryGetColumn(displayColumnAttribute.SortColumn, out this._sortColumn))
                        {
                            object[] args = new object[] { displayColumnAttribute.SortColumn, this.Name };
                            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, DynamicDataResources.MetaTable_CantFindSortColumn, args));
                        }
                        if (this._sortColumn is MetaChildrenColumn)
                        {
                            object[] args = new object[] { this._sortColumn.Name, this.Name };
                            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, DynamicDataResources.MetaTable_CantUseChildrenColumnAsSortColumn, args));
                        }
                    }
                    this._sortColumnProcessed = true;
                }
                return this._sortColumn;
            }
        }

        public virtual bool SortDescending
        {
            get
            {
                return this.Metadata.SortDescending;
            }
        }

        string[] IMetaTable.PrimaryKeyNames
        {
            get
            {
                return this.PrimaryKeyNames;
            }
        }

        ReadOnlyCollection<IMetaColumn> IMetaTable.Columns
        {
            get
            {
                return this.Columns.OfType<IMetaColumn>().ToList<IMetaColumn>().AsReadOnly();
            }
        }

        IMetaModel IMetaTable.Model
        {
            get
            {
                return this.Model;
            }
        }

        IMetaColumn IMetaTable.DisplayColumn
        {
            get
            {
                return this.DisplayColumn;
            }
        }

        ReadOnlyCollection<IMetaColumn> IMetaTable.PrimaryKeyColumns
        {
            get
            {
                return this.PrimaryKeyColumns.OfType<IMetaColumn>().ToList<IMetaColumn>().AsReadOnly();
            }
        }

        IMetaColumn IMetaTable.SortColumn
        {
            get
            {
                return this.SortColumn;
            }
        }
    }
}