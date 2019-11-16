﻿using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Observers;
using Steeltoe.Common.Diagnostics;
using System;
using System.Threading.Tasks;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Handlers
{
    public class InboundEndRequestObserverHandler : DynamicHttpHandlerBase
    {
        IInboundRequestObserver observer;

        public InboundEndRequestObserverHandler(IInboundRequestObserver observer, ILogger<InboundEndRequestObserverHandler> logger)
             : base(logger)
        {
            this.observer = observer ?? throw new ArgumentNullException(nameof(observer));
        }

        public override string Path => null;

        public override DynamicHttpHandlerEvent ApplicationEvent => DynamicHttpHandlerEvent.EndRequest;

        public override void HandleRequest(HttpContextBase context)
        {
            var request = DiagnosticHelpers.GetProperty<HttpRequestBase>(context, "Request");

            observer.ProcessEvent(InboundRequestObserver.STOP_EVNT, context);
        }

        public override async Task<bool> ContinueNextAsync(HttpContextBase context)
        {
            return await Task.FromResult(result: true);
        }
    }
}