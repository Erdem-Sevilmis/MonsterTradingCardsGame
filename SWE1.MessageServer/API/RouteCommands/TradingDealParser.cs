using System.Text.RegularExpressions;

namespace SWE1.MessageServer.API.RouteCommands
{
    internal class TradingDealParser : IRouteParser
    {
        public bool IsMatch(string resourcePath, string routePattern)
        {
            var pattern = "^" + routePattern.Replace("{tradingdealid}", ".*").Replace("/", "\\/") + "(\\?.*)?$";
            return Regex.IsMatch(resourcePath, pattern);
        }

        public Dictionary<string, string> ParseParameters(string resourcePath, string routePattern)
        {
            // query parameters
            var parameters = ParseQueryParameters(resourcePath);

            // id parameter
            var tradingdealid = ParsetradingdealidParameter(resourcePath, routePattern);
            if (tradingdealid != null)
            {
                parameters["tradingdealid"] = tradingdealid;
            }

            return parameters;
        }

        private string? ParsetradingdealidParameter(string resourcePath, string routePattern)
        {
            var pattern = "^" + routePattern.Replace("{tradingdealid}", "(?<tradingdealid>[^\\?\\/]*)").Replace("/", "\\/") + "$";
            var result = Regex.Match(resourcePath, pattern);
            return result.Groups["tradingdealid"].Success ? result.Groups["tradingdealid"].Value : null;
        }

        private Dictionary<string, string> ParseQueryParameters(string route)
        {
            var parameters = new Dictionary<string, string>();

            var query = route
                .Split("?", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .FirstOrDefault();

            if (query != null)
            {
                var keyValues = query
                    .Split("&", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                    .Where(p => p.Length == 2);

                foreach (var p in keyValues)
                {
                    parameters[p[0]] = p[1];
                }
            }

            return parameters;
        }
    }
}