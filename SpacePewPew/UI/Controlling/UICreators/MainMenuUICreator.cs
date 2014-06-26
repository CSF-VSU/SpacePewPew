using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SpacePewPew.GameLogic;
using SpacePewPew.UI.Proxy;

namespace SpacePewPew.UI.Controlling.UICreators
{
    public class MainMenuUICreator : IUiCreator
    {
        public void Create(List<UiElement> components, List<UiElement> modalComponents)
        {
            components.Clear();
            modalComponents.Clear();

            var newGameBtn = new GameButton(new PointF(157, 50), "New Game");
            newGameBtn.OnClick += () =>
            {
                LayoutManager.GetManager().ScreenType = ScreenType.Game;
                LayoutManager.GetManager().SetComponents(ScreenType.Game);
                Game.Instance().StartNewGame();
            };
            components.Add(newGameBtn);

            var pos = newGameBtn.Position;
            pos.Y += Consts.BUTTON_HEIGHT + 5;
            var loadGameBtn = new GameButton(pos, "Load Game");
            loadGameBtn.OnClick += () =>
            {
                Game.Instance().Manager.LoadGame();
                LayoutManager.GetManager().ScreenType = ScreenType.Game;
                LayoutManager.GetManager().SetComponents(ScreenType.Game);
            };
            components.Add(loadGameBtn);

            pos.Y += Consts.BUTTON_HEIGHT + 5;
            var editorBtn = new GameButton(pos, "Editor");
            editorBtn.OnClick += () =>
            {
                LayoutManager.GetManager().ScreenType = ScreenType.Editor;
                LayoutManager.GetManager().SetComponents(ScreenType.Editor);
            };
            components.Add(editorBtn);

            pos.Y += Consts.BUTTON_HEIGHT + 5;
            var exitBtn = new GameButton(pos, "Exit");
            exitBtn.OnClick += Application.Exit;
            components.Add(exitBtn);
        }
    }
}
