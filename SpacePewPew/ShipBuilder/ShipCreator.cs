using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using SpacePewPew.DataTypes;
using SpacePewPew.GameObjects.Ships;
using SpacePewPew.GameObjects.Ships.Abilities;

namespace SpacePewPew.ShipBuilder
{
    [Serializable]
    public class ShipCreator
    {
        #region Singleton

        private ShipCreator()
        {
            _ship = new Ship();
            Builder = new Dictionary<ShipNum, ShipInfo>();

            var shipRecords = new List<ShipInfo>();
            var serializer = new XmlSerializer(shipRecords.GetType());
            using (var reader = XmlReader.Create("shipConfig.xml"))
            {
                shipRecords = (List<ShipInfo>)serializer.Deserialize(reader);
                foreach (var shipRecord in shipRecords)
                {
                    var num = new ShipNum { Index = shipRecord.Index, Race = shipRecord.Race };
                    Builder[num] = shipRecord;
                }
            }
        }

        private static ShipCreator _instance;
        private static Ship _ship;

        public static ShipCreator GetCreator()
        {
            return _instance ?? (_instance = new ShipCreator());
        }
        #endregion

        #region Id

        private static int _id;

        public static int GetNextId()
        {
            return _id++;
        }

        #endregion

        public static Dictionary<ShipNum, ShipInfo> Builder;

        public Ship BuildShip(ShipNum key)
        {
            var product = (Ship) _ship.Clone();
            product.Id = GetNextId();

            product.Name = Builder[key].Name;
            product.Description = Builder[key].Description;
            
            product.Cost = Builder[key].Cost;
            
            product.MaxHealth = Builder[key].HP;
            product.Health = product.MaxHealth;

            product.Speed = Builder[key].Speed;
            product.RemainedSpeed = product.Speed;

            product.MinDamage = Builder[key].MinDamage;
            product.MaxDamage = Builder[key].MaxDamage;
            product.Volleys = Builder[key].Volleys;

            product.Abilities = Builder[key].Abilities;
            product.Exp = 0;

            product.TurnState = TurnState.Finished;
            return product;
        }

    }
}
