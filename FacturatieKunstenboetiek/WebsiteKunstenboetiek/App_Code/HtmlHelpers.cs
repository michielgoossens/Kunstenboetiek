﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WebsiteKunstenboetiek.App_Code
{
    public static class HtmlHelpers
    {

        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper,
                                            string linkText,
                                            string actionName,
                                            string controllerName
                                            )
        {

            string currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            string currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            if (actionName == currentAction && controllerName == currentController)
            {
                return htmlHelper.ActionLink(linkText, actionName, controllerName, null, new { @class = "selected" });
            }

            return htmlHelper.ActionLink(linkText, actionName, controllerName);


        }
    }
}