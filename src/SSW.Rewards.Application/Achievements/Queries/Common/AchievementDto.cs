namespace SSW.Rewards.Application.Achievements.Queries.Common
{
    public class AchievementDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public AchievementType Type { get; set; }
    }
}