using System.Collections.Generic;
using System.Drawing;
using SpacePewPew.DataTypes;
using Tao.OpenGl;

namespace SpacePewPew.UI.GameListView
{
    public class ListView : UiElement
    {
        private float _width = 79;
        private float _height;
        public List<ListViewItem> Items;
        public int VisibleCount { get; set; }

        private int _index;
        public int Index {
            get { return _index; }
            set { 
                if(value == -1)
                    DeselectItems();
                _index = value;
            }
        }

        public ListView(PointF pos, IEnumerable<ListViewItemData> data, int visibleCnt)
        {
            Position = pos;
            Items = new List<ListViewItem>();

            Index = -1;
            var prevItemPos = Position;
            prevItemPos.X += 2;
            prevItemPos.Y += 2;
            foreach (var item in data)
            {
                Items.Add(new ListViewItem(prevItemPos, item));
                prevItemPos.Y += Consts.LISTVIEWITEM_HEIGHT;
            }

            VisibleCount = visibleCnt;
            _height = 4 + VisibleCount*Consts.LISTVIEWITEM_HEIGHT;
        }

        public override void Draw()
        {
            Gl.glColor3f(0.5f, 0.5f, 0.5f);

            Rect(Position.X, Position.Y, Position.X + _width, Position.Y + _height);
            Frame(Position.X, Position.Y, Position.X + _width, Position.Y + _height);

            foreach (var item in Items)
            {
                item.Draw();
            }
        }

        public override bool Click(PointF pos)
        {
            if (!(pos.X >= Position.X + 2 && pos.X <= Position.X + _width - 2 &&
                  pos.Y >= Position.Y + 2 && pos.Y <= Position.Y + _height - 2))
            {
                return false;
            }

            for (var i = 0; i < Items.Count; i++)
            {
                var click = Items[i].Click(pos);
                if (!click) continue;
                
                Index = i;
                DeselectItems();
                Items[i].IsSelected = true;
                return true;
            }

            return false;
        }

        private void DeselectItems()
        {
            foreach (var item in Items)
            {
                item.IsSelected = false;
            }
        }

        public void SetItemsEnabledBy(SetListViewItemEnabled check)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                var data = Items[i].GetData();
                data.GlyphNum = (uint)i;
                Items[i].Enabled = check(data);
            }
        }

        /*public ListView(PointF pos)
        {*/
            /*Items = new ListViewItem[2];
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

            };*/
        //}

        
    }
}
