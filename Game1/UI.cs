using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    /// <summary>
    /// The UI System.
    /// </summary>
    class UI
    {
        private int uiState;
        //private AnimatedSprite currentSprite;

        /// <summary>
        /// Initializes the UI System.
        /// </summary>
        public UI()
        {
            uiState = 0; //no ui
        }
        public void Update()
        {
            
        }
        public void Draw()
        {
            switch (uiState)
            {
                case 1:

                    break;
            }
        }
    }
}
