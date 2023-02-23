using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Newtonsoft.Json;
using SWE1.MessageServer.API.RouteCommands.cards;
using SWE1.MessageServer.API.RouteCommands.game;
using SWE1.MessageServer.API.RouteCommands.packages;
using SWE1.MessageServer.API.RouteCommands.trading;
using SWE1.MessageServer.API.RouteCommands.Users;
using SWE1.MessageServer.BLL.cards;
using SWE1.MessageServer.BLL.game;
using SWE1.MessageServer.BLL.package;
using SWE1.MessageServer.BLL.trading;
using SWE1.MessageServer.BLL.user;
using SWE1.MessageServer.Core.Request;
using SWE1.MessageServer.Core.Routing;
using SWE1.MessageServer.Models;
using HttpMethod = SWE1.MessageServer.Core.Request.HttpMethod;

namespace SWE1.MessageServer.API.RouteCommands
{
    internal class Router : IRouter
    {
        private readonly IUserManager _userManager;
        private readonly ICardsManager _cardsManager;
        private readonly IPackageManager _packageManager;
        private readonly IGameManager _gameManager;
        private readonly ITradingManager _tradingManager;
        private readonly IdentityProvider _identityProvider;
        private readonly IRouteParser _routeParserUsername = new UsernameRouteParser();
        private readonly IRouteParser _routeParserTradingdealId = new TradingDealParser();
        private readonly IRouteParser _routeParserFormat = new FormatParser();

        public Router(IUserManager userManager, ICardsManager cardsManager, IPackageManager packageManager, IGameManager gameManager, ITradingManager tradingManager)
        {
            _userManager = userManager;
            _cardsManager = cardsManager;
            _packageManager = packageManager;
            _gameManager = gameManager;
            _tradingManager = tradingManager;
            // better: define IIdentityProvider interface and get concrete implementation passed in as dependency
            _identityProvider = new IdentityProvider(userManager);
        }

        public IRouteCommand? Resolve(RequestContext request)
        {
            var identity = (RequestContext request) => _identityProvider.GetIdentityForRequest(request) ?? throw new RouteNotAuthenticatedException();
            var isMatchUsername = (string path) => _routeParserUsername.IsMatch(path, "/users/{username}");
            var isMatchTradingdealId = (string path) => _routeParserTradingdealId.IsMatch(path, "/tradings/{tradingdealid}");
            var isMatchformat = (string path) => _routeParserFormat.IsMatch(path, "/deck");

            var parseUsername = (string path) => _routeParserUsername.ParseParameters(path, "/users/{username}")["username"];
            var parseTradingdealId = (string path) => _routeParserTradingdealId.ParseParameters(path, "/tradings/{tradingdealid}")["tradingdealid"];
            Func<string, string> parseFormat = (string path) =>
            {
                var dict = _routeParserFormat.ParseParameters(path, "/deck");
                if (!dict.ContainsKey("format"))
                {
                    dict["format"] = "json";
                }
                return dict["format"];
            };


            /*
            Default return should be in json format
            ToDo change to json. Use the already done json format in GetDeckCommand
            EndPoints to change: 
                 GetCommand
                 GetCardsCommand
                 GetStatsCommand
                 GetScoreBoard
                 GetAllTradingDealsCommand
            */

            IRouteCommand? command = request switch
            {
                //users
                { Method: HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_userManager, Deserialize<Credentials>(request.Payload)),
                { Method: HttpMethod.Get, ResourcePath: var path } when isMatchUsername(path) => new GetCommand(identity(request), _userManager, parseUsername(path)),
                { Method: HttpMethod.Put, ResourcePath: var path } when isMatchUsername(path) => new UpdateCommand(identity(request), _userManager, parseUsername(path), Deserialize<UserData>(request.Payload)),
                { Method: HttpMethod.Post, ResourcePath: "/sessions" } => new LoginCommand(_userManager, Deserialize<Credentials>(request.Payload)),

                //package
                { Method: HttpMethod.Post, ResourcePath: "/packages" } => new NewPackageCommand(identity(request), _packageManager, Deserialize<Card[]>(request.Payload)),
                { Method: HttpMethod.Post, ResourcePath: "/transactions/packages" } => new BuyPackageCommand(identity(request), _packageManager),

                //cards
                { Method: HttpMethod.Get, ResourcePath: "/cards" } => new GetCardsCommand(identity(request), _cardsManager),
                { Method: HttpMethod.Get, ResourcePath: var path } when isMatchformat(path) => new GetDeckCommand(identity(request), _cardsManager, parseFormat(path)),
                { Method: HttpMethod.Put, ResourcePath: "/deck" } => new ConfigureDeckCommand(identity(request), _cardsManager, Deserialize<Guid[]>(request.Payload)),

                //game
                { Method: HttpMethod.Get, ResourcePath: "/stats" } => new GetStatsCommand(identity(request), _gameManager),
                { Method: HttpMethod.Get, ResourcePath: "/score" } => new GetScoreBoard(identity(request), _gameManager),
                { Method: HttpMethod.Post, ResourcePath: "/battles" } => new BattleCommand(identity(request), _gameManager),


                //tradings
                { Method: HttpMethod.Get, ResourcePath: "/tradings" } => new GetAllTradingDealsCommand(identity(request), _tradingManager),
                { Method: HttpMethod.Post, ResourcePath: "/tradings" } => new CreateTradingDealCommand(identity(request), _tradingManager, Deserialize<TradingDeal>(request.Payload)),
                { Method: HttpMethod.Post, ResourcePath: var path } when isMatchTradingdealId(path) => new CarryOutTradeCommand(identity(request), _tradingManager, Guid.Parse(parseTradingdealId(path)), Deserialize<TradingDeal>(request.Payload)),
                { Method: HttpMethod.Delete, ResourcePath: var path } when isMatchTradingdealId(path) => new DeleteTradingdealCommand(identity(request), _tradingManager, Guid.Parse(parseTradingdealId(path))),

                _ => null
            };

            return command;
        }

        private string EnsureBody(string? body)
        {
            if (body == null)
            {
                throw new InvalidDataException();
            }
            return body;
        }

        private T Deserialize<T>(string? body) where T : class
        {
            var data = body != null ? JsonConvert.DeserializeObject<T>(body) : null;
            if (data == null)
            {
                throw new InvalidDataException();
            }
            return data;
        }
    }
}
