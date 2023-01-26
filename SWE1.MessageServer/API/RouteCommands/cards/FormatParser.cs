using System.Text.RegularExpressions;

namespace SWE1.MessageServer.API.RouteCommands.cards
{
    internal class FormatParser : IRouteParser
    {
        public bool IsMatch(string resourcePath, string routePattern)
        {
            var pattern = "^" + routePattern.Replace("{format}", ".*").Replace("/", "\\/") + "(\\?.*)?$";
            return Regex.IsMatch(resourcePath, pattern);
        }

        public Dictionary<string, string> ParseParameters(string resourcePath, string routePattern)
        {
            // query parameters
            var parameters = ParseQueryParameters(resourcePath);

            // id parameter
            var format = ParseformatParameter(resourcePath, routePattern);
            if (format != null)
            {
                parameters["format"] = format;
            }

            return parameters;
        }

        private string? ParseformatParameter(string resourcePath, string routePattern)
        {
            var pattern = "^" + routePattern.Replace("{format}", "(?<format>[^\\?\\/]*)").Replace("/", "\\/") + "$";
            var result = Regex.Match(resourcePath, pattern);
            return result.Groups["format"].Success ? result.Groups["format"].Value : null;
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