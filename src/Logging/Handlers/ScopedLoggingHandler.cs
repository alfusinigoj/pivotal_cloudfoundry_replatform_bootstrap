﻿using Microsoft.Extensions.Logging;
using PivotalServices.AspNet.Bootstrap.Extensions.Handlers;
using PivotalServices.AspNet.Bootstrap.Extensions.Ioc;
using Serilog.Context;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Handlers
{
    public class ScopedLoggingHandler : DynamicHttpHandlerBase
    {
        IEnumerable<IDynamicMessageProcessor> messageProcessors;
        const string CORR_CONTXT = "CorrelationContext";
        const string REQ_PATH_LOG_PROP_NM = "RequestPath";

        public ScopedLoggingHandler(ILogger<ScopedLoggingHandler> logger)
           : base(logger)
        {
        }

        public override string Path => null;

        public override DynamicHttpHandlerEvent ApplicationEvent => DynamicHttpHandlerEvent.BeginRequest;

        public override void HandleRequest(HttpContextBase context)
        {
            if (messageProcessors == null)
            {
                messageProcessors = DependencyContainer.GetService<IEnumerable<IDynamicMessageProcessor>>()
                   ?? throw new ArgumentNullException(nameof(IEnumerable<IDynamicMessageProcessor>));

                if (!messageProcessors.Any())
                    throw new Exception("No message procesors of type 'IDynamicMessageProcessor' found");
            }

            var request = DiagnosticHelpers.GetProperty<HttpRequestBase>(context, "Request");

            if (request == null) return;

            PushCorelationProperties(request);
        }

        public override bool ContinueNext(HttpContextBase context)
        {
            return true;
        }

        private void PushCorelationProperties(HttpRequestBase request)
        {
            var correlationContextInfo = string.Empty;

            foreach (var processor in messageProcessors)
            {
                correlationContextInfo = processor.Process(correlationContextInfo);
            }

            LogContext.PushProperty(CORR_CONTXT, correlationContextInfo, true);
            LogContext.PushProperty(REQ_PATH_LOG_PROP_NM, request.Url, true);
        }
    }
}
