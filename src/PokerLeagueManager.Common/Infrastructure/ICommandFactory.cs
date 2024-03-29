﻿namespace PokerLeagueManager.Common.Infrastructure
{
    public interface ICommandFactory
    {
        T Create<T>()
            where T : ICommand, new();

        T Create<T>(T cmd)
            where T : ICommand;
    }
}
