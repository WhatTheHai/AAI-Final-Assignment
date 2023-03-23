using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.Fuzzy;
using AAI_Final_Assignment_WinForms.Fuzzy.FuzzySets;
using AAI_Final_Assignment_WinForms.Fuzzy.FuzzyTerms;

namespace Test
{
    public class FuzzyLogicTest
    {
        private FuzzyModule testModule;

        [SetUp]
        public void Setup()
        {
            // main module
            testModule = new FuzzyModule();

            //antecedent
            FuzzyVariable distToTarget = testModule.CreateFLV("DistanceToTarget");
            FuzzyVariable ammoStatus = testModule.CreateFLV("AmmoStatus");

            // consequent

            FuzzyVariable desirability = testModule.CreateFLV("Desirability");


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
        }

        [Test]
        public void Logic()
        {
            testModule.Fuzzify("DistanceToTarget", 200);
            testModule.Fuzzify("AmmoStatus", 8);

            // av 
            var result = testModule.DeFuzzify("Desirability");

            Assert.AreEqual(0.67, result);
        }

        [Test]
        public void Fuzzify()
        {
            LeftShoulderFuzzySet close = new LeftShoulderFuzzySet(25, 25 - 0, 150 - 25);
            TriangleFuzzySet medium = new TriangleFuzzySet(150, 150 - 25, 300 - 150);
            RightShoulderFuzzySet far = new RightShoulderFuzzySet(300, 300 - 150, 500 - 300);

            var closeC = close.CalculateDOM(200);
            var mediumC = medium.CalculateDOM(200);
            var farC = far.CalculateDOM(200);
            // todo: waarom fuzzify fout...
            var r1 = 0;
        }
    }
}