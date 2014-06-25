using System.Collections.Generic;
using System.Drawing;

namespace SpacePewPew.UI.Proxy.UICreators
{
    public class EditorUICreator : IUiCreator
    {
        public void Create(List<UiElement> components, List<UiElement> modalComponents)
        {
            components.Clear();
            modalComponents.Clear();

            var panel = new Panel(Consts.SCREEN_WIDTH, Consts.STATUS_BAR_HEIGHT + 1);
            components.Add(panel);

            var newMapBtn = new GameButton(new PointF(5, 1.5f), "New Map");
            newMapBtn.OnClick += () =>
            {
                LayoutManager.GetManager().IsShowingModal = true;
            };
            components.Add(newMapBtn);

            var btnPos = newMapBtn.Position;
            btnPos.X += Consts.BUTTON_WIDTH + 3;
            var loadMapBtn = new GameButton(btnPos, "Load Map");
            loadMapBtn.OnClick += () =>
            {

                LayoutManager.GetManager().IsShowingModal = true;
            };
            components.Add(loadMapBtn);

            btnPos.X += Consts.BUTTON_WIDTH + 3;
            var saveMapBtn = new GameButton(btnPos, "SaveMap") { Enabled = false };
            saveMapBtn.OnClick += () =>
            {
                LayoutManager.GetManager().IsShowingModal = true;
            };
            components.Add(saveMapBtn);

            btnPos.X = Consts.SCREEN_WIDTH - Consts.BUTTON_WIDTH - 1;
            var backToMenuBtn = new GameButton(btnPos, "Return");
            backToMenuBtn.OnClick += () =>
            {
                LayoutManager.GetManager().ScreenType = ScreenType.MainMenu;
                LayoutManager.GetManager().SetComponents(ScreenType.MainMenu);
            };
            components.Add(backToMenuBtn);
        }
    }
}
