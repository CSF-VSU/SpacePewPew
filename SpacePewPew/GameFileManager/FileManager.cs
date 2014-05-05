using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace SpacePewPew.GameFileManager
{
    public class FileManager
    {
        private GameState state;

        public void SaveGame(Game game)
        {
            state = new GameState(game.Map, game.Players);

            try
            {
                using (Stream stream = File.Open("test.bin", FileMode.Create))
                {
                    var bin = new BinaryFormatter();
                    bin.Serialize(stream, state);

                    //var xm = new XmlSerializer(typeof(GameState));
                    //xm.Serialize(stream, state);
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
                state = (GameState) bin.Deserialize(fs);

                Game.Instance().LoadGame(state);
            }
        }
    }
}
