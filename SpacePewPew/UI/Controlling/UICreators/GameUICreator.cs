using System.Collections.Generic;
using System.Drawing;
using SpacePewPew.DataTypes;
using SpacePewPew.GameLogic;

namespace SpacePewPew.UI.Controlling.UICreators
{
    public class GameUICreator : IUiCreator
    {
        public void Create(List<UiElement> components, List<UiElement> modalComponents)
        {
            components.Clear();
            modalComponents.Clear();

            components.Add(new PlayerInfoStatusBar());

            var saveBtn = new GameButton(new PointF(140, 1), "Save");
            saveBtn.OnClick += () => Game.Instance().Manager.SaveGame(Game.Instance());
            components.Add(saveBtn);

            var endTurnBtn = new GameButton(new PointF(160, 1), "End Turn");
            endTurnBtn.OnClick += () => Game.Instance().PassTurn();
            components.Add(endTurnBtn);


            var listViewPos = new PointF(20, Consts.STATUS_BAR_HEIGHT + 1);
            var data = new List<ListViewItemData>
                       {
                           new ListViewItemData {GlyphNum = 6, Appendix = "20", Text = "Fighter"},
                           new ListViewItemData {GlyphNum = 3, Appendix = "30", Text = "Barge"},
                           new ListViewItemData {GlyphNum = 14, Appendix = "18", Text = "Healer"},
                           new ListViewItemData {GlyphNum = 17, Appendix = "20", Text = "Infestor"}
                       };
            var listView = new GameListView.ListView(listViewPos, data, 4);
            modalComponents.Add(listView);

            var quitShopBtn = new GameButton(new PointF(70, 87), "Quit Shop");
            quitShopBtn.OnClick += () =>
            {
                listView.Index = -1;
                LayoutManager.GetManager().IsShowingModal = false;
            };
            modalComponents.Add(quitShopBtn);

            var pos = quitShopBtn.Position;
            pos.X -= Consts.BUTTON_WIDTH + 2;

            var buyShipBtn = new GameButton(pos, "Buy Ship");
            buyShipBtn.OnClick += () =>
            {
                Game.Instance().BuildShip(listView.Index);
                listView.Index = -1;
                LayoutManager.GetManager().IsShowingModal = false;
            };
            modalComponents.Add(buyShipBtn);
        }
    }
}
