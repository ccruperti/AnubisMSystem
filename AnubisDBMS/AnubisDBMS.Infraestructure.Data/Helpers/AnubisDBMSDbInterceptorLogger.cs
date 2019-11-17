using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace AnubisDBMS.Infraestructure.Data.Helpers
{
    public class AnubisDBMSDbInterceptorLogger : DbCommandInterceptor
    {
        private Logger _logger = LogManager.GetLogger("SqlDataLog");
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }

        public override void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            _stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                _logger.Error(interceptionContext.Exception, "Error ejecutando instruccion: {0}", command.CommandText);
            }
            else
            {
                _logger.Trace("ScalarExecuted TS:{0} \t| Servidor: {1} \t| Base de Datos: {2} \t| Instruccion: {3}", _stopwatch.Elapsed, command.Connection.DataSource, command.Connection.Database, command.CommandText);
            }
            base.ScalarExecuted(command, interceptionContext);
        }

        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }

        public override void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            _stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                _logger.Error(interceptionContext.Exception, "Error ejecutando instruccion: {0}", command.CommandText);
            }
            else
            {
                _logger.Trace("NonQueryExecuted TS:{0} \t| Servidor: {1} \t| Base de Datos: {2} \t| Instruccion: {3}", _stopwatch.Elapsed, command.Connection.DataSource, command.Connection.Database, command.CommandText);
            }
            base.NonQueryExecuted(command, interceptionContext);
        }

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            base.ReaderExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }
        public override void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            _stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                _logger.Error(interceptionContext.Exception, "Error ejecutando instruccion: {0}", command.CommandText);
            }
            else
            {
                _logger.Trace("ReaderExecuted TS:{0} \t| Servidor: {1} \t| Base de Datos: {2} \t| Instruccion: {3}", _stopwatch.Elapsed, command.Connection.DataSource, command.Connection.Database, command.CommandText);
            }
            base.ReaderExecuted(command, interceptionContext);
        }
    }
}
