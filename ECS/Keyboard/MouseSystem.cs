using System;
using System.Runtime.InteropServices;
using Leopotam.Ecs;

namespace ECS.Keyboard
{
    public class MouseSystem : IEcsRunSystem
    {
        private readonly EcsFilter<InputComponent> _inputFilter;

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        public void Run()
        {
            GetCursorPos(out POINT point);
            ScreenToClient(GetActiveWindow(), ref point);

            foreach (int i in _inputFilter)
            {
                ref var inputComponent = ref _inputFilter.Get1(i);

                inputComponent.X = point.X;
                inputComponent.Y = point.Y;
                inputComponent.LeftButton = inputComponent.Mouse.GetCurrentState().Buttons[0];
            }
        }
    }
}
