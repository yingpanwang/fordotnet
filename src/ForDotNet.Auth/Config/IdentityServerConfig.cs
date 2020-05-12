using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForDotNet.Auth.Config
{

    /// <summary>
    /// IdentityServer4配置信息
    /// </summary>
    public static class IdentityServerConfig
    {
        private const string AuthClientId = "Auth";
        private const string UserCenterClientId = "UserCenter";

        /// <summary>
        /// 获取api资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("Auth","AuthApi"),
                new ApiResource("UserCenter","UserCenterApi")
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

            Client userClient = new Client() 
            {
                ClientId = UserCenterClientId,
                ClientSecrets = new List<Secret>()
                {
                    GetSecret(UserCenterClientId)
                },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes = new string[]
                {
                    "UserCenter"
                }
            };

            return new List<Client>()
            {
                authClient,
                userClient
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources() 
        {
            return new List<IdentityResource>() 
            {
               
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

        #endregion
    }
}
