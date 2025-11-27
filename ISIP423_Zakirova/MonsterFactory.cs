using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TextRPG
{
    public class MonsterFactory
    {
        public static Enemy CreateRandomEnemy(Random random)
        {
            EnemyType[] types = (EnemyType[])Enum.GetValues(typeof(EnemyType));
            EnemyType chosenType = types[random.Next(types.Length)];

            return chosenType switch
            {
                EnemyType.Goblin => new Goblin(),
                EnemyType.Skeleton => new Skeleton(),
                EnemyType.Mage => new Mage(),
                EnemyType.Slime => new Slime(),
            };
        }

        public static Enemy CreateRandomBoss(Random random)
        {
            BossType[] types = (BossType[])Enum.GetValues(typeof(BossType));
            BossType chosenType = types[random.Next(types.Length)];

            return chosenType switch
            {
                BossType.VVG => new VVG(),
                BossType.Kovalsky => new Kovalsky(),
                BossType.ArchmageCPP => new ArchmageCPP(),
                BossType.PestovCS => new PestovCS(),
            };
        }
    }

    public enum EnemyType
    {
        Goblin,
        Skeleton,
        Mage,
        Slime 
    }

    public enum BossType
    {
        VVG,
        Kovalsky,
        ArchmageCPP,
        PestovCS
    }
}

