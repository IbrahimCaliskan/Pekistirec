

using System;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace Pekistirec.Engine.Toolbox.GenelToolbox
{
    public class ControllerNameAndActionName
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }

    public class GenelTools
    {
        public static string GetMd5Hash(string input)
        {
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
               
                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }                
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static ControllerNameAndActionName GetControllerAndActionName<TController>(Expression<Func<TController, object>> actionExpression) where TController: System.Web.Mvc.IController
        {
            var result = new ControllerNameAndActionName();

            result.ControllerName = typeof(TController).Name.Replace("Controller", "");
            string expressionBody = actionExpression.Body.ToString();
            result.ActionName = expressionBody.Substring(expressionBody.IndexOf('.') + 1).Substring(0, expressionBody.IndexOf('(') - 2);
            return result;
        }

        public static string GetCorrectPropertyName<T>(Expression<Func<T, Object>> expression)
        {
            if (expression.Body is MemberExpression)
            {
                return ((MemberExpression)expression.Body).Member.Name;
            }

            if (expression.Body is UnaryExpression)
            {
                var op = ((UnaryExpression)expression.Body).Operand;
                return ((MemberExpression)op).Member.Name;
            }

            return null;
        }
    }
}