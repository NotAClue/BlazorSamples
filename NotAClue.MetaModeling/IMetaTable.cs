using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NotAClue.MetaModeling
{
    internal interface IMetaTable
    {
        bool CanDelete(IPrincipal principal);
        bool CanInsert(IPrincipal principal);
        bool CanRead(IPrincipal principal);
        bool CanUpdate(IPrincipal principal);
        object CreateContext();
        string GetActionPath(string action);
        string GetActionPath(string action, IList<object> primaryKeyValues);
        string GetActionPath(string action, object row);
        string GetActionPath(string action, RouteValueDictionary routeValues);
        string GetActionPath(string action, IList<object> primaryKeyValues, string path);
        string GetActionPath(string action, object row, string path);
        IMetaColumn GetColumn(string columnName);
        DataKey GetDataKeyFromRoute();
        string GetDisplayString(object row);
        IEnumerable<IMetaColumn> GetFilteredColumns();
        IDictionary<string, object> GetPrimaryKeyDictionary(object row);
        string GetPrimaryKeyString(IList<object> primaryKeyValues);
        string GetPrimaryKeyString(object row);
        IList<object> GetPrimaryKeyValues(object row);
        IQueryable GetQuery();
        IQueryable GetQuery(object context);
        IEnumerable<IMetaColumn> GetScaffoldColumns(DataBoundControlMode mode, ContainerType containerType);
        bool TryGetColumn(string columnName, out IMetaColumn column);

        AttributeCollection Attributes { get; }

        ReadOnlyCollection<IMetaColumn> Columns { get; }

        string DataContextPropertyName { get; }

        Type DataContextType { get; }

        IMetaColumn DisplayColumn { get; }

        string DisplayName { get; }

        Type EntityType { get; }

        string[] PrimaryKeyNames { get; }

        string ForeignKeyColumnsNames { get; }

        bool HasPrimaryKey { get; }

        bool IsReadOnly { get; }

        string ListActionPath { get; }

        IMetaModel Model { get; }

        string Name { get; }

        ReadOnlyCollection<IMetaColumn> PrimaryKeyColumns { get; }

        TableProvider Provider { get; }

        Type RootEntityType { get; }

        bool Scaffold { get; }

        IMetaColumn SortColumn { get; }

        bool SortDescending { get; }
    }
}
