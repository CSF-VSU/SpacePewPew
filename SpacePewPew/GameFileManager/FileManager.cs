using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using SpacePewPew.GameLogic;

namespace SpacePewPew.GameFileManager
{
    public class FileManager
    {
        private GameState _state;

        public void SaveGame(Game game)
        {
            _state = new GameState(game.Map, game.Players, game.Players.IndexOf(game.CurrentPlayer));

            try
            {
                using (Stream stream = File.Open("test.bin", FileMode.Create))
                {
                    var bin = new BinaryFormatter();
                    bin.Serialize(stream, _state);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Error while saving to file!");
            }
        }

        public void LoadGame()
        {
            using (var fs = new FileStream("test.bin", FileMode.Open))
            {
                var bin = new BinaryFormatter();
                _state = (GameState) bin.Deserialize(fs);

                Game.Instance().LoadGame(_state);
            }
        }
    }
}
