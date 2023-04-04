using AAI_Final_Assignment_WinForms.Fuzzy;

namespace Test; 

public class GameModuleTest {
    private EnemyModule enemyModule;

    [SetUp]
    public void Setup() {
        // game module
        enemyModule = new EnemyModule();
    }

    [Test]
    [DefaultFloatingPointTolerance(0.1f)]
    public void EnemyDefuzzifyHighDamage() {
        //As slow and heavy as possible -> Should return max value.

        enemyModule.FuzzyEnemyModule.Fuzzify("Speed", 1);
        enemyModule.FuzzyEnemyModule.Fuzzify("Mass", 100);

        var result = enemyModule.FuzzyEnemyModule.DeFuzzify("Damage");
        Assert.AreEqual(25f, result);
    }

    [Test]
    [DefaultFloatingPointTolerance(0.1f)]
    public void EnemyDefuzzifyLowDamage() {
        //As fast and light as possible -> Should return 1 damage.
        enemyModule.FuzzyEnemyModule.Fuzzify("Speed", 20);
        enemyModule.FuzzyEnemyModule.Fuzzify("Mass", 1);

        var result2 = enemyModule.FuzzyEnemyModule.DeFuzzify("Damage");
        Assert.AreEqual(1f, result2);
    }
}