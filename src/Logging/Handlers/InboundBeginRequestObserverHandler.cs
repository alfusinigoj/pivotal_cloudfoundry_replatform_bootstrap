﻿using Microsoft.Extensions.Logging;
using PivotalServices.AspNet.Bootstrap.Extensions.Handlers;
using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Observers;
using Steeltoe.Common.Diagnostics;
using System;
using System.Web;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Handlers
{
    public class InboundBeginRequestObserverHandler : DynamicHttpHandlerBase
    {
        IInboundRequestObserver observer;

        public InboundBeginRequestObserverHandler(IInboundRequestObserver observer, ILogger<InboundBeginRequestObserverHandler> logger)
             : base(logger)
        {
            this.observer = observer ?? throw new ArgumentNullException(nameof(observer));
        }

        public override string Path => null;

        public override DynamicHttpHandlerEvent ApplicationEvent => DynamicHttpHandlerEvent.BeginRequest;

        public override void HandleRequest(HttpContextBase context)
        {
            var request = DiagnosticHelpers.GetProperty<HttpRequestBase>(context, "Request");

            if (request == null)
                return;

            observer.ProcessEvent(InboundRequestObserver.START_EVNT, context);
        }

        public override bool ContinueNext(HttpContextBase context)
        {
            return true;
        }
    }
}
