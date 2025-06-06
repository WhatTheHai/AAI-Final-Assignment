﻿using AAI_Final_Assignment_WinForms.Fuzzy.FuzzyTerms;
using Newtonsoft.Json;

namespace AAI_Final_Assignment_WinForms.Fuzzy; 

public class FuzzyConfig {
    public Dictionary<string, FuzzyVariableConfig> Variables { get; set; }
    public Dictionary<string, FuzzyRuleConfig> Rules { get; set; }
}

public class FuzzyVariableConfig {
    public string Name { get; set; }
    public List<FuzzySetConfig> Sets { get; set; }
}

public class FuzzySetConfig {
    public string Name { get; set; }
    public string Type { get; set; }
    public float[] MinPeakMax { get; set; }
}

public class FuzzyRuleConfig {
    public string Antecedent1 { get; set; }
    public string Antecedent2 { get; set; }
    public string Consequent { get; set; }
}

public class EnemyModule {
    public FuzzyModule FuzzyEnemyModule;

    public EnemyModule() {
        // Main module init
        FuzzyEnemyModule = new FuzzyModule();
        InitializeModule();
    }

    public void InitializeModule() {
        // Load JSON
        var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        var jsonString = File.ReadAllText(jsonPath);
        var config = JsonConvert.DeserializeObject<FuzzyConfig>(jsonString);

        foreach (var variableName in config.Variables.Keys) {
            // Create Speed Mass and Damage Dealt
            var flv = FuzzyEnemyModule.CreateFLV(variableName);
            foreach (var set in config.Variables[variableName].Sets)
                flv.AddFzSet(set.Name, set.Type, set.MinPeakMax[0], set.MinPeakMax[1], set.MinPeakMax[2]);
        }

        // Rules
        foreach (var rule in config.Rules.Values) {
            var antecedent1 = FuzzyEnemyModule.GetFLV(config.Variables.Keys.First()).GetFzSet(rule.Antecedent1);
            var antecedent2 = FuzzyEnemyModule.GetFLV(config.Variables.Keys.ElementAt(1)).GetFzSet(rule.Antecedent2);
            var consequent = FuzzyEnemyModule.GetFLV(config.Variables.Keys.Last()).GetFzSet(rule.Consequent);
            FuzzyEnemyModule.AddRule(new AndFuzzyTerm(antecedent1, antecedent2), consequent);
        }
    }
}