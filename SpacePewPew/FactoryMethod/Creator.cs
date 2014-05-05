using System;
using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.FactoryMethod
{
    [Serializable]
    public abstract class Creator
    {
        public abstract Ship FactoryMethod();


        private static int id;

        public static int GetNextId()
        {
            return id++;
        }
    }
}
