using HackathonProblem.Service.Hr.Director;
using HackathonProblem.Service.Hr.Director.Calculator;
using Nsu.HackathonProblem.Contracts;

namespace HackathonProblemTest;

public class HrDirectorTest
{
    /// <summary>
    /// При несоответствии количества команд количеству вишлистов тимлидов должно выбрасываться исключение
    /// </summary>
    [Fact]
    public void TestNumberOfTeamLeadsWishlists()
    {
        // ARRANGE
        var teams = new List<Team>
        {
            new(new Employee(10, "Астафьев Андрей"), new Employee(15, "Добрынин Степан")),
            new(new Employee(11, "Демидов Дмитрий"), new Employee(16, "Фомин Никита")),
            new(new Employee(12, "Климов Михаил"), new Employee(17, "Маркина Кристина"))
        };

        var teamLeadsWishlists = new List<Wishlist> { new(10, [15, 16, 17]), new(11, [16, 15, 17]) };
        var juniorsWishlists = new List<Wishlist> { new(15, [10, 11, 12]), new(16, [11, 10, 12]), new(17, [12, 10, 11]) };

        // ACT & ASSERT
        Assert.Throws<ArgumentException>(() => new HrDirector(new HarmonicMeanCalculator()).CalculateHarmonicMean(teams, teamLeadsWishlists, juniorsWishlists));
    }

    /// <summary>
    /// При несоответствии количества команд количеству вишлистов джунов должно выбрасываться исключение
    /// </summary>
    [Fact]
    public void TestNumberOfJuniorsWishlists()
    {
        // ARRANGE
        var teams = new List<Team>
        {
            new(new Employee(10, "Астафьев Андрей"), new Employee(15, "Добрынин Степан")),
            new(new Employee(11, "Демидов Дмитрий"), new Employee(16, "Фомин Никита")),
            new(new Employee(12, "Климов Михаил"), new Employee(17, "Маркина Кристина"))
        };

        var teamLeadsWishlists = new List<Wishlist> { new(10, [15, 16, 17]), new(11, [16, 15, 17]), new(12, [17, 15, 16]) };
        var juniorsWishlists = new List<Wishlist> { new(15, [10, 11, 12]), new(16, [11, 10, 12]) };

        // ACT & ASSERT
        Assert.Throws<ArgumentException>(() => new HrDirector(new HarmonicMeanCalculator()).CalculateHarmonicMean(teams, teamLeadsWishlists, juniorsWishlists));
    }

    /// <summary>
    /// Проверка алгоритма вычисления среднего гармонического
    /// (например, среднее гармоническое одинаковых чисел равно им всем)
    /// </summary>
    [Fact]
    public void TestHarmonicMeanAlgorithm()
    {
        // ARRANGE
        const int number = 5;
        var similarNumbers = new List<int> { number, number, number, number, number };

        // ACT
        var harmonicMean = new HarmonicMeanCalculator().CalculateHarmony(similarNumbers);

        // ASSERT
        Assert.Equal(number, harmonicMean);
    }

    /// <summary>
    /// Проверка вычисления среднего гармонического, конкретные примеры (например, 2 и 6 должны дать 3)
    /// </summary>
    [Fact]
    public void TestHarmonicMeanCalculation()
    {
        // ARRANGE
        var numbers = new List<int> { 2, 6 };
        const decimal expected = 3.0m;

        // ACT
        var harmonicMean = new HarmonicMeanCalculator().CalculateHarmony(numbers);

        // ASSERT
        Assert.True(Math.Abs(expected - harmonicMean) < 0.001m);
    }

    /// <summary>
    /// Заранее определённые списки предпочтений и команды должны дать заранее определённое значение
    /// </summary>
    [Fact]
    public void TestCertainPreferencesAndTeams()
    {
        // ARRANGE
        var teams = new List<Team>
        {
            new(new Employee(10, "Астафьев Андрей"), new Employee(15, "Добрынин Степан")),
            new(new Employee(11, "Демидов Дмитрий"), new Employee(16, "Фомин Никита")),
            new(new Employee(12, "Климов Михаил"), new Employee(17, "Маркина Кристина"))
        };

        var teamLeadsWishlists = new List<Wishlist> { new(10, [15, 16, 17]), new(11, [16, 15, 17]), new(12, [17, 15, 16]) };
        var juniorsWishlists = new List<Wishlist> { new(15, [10, 11, 12]), new(16, [11, 10, 12]), new(17, [12, 10, 11]) };

        const decimal expected = 3.0m;

        // ACT
        var harmony = new HrDirector(new HarmonicMeanCalculator()).CalculateHarmonicMean(teams, teamLeadsWishlists, juniorsWishlists);

        // ASSERT
        Assert.True(Math.Abs(expected - harmony) < 0.001m);
    }
}
