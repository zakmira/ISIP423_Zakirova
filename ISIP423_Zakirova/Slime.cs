using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG;

namespace TextRPG
{
    public class Slime : Enemy
    {
        public Slime()
        {
            Name = "Слизень";
            HP = 20;
            Attack = 8;
            Defense = 0;
        }

        public override int CalculateDamage(Player player, Random random)
        {
            // слизень уменьшает входящий урон на 2
            int baseDamage = player.EquippedWeapon.Damage;
            int reducedDamage = Math.Max(0, baseDamage - 2); // урон не может быть < 0
            return reducedDamage;
        }

        public override void ApplySpecialEffect(Player player, Random random)
        {
            // нету
        }
    }
}
