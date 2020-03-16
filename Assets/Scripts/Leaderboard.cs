using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    [Serializable]
    public class Leaderboard
    {
        public List<LeaderboardPlayer> players;

        public Leaderboard()
        {
            players = new List<LeaderboardPlayer>();
        }
    }

    [Serializable]
    public class LeaderboardPlayer 
    {
        public long id;

        public LeaderboardPlayer(long score)
        {
            id = score;
        }
    }
}
