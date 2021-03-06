namespace NeonStream.Models
{
    public class GameStatus
    {
        public Character[] Party { get; set; }
        public int Gil { get; set; }
        public string Location { get; set; }
        public bool ActiveBattle { get; set; }

        public string ColorTopLeft { get; set; }
        public string ColorTopRight { get; set; }
        public string ColorBottomLeft { get; set; }
        public string ColorBottomRight { get; set; }
        public string TimeActive { get; set; }
    }
}