﻿using System;

namespace PokerLeagueManager.UI.Wpf.Views
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = "Need an interface in order to make it mockable")]
    public interface IEnterGameResultsView
    {
        Guid GameId { get; set; }
    }
}
