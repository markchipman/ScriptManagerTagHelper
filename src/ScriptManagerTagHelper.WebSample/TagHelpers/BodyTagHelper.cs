﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace ScriptManagerTagHelper.WebSample.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project

    
    public class BodyTagHelper : TagHelper
    {
        private readonly IScriptManager _scriptManager;
        private readonly IOptions<ScriptManagerOptions> _scriptManagerOptions;
        private readonly IHostingEnvironment _hostingEnvironment;

        public BodyTagHelper(IScriptManager scriptManager,
            IOptions<ScriptManagerOptions> scriptManagerOptions,
            IHostingEnvironment hostingEnvironment)
        {
            _scriptManager = scriptManager;
            _scriptManagerOptions = scriptManagerOptions;
            _hostingEnvironment = hostingEnvironment;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            // hostingEnvironment gets me path to file system file

            // from string, need to figure out if it's http type or file system type,
            // don't minimizie if not file system type

            var allScripts = _scriptManager.Scripts; // all my scripts to generate

           
            var combine = _scriptManagerOptions.Value.CombinedSrc;

            var combinedScriptTexts = _scriptManager.ScriptTexts;

            // output filenames first, then combine in script tag the combined scripttexts
            // .Append (return multiple times so OK to return without variable, resharper area)
            output.PostContent.Append("this is all the script tags with ...");

            // NOTE: make sure to add CRC cache breaker on javascripts




        }
    }
}
