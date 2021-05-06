using System;
using System.Linq;
using A5Soft.CARMA.Domain;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace A5Soft.A5App.WebApp
{
    /// <inheritdoc cref="A5Soft.CARMA.Application.ILogger"/>
    [DefaultServiceImplementation(typeof(A5Soft.CARMA.Application.ILogger))]
    public class Logger : A5Soft.CARMA.Application.ILogger
    {
        private const string SecurityIssueTag = "[SecurityIssue]";
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        private readonly ILogger _logger;


        public Logger(ILogger<Logger> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public void LogCritical(Exception exception, params object[] args)
        {
            DoLog(LogLevel.Critical, exception, args);
        }

        public void LogDebug(Exception exception, params object[] args)
        {
            DoLog(LogLevel.Debug, exception, args);
        }

        public void LogDebug(string message, params object[] args)
        {
            DoLog(LogLevel.Debug, message, args);
        }

        public void LogError(Exception exception, params object[] args)
        {
            DoLog(LogLevel.Error, exception, args);
        }

        public void LogMethodEntry(Type ownerType, string methodName, params object[] methodParams)
        {
            DoLog(LogLevel.Information, $"Enter {methodName} on type {ownerType?.FullName}.", methodParams);
        }

        public void LogMethodExit(Type ownerType, string methodName, object returnValue = null)
        {
            DoLog(LogLevel.Information, $"Successfully completed {methodName} on type {ownerType?.FullName}.", returnValue);
        }

        public void LogSecurityIssue(Exception exception, params object[] args)
        {
            DoLog(LogLevel.Warning, new Exception($"{SecurityIssueTag} {exception?.Message}", exception), args);
        }

        public void LogSecurityIssue(string message, params object[] args)
        {
            DoLog(LogLevel.Warning, $"{SecurityIssueTag} {message}", args);
        }

        public void LogWarning(Exception exception, params object[] args)
        {
            DoLog(LogLevel.Warning, exception, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            DoLog(LogLevel.Warning, message, args);
        }


        private void DoLog(LogLevel level, string message, params object[] args)
        {
            if (null == args || args.Length < 1)
            {
                _logger.Log(level, message);
            }
            else
            {
                _logger.Log(level, (message ?? string.Empty) + Environment.NewLine 
                    + string.Join(Environment.NewLine, args.Select(o => ParamToString(o))));
            }
        }

        private void DoLog(LogLevel level, Exception ex, params object[] args)
        {
            if (null == args || args.Length < 1)
            {
                _logger.Log(level, $"{ex?.Message}{Environment.NewLine}{ParamToString(ex)}");
            }
            else
            {
                _logger.Log(level, $"{ex?.Message}{Environment.NewLine}{ParamToString(ex)}{Environment.NewLine}" +
                    string.Join(Environment.NewLine, args.Select(o => ParamToString(o))));
            }
        }

        private string ParamToString(object parameter)
        {
            if (object.ReferenceEquals(null, parameter)) return "null";

            var parameterType = parameter.GetType();

            if (parameterType.IsPrimitive || parameterType == typeof(string)
                || parameterType == typeof(decimal) || parameterType == typeof(Guid)) 
                return parameter.ToString();

            return JsonConvert.SerializeObject(parameter, parameterType, _serializerSettings);
        }

    }
}
