using System;

namespace Coop
{
    public class BirthOccuredEventArgs : EventArgs
    {
        public int BabyCount { get; set; }

        public BirthOccuredEventArgs(int babyCount)
        {
            this.BabyCount = babyCount;
        }
    }
}