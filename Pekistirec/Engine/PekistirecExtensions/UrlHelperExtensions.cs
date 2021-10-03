using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Pekistirec
{
    public static class UrlHelperExtensions
    {
        public static string Action<TController>(this UrlHelper urlHelper, Expression<Func<TController, object>> actionExpression)
        {
            var controllerName = typeof(TController).Name.Replace("Controller", "");
            string expressionBody = actionExpression.Body.ToString();
            var actionName = expressionBody.Substring(expressionBody.IndexOf('.') + 1).Substring(0, expressionBody.IndexOf('(') - 2);
            return urlHelper.Action(actionName, controllerName);
        }

    }



    
}