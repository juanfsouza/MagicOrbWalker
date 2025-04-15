using System.Collections.Generic;
using System.Drawing;

namespace MagicOrbwalker1.Essentials
{
    class Values
    {
        public static bool? IsChampionDead = false;
        public static float attackSpeed = 1;
        public static float attackRange = 550;
        public static float Windup;
        public static string? SelectedChamp = "Ezreal";
        public static Point EnemyPosition = Point.Empty;
        public static Point originalMousePosition;
        public static bool ShowAttackRange = true;
        public static bool AttackChampionOnly = true;
        public static int SleepOnLowAS = 150;

        public static bool DrawingsEnabled = true;

        private static readonly Dictionary<string, float> ChampionWindups = new Dictionary<string, float>
        {
            { "Ashe", 21.93f },
            { "Caitlyn", 17.708f },
            { "Corki", 10.00f },
            { "Draven", 15.614f },
            { "Ezreal", 18.839f },
            { "Jinx", 16.875f },
            { "Kaisa", 16.108f },
            { "Kalista", 36.000f },
            { "Kayle", 19.355f },
            { "Kindred", 17.544f },
            { "KogMaw", 16.622f },
            { "Lucian", 15.00f },
            { "MissFortune", 14.801f },
            { "Quinn", 17.544f },
            { "Samira", 15.00f },
            { "Senna", 31.25f },
            { "Sivir", 12.00f },
            { "Tristana", 14.801f },
            { "Twitch", 20.192f },
            { "Varus", 17.544f },
            { "Vayne", 17.544f },
            { "Xayah", 17.687f },
            { "Xerath", 25.074f }
        };

        public static readonly Dictionary<string, float> QSkillRanges = new Dictionary<string, float>
        {
            { "Ashe", 0f },
            { "Caitlyn", 1380f },
            { "Corki", 1225f },
            { "Draven", 0f },
            { "Ezreal", 1150f },
            { "Jinx", 0f },
            { "Kaisa", 0f },
            { "Kalista", 1480f },
            { "Kayle", 1180f },
            { "Kindred", 0f },
            { "KogMaw", 1380f },
            { "Lucian", 900f },
            { "MissFortune", 910f },
            { "Quinn", 1380f },
            { "Samira", 1180f },
            { "Senna", 0f },
            { "Sivir", 1380f },
            { "Tristana", 0f },
            { "Twitch", 0f },
            { "Varus", 1475f },
            { "Vayne", 0f },
            { "Xayah", 1180f },
            { "Xerath", 1480f }
        };

        public static readonly Dictionary<string, float> WSkillRanges = new Dictionary<string, float>
        {
            { "Ashe", 1380f },
            { "Caitlyn", 1216f },
            { "Corki", 0f },
            { "Draven", 0f },
            { "Ezreal", 1000f },
            { "Jinx", 1480f },
            { "Kaisa", 1480f },
            { "Kalista", 0f },
            { "Kayle", 0f },
            { "Kindred", 0f },
            { "KogMaw", 0f },
            { "Lucian", 1180f },
            { "MissFortune", 0f },
            { "Quinn", 0f },
            { "Samira", 0f },
            { "Senna", 0f },
            { "Sivir", 0f },
            { "Tristana", 1180f },
            { "Twitch", 1180f },
            { "Varus", 0f },
            { "Vayne", 0f },
            { "Xayah", 0f },
            { "Xerath", 1380f }
        };

        public static readonly Dictionary<string, float> ESkillRanges = new Dictionary<string, float>
        {
            { "Ashe", 0f },
            { "Caitlyn", 800f },
            { "Corki", 0f },
            { "Draven", 1100f },
            { "Ezreal", 750f },
            { "Jinx", 900f },
            { "Kaisa", 0f },
            { "Kalista", 0f },
            { "Kayle", 0f },
            { "Kindred", 0f },
            { "KogMaw", 1200f },
            { "Lucian", 425f },
            { "MissFortune", 800f },
            { "Quinn", 0f },
            { "Samira", 0f },
            { "Senna", 0f },
            { "Sivir", 0f },
            { "Tristana", 0f },
            { "Twitch", 0f },
            { "Varus", 925f },
            { "Vayne", 550f },
            { "Xayah", 0f },
            { "Xerath", 1050f }
        };

        public static readonly Dictionary<string, float> RSkillRanges = new Dictionary<string, float>
        {
            { "Ashe", 25000f },
            { "Caitlyn", 3500f },
            { "Corki", 0f },
            { "Draven", 25000f },
            { "Ezreal", 25000f },
            { "Jinx", 25000f },
            { "Kaisa", 0f },
            { "Kalista", 0f },
            { "Kayle", 0f },
            { "Kindred", 0f },
            { "KogMaw", 0f },
            { "Lucian", 1400f },
            { "MissFortune", 0f },
            { "Quinn", 0f },
            { "Samira", 0f },
            { "Senna", 25000f },
            { "Sivir", 0f },
            { "Tristana", 0f },
            { "Twitch", 0f },
            { "Varus", 1200f },
            { "Vayne", 0f },
            { "Xayah", 0f },
            { "Xerath", 5000f }
        };

        public static float getWindup()
        {
            if (ChampionWindups.TryGetValue(SelectedChamp ?? "Ezreal", out float windup))
            {
                Windup = windup;
                return windup;
            }
            return 18.839f;
        }

        public static bool MakeCorrectWindup()
        {
            return true;
        }
    }
}