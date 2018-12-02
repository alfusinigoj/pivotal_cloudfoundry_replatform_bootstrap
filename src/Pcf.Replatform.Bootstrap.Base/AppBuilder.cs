﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base
{
    public class AppBuilder
    {
        private AppBuilder() { }

        private static Action<HostBuilderContext, IConfigurationBuilder> configureAppConfiguration;
        private static Action<HostBuilderContext, IServiceCollection> configureServices;
        private static Action<ILoggingBuilder> configureLogging;

        public static readonly AppBuilder Instance = new AppBuilder();

        public AppBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            configureAppConfiguration = configureDelegate;
            return Instance;
        }

        public AppBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            configureServices = configureDelegate;
            return Instance;
        }

        public AppBuilder ConfigureLogging(Action<ILoggingBuilder> configureDelegate)
        {
            configureLogging = configureDelegate;
            return Instance;
        }

        public void Build()
        {
            AppConfig.Configure(configureAppConfiguration, configureServices, configureLogging);
        }

        public static T GetService<T>()
        {
            return AppConfig.GetService<T>();
        }
    }
}
