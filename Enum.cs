namespace Coop
{
    public enum AnimalType
    {
        Empty = 0,
        Rabbit = 1
    }

    public enum AnimalGenderType
    {
        Empty = 0,
        Male = 1,
        Female = 2,
    }

    public enum AnimalState
    {
        Empty = 0,
        Immature = 1,
        MatureAndHealthForMating = 2,
        BreedingAgeFinished = 3,
        Pregnant = 4,
        Sick = 5,
        Death = 6
    }

    public enum AnimalMatingState
    {
        Available = 1,
        Busy = 2
    }
}