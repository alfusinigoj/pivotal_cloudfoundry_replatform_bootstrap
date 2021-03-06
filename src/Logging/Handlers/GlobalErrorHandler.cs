﻿using Microsoft.Extensions.Logging;
using PivotalServices.AspNet.Bootstrap.Extensions.Handlers;
using System;
using System.Diagnostics;
using System.Web;
using System.Web.Hosting;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Handlers
{
    public class GlobalErrorHandler : DynamicHttpHandlerBase
    {
        public GlobalErrorHandler(ILogger<GlobalErrorHandler> logger)
            : base(logger)
        {
        }

        public override string Path => null;

        public override DynamicHttpHandlerEvent ApplicationEvent => DynamicHttpHandlerEvent.Error;

        public override void HandleRequest(HttpContextBase context)
        {
            try
            {
                var lastError = context.Server.GetLastError();

                if (lastError == null)
                    lastError = new Exception("Unknown/Unhandled application error, no further details available");

                LogError(lastError);
            }
            catch (Exception exception)
            {
                LogError(exception);
            }
        }

        public override bool ContinueNext(HttpContextBase context)
        {
            return true;
        }

        private void LogError(Exception exception)
        {
            this.Logger().Log(LogLevel.Error, exception, exception.ToString());

            try { EventLog.WriteEntry(HostingEnvironment.ApplicationHost.GetSiteName(), exception.ToString()); } catch { }
        }
    }
}
