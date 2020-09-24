using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace JWTDemo2.Ultil
{
    public class CustomAuthenticationFilter: AuthorizeAttribute, IAuthenticationFilter
    {
        public bool AllowMultiple
        {
            get { return false; }
        }

    
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            string authParater = string.Empty;
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            string[] TokenAndUser = null;

            if (authorization == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Miss auth header", request);
                return;
            }
            if (authorization.Scheme != "bearer")
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid auth schema", request);
                return;

            }
            TokenAndUser = authorization.Parameter.Split(' ');
            string token = TokenAndUser[0];
            // string userName = TokenAndUser[1];
            if (String.IsNullOrEmpty(token))
            {
                context.ErrorResult = new AuthenticationFailureResult("Miss Token", request);
                return;

            }

            string validateUserName = TokenManager.ValidateToken(authorization.Parameter);
            //if(userName!=validateUserName)
            //{
            //    context.ErrorResult = new AuthenticationFailreResult("Ivalid Token", request);
            //    return;

            //}

            context.Principal = TokenManager.GetPrincipal(authorization.Parameter);
        }

     
        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var reuslt = await context.Result.ExecuteAsync(cancellationToken);
            if (reuslt.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                reuslt.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Basic", "realm=localhost"));
            }
            context.Result = new ResponseMessageResult(reuslt);
        }
    }
}

public class AuthenticationFailureResult : IHttpActionResult
{
    public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
    {
        ReasonPhrase = reasonPhrase;
        Request = request;
    }

    public string ReasonPhrase { get; private set; }

    public HttpRequestMessage Request { get; private set; }

    public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(Execute());
    }

    private HttpResponseMessage Execute()
    {
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        response.RequestMessage = Request;
        response.ReasonPhrase = ReasonPhrase;
        return response;
    }
}