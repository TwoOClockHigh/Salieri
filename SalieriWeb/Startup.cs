﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
[assembly: OwinStartup(typeof(SalieriWeb.Startup))]

namespace SalieriWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            
            
        }
    }
}
