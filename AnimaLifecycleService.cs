using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Coop
{
    public class AnimaLifecycleService
    {
        private List<AnimalLifeFeature> animalLifeFeatures;

        public AnimaLifecycleService(List<AnimalLifeFeature> _animalLifeFeatures)
        {
            this.animalLifeFeatures = _animalLifeFeatures;
        }

        public Coop Simulate(int givenTimeAsMonth)
        {
            var coop = new Coop();

            var rabbitLifeFeature = this.animalLifeFeatures.FirstOrDefault(a => a.AnimalType == AnimalType.Rabbit);

            if (rabbitLifeFeature == null)
                throw new ApplicationException(
                    "Rabbit Life Feature Could not be read from AnimalLifeFeatures.json file");
            var rabbitFemale = new Animal()
            {
                AnimalGenderType = AnimalGenderType.Female,
                AnimalState = AnimalState.MatureAndHealthForMating,
                AnimalType = AnimalType.Rabbit,
                Age = DateTime.Now.AddSeconds(rabbitLifeFeature.MatingAgeBeginFemale)
            };

            var rabbitMale = new Animal()
            {
                AnimalGenderType = AnimalGenderType.Male,
                AnimalState = AnimalState.MatureAndHealthForMating,
                AnimalType = AnimalType.Rabbit,
                Age = DateTime.Now.AddSeconds(rabbitLifeFeature.MatingAgeBeginMale)
            };

            coop.MaxAnimalLimit = 1000;

            coop.AddAnimal(rabbitFemale);
            coop.AddAnimal(rabbitMale);

            var beginDate = DateTime.Now;
            var tokenSource = new CancellationTokenSource();
            ParallelOptions parallelLoopOptions =
                new ParallelOptions()
                {
                    CancellationToken = tokenSource.Token
                };
            
            Task.Run(() =>
            {
                while (1 == 1)
                {
                    Parallel.ForEach(this.animalLifeFeatures, parallelLoopOptions, animalLifeFeature =>
                    {
                        Parallel.ForEach(coop.GetAnimals(animalLifeFeature.AnimalType), parallelLoopOptions,
                            animal => { animal.UpdateAnimalState(animalLifeFeature); });
                    });
                }
            }, tokenSource.Token);

            Task.Run(() =>
            {
                while (1 == 1)
                {
                    Parallel.ForEach(this.animalLifeFeatures, parallelLoopOptions, animalLifeFeature =>
                    {
                        Parallel.ForEach(coop.GetMalesForMating(animalLifeFeature.AnimalType), parallelLoopOptions,
                            maleAnimal =>
                            {
                                Parallel.ForEach(coop.GetFemalesForMating(animalLifeFeature.AnimalType),
                                    parallelLoopOptions,
                                    femaleAnimal => { maleAnimal.Mate(femaleAnimal, animalLifeFeature); });
                            });
                    });
                }
            }, tokenSource.Token);

            while (2 == 2)
            {
                if (DateTime.Now.Subtract(beginDate).TotalMilliseconds >= givenTimeAsMonth * 1000)
                {
                    tokenSource.Cancel();
                    break;
                }
            }

            return coop;
        }
    }
}