﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using log4net;
using Microsoft.Practices.Unity;
using PokerLeagueManager.Common;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.Infrastructure;
using PokerLeagueManager.Common.Queries;
using PokerLeagueManager.UI.Wpf.Infrastructure;
using PokerLeagueManager.UI.Wpf.Views;

namespace PokerLeagueManager.UI.Wpf.ViewModels
{
    public class PlayerStatisticsViewModel : BaseViewModel, INotifyPropertyChanged, IPlayerStatisticsViewModel
    {
        private ObservableCollection<GetPlayerStatisticsDto> _players;

        public PlayerStatisticsViewModel(ICommandService commandService, IQueryService queryService, IMainWindow mainWindow, ILog logger)
            : base(commandService, queryService, mainWindow, logger)
        {
            _players = new ObservableCollection<GetPlayerStatisticsDto>(QueryService.GetPlayerStatistics());

            GamesCommand = new RelayCommand(x => NavigateToGamesView());
            PlayerDoubleClickCommand = new RelayCommand(x => PlayerDoubleClick());

            Height = 400;
            Width = 500;
            WindowTitle = "Player Statistics";
        }

        public IEnumerable<string> Players
        {
            get
            {
                return _players.OrderByDescending(p => p.Winnings)
                             .Select(p => string.Format(
                                 "{0} - Games Played: {1} - Winnings: ${2} - Pay In: ${3} - Profit: ${4} - Profit Per Game: ${5}",
                                 p.PlayerName,
                                 p.GamesPlayed,
                                 p.Winnings,
                                 p.PayIn,
                                 p.Profit,
                                 p.ProfitPerGame.ToString()));
            }
        }

        public System.Windows.Input.ICommand GamesCommand { get; set; }

        public System.Windows.Input.ICommand PlayerDoubleClickCommand { get; set; }

        public int SelectedPlayerIndex { get; set; }

        private void NavigateToGamesView()
        {
            var view = Resolver.Container.Resolve<IGamesListView>();
            MainWindow.ShowView(view);
        }

        private void PlayerDoubleClick()
        {
            if (_players.Count() == 0 || SelectedPlayerIndex < 0)
            {
                return;
            }

            var view = Resolver.Container.Resolve<IPlayerDetailsView>();
            view.PlayerName = GetSelectedPlayer().PlayerName;
            MainWindow.ShowView(view);
        }

        private GetPlayerStatisticsDto GetSelectedPlayer()
        {
            return _players.OrderByDescending(p => p.Winnings).ElementAt(SelectedPlayerIndex);
        }
    }
}
