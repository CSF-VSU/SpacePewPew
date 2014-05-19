using System.Drawing;
using SpacePewPew.GameLogic;
using SpacePewPew.ShipBuilder;

namespace SpacePewPew.UI
{
    public class ListView : IUiElement
    {
        public PointF Position { get; set; }
        public ListViewItem[] Items;
        public bool Visible { get; set; }
        public int Index { get; set; }

        public ListView(PointF pos)
        {
            Items = new ListViewItem[2];
            Position = pos;
            Visible = false;
            Index = -1;
            var ind = new ShipNum();
                ind.Index = 0;
                ind.Race = Game.Instance().CurrentPlayer.Race;
            Items[0] = new ListViewItem(new PointF(Position.X + 1, Position.Y + 1), "Fighter", ShipCreator._builder[ind].Cost);
            Items[0].OnClick += () => { Index = 0; };
            Items[0].OnClick += () =>
            {

                ind.Index = Index;
                ind.Race = Game.Instance().CurrentPlayer.Race;
                if (Game.Instance().CurrentPlayer.Money < ShipCreator._builder[ind].Cost)
                    (LayoutManager.GetManager().Components["Buy Ship"] as GameButton).Enabled = false;
                else
                    (LayoutManager.GetManager().Components["Buy Ship"] as GameButton).Enabled = true;

            };

            ind.Index = 1;
            ind.Race = Game.Instance().CurrentPlayer.Race;
           
            Items[1] = new ListViewItem(new PointF(Position.X + 1, Position.Y + 1 + Consts.LISTVIEWITEM_HEIGHT), "Barge", ShipCreator._builder[ind].Cost);
            Items[1].OnClick += () => { Index = 1; };
            Items[1].OnClick += () =>
            {
               // var ind = new ShipNum();
                ind.Index = Index;
                ind.Race = Game.Instance().CurrentPlayer.Race;
                 if (Game.Instance().CurrentPlayer.Money < ShipCreator._builder[ind].Cost)
                    (LayoutManager.GetManager().Components["Buy Ship"] as GameButton).Enabled = false;
                else
                    (LayoutManager.GetManager().Components["Buy Ship"] as GameButton).Enabled = true;

            };
        }
    }
}
