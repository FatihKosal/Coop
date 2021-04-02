using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Coop
{
    public class Coop
    {
        private CancellationTokenSource cancellationTokenSource;

        public Coop(int maxAnimalLimit, CancellationTokenSource _cancellationTokenSource)
        {
            this.maxAnimalLimit = maxAnimalLimit;
            this.cancellationTokenSource = _cancellationTokenSource;
        }

        private int maxAnimalLimit { get; set; }
        private ConcurrentBag<Animal> animalList { get; set; } = new ConcurrentBag<Animal>();

        private static readonly Object obj = new Object();

        public void AddAnimal(Animal animal)
        {
            lock (obj)
            {
                if (this.animalList.Count >= this.maxAnimalLimit)
                {
                    this.cancellationTokenSource.Cancel();
                }
                else
                {
                    if (animal.AnimalGenderType == AnimalGenderType.Female)
                        animal.BirthOccured += BirthOccuredEvent;
                    this.animalList.Add(animal);
                }
            }
        }

        private void BirthOccuredEvent(object sender, BirthOccuredEventArgs e)
        {
            for (int i = 0; i < e.BabyCount; i++)
            {
                var animal = new Animal();
                Random rand = new Random();
                if (rand.Next(2) == 0)
                    animal.AnimalGenderType = AnimalGenderType.Male;
                else
                    animal.AnimalGenderType = AnimalGenderType.Female;

                this.AddAnimal(animal);
            }
        }

        public int GetFemaleCount(AnimalState animalState)
        {
            if (animalState == AnimalState.Empty)
                return this.animalList.Count(a => a.AnimalGenderType == AnimalGenderType.Female);
            else
                return this.animalList.Count(a =>
                    a.AnimalGenderType == AnimalGenderType.Female && a.AnimalState == animalState);
        }

        public int GetAliveFemaleCount()
        {
            return this.animalList.Count(a =>
                a.AnimalGenderType == AnimalGenderType.Female && a.AnimalState != AnimalState.Death);
        }

        public List<Animal> GetFemalesForMating(AnimalType animalType)
        {
            return this.animalList
                .Where(a => a.AnimalGenderType == AnimalGenderType.Female && a.AnimalType == animalType).ToList();
        }

        public int GetMaleCount(AnimalState animalState)
        {
            if (animalState == AnimalState.Empty)
                return this.animalList.Count(a => a.AnimalGenderType == AnimalGenderType.Male);
            else
                return this.animalList.Count(a =>
                    a.AnimalGenderType == AnimalGenderType.Male && a.AnimalState == animalState);
        }

        public int GetAliveMaleCount()
        {
            return this.animalList.Count(a =>
                a.AnimalGenderType == AnimalGenderType.Male && a.AnimalState != AnimalState.Death);
        }

        public List<Animal> GetMalesForMating(AnimalType animalType)
        {
            return this.animalList.Where(a => a.AnimalGenderType == AnimalGenderType.Male && a.AnimalType == animalType)
                .ToList();
        }

        public List<Animal> GetAnimals(AnimalType animalType)
        {
            return this.animalList.Where(a => a.AnimalType == animalType).ToList();
        }
    }
}