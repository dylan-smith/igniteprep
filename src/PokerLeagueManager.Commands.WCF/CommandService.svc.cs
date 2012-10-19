﻿using Microsoft.Practices.Unity;
using PokerLeagueManager.Commands.Domain.Infrastructure;
using PokerLeagueManager.Common.Commands.Infrastructure;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PokerLeagueManager.Commands.WCF
{
    public class CommandService : ICommandService
    {
        public void ExecuteCommand(ICommand command)
        {
            Resolver.Container.RegisterInstance<OperationContext>(OperationContext.Current);

            var commandHandlerFactory = Resolver.Container.Resolve<ICommandHandlerFactory>();
            var commandFactory = Resolver.Container.Resolve<ICommandFactory>();
            commandHandlerFactory.ExecuteCommand(commandFactory.Create(command));
        }
    }
}
