﻿using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLeagueManager.UI.Wpf.TestFramework;

namespace PokerLeagueManager.UI.Wpf.CodedUITests
{
    [CodedUITest]
    public class DuplicatePlayerNamesCaseInsensitiveTest
    {
        private ApplicationUnderTest _app;
        private ViewGamesListScreen _gamesListScreen;

        [TestInitialize]
        public void TestInitialize()
        {
            _app = ApplicationUnderTest.Launch(@"C:\PokerLeagueManager.UI.Wpf\PokerLeagueManager.UI.Wpf.exe");
            _gamesListScreen = new ViewGamesListScreen(_app);
        }

        [TestMethod]
        public void DuplicatePlayerNamesCaseInsensitive()
        {
            var testDate = _gamesListScreen.FindUnusedGameDate();

            var enterGameScreen = _gamesListScreen.ClickAddGame();

            enterGameScreen.EnterGameDate(testDate)
                           .EnterPlayerName("Jennifer Aniston")
                           .EnterPlacing("1")
                           .EnterWinnings("130")
                           .ClickAddPlayer()
                           .EnterPlayerName("jennifer aniston")
                           .EnterPlacing("2")
                           .EnterWinnings("20")
                           .ClickAddPlayer()
                           .ClickSaveGame();

            enterGameScreen.VerifyDuplicatePlayerWarning();

            enterGameScreen.DismissWarningDialog()
                           .VerifyScreen();
        }
    }
}
