﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using PokerLeagueManager.Common;
using PokerLeagueManager.Common.Infrastructure;

namespace PokerLeagueManager.Queries.Core.Infrastructure
{
    public class QueryHandlerFactory : IQueryHandlerFactory
    {
        private IQueryDataStore _queryDataStore;

        public QueryHandlerFactory(IQueryDataStore queryDataStore)
        {
            _queryDataStore = queryDataStore;
        }

        public TResult Execute<TResult>(IQuery<TResult> query)
        {
            return Execute<TResult>((IQuery)query);
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes", Justification = "This Exception should never happen, so I'm ok with leaving it as-is")]
        public TResult Execute<TResult>(IQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query", "Cannot execute a null Query.");
            }

            var executeQueryHandlerMethods = from m in typeof(QueryHandlerFactory).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                                             where m.Name == "ExecuteQueryHandler" && m.ContainsGenericParameters &&
                                                   m.IsGenericMethod && m.IsGenericMethodDefinition && m.GetGenericArguments().Count() == 2
                                             select m;

            if (executeQueryHandlerMethods.Count() != 1)
            {
                throw new Exception("Unexpected Exception. Could not find the ExecuteQueryHandler method via Reflection.");
            }

            var executeQueryHandlerMethod = executeQueryHandlerMethods.First();
            var queryType = query.GetType();
            var genericInterface = queryType.GetInterfaces().First(i => i.IsGenericType);
            var queryReturnType = genericInterface.GenericTypeArguments[0];

            var generic = executeQueryHandlerMethod.MakeGenericMethod(queryType, queryReturnType);

            try
            {
                return (TResult)generic.Invoke(this, new object[] { query });
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public IDataTransferObject ExecuteQueryDto(IQuery query)
        {
            return Execute<IDataTransferObject>(query);
        }

        public IEnumerable<IDataTransferObject> ExecuteQueryList(IQuery query)
        {
            return Execute<IEnumerable<IDataTransferObject>>(query);
        }

        public int ExecuteQueryInt(IQuery query)
        {
            return Execute<int>(query);
        }

        private TResult ExecuteQueryHandler<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>
        {
            var matchingTypes = typeof(IHandlesQuery<,>).FindHandlers<TQuery>(Assembly.GetExecutingAssembly());

            if (matchingTypes.Count() == 0)
            {
                throw new ArgumentException(string.Format("Could not find Query Handler for {0}", typeof(TQuery).Name));
            }

            if (matchingTypes.Count() > 1)
            {
                throw new ArgumentException(string.Format("Found more than 1 Query Handler for {0}", typeof(TQuery).Name));
            }

            var queryHandlerType = matchingTypes.First();
            var handler = UnitySingleton.Container.Resolve(queryHandlerType, null);

            var repoProperty = queryHandlerType.GetProperty("Repository");
            var executeMethod = queryHandlerType.GetMethod("Execute");

            repoProperty.SetValue(handler, _queryDataStore);
            return (TResult)executeMethod.Invoke(handler, new object[] { query });
        }
    }
}
