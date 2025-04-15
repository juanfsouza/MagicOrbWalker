using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace MagicOrbwalker1.Essentials
{
    class SpecialFunctions
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;

        public static bool IsTargetProcessFocused(string processName)
        {
            IntPtr activeWindowHandle = GetForegroundWindow();
            GetWindowThreadProcessId(activeWindowHandle, out int activeProcId);

            try
            {
                Process activeProcess = Process.GetProcessById(activeProcId);
                return activeProcess != null && activeProcess.ProcessName.Equals(processName, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                Console.WriteLine("League of Legends Game not found!!");
                return false;
            }
        }

        public static void ClickAt(Point location, bool rightClick = false)
        {
            SetCursorPos(location.X, location.Y);
            Thread.Sleep(10);

            if (rightClick)
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, (uint)location.X, (uint)location.Y, 0, 0);
                Thread.Sleep(10);
                mouse_event(MOUSEEVENTF_RIGHTUP, (uint)location.X, (uint)location.Y, 0, 0);
            }
            else
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)location.X, (uint)location.Y, 0, 0);
                Thread.Sleep(10);
                mouse_event(MOUSEEVENTF_LEFTUP, (uint)location.X, (uint)location.Y, 0, 0);
            }
        }

        public static void Click(bool rightClick = false)
        {
            if (rightClick)
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                Thread.Sleep(10);
                mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            }
            else
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                Thread.Sleep(10);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
        }

        public static int AAtick;
        public static int MoveCT;

        public static int GetAttackWindup()
        {
            float windup = Values.getWindup();
            float attackSpeed = Values.attackSpeed; // Pode ser atualizado via memória no futuro
            int finalWindUP = (int)((1 / attackSpeed * 1000) * windup);
            return finalWindUP;
        }

        public static int GetAttackDelay()
        {
            float attackSpeed = Values.attackSpeed;
            return (int)(1000.0f / attackSpeed);
        }

        public static bool CanAttack()
        {
            int attackDelay = GetAttackDelay();
            return AAtick + attackDelay < Environment.TickCount;
        }

        public static bool CanMove()
        {
            if (Values.SelectedChamp == "Kalista")
            {
                return true;
            }
            else
            {
                return MoveCT <= Environment.TickCount;
            }
        }
    }
}