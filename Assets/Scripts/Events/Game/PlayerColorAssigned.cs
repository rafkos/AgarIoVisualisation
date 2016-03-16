using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Events.Game
{
    public class PlayerColorAssignedEvent : IEvent
    {
        public string PlayerName { get; private set; }
        public Color Color { get; private set; }

        public PlayerColorAssignedEvent(string playerName, Color color)
        {
            PlayerName = playerName;
            Color = color;
        }
    }
}