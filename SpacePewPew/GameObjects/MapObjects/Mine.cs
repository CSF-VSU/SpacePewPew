using System;

namespace SpacePewPew.GameObjects.MapObjects
{
    [Serializable]
    public class Mine : IObstacle
    {
        public int Income { get; set; }
        public int RechargeTime { get; set; }
        public int CoolDown { get; set; }

        public Mine()
        {
            IsPassable = true;
            IsDestructable = false;
            Income = 20;
            RechargeTime = 5;
            CoolDown = 0;
        }
    }
}
