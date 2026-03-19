namespace CrossRoundArena.Core
{
    public interface IBattleTarget
    {
        int CurrentHP { get; }
        void TakeDamage(int amount, DamageSource source);
        bool IsDead { get; }
    }

    public enum DamageSource
    {
        MonsterAttack,
        Skill,
        Event,
        Poison
    }
}
