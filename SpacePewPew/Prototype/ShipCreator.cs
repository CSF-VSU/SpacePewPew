using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.Prototype
{
    [Serializable]
    public class ShipCreator
    {
        #region Singleton

        private ShipCreator()
        {
            _ship = new Ship(GetNextId());
            _builder = new Dictionary<ShipNum, ShipInfo>();

            var shipRecords = new List<ShipInfo>();
            var serializer = new XmlSerializer(shipRecords.GetType());
            using (var reader = XmlReader.Create("shipConfig.xml"))
            {
                shipRecords = (List<ShipInfo>)serializer.Deserialize(reader);
                foreach (var shipRecord in shipRecords)
                {
                    var num = new ShipNum { Index = shipRecord.Index, Race = shipRecord.Race };
                    _builder[num] = shipRecord;
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

        private static int id;

        public static int GetNextId()
        {
            return id++;
        }

        #endregion

        private static Dictionary<ShipNum, ShipInfo> _builder;


        public Ship BuildShip(ShipNum key)
        {
            var product = (Ship) _ship.Clone();
            product.Id = GetNextId();

            product.Name = _builder[key].Name;
            product.Description = _builder[key].Description;
            
            product.Cost = _builder[key].Cost;
            
            product.MaxHealth = _builder[key].HP;
            product.Health = product.MaxHealth;

            product.Speed = _builder[key].Speed;
            product.RemainedSpeed = product.Speed;

            product.MinDamage = _builder[key].MinDamage;
            product.MaxDamage = _builder[key].MaxDamage;
            product.Volleys = _builder[key].Volleys;
            
            product.Exp = 0;

            product.TurnState = TurnState.Finished;
            return product;
        }

    }
}
