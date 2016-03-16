namespace Assets.Scripts.ServerConnection
{
    public class GameSettings
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsVisualization { get; set; }

        public bool IsFlyCameraEnabled { get; set; }
    }
}