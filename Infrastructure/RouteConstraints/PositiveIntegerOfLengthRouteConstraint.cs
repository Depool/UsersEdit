using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Infrastructure.RouteConstraints
{
    public class PositiveIntegerOfLengthRouteConstraint : IRouteConstraint
    {
        private string routeKeyName;

        public PositiveIntegerOfLengthRouteConstraint(string routeKeyName)
        {
            this.routeKeyName = routeKeyName;
        }

        public bool Match(System.Web.HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            object res;
            values.TryGetValue(routeKeyName, out res);

            if (res is string)
            {
                string strVal = res as String;

                int intValue;

                if (strVal.Length <= 6 && Int32.TryParse(strVal, out intValue))
                {
                    return intValue > 0;
                }
            }

            return false;
        }
    }
}
