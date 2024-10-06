namespace Game.Scripts.Entities
{
    public class LevelState
    {
        public float MaxScore { get; set; } = 30f;
        public float CurrentScore { get; set; }
        
        public int TotalSquares { get; set; }
        public int TotalCircles { get; set; }
        public int TotalRedSquares { get; set; }
        public int TotalWoos { get; set; }
        public int CorrectWoos { get; set; }
        public int WrongWoos { get; set; }

        public float InvalidNormalChance = 0.3f;
        public float InvalidRedChance = 0f;

        public float ConveyorInterval = 0.6f;
        
        public float ScreenShakeInterval = 0.5f;

        public bool IsPaused { get; set; } = true;
        
        public void Reset()
        {
            CurrentScore = 0;
            TotalSquares = 0;
            TotalCircles = 0;
            TotalRedSquares = 0;
            TotalWoos = 0;
            CorrectWoos = 0;
            WrongWoos = 0;
        }
    }
}