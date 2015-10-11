using System.Collections.Generic;
using System.Xml;
using FantasyBaseBall.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FantasyBaseBall
{
    [HubName("games")]
    public class GameHub : Hub
    {
        private readonly GameWatcher _watcher;

        public GameHub(GameWatcher watcher)
        {
            _watcher = watcher;
        }

        public GameHub() : this(GameWatcher.Instance)
        {
            
        }

        public override Task OnConnected()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = manager.FindById(Context.Request.GetHttpContext().User.Identity.GetUserId());
            foreach (var favoriteTeam in user.FavoriteTeams)
            {
                Groups.Add(Context.ConnectionId, favoriteTeam.Name);
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = manager.FindById(Context.Request.GetHttpContext().User.Identity.GetUserId());
            foreach (var favoriteTeam in user.FavoriteTeams)
            {
                Groups.Remove(Context.ConnectionId, favoriteTeam.Name);
            }
            return base.OnDisconnected(stopCalled);
        }

        public IEnumerable<Game> GetAllGames()
        {
            return _watcher.GetAllGames();
        }

        public string GetGamesState()
        {
            return _watcher.GamesState.ToString();
        }

        public void RunGames()
        {
            _watcher.StartWatcher();
        }

        public void PauseGames()
        {
            _watcher.PauseWatcher();
        }

        public void Reset()
        {
            _watcher.Reset();
        }
    }
}