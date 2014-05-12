using System.Collections.Generic;
using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.GameObjects.GameMap
{
	public class ShipIterator : IShipIterator
	{
		private List<Ship> _ships; 
		private int _current;

		public PlayerColor Color { get; set; }
		public int Count { get; set; }

		private void GetShips(Cell[,] cells)
		{
			foreach (var cell in cells)
				if (cell.Ship != null && cell.Ship.Color == Color)
					_ships.Add(cell.Ship); 

			Count = _ships.Count;   
		}

		public ShipIterator(Cell[,] cells, PlayerColor color)
		{
			Color = color;
			_ships = new List<Ship>();
			GetShips(cells);
		}

		public Ship  First()
		{
			_current = 0;
			return !IsDone ? _ships[_current] : null;
		}

		public Ship  Next()
		{
			_current ++;
			return !IsDone ? _ships[_current] : null;
		}

		public bool  IsDone
		{
			get { return _current >= _ships.Count; }
		}

		public Ship  CurrentShip
		{
			get { return _ships[_current]; }
		}
	}
}
