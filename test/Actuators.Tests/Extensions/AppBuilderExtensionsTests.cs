﻿using PivotalServices.AspNet.Bootstrap.Extensions.Testing;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Actuators.Tests
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
            Assert.Equal(2, TestProxy.ConfigureServicesDelegatesProxy.Count);

            Assert.Equal("/cloudfoundryapplication", TestProxy.InMemoryConfigStoreProxy["management:endpoints:path"]);
            Assert.Equal("false", TestProxy.InMemoryConfigStoreProxy["management:endpoints:cloudfoundry:validateCertificates"]);
            Assert.Equal("${vcap:application:name}", TestProxy.InMemoryConfigStoreProxy["info:ApplicationName"]);
            Assert.Equal("${ASPNETCORE_ENVIRONMENT}", TestProxy.InMemoryConfigStoreProxy["info:CurrentEnvironment"]);
            Assert.Equal("PivotalServices.Bootstrap.Extensions.Cf.Actuators.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TestProxy.InMemoryConfigStoreProxy["info:AssemblyInfo"]);
        }

        [Fact]
        public void Test_AddCloudFoundryActuators_AddsIntoActuatorCollection_WithBasePath()
        {
            TestProxy.InMemoryConfigStoreProxy.Clear();
            TestProxy.ConfigureServicesDelegatesProxy.Clear();
            TestProxy.ActuatorsProxy.Clear();

            AppBuilder.Instance.AddCloudFoundryActuators("/foo");
            Assert.Equal(2, TestProxy.ConfigureServicesDelegatesProxy.Count);

            Assert.Single(TestProxy.ActuatorsProxy);
            Assert.Equal("/foo/cloudfoundryapplication", TestProxy.InMemoryConfigStoreProxy["management:endpoints:path"]);
            Assert.Equal("false", TestProxy.InMemoryConfigStoreProxy["management:endpoints:cloudfoundry:validateCertificates"]);
        }

        [Fact]
        public void Test_AddCloudFoundryMetricsForwarder_AddsIntoActuatorCollection()
        {
            TestProxy.InMemoryConfigStoreProxy.Clear();
            TestProxy.ConfigureServicesDelegatesProxy.Clear();
            TestProxy.ActuatorsProxy.Clear();

            AppBuilder.Instance.AddCloudFoundryMetricsForwarder();

            Assert.Single(TestProxy.ActuatorsProxy);
            Assert.Equal(2, TestProxy.ConfigureServicesDelegatesProxy.Count);

            Assert.Equal("false", TestProxy.InMemoryConfigStoreProxy["management:metrics:exporter:cloudfoundry:validateCertificates"]);
        }
    }
}
