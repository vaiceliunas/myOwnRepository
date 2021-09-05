using Idp4.Models.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Idp4.Models.Stores;
using Microsoft.AspNetCore.Authentication.Facebook;
using IdentityServer4;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using System.Security;

namespace Idp4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);

            var builder = services.AddIdentityServer()
                .AddUserStore()
                .AddPersistedGrantStore<IdpPersistedGrantStore>()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryClients(Config.GetClients());
            services.AddDbContext<UserDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("VaiceliunasDb")));

            services.AddAuthentication()
                .AddFacebook("Facebook", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.AppId = Configuration.GetValue<string>("FacebookAppId");
                    options.AppSecret = Configuration.GetValue<string>("FacebookAppSecret");
                })
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = Configuration.GetValue<string>("GoogleAppId");
                    options.ClientSecret = Configuration.GetValue<string>("GoogleAppSecret");
                });
            
            string thumbPrint = Configuration.GetValue<string>("ThumbPrint");
            if (false)
                builder.AddDeveloperSigningCredential();
            else
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var kvClient = new KeyVaultClient((authority, resource, scope) => azureServiceTokenProvider.KeyVaultTokenCallback(authority, resource, scope));
                var certificateSecret = kvClient.GetSecretAsync("https://arnasidpkeyvault.vault.azure.net/", "arnasIdpCert").Result;
                var privateKeyBytes = Convert.FromBase64String(certificateSecret.Value);
                X509Certificate2 Certificate = new X509Certificate2(privateKeyBytes,
                                                 new SecureString(),
                                                 X509KeyStorageFlags.MachineKeySet |
                                                 X509KeyStorageFlags.PersistKeySet |
                                                 X509KeyStorageFlags.Exportable);
                builder.AddSigningCredential(Certificate);
                //builder.AddSigningCredential(LoadCertificateFromStore(thumbPrint));
            }

            //builder.AddSigningCredential(LoadCertificateFromStore(thumbPrint));
            //builder.AddValidationKey()
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory factory)
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole().AddDebug());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();
            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }

        public X509Certificate2 LoadCertificateFromStore(string thumbPrint)
        {
            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint,
                    thumbPrint, true);
                if (certCollection.Count == 0)
                {
                    throw new Exception("The specified certificate wasn't found.");
                }
                return certCollection[0];
            }
        }
    }
}
