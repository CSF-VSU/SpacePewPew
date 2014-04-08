using System.Collections.Generic;
using SpacePewPew.GameObjects.MapObjects;

namespace SpacePewPew.GameObjects.GameMap
{
	public class StationIterator : IStationIterator
	{
		private List<Station> _stations; 
		private int _current = 0;

		public PlayerColor Color { get; set; }
		public int Count { get; set; }

		private void GetStations(Cell[,] cells)
		{
			foreach (var cell in cells)
			{
				if (cell.Obstacle is Station && (cell.Obstacle as Station).OwnerColor == Color)
				{
					_stations.Add(cell.Obstacle as Station);
				}
			}
			Count = _stations.Count;
		}

		public StationIterator(Cell[,] cells, PlayerColor color)
		{
			Color = color;
			GetStations(cells);
		}

		public Station First()
		{
			_current = 0;
			return !IsDone ? _stations[_current] : null;
		}

		public Station Next()
		{
			_current ++;
			return !IsDone ? _stations[_current] : null;
		}

		public bool IsDone
		{
			get { return _current >= _stations.Count; }
		}

	    public Station CurrentStation
	    {
	        get { return _stations[_current]; }
	    }
	}
}
