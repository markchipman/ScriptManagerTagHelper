﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ScriptManagerTagHelper.WebSample
{

    /// <summary>
    /// http://www.binaryintellect.net/articles/439edbad-fd51-4eaf-953a-5484941c7e8c.aspx
    /// http://blog.dudak.me/2014/custom-middleware-with-dependency-injection-in-asp-net-core/
    /// </summary>
    public class MyMiddleware
    {
        private readonly RequestDelegate nextMiddleware;
        private readonly IScriptManager _scriptManager;

        public MyMiddleware(RequestDelegate next, IScriptManager scriptManager)
        {
            this.nextMiddleware = next;
            _scriptManager = scriptManager;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method.ToUpper() == "GET")
            {
                

                Stream originalStream = context.Response.Body;

                using (MemoryStream newStream = new MemoryStream())
                {


                    context.Response.Body = newStream;
                    await this.nextMiddleware.Invoke(context);
                    context.Response.Body = originalStream;
                    newStream.Seek(0, SeekOrigin.Begin);
                    StreamReader reader = new StreamReader(newStream);
                    var htmlData = reader.ReadToEnd();

                    if (_scriptManager.Scripts.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var scriptRef in _scriptManager.Scripts.OrderBy(a => a.IncludeOrderPriorty))
                        {
                            sb.AppendLine(string.Format("<script src='{0}' ></script>", scriptRef.ScriptPath));
                        }
                        htmlData = htmlData.Replace("</body>", "</body>" + sb);

                    }
                    await context.Response.WriteAsync(htmlData);
                }
            }
            else
            {
                await this.nextMiddleware.Invoke(context);
            }
        }
    }
}