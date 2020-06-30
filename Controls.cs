using System.Collections.Generic;
using System.Windows.Forms;

namespace TableFootball
{
    class Controls
    {
        public Dictionary<Keys, Action> ControlsKeys { get; }

        public Controls()
        {
            ControlsKeys = new Dictionary<Keys, Action>
            {
                { Keys.W, Action.MoveUp },
                { Keys.S, Action.MoveDown }
            };
        }
    }
}
