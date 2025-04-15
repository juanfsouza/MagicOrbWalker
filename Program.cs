using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MagicOrbwalker1.Essentials;
using static MagicOrbwalker1.Essentials.Keyboard;

namespace MagicOrbwalker1
{
    internal static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        static bool cKeyPressed = false;
        static bool middlePressed = false;

        [STAThread]
        static async Task Main()
        {
            if (!AllocConsole())
            {
                Console.WriteLine("Falha ao alocar console.");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Menu
            Thread LobbyHandle = new Thread(() => CNSL.LobbyShow());
            LobbyHandle.Start();

            // Overlay
            Thread overlay = new Thread(() => makeoverlay());
            overlay.Start();

            // Inicializar Orbwalker
            var orbwalker = new Orbwalker();

            // Loop principal
            while (true)
            {
                if (SpecialFunctions.IsTargetProcessFocused("League of Legends"))
                {
                    bool isChampionDead = Values.IsChampionDead ?? true;
                    if (!isChampionDead)
                    {
                        // Skill shots
                        if ((GetAsyncKeyState(Keys.Q) & 0x8000) != 0)
                        {
                            await orbwalker.CastSkillShot("Q", ScanCodeShort.KEY_Q);
                        }
                        if ((GetAsyncKeyState(Keys.W) & 0x8000) != 0)
                        {
                            await orbwalker.CastSkillShot("W", ScanCodeShort.KEY_W);
                        }

                        // Orbwalking
                        bool isSpacePressed = (GetAsyncKeyState(Keys.Space) & 0x8000) != 0;
                        if (isSpacePressed && SpecialFunctions.AAtick < Environment.TickCount)
                        {
                            if (Values.ShowAttackRange && !cKeyPressed)
                            {
                                SendKeyDown(ScanCodeShort.KEY_C);
                                cKeyPressed = true;
                            }
                            if (Values.AttackChampionOnly && !middlePressed)
                            {
                                SendMiddleMouseDown();
                                middlePressed = true;
                            }
                            await OrbwalkEnemy(orbwalker);
                        }
                        else
                        {
                            if (Values.ShowAttackRange && cKeyPressed)
                            {
                                SendKeyUp(ScanCodeShort.KEY_C);
                                cKeyPressed = false;
                            }
                            if (Values.AttackChampionOnly && middlePressed)
                            {
                                SendMiddleMouseUp();
                                middlePressed = false;
                            }
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        private static void makeoverlay()
        {
            using (var overlay = new Drawings())
            {
                overlay.Run();
            }
        }

        private static Random rnd = new Random();

        private static async Task OrbwalkEnemy(Orbwalker orbwalker)
        {
            if (SpecialFunctions.CanAttack())
            {
                try
                {
                    // Usar leitura de memória para encontrar inimigo
                    GameObject player = orbwalker.memory.ReadMemory<GameObject>(Orbwalker.LOCAL_PLAYER);
                    Vector3 targetPos = orbwalker.GetClosestEnemyPosition(player.Position);
                    if (targetPos.X != 0 || targetPos.Y != 0)
                    {
                        Vector2 screenPos = orbwalker.WorldToScreen(targetPos);
                        if (screenPos.X != 0 || screenPos.Y != 0)
                        {
                            Point enemyPos = new Point((int)screenPos.X, (int)screenPos.Y);
                            Point originalPos = Cursor.Position;
                            SpecialFunctions.ClickAt(enemyPos, rightClick: true); // Clique direito para atacar

                            SpecialFunctions.AAtick = Environment.TickCount;
                            int windupDelay = SpecialFunctions.GetAttackWindup();
                            SpecialFunctions.MoveCT = Environment.TickCount + windupDelay;

                            SpecialFunctions.SetCursorPos(originalPos.X, originalPos.Y);

                            if (Values.attackSpeed < 1.75)
                            {
                                await Task.Delay(Values.SleepOnLowAS);
                            }
                        }
                    }
                    else
                    {
                        SpecialFunctions.Click(rightClick: true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro no orbwalking: {ex.Message}");
                }
            }
            else if (SpecialFunctions.CanMove() && SpecialFunctions.MoveCT <= Environment.TickCount)
            {
                SpecialFunctions.Click(rightClick: true);
                Values.originalMousePosition = Cursor.Position;
                SpecialFunctions.MoveCT = Environment.TickCount + rnd.Next(50, 80);
            }
            else
            {
                await Task.Delay(1);
                SpecialFunctions.Click(rightClick: true);
            }
        }
    }
}