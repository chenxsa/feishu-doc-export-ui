﻿using feishu_doc_export.Dtos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiClientCore.Extensions.OAuths;
using WebApiClientCore.Extensions.OAuths.TokenProviders;

namespace feishu_doc_export.HttpApi
{
    public class FeiShuTokenProvider : TokenProvider
    {
        private readonly IFeiShuHttpApi _feiShuHttpApi;

        public FeiShuTokenProvider(IServiceProvider services) : base(services)
        {
            _feiShuHttpApi = services.GetService<IFeiShuHttpApi>();
        }

        protected override async Task<TokenResult> RefreshTokenAsync(IServiceProvider serviceProvider, string refresh_token)
        {
            return await RequestTokenAsync(serviceProvider);
        }

        protected override async Task<TokenResult> RequestTokenAsync(IServiceProvider serviceProvider)
        {
            var tokenResult = new TokenResult();

            var requestData = RequestData.CreateAccessToken(GlobalConfig.AppId, GlobalConfig.AppSecret);
            var result = await _feiShuHttpApi.GetTenantAccessToken(requestData);

            tokenResult.Access_token = result.TenantAccessToken;
            tokenResult.Refresh_token = result.TenantAccessToken;
            tokenResult.Expires_in = result.Expire;

            return tokenResult;
        }
    }
}
