using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Newtonsoft.Json;
using SWE1.MessageServer.API.RouteCommands.cards;
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
        private readonly IRouteParser _routeParser = new IdRouteParser();

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
            var isMatchUsername = (string path) => _routeParser.IsMatch(path, "/users/{usersname}");
            var isMatchTradingdealId = (string path) => _routeParser.IsMatch(path, "/tradings/{tradingdealid}");
            
            var parseUsername = (string path) => _routeParser.ParseParameters(path, "/users/{usersname}")["usersname"];
            var parseTradingdealId = (string path) => int.Parse(_routeParser.ParseParameters(path, "/tradings/{tradingdealid}")["tradingdealid"]);

            IRouteCommand? command = request switch
            {
                /*
                { Method: HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_userManager, Deserialize<Credentials>(request.Payload)),
                { Method: HttpMethod.Post, ResourcePath: "/sessions"} => new LoginCommand(_userManager, Deserialize<Credentials>(request.Payload)),

                { Method: HttpMethod.Post, ResourcePath: "/messages"} => new AddMessageCommand(_messageManager, identity(request), EnsureBody(request.Payload)),
                { Method: HttpMethod.Get, ResourcePath: "/messages" } => new ListMessagesCommand(_messageManager, identity(request)),

                { Method: HttpMethod.Get, ResourcePath: var path} when isMatch(path) => new ShowMessageCommand(_messageManager, identity(request), parseId(path)),
                { Method: HttpMethod.Put, ResourcePath: var path } when isMatch(path) => new UpdateMessageCommand(_messageManager, identity(request), parseId(path), EnsureBody(request.Payload)),
                { Method: HttpMethod.Delete, ResourcePath: var path } when isMatch(path) => new RemoveMessageCommand(_messageManager, identity(request), parseId(path)),
                */
                //users
                { Method: HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_userManager, Deserialize<Credentials>(request.Payload)),
                { Method: HttpMethod.Get, ResourcePath: var path } when isMatchUsername(path) => new GetCommand(Deserialize<Credentials>(request.Payload), _userManager),
                { Method: HttpMethod.Put, ResourcePath: var path } when isMatchUsername(path)  => new UpdateCommand(Deserialize<Credentials>(request.Payload), _userManager),
                { Method: HttpMethod.Post, ResourcePath: "/sessions" } => new LoginCommand(_userManager, Deserialize<Credentials>(request.Payload)),

                //package
                { Method: HttpMethod.Post, ResourcePath: "/packages" } => new NewPackageCommand(Deserialize<Credentials>(request.Payload),_packageManager),
                { Method: HttpMethod.Post, ResourcePath: "/transactions/packages" } => new BuyPackageCommand(Deserialize<Credentials>(request.Payload), _packageManager),

                //cards
                { Method: HttpMethod.Get, ResourcePath: "/cards" } => new GetCardsCommand(Deserialize<Credentials>(request.Payload), _cardsManager),
                { Method: HttpMethod.Get, ResourcePath: "/deck" } => new GetDeckCommand(Deserialize<Credentials>(request.Payload), _cardsManager),
                { Method: HttpMethod.Put, ResourcePath: "/deck" } => new ConfigureDeckCommand(Deserialize<Credentials>(request.Payload), _cardsManager),

                //game
                /*
                { Method: HttpMethod.Get, ResourcePath: "/stats" } => new GetCardsCommand(Deserialize<Credentials>(request.Payload), _gameManager),
                { Method: HttpMethod.Get, ResourcePath: "/scoreboard" } => new GetDeckCommand(Deserialize<Credentials>(request.Payload), _gameManager),
                { Method: HttpMethod.Post, ResourcePath: "/battle" } => new ConfigureDeckCommand(Deserialize<Credentials>(request.Payload), _gameManager), 
                 */

                //tradings
                { Method: HttpMethod.Get, ResourcePath: "/tradings " } => new GetAllTradingDealsCommand(Deserialize<Credentials>(request.Payload), _tradingManager),
                { Method: HttpMethod.Post, ResourcePath: var path } when isMatchTradingdealId(path) => new CreateTradingDealCommand(Deserialize<Credentials>(request.Payload), _tradingManager),
                { Method: HttpMethod.Delete, ResourcePath: var path } when isMatchTradingdealId(path) => new DeleteTradingdealCommand(Deserialize<Credentials>(request.Payload), _tradingManager),
                { Method: HttpMethod.Post, ResourcePath: "/deck" } => new CarryOutTradeCommand(Deserialize<Credentials>(request.Payload), _tradingManager),
                
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
