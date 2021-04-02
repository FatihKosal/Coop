using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Coop
{
    public class Animal
    {
        public AnimalType AnimalType { get; set; } = AnimalType.Rabbit;
        public AnimalGenderType AnimalGenderType { get; set; } = AnimalGenderType.Male;
        public AnimalState AnimalState { get; set; } = AnimalState.Immature;
        public DateTime Age { get; set; } = DateTime.Now;
        public DateTime? SickDateBegin { get; set; }

        public event EventHandler<BirthOccuredEventArgs> BirthOccured;

        public void UpdateAnimalState(AnimalLifeFeature animalLifeFeature)
        {
            //1000 ms = 1 sn = 1 month
            var month = (int) (DateTime.Now.Subtract(Age).TotalMilliseconds / 1000);

            //----------------------------Death Case Begin-----------------------------------------------//
            if (month >= animalLifeFeature.MaxLifeDuration)
            {
                this.AnimalState = AnimalState.Death;
                return;
            }
            else
            {
                Random rand = new Random();
                if (rand.Next(2) == 0)
                {
                    this.AnimalState = AnimalState.Death;
                    return;
                }
            }
            //----------------------------Death Case End-----------------------------------------------//


            //----------------------------Sick Case Begin-----------------------------------------------//
            if (this.AnimalState == AnimalState.Sick)
            {
                if (this.SickDateBegin?.Subtract(DateTime.Now).TotalMilliseconds >= (int)animalLifeFeature.SickImprovementDuration * 1000
                ) //assumed half month improvement duration
                {
                    this.AnimalState =
                        AnimalState.Empty; //Animal State will be set at Mate Maturity Case Begin in the below
                    this.SickDateBegin = null;
                }
            }
            else
            {
                Random rand = new Random();
                if (rand.Next(2) == 0)
                {
                    this.AnimalState = AnimalState.Sick;
                    this.SickDateBegin = DateTime.Now;
                    return;
                }
            }
            //----------------------------Sick Case End-----------------------------------------------//

            //----------------------------Mate Maturity Case Begin------------------------------------//
            if (
                (AnimalGenderType == AnimalGenderType.Female && month < animalLifeFeature.MatingAgeBeginFemale)
                ||
                (AnimalGenderType == AnimalGenderType.Male && month < animalLifeFeature.MatingAgeBeginMale)
            )
                this.AnimalState = AnimalState.Immature;
            else if (
                (AnimalGenderType == AnimalGenderType.Female && month >= animalLifeFeature.MatingAgeEndFemale)
                ||
                (AnimalGenderType == AnimalGenderType.Male && month >= animalLifeFeature.MatingAgeEndMale)
            )
                this.AnimalState = AnimalState.BreedingAgeFinished;
            else
                this.AnimalState = AnimalState.MatureAndHealthForMating;
            //----------------------------Mate Maturity Case End------------------------------------//
        }

        private object mateLockObject = new object();

        public void Mate(Animal pair, AnimalLifeFeature animalLifeFeature)
        {
            lock (pair.mateLockObject)
            {
                if (this.AnimalState == AnimalState.MatureAndHealthForMating &&
                    pair.AnimalState == AnimalState.MatureAndHealthForMating)
                {
                    Random rand = new Random();
                    var pregnancyOccured = rand.Next(2) == 0;
                    if (pregnancyOccured)
                    {
                        if (this.AnimalGenderType == AnimalGenderType.Female)
                        {
                            if (this.AnimalState != AnimalState.Pregnant) //She already may be pregnant beforehand
                            {
                                this.AnimalState = AnimalState.Pregnant;
                            }

                            Task task = Task.Delay(animalLifeFeature.PregnancyDuration * 1000)
                                .ContinueWith(t => giveBirth(this, animalLifeFeature));
                        }
                        else if (pair.AnimalGenderType == AnimalGenderType.Female)
                        {
                            if (pair.AnimalState != AnimalState.Pregnant) //She already may be pregnant beforehand
                            {
                                pair.AnimalState = AnimalState.Pregnant;
                            }

                            Task task = Task.Delay(animalLifeFeature.PregnancyDuration * 1000)
                                .ContinueWith(t => giveBirth(pair, animalLifeFeature));
                        }
                    }


                    Thread.Sleep((int)animalLifeFeature.MateWaitDurationForAnotherMate * 1000); //Mate wait duration for another mate 
                }
            }
        }

        private void giveBirth(Animal animal, AnimalLifeFeature animalLifeFeature)
        {
            animal.AnimalState = AnimalState.MatureAndHealthForMating;
            Random rand = new Random();
            var babyCount = rand.Next(animalLifeFeature.MaxBabyCountDuringBirth);
            animal.BirthOccured?.Invoke(this, new BirthOccuredEventArgs(babyCount));
        }
    }
}