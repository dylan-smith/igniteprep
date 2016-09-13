﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.Infrastructure;
using PokerLeagueManager.Common.Queries;
using PokerLeagueManager.Queries.Core.Infrastructure;

namespace PokerLeagueManager.Queries.Core.QueryHandlers
{
    public class GetGamesWithPlayerQueryHandler : BaseQueryHandler, IHandlesQuery<GetGamesWithPlayerQuery>
    {
        public object Execute(GetGamesWithPlayerQuery query)
        {
            return Repository.GetData<GetGamesWithPlayerDto>().Where(g => g.PlayerName.ToUpper().Trim() == query.PlayerName.ToUpper().Trim());
        }
    }
}