using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.Fuzzy.FuzzyTerms;

namespace AAI_Final_Assignment_WinForms.Fuzzy
{
    public class EnemyModule
    {
        private FuzzyVariable speed;
        private FuzzyVariable massa;
        private FuzzyVariable damageDealt;

        public FuzzyModule FuzzyEnemyModule;
        public EnemyModule() {
            // Main module init
            FuzzyEnemyModule = new FuzzyModule();

            // Antecedents
            speed = FuzzyEnemyModule.CreateFLV("Speed");
            massa = FuzzyEnemyModule.CreateFLV("Massa");

            // Consequent
            damageDealt = FuzzyEnemyModule.CreateFLV("DamageDealt");

            // Sets
            FzSet slow = speed.AddLeftShoulderSet("EnemySlow", 0, 3, 7);
            FzSet average = speed.AddTriangularSet("EnemyNormal", 3, 7, 16);
            FzSet fast = speed.AddRightShoulderSet("EnemyFast", 7, 16, 20);

            FzSet light = massa.AddTriangularSet("EnemyLight", 0, 0, 50);
            FzSet normal = massa.AddTriangularSet("EnemyNormal", 0, 50, 80);
            FzSet heavy = massa.AddRightShoulderSet("EnemyHeavy", 50, 80, 100);
            
            FzSet lowDamage = damageDealt.AddTriangularSet("LowDamageDealt", 1, 1, 10);
            FzSet mediumDamage = damageDealt.AddTriangularSet("MediumDamageDealt", 1, 10, 25);
            FzSet highDamage = damageDealt.AddTriangularSet("HighDamageDealt", 10, 25, 25);

            FuzzyEnemyModule.AddRule(new AndFuzzyTerm(slow, light), mediumDamage);
            FuzzyEnemyModule.AddRule(new AndFuzzyTerm(slow, normal), mediumDamage);
            FuzzyEnemyModule.AddRule(new AndFuzzyTerm(slow, heavy), highDamage);

            FuzzyEnemyModule.AddRule(new AndFuzzyTerm(average, light), lowDamage);
            FuzzyEnemyModule.AddRule(new AndFuzzyTerm(average, normal), mediumDamage);
            FuzzyEnemyModule.AddRule(new AndFuzzyTerm(average, heavy), highDamage);

            FuzzyEnemyModule.AddRule(new AndFuzzyTerm(fast, light), lowDamage);
            FuzzyEnemyModule.AddRule(new AndFuzzyTerm(fast, normal), lowDamage);
            FuzzyEnemyModule.AddRule(new AndFuzzyTerm(fast, heavy), mediumDamage);
        }
    }
}
