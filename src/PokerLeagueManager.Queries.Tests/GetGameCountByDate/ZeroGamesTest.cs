﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.Common.Queries;
using PokerLeagueManager.Queries.Tests.Infrastructure;

namespace PokerLeagueManager.Queries.Tests.GetGameCountByDate
{
    [TestClass]
    public class ZeroGamesTest : BaseQueryTest
    {
        private readonly DateTime _gameDate = DateTime.Parse("03-Jul-1981");

        [TestMethod]
        public void GetGameCountByDate_ZeroGames()
        {
            var result = SetupQueryService().Execute(new GetGameCountByDateQuery(_gameDate));

            Assert.AreEqual(0, result);
        }
    }
}
