using System;
using System.Drawing;
using System.Threading.Tasks;
using static MagicOrbwalker1.Essentials.Keyboard;

namespace MagicOrbwalker1.Essentials
{
    public class Orbwalker
    {
        public readonly MemoryReader memory;
        public static readonly IntPtr LOCAL_PLAYER = (IntPtr)0x1b2b0f8;       // Jogador local
        public static readonly IntPtr HERO_LIST = (IntPtr)0x1afeb38;         // Lista de Heróis
        public static readonly IntPtr VIEW_PROJ_MATRICES = (IntPtr)0x1b88740; // Matrizes de projeção
        public static readonly IntPtr RENDERER = (IntPtr)0x1b8de48;          // Renderizador

        public Orbwalker()
        {
            memory = new MemoryReader();
        }

        public async Task CastSkillShot(string ability, ScanCodeShort key)
        {
            try
            {
                // Verificar se a habilidade está pronta
                int spellOffset = ability == "Q" ? 0x0 : ability == "W" ? 0x28 : 0x50; // Ajustar para Q, W, etc.
                float readyTime = memory.ReadMemory<float>(LOCAL_PLAYER, new[] { 0x3b80, spellOffset, 0x30 }); // SpellReadyTime1
                if (readyTime > 0)
                {
                    Console.WriteLine($"{ability} está em cooldown: {readyTime}s");
                    return;
                }

                // Obter dados do jogador local
                GameObject localPlayer = memory.ReadMemory<GameObject>(LOCAL_PLAYER);

                // Encontrar o inimigo mais próximo
                Vector3 targetPos = GetClosestEnemyPosition(localPlayer.Position);

                if (targetPos.X == 0 && targetPos.Y == 0)
                {
                    Console.WriteLine("Nenhum inimigo encontrado.");
                    return;
                }

                // Converter posição 3D para 2D
                Vector2 screenPos = WorldToScreen(targetPos);

                if (screenPos.X == 0 && screenPos.Y == 0)
                {
                    Console.WriteLine("Falha ao converter posição para tela.");
                    return;
                }

                // Mover cursor e disparar habilidade
                Point originalPos = Cursor.Position;
                SpecialFunctions.SetCursorPos((int)screenPos.X, (int)screenPos.Y);
                await Task.Delay(50);
                SendKeyDown(key);
                await Task.Delay(50);
                SendKeyUp(key);
                SpecialFunctions.SetCursorPos(originalPos.X, originalPos.Y);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao lançar skill shot: {ex.Message}");
            }
        }

        public Vector3 GetClosestEnemyPosition(Vector3 playerPos)
        {
            Vector3 closestPos = new Vector3();
            float minDistance = float.MaxValue;

            try
            {
                // Ler lista de heróis
                int heroCount = memory.ReadMemory<int>(HERO_LIST, new[] { 0x8 });
                IntPtr heroArray = memory.ReadMemory<IntPtr>(HERO_LIST, new[] { 0x4 });

                for (int i = 0; i < heroCount; i++)
                {
                    IntPtr heroPtr = memory.ReadMemory<IntPtr>(heroArray + i * IntPtr.Size);
                    GameObject hero = memory.ReadMemory<GameObject>(heroPtr);

                    if (hero.IsAlive && hero.IsVisible && hero.Health > 0 && hero.Team != memory.ReadMemory<int>(LOCAL_PLAYER, new[] { 0x249 }))
                    {
                        float distance = CalculateDistance(playerPos, hero.Position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestPos = hero.Position;
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Erro ao ler lista de heróis.");
            }

            return closestPos;
        }

        private float CalculateDistance(Vector3 a, Vector3 b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
        }

        public Vector2 WorldToScreen(Vector3 worldPos)
        {
            try
            {
                // Ler matriz de projeção (simplificado, 4x4 matriz)
                byte[] matrixData = memory.ReadMemoryBytes(VIEW_PROJ_MATRICES, 16 * sizeof(float));
                float[] matrix = new float[16];
                Buffer.BlockCopy(matrixData, 0, matrix, 0, matrixData.Length);

                // Transformação 3D para 2D
                float screenX = worldPos.X * matrix[0] + worldPos.Y * matrix[4] + worldPos.Z * matrix[8] + matrix[12];
                float screenY = worldPos.X * matrix[1] + worldPos.Y * matrix[5] + worldPos.Z * matrix[9] + matrix[13];
                float w = worldPos.X * matrix[3] + worldPos.Y * matrix[7] + worldPos.Z * matrix[11] + matrix[15];

                if (w < 0.01f)
                    return new Vector2(0, 0);

                screenX /= w;
                screenY /= w;

                // Obter dimensões da tela
                int screenWidth = memory.ReadMemory<int>(RENDERER, new[] { 0xC });
                int screenHeight = memory.ReadMemory<int>(RENDERER, new[] { 0x10 });

                return new Vector2(
                    (screenX + 1) * screenWidth / 2,
                    (1 - screenY) * screenHeight / 2
                );
            }
            catch
            {
                Console.WriteLine("Erro ao converter posição 3D para 2D.");
                return new Vector2(0, 0);
            }
        }
    }
}