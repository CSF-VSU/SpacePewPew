namespace SpacePewPew
{
    public class Drawer
    {
        #region Singleton pattern
        private static Drawer _instance;

        protected Drawer()
        {
        }

        public static Drawer Instance()
        {
            return _instance ?? (_instance = new Drawer());
        }
        #endregion

        public void Draw(GameState gameState)
        {
            // do all drawing
        }
    }
}
