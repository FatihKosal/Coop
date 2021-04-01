namespace Coop
{
    public class AnimalLifeFeature
    {
        public AnimalType AnimalType { get; set; }
        public int MaxLifeDuration { get; set; }
        public int MatingAgeBeginFemale { get; set; }
        public int MatingAgeEndFemale { get; set; }
        public int MatingAgeBeginMale { get; set; }
        public int MatingAgeEndMale { get; set; }
        public int PregnancyDuration { get; set; }
        public int MaxBabyCountDuringBirth { get; set; }
    }
}