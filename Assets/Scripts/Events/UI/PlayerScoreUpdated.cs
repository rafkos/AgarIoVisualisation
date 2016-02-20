using System.Collections.Generic;
using Assets.Scripts.Models;
using Assets.Scripts.Services;

namespace Assets.Scripts.Events.UI
{
    public class PlayerScoreUpdatedEvent : IEvent
    {
        public PlayerScoreUpdatedEvent(IEnumerable<PlayerScore> playerScores)
        {
            PlayerScores = playerScores;
        }

        public IEnumerable<PlayerScore> PlayerScores;
    }
}
