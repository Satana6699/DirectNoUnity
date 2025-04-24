using SharpDX.DirectInput;

namespace ECS.Keyboard
{
    public struct InputComponent
    {
        public int X;
        public int Y;
        public bool LeftButton;
        public bool CurrentMouseState;
        public bool PreviousMouseState;

        public SharpDX.DirectInput.Keyboard Keyboard;
        public KeyboardState CurrentKeyboardState;
        public KeyboardState PreviousKeyboardState;
        public Mouse Mouse;

        public void UpdateInput()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetCurrentState();

            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetCurrentState().Buttons[0];
        }

        public bool IsKeyJustPressed(Key key)
        {
            return CurrentKeyboardState.IsPressed(key) && !PreviousKeyboardState.IsPressed(key);
        }

        public bool IsMouseJustPressed()
        {
            return CurrentMouseState && !PreviousMouseState;
        }
    }
}
