using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilities;

namespace exhibit
{
    class Combination
    {
        public delegate void EventFunction(List<Keys> keys);

        private globalKeyboardHook _gkh = new globalKeyboardHook();
        private EventFunction _f;
        private List<Keys> _combinaison;
        private List<bool> _pressed = new List<bool>();
        private bool _event = false;

        public Combination(List<Keys> combinaison, EventFunction f)
        {
            _f = f;
            _combinaison = combinaison;
            foreach (Keys k in _combinaison)
            {
                _gkh.HookedKeys.Add(k);
                _pressed.Add(false);
            }
            _gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            _gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
        }
        public void Start()
        {
            _gkh.hook();
        }
        public void Stop()
        {
            _gkh.unhook();
        }
        private void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            _event = false;
            _pressed[_combinaison.IndexOf(e.KeyCode)] = false;
        }

        private void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (_combinaison.IndexOf(e.KeyCode) == _combinaison.Count - 1)
            {
                bool before_pressed = true;
                for (int i = 0; i < _combinaison.IndexOf(e.KeyCode); i++)
                {
                    if (_pressed[i] == false)
                    {
                        before_pressed = false;
                    }
                }

                if (before_pressed == true && _event == false)
                {
                    _f(_combinaison);
                    _event = true;
                }
            }
            else
            {
                _pressed[_combinaison.IndexOf(e.KeyCode)] = true;
            }
        }
    }
}
