using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using SpacePewPew.GameLogic;

namespace SpacePewPew.UI
{
    public class LayoutManager
    {
        #region Singleton

        private LayoutManager(ScreenType screenType)
        {
            ScreenType = screenType;
            Components = new Dictionary<string, IUiElement>();
            SetComponents(screenType);
        }

        public static LayoutManager GetManager()
        {
            return ins ?? (ins = new LayoutManager(ScreenType.MainMenu));
        }

        private static LayoutManager ins;

        #endregion

        public Dictionary<string, IUiElement> Components  { get; set; }
        public ScreenType ScreenType { get; private set; }


        public void SetComponents(ScreenType screenType) //установка набора компонентов для нынешнего состояния игры
        {
           switch (screenType)
           {
               case ScreenType.MainMenu:
                   {
                       Components.Clear();
                       Components["Exit"] = new GameButton(new PointF(157, 90), true);
                       (Components["Exit"] as GameButton).OnClick += () => { MainForm.ActiveForm.Close(); };

                       Components["New Game"] = new GameButton(new PointF(157, 50), true);
                       (Components["New Game"] as GameButton).OnClick += () => { ScreenType = ScreenType.Game;
                                                                                   Game.Instance().StartNewGame();
                                                                                   SetComponents(ScreenType.Game);
                                                                                       /* ChangeStage(screenType) //для смены дикшнари  */
                       };

                       Components["Load"] = new GameButton(new PointF(157, 50 + Consts.BUTTON_HEIGHT + 5), true);
                       (Components["Load"] as GameButton).OnClick += () =>
                       {
                           Game.Instance().Manager.LoadGame();
                           ScreenType = ScreenType.Game;
                           SetComponents(ScreenType.Game);
                       };

                       break;
                   }
               case ScreenType.Game:
                   {
                       Components.Clear();
                       //компоненты меню игры: статус бар, менюшки и тд
                       Components["Status Bar"] = new StatusBar();

                       Components["Save"] = new GameButton(new PointF(140, 1), true);
                       (Components["Save"] as GameButton).OnClick += () => Game.Instance().Manager.SaveGame(Game.Instance());

                       Components["End Turn"] = new GameButton(new PointF(160, 1), true);
                       (Components["End Turn"] as GameButton).OnClick += () => Game.Instance().PassTurn();

                       Components["Shop Menu"] = new ListView(new PointF(Consts.SCREEN_WIDTH / 7 * 2, Consts.STATUS_BAR_HEIGHT + 1));
                       
                       Components["Quit Shop"] = new GameButton(new PointF(105, 87), true);  //TODO: fix this coordinates later
                       (Components["Quit Shop"] as GameButton).OnClick += () =>
                       {
                           (Components["Shop Menu"] as ListView).Visible = false;
                           (Components["Shop Menu"] as ListView).Index = -1;
                           Game.Instance().IsShowingModal = false;
                       };
                       Components["Buy Ship"] = new GameButton(new PointF(105 - Consts.BUTTON_WIDTH - 2, 87), false);
                       (Components["Buy Ship"] as GameButton).OnClick += () =>
                       {
                           Game.Instance().BuildShip((Components["Shop Menu"] as ListView).Index);
                           (Components["Shop Menu"] as ListView).Visible = false;
                           (Components["Shop Menu"] as ListView).Index = -1;
                           Game.Instance().IsShowingModal = false;

                       };

                       break;
                   }
           }
        }


        public bool OnClick(PointF pos) //TODO:подправить под вид компонента (Т) !!!!
        {
            switch (ScreenType)
            {
                case ScreenType.MainMenu:
                    foreach (var btn in Components.Where(btn => (pos.X > btn.Value.Position.X) && (pos.X < btn.Value.Position.X + Consts.BUTTON_WIDTH) &&
                                                     (pos.Y > btn.Value.Position.Y) && (pos.Y < btn.Value.Position.Y + Consts.BUTTON_HEIGHT)))
                    {
                        (btn.Value as GameButton).InvokeOnClick();
                        return true;
                    }
                    break;

                case ScreenType.Game:
                    if ((Components["Shop Menu"] as ListView).Visible)
                        foreach (var menu in (Components["Shop Menu"] as ListView).Items.Where(menu => (pos.X > menu.Position.X) && (pos.X < menu.Position.X + Consts.LISTVIEWITEM_WIDTH) &&
                                                             (pos.Y > menu.Position.Y) && (pos.Y < menu.Position.Y + Consts.LISTVIEWITEM_HEIGHT)))
                        {
                            menu.InvokeOnClick();
                            return true;
                        }

                    var buyBtn = Components["Buy Ship"] as GameButton;
                    if (pos.X >= buyBtn.Position.X && pos.X <= buyBtn.Position.X + Consts.BUTTON_WIDTH
                        && pos.Y >= buyBtn.Position.Y && pos.Y <= buyBtn.Position.Y + Consts.BUTTON_HEIGHT)
                    {
                        buyBtn.InvokeOnClick();
                        return true;
                    }

                    var quitBtn = Components["Quit Shop"] as GameButton;
                    if (pos.X >= quitBtn.Position.X && pos.X <= quitBtn.Position.X + Consts.BUTTON_WIDTH
                        && pos.Y >= quitBtn.Position.Y && pos.Y <= quitBtn.Position.Y + Consts.BUTTON_HEIGHT)
                    {
                        quitBtn.InvokeOnClick();
                        return true;
                    }

                    var saveBtn = Components["Save"] as GameButton;
                    if (pos.X >= saveBtn.Position.X && pos.X <= saveBtn.Position.X + Consts.BUTTON_WIDTH
                        && pos.Y >= saveBtn.Position.Y && pos.Y <= saveBtn.Position.Y + Consts.BUTTON_HEIGHT)
                    {
                        saveBtn.InvokeOnClick();
                        return true;
                    }

                    var endBtn = Components["End Turn"] as GameButton;
                    if (pos.X >= endBtn.Position.X && pos.X <= endBtn.Position.X + Consts.BUTTON_WIDTH
                        && pos.Y >= endBtn.Position.Y && pos.Y <= endBtn.Position.Y + Consts.BUTTON_HEIGHT)
                    {
                        endBtn.InvokeOnClick();
                        return true;
                    }
                    break;
            }    
            return false;
        }
    }
}
