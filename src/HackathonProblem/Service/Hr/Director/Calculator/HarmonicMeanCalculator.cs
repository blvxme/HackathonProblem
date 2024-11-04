namespace HackathonProblem.Service.Hr.Director.Calculator;

public class HarmonicMeanCalculator : IHarmonyCalculator
{
    public decimal CalculateHarmony(IEnumerable<int> satisfactions) =>
        satisfactions.Count() / satisfactions.Sum(satisfaction => 1.0m / satisfaction);
}
