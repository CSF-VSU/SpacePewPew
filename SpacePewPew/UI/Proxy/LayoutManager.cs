using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SpacePewPew.DataTypes;
using SpacePewPew.GameLogic;

namespace SpacePewPew.UI.Proxy
{
    public class LayoutManager
    {
        #region Singleton

        private LayoutManager(ScreenType screenType)
        {
            ScreenType = screenType;
            IsShowingModal = false;
            Components = new List<UiElement>();
            ModalComponents = new List<UiElement>();
            SetComponents(screenType);
        }

        public static LayoutManager GetManager()
        {
            return _ins ?? (_ins = new LayoutManager(ScreenType.MainMenu));
        }

        private static LayoutManager _ins;

        #endregion

        private bool _showingModal;

        public bool IsShowingModal
        {
            get
            {
                return _showingModal;
            }
            set
            {
                Game.Instance().TimerEnabled = !value;
                _showingModal = value;
            }
        }


        public List<UiElement> Components { get; set; }
        public List<UiElement> ModalComponents { get; set; } 
        public ScreenType ScreenType { get; private set; }
        
        public void SetComponents(ScreenType screenType) //установка набора компонентов для нынешнего состояния игры
        {
           switch (screenType)
           {
               case ScreenType.MainMenu:
                   {
                       Components.Clear();

                       var newGameBtn = new GameButton(new PointF(157, 50), "New Game");
                       newGameBtn.OnClick += () =>
                       {
                           ScreenType = ScreenType.Game;
                           Game.Instance().StartNewGame();
                           SetComponents(ScreenType.Game);
                       };
                       Components.Add(newGameBtn);

                       var pos = Components[Components.Count - 1].Position;
                       pos.Y += Consts.BUTTON_HEIGHT + 5;
                       var loadGameBtn = new GameButton(pos, "Load Game");
                       loadGameBtn.OnClick += () =>
                       {
                           Game.Instance().Manager.LoadGame();
                           ScreenType = ScreenType.Game;
                           SetComponents(ScreenType.Game);
                       };
                       Components.Add(loadGameBtn);


                       pos.Y += Consts.BUTTON_HEIGHT + 5;
                       var exitBtn = new GameButton(pos, "Exit");
                       exitBtn.OnClick += Application.Exit;
                       Components.Add(exitBtn);
                       break;
                   }
               case ScreenType.Game:
                   {
                       Components.Clear();

                       Components.Add(new PlayerInfoStatusBar());

                       var saveBtn = new GameButton(new PointF(140, 1), "Save");
                       saveBtn.OnClick += () => Game.Instance().Manager.SaveGame(Game.Instance());
                       Components.Add(saveBtn);

                       var endTurnBtn = new GameButton(new PointF(160, 1), "End Turn");
                       endTurnBtn.OnClick += () => Game.Instance().PassTurn();
                       Components.Add(endTurnBtn);

                       
                       var listViewPos = new PointF(20, Consts.STATUS_BAR_HEIGHT + 1);
                       var data = new List<ListViewItemData>
                       {
                           new ListViewItemData {GlyphNum = 3, Appendix = "20", Text = "Fighter"},
                           new ListViewItemData {GlyphNum = 6, Appendix = "30", Text = "Barge"}
                       };
                       var listView = new GameListView.ListView(listViewPos, data, 3);
                       ModalComponents.Add(listView);

                       var quitShopBtn = new GameButton(new PointF(70, 87), "Quit Shop");
                       quitShopBtn.OnClick += () =>
                       {
                           listView.Index = -1;
                           IsShowingModal = false;
                       };
                       ModalComponents.Add(quitShopBtn);

                       var pos = quitShopBtn.Position;
                       pos.X -= Consts.BUTTON_WIDTH + 2;
                       var buyShipBtn = new GameButton(pos, "Buy Ship");
                       buyShipBtn.OnClick += () =>
                       {
                           Game.Instance().BuildShip(listView.Index);
                           listView.Index = -1;
                           IsShowingModal = false;
                       };
                       ModalComponents.Add(buyShipBtn);
                       break;
                   }
           }
        }


        public bool OnClick(PointF pos)
        {
            switch (ScreenType)
            {
                case ScreenType.MainMenu:
                    if (Components.Select(item => item.Click(pos)).Any(a => a))
                    {
                        return true;
                    }
                    break;

                case ScreenType.Game:
                    return IsShowingModal ? ModalComponents.Select(item => item.Click(pos)).Any(q => q) : Components.Select(item => item.Click(pos)).Any(clickProcessed => clickProcessed);
            }    
            return false;
        }
    }
}
