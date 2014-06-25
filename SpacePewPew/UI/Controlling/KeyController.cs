using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using SpacePewPew.UI.Proxy;

namespace SpacePewPew.UI.Controlling
{
    public class KeyController
    {
        private readonly LayoutManager _manager;
        private readonly Dictionary<ScreenType, Dictionary<Keys, EventHandler>> _keys;
        private readonly HashSet<Keys> _characters;
        private readonly HashSet<Keys> _numbers;
        private readonly HashSet<Keys> _controls;
        
        public KeyController()
        {
            _manager = LayoutManager.GetManager();

            _characters = new HashSet<Keys>
            {
                Keys.A,
                Keys.B,
                Keys.C,
                Keys.D,
                Keys.E,
                Keys.F,
                Keys.G,
                Keys.H,
                Keys.I,
                Keys.J,
                Keys.K,
                Keys.L,
                Keys.M,
                Keys.N,
                Keys.O,
                Keys.P,
                Keys.Q,
                Keys.R,
                Keys.S,
                Keys.T,
                Keys.U,
                Keys.V,
                Keys.W,
                Keys.X,
                Keys.Y,
                Keys.Z
            };

            _numbers = new HashSet<Keys>
            {
                Keys.D0,
                Keys.D1,
                Keys.D2,
                Keys.D3,
                Keys.D4,
                Keys.D5,
                Keys.D6,
                Keys.D7,
                Keys.D8,
                Keys.D9,
            };

            _controls = new HashSet<Keys>
            {
                Keys.Escape,
                Keys.Enter,
                Keys.ControlKey,
                Keys.Alt,
                Keys.Space,
                Keys.Return,
                Keys.Delete
            };
            
            _keys = new Dictionary<ScreenType, Dictionary<Keys, EventHandler>>();
            
            _keys[ScreenType.MainMenu] = new Dictionary<Keys, EventHandler>();
            _keys[ScreenType.MainMenu][Keys.Escape] = Application.Exit;
        }

        public void KeyPress(Keys key)
        {
            var kc = new KeysConverter();
                    
            if (_manager.HasFocus)
            {
                var item = _manager.GetComponentWithFocus();

                if (_characters.Contains(key) || _numbers.Contains(key))
                    item.TypeCharacter(kc.ConvertToString(key)[0]);
                else if (key == Keys.Back)
                    item.EraseLastCharacter();
            }
            else
            {
                if (_controls.Contains(key))
                    _keys[_manager.ScreenType][key].Invoke();
            }
        }
    }
}
