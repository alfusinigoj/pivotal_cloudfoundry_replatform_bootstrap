﻿using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Actuators;
using PivotalServices.AspNet.Bootstrap.Extensions.Reflection;
using System.Collections.Generic;
using System.Reflection;

namespace PivotalServices.AspNet.Bootstrap.Extensions
{
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// Enables all CF Actuators for Apps Manager
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public static AppBuilder AddCloudFoundryActuators(this AppBuilder instance, string basePath = null)
        {
            var inMemoryConfigStore = ReflectionHelper
                .GetNonPublicInstancePropertyValue<Dictionary<string, string>>(instance, "InMemoryConfigStore");

            inMemoryConfigStore["Logging:LogLevel:Default"] = "Information";
            inMemoryConfigStore["Logging:LogLevel:System"] = "Warning";
            inMemoryConfigStore["Logging:LogLevel:Microsoft"] = "Warning";
            inMemoryConfigStore["Logging:LogLevel:Steeltoe"] = "Warning";
            inMemoryConfigStore["Logging:LogLevel:Pivotal"] = "Warning";
            inMemoryConfigStore["Logging:Console:IncludeScopes"] = "true";

            if (string.IsNullOrWhiteSpace(basePath))
                inMemoryConfigStore["management:endpoints:path"] = "/cloudfoundryapplication";
            else
                inMemoryConfigStore["management:endpoints:path"] = $"{basePath.TrimEnd('/')}/cloudfoundryapplication";

            inMemoryConfigStore["management:endpoints:enabled"] = "true";
            inMemoryConfigStore["management:endpoints:cloudfoundry:validateCertificates"] = "false";

            inMemoryConfigStore["info:ApplicationName"] = "${vcap:application:name}";
            inMemoryConfigStore["info:CurrentEnvironment"] = "${ASPNETCORE_ENVIRONMENT}";
            inMemoryConfigStore["info:AssemblyInfo"] = Assembly.GetCallingAssembly().FullName;

            ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<IActuator>>(instance, "Actuators").Add(new CfActuator());

            instance.AddDefaultConfigurations();
            instance.AddConfigServer();

            return instance;
        }

        public static AppBuilder AddCloudFoundryMetricsForwarder(this AppBuilder instance)
        {
            var inMemoryConfigStore = ReflectionHelper
                .GetNonPublicInstancePropertyValue<Dictionary<string, string>>(instance, "InMemoryConfigStore");

            inMemoryConfigStore["management:metrics:exporter:cloudfoundry:validateCertificates"] = "false";

            ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<IActuator>>(instance, "Actuators").Add(new CfMetricsForwarder());

            instance.AddDefaultConfigurations();
            instance.AddConfigServer();

            return instance;
        }
    }
}
