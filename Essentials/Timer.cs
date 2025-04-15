using System;

namespace MagicOrbwalker1.Essentials
{
    public static class Timer
    {
        /// <summary>
        /// Verifica se o tempo atual est� dentro de um intervalo espec�fico desde o �ltimo tick.
        /// </summary>
        /// <param name="lastTick">�ltimo tick registrado (em milissegundos).</param>
        /// <param name="interval">Intervalo desejado (em milissegundos).</param>
        /// <returns>True se o intervalo foi atingido, False caso contr�rio.</returns>
        public static bool HasPassed(int lastTick, int interval)
        {
            return Environment.TickCount >= lastTick + interval;
        }

        /// <summary>
        /// Calcula o tempo restante at� que um intervalo espec�fico seja atingido.
        /// </summary>
        /// <param name="lastTick">�ltimo tick registrado (em milissegundos).</param>
        /// <param name="interval">Intervalo desejado (em milissegundos).</param>
        /// <returns>Tempo restante em milissegundos, ou 0 se o intervalo j� passou.</returns>
        public static int TimeLeft(int lastTick, int interval)
        {
            int timeLeft = lastTick + interval - Environment.TickCount;
            return timeLeft > 0 ? timeLeft : 0;
        }

        /// <summary>
        /// Registra um novo tick com o tempo atual.
        /// </summary>
        /// <returns>O tick atual (em milissegundos).</returns>
        public static int GetCurrentTick()
        {
            return Environment.TickCount;
        }

        /// <summary>
        /// Aguarda at� que um intervalo espec�fico tenha passado desde o �ltimo tick.
        /// </summary>
        /// <param name="lastTick">�ltimo tick registrado (em milissegundos).</param>
        /// <param name="interval">Intervalo desejado (em milissegundos).</param>
        public static void WaitUntil(int lastTick, int interval)
        {
            int timeLeft = TimeLeft(lastTick, interval);
            if (timeLeft > 0)
            {
                System.Threading.Thread.Sleep(timeLeft);
            }
        }
    }
}