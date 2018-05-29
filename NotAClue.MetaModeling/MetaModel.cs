using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NotAClue.MetaModeling
{
    public class MetaModel : IMetaModel
    {
        private List<MetaTable> _tables;

        public MetaModel()
        {
        }


        private void AddTable(MetaTable table)
        {
            this._tables.Add(table);
        }

        public string GetActionPath(string tableName, string action, object row)
        {
            return this.GetTable(tableName).GetActionPath(action, row);
        }

        private bool IsTableVisible(MetaTable table)
        {
            return ((!table.EntityType.IsAbstract && !string.IsNullOrEmpty(table.ListActionPath)) && table.CanRead(this.Context.User));
        }

        private static void RegisterMetadataTypeDescriptionProvider(TableProvider entity, Func<Type, TypeDescriptionProvider> providerFactory)
        {
            if (providerFactory != null)
            {
                Type entityType = entity.EntityType;
                if (entityType != null)
                {
                    TypeDescriptionProvider provider = providerFactory(entityType);
                    if (provider != null)
                    {
                        TypeDescriptor.AddProviderTransparent(provider, entityType);
                    }
                }
            }
        }

        IMetaTable IMetaModel.GetTable(string uniqueTableName)
        {
            return this.GetTable(uniqueTableName);
        }

        IMetaTable IMetaModel.GetTable(Type entityType)
        {
            return this.GetTable(entityType);
        }

        bool IMetaModel.TryGetTable(string uniqueTableName, out IMetaTable table)
        {
            MetaTable table2;
            table = null;
            if (this.TryGetTable(uniqueTableName, out table2))
            {
                table = table2;
                return true;
            }
            return false;
        }

        bool IMetaModel.TryGetTable(Type entityType, out IMetaTable table)
        {
            MetaTable table2;
            table = null;
            if (this.TryGetTable(entityType, out table2))
            {
                table = table2;
                return true;
            }
            return false;
        }

        public bool TryGetTable(string uniqueTableName, out MetaTable table)
        {
            CheckForRegistrationException();
            if (uniqueTableName == null)
            {
                throw new ArgumentNullException("uniqueTableName");
            }
            return this._tablesByUniqueName.TryGetValue(uniqueTableName, out table);
        }

        public bool TryGetTable(Type entityType, out MetaTable table)
        {
            CheckForRegistrationException();
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }
            if (!this._registerGlobally)
            {
                table = this.Tables.SingleOrDefault<MetaTable>(t => t.EntityType == entityType);
                return (table > null);
            }
            return MetaModelManager.TryGetTable(entityType, out table);
        }

        internal virtual int RegisteredDataModelsCount
        {
            get
            {
                return this._contextTypes.Count;
            }
        }

        public string DynamicDataFolderVirtualPath
        {
            get
            {
                if (this._dynamicDataFolderVirtualPath == null)
                {
                    this._dynamicDataFolderVirtualPath = "~/DynamicData/";
                }
                return this._dynamicDataFolderVirtualPath;
            }
            set
            {
                this._dynamicDataFolderVirtualPath = VirtualPathUtility.AppendTrailingSlash(value);
            }
        }

        public ReadOnlyCollection<MetaTable> Tables
        {
            get
            {
                return this._tables;
            }
        }

        public List<MetaTable> VisibleTables
        {
            get
            {
                CheckForRegistrationException();
                return (from t in this.Tables.Where<MetaTable>(new Func<MetaTable, bool>(this.IsTableVisible))
                        orderby t.DisplayName
                        select t).ToList<MetaTable>();
            }
        }

        public IFieldTemplateFactory FieldTemplateFactory
        {
            get
            {
                if (this._fieldTemplateFactory == null)
                {
                    this.FieldTemplateFactory = new System.Web.DynamicData.FieldTemplateFactory();
                }
                return this._fieldTemplateFactory;
            }
            set
            {
                this._fieldTemplateFactory = value;
                if (this._fieldTemplateFactory != null)
                {
                    this._fieldTemplateFactory.Initialize(this);
                }
            }
        }

        public EntityTemplateFactory EntityTemplateFactory
        {
            get
            {
                if (this._entityTemplateFactory == null)
                {
                    this.EntityTemplateFactory = new System.Web.DynamicData.EntityTemplateFactory();
                }
                return this._entityTemplateFactory;
            }
            set
            {
                this._entityTemplateFactory = value;
                if (this._entityTemplateFactory != null)
                {
                    this._entityTemplateFactory.Initialize(this);
                }
            }
        }

        public FilterFactory FilterFactory
        {
            get
            {
                if (this._filterFactory == null)
                {
                    this.FilterFactory = new System.Web.DynamicData.FilterFactory();
                }
                return this._filterFactory;
            }
            set
            {
                this._filterFactory = value;
                if (this._filterFactory != null)
                {
                    this._filterFactory.Initialize(this);
                }
            }
        }

        List<IMetaTable> IMetaModel.VisibleTables
        {
            get
            {
                return this.VisibleTables.OfType<IMetaTable>().ToList<IMetaTable>();
            }
        }
    }
}
