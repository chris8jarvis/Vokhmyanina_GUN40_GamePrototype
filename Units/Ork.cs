

namespace GamePrototype.Units
{
    public sealed class Ork : Unit
    {
        public Ork(string name, uint health, uint maxHealth, uint baseDamage) : base(name, health, maxHealth, baseDamage)
        {
        }

        public override uint GetUnitDamage() => BaseDamage;

        public override void HandleCombatComplete() => Health = MaxHealth;

        protected override uint CalculateAppliedDamage(uint damage) => damage;
    }
}
