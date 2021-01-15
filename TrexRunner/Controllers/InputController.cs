using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TrexRunner.Entities;

namespace TrexRunner.Controllers
{
    public class InputController
    {
        private TrexEntity _trexEntity;

        private KeyboardState _previousKeyboardState;

        public InputController(TrexEntity trexEntity)
        {
            _trexEntity = trexEntity;
        }

        public void ProcessControls(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if(!_previousKeyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Up))
            {
                if (_trexEntity.State != TrexState.Jumping)
                    _trexEntity.StartJump();
                else
                    _trexEntity.ContinueJump();
            }

            _previousKeyboardState = keyboardState;
        }
    }
}
