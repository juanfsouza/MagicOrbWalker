namespace MagicOrbwalker1.Essentials
{
    public struct Vector3
    {
        public float X;
        public float Y;
        public float Z;
    }

    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    public struct GameObject
    {
        public int Team;
        public Vector3 Position;
        public bool IsVisible;
        public float Health;
        public float MaxHealth;
        public float AttackRange;
        public float AttackSpeed;
        public bool IsAlive;
    }

    public struct Spell
    {
        public int Level;
        public float ReadyTime1;
        public float ReadyTime2;
        public float TotalCooldown1;
        public float TotalCooldown2;
    }
}