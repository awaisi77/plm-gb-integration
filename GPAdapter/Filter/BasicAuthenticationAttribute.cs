﻿using GPDataLayer.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GPAdapter.Filter
{
    public class BasicAuthenticationAttribute
    : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                var authToken = actionContext.Request.Headers
                    .Authorization.Parameter;

                // decoding authToken we get decode value in 'Username:Password' format  
                var decodeauthToken = System.Text.Encoding.UTF8.GetString(
                    Convert.FromBase64String(authToken));

                // spliting decodeauthToken using ':'   
                var arrUserNameandPassword = decodeauthToken.Split(':');

                // at 0th postion of array we get username and at 1st we get password  
                if (IsAuthorizedUser(arrUserNameandPassword[0], arrUserNameandPassword[1]))
                {
                    // setting current principle  
                    Thread.CurrentPrincipal = new GenericPrincipal(
                           new GenericIdentity(arrUserNameandPassword[0]), null);
                }
                else
                {

                    actionContext.Response = actionContext.Request
                        .CreateResponse(HttpStatusCode.Unauthorized, new GenericResponseAPI<string>(false, "UNAUTHORIZED", "UNAUTHORIZED", GPDataLayer.Data.Enums.ResponseCode.UNAUTHORIZED));
                }
            }
            else
            {
                actionContext.Response = actionContext.Request
                   .CreateResponse(HttpStatusCode.Unauthorized, new GenericResponseAPI<string>(false, "UNAUTHORIZED", "UNAUTHORIZED", GPDataLayer.Data.Enums.ResponseCode.UNAUTHORIZED));

            }
        }
        public static bool IsAuthorizedUser(string Username, string Password)
        {
            // In this method we can handle our database logic here...  
            return Username == "GS_INT_USER" && Password == "gosaas@123";
        }
    }
}