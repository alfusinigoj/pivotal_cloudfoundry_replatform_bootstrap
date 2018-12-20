﻿using Microsoft.Extensions.Configuration;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Configuration
{
    public class WebConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new WebConfigurationProvider();
        }
    }
}