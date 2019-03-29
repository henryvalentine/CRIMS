﻿using System;
using System.Web;

namespace Crims.API.Helpers
{
    public static class GenericHelpers
    {
        public static string MapPath(string path)
        {
            return @"~/" + path.Replace(HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"], String.Empty).Replace('\\', '/');
        }
    }
}