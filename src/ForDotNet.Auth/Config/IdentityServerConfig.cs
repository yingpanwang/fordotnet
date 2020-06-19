using IdentityServer4.Models;
using System.Collections.Generic;

namespace ForDotNet.Auth.Config
{
    /// <summary>
    /// IdentityServer4配置信息
    /// </summary>
    public static class IdentityServerConfig
    {
        private const string AuthClientId = "Auth";
        private const string Api1ClientId = "Api1";

        /// <summary>
        /// 获取api资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("Auth","AuthApi"),
                // new ApiResource("Api1","Api1"),
                new ApiResource()
                {
                    Name = "Api1",
                    DisplayName = "Api1Display",
                    Scopes = new Scope[]
                    {
                         new Scope("Api1","This Api1 Scope"),
                         new Scope ("Business","This is Business Scope"),
                         new Scope ("Admin","This Admin Scope")
                    }
                    
                }
            };
        }

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            Client authClient = new Client()
            {
                ClientId = AuthClientId,
                ClientSecrets = new List<Secret>()
                {
                    GetSecret(AuthClientId)
                },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes = new string[]
                {
                    "Auth"
                }
            };

            Client api1Client = new Client()
            {
                ClientId = Api1ClientId,
                ClientSecrets = new List<Secret>()
                {
                    GetSecret(Api1ClientId)
                },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes = new string[]
                {
                    "Api1",
                    "Admin"
                }
            };

            return new List<Client>()
            {
                authClient,
                api1Client
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId()
            };
        }

        #region 私有方法

        /// <summary>
        /// 获取Secret
        /// </summary>
        /// <param name="clientId">clientId</param>
        /// <returns></returns>
        private static Secret GetSecret(string clientId)
        {
            return new Secret(clientId.Sha256());
        }

        #endregion 私有方法
    }
}