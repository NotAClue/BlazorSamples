using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace NotAClue.ExtensionMethods
{
    public static class LinqExpressionExtensionMethods
    {
        /// <summary>
        /// Builds a property expression from the parts it joins
        /// the parameterExpression and the propertyName together.
        /// i.e. {RequiredPlots}  and "Builders.Id"
        /// becomes: {RequiredPlots.Developers.Id}
        /// </summary>
        /// <param name="parameterExpression">
        /// The parameter expression.
        /// </param>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        /// <returns>
        /// A property expression
        /// </returns>
        public static Expression BuildPropertyExpression(Expression parameterExpression, string propertyName)
        {
            Expression expression = null;
            // split the propertyName into each part to 
            // be build into a property expression
            string[] strArray = propertyName.Split(new char[] { '.' });
            foreach (string str in strArray)
            {
                if (expression == null)
                    expression
                        = Expression.PropertyOrField(parameterExpression, str);
                else
                    expression
                        = Expression.PropertyOrField(expression, str);
            }
            // {RequiredPlots.Developers.Id}
            return expression;
        }

        public static Expression GetValue(Expression exp)
        {
            Type realType = GetUnderlyingType(exp.Type);
            if (realType == exp.Type)
                return exp;

            return Expression.Convert(exp, realType);
        }

        private static Type RemoveNullableFromType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public static Type GetUnderlyingType(Type type)
        {
            return RemoveNullableFromType(type);
        }

        public static Expression Join(IEnumerable<Expression> expressions, Func<Expression, Expression, Expression> joinFunction)
        {
            Expression result = null;
            foreach (Expression e in expressions)
            {
                if (e == null)
                    continue;

                if (result == null)
                    result = e;
                else
                    result = joinFunction(result, e);
            }
            return result;
        }

        public static Expression CreatePropertyExpression(Expression parameterExpression, string propertyName)
        {
            Expression propExpression = null;
            string[] props = propertyName.Split('.');
            foreach (var p in props)
            {
                if (propExpression == null)
                    propExpression = Expression.PropertyOrField(parameterExpression, p);
                else
                    propExpression = Expression.PropertyOrField(propExpression, p);
            }
            return propExpression;
        }

        /// <summary>
        /// Changes the type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="type">The type to convert to.</param>
        /// <returns>The value converted to the type.</returns>
        public static object ChangeType(object value, Type type)
        {
            // if type is null throw exception can't
            // carry on nothing to convert to.
            if (type == null)
                throw new ArgumentNullException("type");

            if (value == null)
            {
                // test for nullable type 
                // (i.e. if Nullable.GetUnderlyingType(type)
                // is not null then it is a nullable type 
                // OR if it is a reference type
                if ((Nullable.GetUnderlyingType(type) != null)
                    || !type.IsValueType)
                    return null;
                else // for 'not nullable value types' return the default value.
                    return Convert.ChangeType(value, type);
            }

            // ==== Here we are guaranteed to have a type and value ====

            // get the type either the underlying type or 
            // the type if there is no underlying type.
            type = Nullable.GetUnderlyingType(type) ?? type;

            // Convert using the type
            TypeConverter converter
                = TypeDescriptor.GetConverter(type);
            if (converter.CanConvertFrom(value.GetType()))
            {
                // return the converted value
                return converter.ConvertFrom(value);
            }

            // Convert using the values type
            TypeConverter converter2
                = TypeDescriptor.GetConverter(value.GetType());
            if (!converter2.CanConvertTo(type))
            {
                // if the type cannot be converted throw an error
                throw new InvalidOperationException(
                    String.Format("Unable to convert type '{0}' to '{1}'",
                    new object[] { value.GetType(), type }));
            }
            // return the converted value
            return converter2.ConvertTo(value, type);
        }

        private static object ChangeValueType(Type type, string value)
        {
            if (type == typeof(Guid))
                return new Guid(value);
            else
                return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
        }

        private static bool TypeAllowsNull(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null || !type.IsValueType;
        }
    }
}

        //public static MethodCallExpression BuildSingleItemQuery(this IQueryable query, MetaTable metaTable, string[] primaryKeyValues)
        //{
        //    // Items.Where(row => row.ID == 1)
        //    var whereCall = BuildItemsQuery(query, metaTable, metaTable.PrimaryKeyColumns, primaryKeyValues);
        //    // Items.Where(row => row.ID == 1).Single()
        //    var singleCall = Expression.Call(typeof(Queryable), "Single", new Type[] { metaTable.EntityType }, whereCall);

        //    return singleCall;
        //}

        //public static MethodCallExpression BuildItemsQuery(this IQueryable query, MetaTable metaTable, IList<MetaColumn> columns, string[] values)
        //{
        //    // row
        //    var rowParam = Expression.Parameter(metaTable.EntityType, "row");
        //    // row.ID == 1
        //    var whereBody = BuildWhereBody(rowParam, columns, values);
        //    // row => row.ID == 1
        //    var whereLambda = Expression.Lambda(whereBody, rowParam);
        //    // Items.Where(row => row.ID == 1)
        //    var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { metaTable.EntityType }, query.Expression, whereLambda);

        //    return whereCall;
        //}

        //public static MethodCallExpression BuildWhereQuery(this IQueryable query, MetaTable metaTable, MetaColumn column, string value)
        //{
        //    // row
        //    var rowParam = Expression.Parameter(metaTable.EntityType, column.Name);
        //    // row.ID == 1
        //    var whereBody = BuildWhereBodyFragment(rowParam, column, value);
        //    // row => row.ID == 1
        //    var whereLambda = Expression.Lambda(whereBody, rowParam);
        //    // Items.Where(row => row.ID == 1)
        //    var whereCall = Expression.Call(
        //                        typeof(Queryable),
        //                        "Where",
        //                        new Type[] { metaTable.EntityType },
        //                        query.Expression,
        //                        whereLambda);

        //    return whereCall;
        //}

        //public static MethodCallExpression BuildCustomQuery(this IQueryable query, MetaTable metaTable, MetaColumn column, string value, QueryType queryType)
        //{
        //    // row
        //    var rowParam = Expression.Parameter(metaTable.EntityType, metaTable.Name);

        //    // row.DisplayName
        //    var property = Expression.Property(rowParam, column.Name);

        //    // "prefix"
        //    var constant = Expression.Constant(value);

        //    // row.DisplayName.StartsWith("prefix")
        //    var startsWithCall = Expression.Call(property, typeof(string).GetMethod(queryType.ToString(), new Type[] { typeof(string) }), constant);

        //    // row => row.DisplayName.StartsWith("prefix")
        //    var whereLambda = Expression.Lambda(startsWithCall, rowParam);

        //    // Customers.Where(row => row.DisplayName.StartsWith("prefix"))
        //    var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { metaTable.EntityType }, query.Expression, whereLambda);

        //    return whereCall;
        //}

        //public static BinaryExpression BuildWhereBody(ParameterExpression parameter, IList<MetaColumn> columns, string[] values)
        //{
        //    // row.ID == 1
        //    var whereBody = BuildWhereBodyFragment(parameter, columns[0], values[0]);
        //    for (int i = 1; i < values.Length; i++)
        //    {
        //        // row.ID == 1 && row.ID2 == 2
        //        whereBody = Expression.AndAlso(whereBody, BuildWhereBodyFragment(parameter, columns[i], values[i]));
        //    }

        //    return whereBody;
        //}

        //private static BinaryExpression BuildWhereBodyFragment(ParameterExpression parameter, MetaColumn column, string value)
        //{
        //    // row.ID
        //    var property = Expression.Property(parameter, column.Name);
        //    // row.ID == 1
        //    return Expression.Equal(property, Expression.Constant(ChangeValueType(column, value)));
        //}

        ///// <summary>
        ///// Compares the values.
        ///// </summary>
        ///// <param name="valueType">Type of the value.</param>
        ///// <param name="value">The value.</param>
        ///// <param name="valueToCompareWith">The value to compare with.</param>
        ///// <param name="comparisonOperation">The comparison operation.</param>
        ///// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        //public static bool CompareValues(this Type valueType, String value, String valueToCompareWith, ComparisonOperations comparisonOperation)
        //{
        //    // get the left hand value for comparison
        //    var leftValue = Expression.Constant(ChangeType(value, valueType), valueType);
        //    // get the right hand value for comparison
        //    var rightValue = Expression.Constant(ChangeType(valueToCompareWith, valueType), valueType);

        //    // get new expression
        //    Expression comparison = null;
        //    switch (comparisonOperation)
        //    {
        //        case ComparisonOperations.LessThan:
        //            comparison = Expression.LessThan(leftValue, rightValue);
        //            break;
        //        case ComparisonOperations.LessThanOrEqual:
        //            comparison = Expression.LessThanOrEqual(leftValue, rightValue);
        //            break;
        //        case ComparisonOperations.Equal:
        //            comparison = Expression.Equal(leftValue, rightValue);
        //            break;
        //        case ComparisonOperations.NotEqual:
        //            comparison = Expression.NotEqual(leftValue, rightValue);
        //            break;
        //        case ComparisonOperations.GreaterThan:
        //            comparison = Expression.GreaterThan(leftValue, rightValue);
        //            break;
        //        case ComparisonOperations.GreaterThanOrEqual:
        //            comparison = Expression.GreaterThanOrEqual(leftValue, rightValue);
        //            break;
        //    }

        //    try
        //    {
        //        // execute expression
        //        var result = Expression.Lambda<Func<bool>>(comparison).Compile()();
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = ex.Message;
        //    }
        //    return false;
        //}
