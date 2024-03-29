﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using payment.Constants;
using payment.Interfaces.Operations;

namespace payment.Operations
{
    public abstract class ApiOperation
    {
        protected abstract string? Route { get; }
        protected virtual string? ClientToken => configurationOperation.Get<string>(ConfigurationConstants.CLIENT_TOKEN);


        protected readonly IConfigurationOperation configurationOperation;

        public ApiOperation(IConfigurationOperation configurationOperation)
        {
            this.configurationOperation = configurationOperation;
        }

        protected IDictionary<string, string> DefaultHeaders
        {
            get
            {
                var headers = new Dictionary<string, string>();
                headers.Add(HeaderNames.Authorization, string.Concat(JwtBearerDefaults.AuthenticationScheme, " ", ClientToken));
                return headers;
            }
        }
    }
}
