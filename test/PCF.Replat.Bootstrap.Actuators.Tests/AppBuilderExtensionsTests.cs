﻿using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Testing;
using Xunit;

namespace PCF.Replat.Bootstrap.Actuators.Tests
{
    public class AppBuilderExtensionsTests
    {
        [Fact]
        public void Test_AddCloudFoundryHealthActuators_AddsIntoActuatorCollection()
        {
            TestProxy.InMemoryConfigStoreProxy.Clear();
            TestProxy.ConfigureServicesDelegatesProxy.Clear();
            TestProxy.ActuatorsProxy.Clear();
            AppBuilder.Instance.AddCloudFoundryActuators();
            Assert.Single(TestProxy.ActuatorsProxy);
            Assert.Single(TestProxy.ConfigureServicesDelegatesProxy);
            Assert.Equal("/cloudfoundryapplication", TestProxy.InMemoryConfigStoreProxy["management:endpoints:path"]);
            Assert.Equal("false", TestProxy.InMemoryConfigStoreProxy["management:endpoints:cloudfoundry:validateCertificates"]);
            Assert.Equal("${vcap:application:name}", TestProxy.InMemoryConfigStoreProxy["info:ApplicationName"]);
            Assert.Equal("${ASPNETCORE_ENVIRONMENT}", TestProxy.InMemoryConfigStoreProxy["info:CurrentEnvironment"]);
            Assert.Equal("PCF.Replat.Bootstrap.Actuators.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TestProxy.InMemoryConfigStoreProxy["info:AssemblyInfo"]);
        }

        [Fact]
        public void Test_AddCloudFoundryActuators_AddsIntoActuatorCollection_WithBasePath()
        {
            TestProxy.InMemoryConfigStoreProxy.Clear();
            TestProxy.ConfigureServicesDelegatesProxy.Clear();
            TestProxy.ActuatorsProxy.Clear();
            AppBuilder.Instance.AddCloudFoundryActuators("/foo");
            Assert.Single(TestProxy.ActuatorsProxy);
            Assert.Single(TestProxy.ConfigureServicesDelegatesProxy);
            Assert.Equal("/foo/cloudfoundryapplication", TestProxy.InMemoryConfigStoreProxy["management:endpoints:path"]);
            Assert.Equal("false", TestProxy.InMemoryConfigStoreProxy["management:endpoints:cloudfoundry:validateCertificates"]);
        }

        [Fact]
        public void Test_AddCloudFoundryMetricsForwarder_AddsIntoActuatorCollection()
        {
            TestProxy.InMemoryConfigStoreProxy.Clear();
            TestProxy.ActuatorsProxy.Clear();
            AppBuilder.Instance.AddCloudFoundryMetricsForwarder();
            Assert.Single(TestProxy.ActuatorsProxy);
            Assert.Equal("false", TestProxy.InMemoryConfigStoreProxy["management:metrics:exporter:cloudfoundry:validateCertificates"]);
        }
    }
}
