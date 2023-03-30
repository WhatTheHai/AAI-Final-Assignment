using AAI_Final_Assignment_WinForms.Fuzzy;
using AAI_Final_Assignment_WinForms.Fuzzy.FuzzySets;
using AAI_Final_Assignment_WinForms.Fuzzy.FuzzyTerms;


namespace Test
{
    public class FuzzyLogicTest
    {
        private FuzzyModule testModule;
        private EnemyModule enemyModule;
        private FuzzyVariable distToTarget;
        private FuzzyVariable ammoStatus;
        private FuzzyVariable desirability;

        [SetUp]
        public void Setup()
        {
            // main module
            testModule = new FuzzyModule();

            //antecedent
            distToTarget = testModule.CreateFLV("DistanceToTarget");
            ammoStatus = testModule.CreateFLV("AmmoStatus");

            // consequent

            desirability = testModule.CreateFLV("Desirability");

            // game module
            enemyModule = new EnemyModule();
        }

        [Test]
        public void Logic()
        {
            // create sets

            FzSet targetClose = distToTarget.AddLeftShoulderSet("TargetClose", 0, 25, 150); // 0.0f ok
            FzSet targetMedium = distToTarget.AddTriangularSet("TargetMedium", 25, 150, 300); // 
            FzSet targetFar = distToTarget.AddRightShoulderSet("TargetFar", 150, 300, 500);

            FzSet ammoLow = ammoStatus.AddTriangularSet("AmmoLow", 0, 0, 10);
            FzSet ammoOkay = ammoStatus.AddTriangularSet("AmmoOkay", 0, 10, 30);
            FzSet ammoLoads = ammoStatus.AddRightShoulderSet("AmmoLoads", 10, 30, 100);

            FzSet undesirable = desirability.AddLeftShoulderSet("Undesirable", 0, 25, 50);
            FzSet desirable = desirability.AddTriangularSet("Desirable", 25, 50, 75);
            FzSet veryDesirable = desirability.AddRightShoulderSet("VeryDesirable", 50, 75, 100);

            testModule.AddRule(new AndFuzzyTerm(targetFar, ammoLoads), desirable);
            testModule.AddRule(new AndFuzzyTerm(targetFar, ammoOkay), undesirable);
            testModule.AddRule(new AndFuzzyTerm(targetFar, ammoLow), undesirable);

            testModule.AddRule(new AndFuzzyTerm(targetMedium, ammoLoads), veryDesirable);
            testModule.AddRule(new AndFuzzyTerm(targetMedium, ammoOkay), veryDesirable);
            testModule.AddRule(new AndFuzzyTerm(targetMedium, ammoLow), desirable);

            testModule.AddRule(new AndFuzzyTerm(targetClose, ammoLoads), undesirable);
            testModule.AddRule(new AndFuzzyTerm(targetClose, ammoOkay), undesirable);
            testModule.AddRule(new AndFuzzyTerm(targetClose, ammoLow), undesirable);

            testModule.Fuzzify("DistanceToTarget", 200);
            testModule.Fuzzify("AmmoStatus", 8);

            // av 
            var result = testModule.DeFuzzify("Desirability");

            Assert.AreEqual(60.625, result);
        }

        [Test]
        [DefaultFloatingPointTolerance(0.01)]
        public void CalculateDom()
        {
            // target
            LeftShoulderFuzzySet targetLeft = new LeftShoulderFuzzySet(25, 25 - 0, 150 - 25);
            TriangleFuzzySet targetMiddle = new TriangleFuzzySet(150, 150 - 25, 300 - 150);
            RightShoulderFuzzySet targetRight = new RightShoulderFuzzySet(300, 300 - 150, 500 - 300);


            var targetClose = targetLeft.CalculateDOM(200);
            var targetMedium = targetMiddle.CalculateDOM(200);
            var targetFar = targetRight.CalculateDOM(200);

            Assert.AreEqual(0f, targetClose);
            Assert.AreEqual(0.66f, targetMedium);
            Assert.AreEqual(0.33f, targetFar);

            // ammo
            TriangleFuzzySet ammoLeft = new TriangleFuzzySet(0, 0 - 0, 10 - 0);
            TriangleFuzzySet ammoMiddle = new TriangleFuzzySet(10, 10 - 0, 30 - 10);
            RightShoulderFuzzySet ammoRight = new RightShoulderFuzzySet(30, 30 - 10, 100 - 30);

            var ammoLow = ammoLeft.CalculateDOM(8);
            var ammoMedium = ammoMiddle.CalculateDOM(8);
            var ammoLoads = ammoRight.CalculateDOM(8);

            Assert.AreEqual(0.2f, ammoLow);
            Assert.AreEqual(0.8f, ammoMedium);
            Assert.AreEqual(0f, ammoLoads);
        }

        [Test]
        [DefaultFloatingPointTolerance(0.000001)]
        public void DeFuzzify()
        {
            // create sets
            FzSet targetClose = distToTarget.AddLeftShoulderSet("TargetClose", 0, 25, 150); // 0.0f ok
            FzSet targetMedium = distToTarget.AddTriangularSet("TargetMedium", 25, 150, 300); // 
            FzSet targetFar = distToTarget.AddRightShoulderSet("TargetFar", 150, 300, 500);

            FzSet ammoLow = ammoStatus.AddTriangularSet("AmmoLow", 0, 0, 10);
            FzSet ammoOkay = ammoStatus.AddTriangularSet("AmmoOkay", 0, 10, 30);
            FzSet ammoLoads = ammoStatus.AddRightShoulderSet("AmmoLoads", 10, 30, 100);

            FzSet undesirable = desirability.AddLeftShoulderSet("Undesirable", 0, 25, 50);
            FzSet desirable = desirability.AddTriangularSet("Desirable", 25, 50, 75);
            FzSet veryDesirable = desirability.AddRightShoulderSet("VeryDesirable", 50, 75, 100);

            testModule.Fuzzify("DistanceToTarget", 200);
            testModule.Fuzzify("AmmoStatus", 8);

            testModule.AddRule(new AndFuzzyTerm(targetFar, ammoLoads), desirable);

            var result = testModule.DeFuzzify("Desirability"); // 0

            Assert.AreEqual(0f, result);

            testModule.rules.Clear();

            testModule.AddRule(new AndFuzzyTerm(targetFar, ammoOkay), undesirable);

            result = testModule.DeFuzzify("Desirability"); // 0.33 = 12.5 
            Assert.AreEqual(12.5f, result);

            testModule.rules.Clear();

            testModule.AddRule(new AndFuzzyTerm(targetFar, ammoLow), undesirable);

            result = testModule.DeFuzzify("Desirability"); // 0.2 = 12.5 
            Assert.AreEqual(12.5f, result);
        }

        [Test]
        [DefaultFloatingPointTolerance(0.1f)]
        public void EnemyDefuzzify() {
            //As slow and heavy as possible -> Should return max value.

            enemyModule.FuzzyEnemyModule.Fuzzify("Speed", 1);
            enemyModule.FuzzyEnemyModule.Fuzzify("Massa", 100);
            
            var result = enemyModule.FuzzyEnemyModule.DeFuzzify("DamageDealt");
            Assert.AreEqual(25f, result);

            
            //As fast and light as possible -> Should return 1 damage.
            enemyModule.FuzzyEnemyModule.Fuzzify("Speed", 20);
            enemyModule.FuzzyEnemyModule.Fuzzify("Massa", 1);

            var result2 = enemyModule.FuzzyEnemyModule.DeFuzzify("DamageDealt");
            Assert.AreEqual(1f, result2);
        }
    }
}