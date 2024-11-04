using HackathonProblem.Service.Hr.Manager;
using HackathonProblem.Strategy.Marriage;
using Moq;
using Nsu.HackathonProblem.Contracts;

namespace HackathonProblemTest;

public class HrManagerTest
{
    /// <summary>
    /// При несоответствии количества тимлидов размеру вишлиста джуна выбрасывается исключение
    /// </summary>
    [Fact]
    public void TestNumberOfTeamLeads()
    {
        // ARRANGE
        var teamLeads = new List<Employee> { new(10, "Астафьев Андрей"), new(11, "Демидов Дмитрий") };
        var juniors = new List<Employee> { new(15, "Добрынин Степан"), new(16, "Фомин Никита") };

        var teamLeadsWishlists = new List<Wishlist> { new(10, [15, 16]), new(11, [16, 15]) };
        var juniorsWishlists = new List<Wishlist> { new(15, [10]), new(16, [11, 10]) };

        // ACT & ASSERT
        Assert.Throws<ArgumentException>(() => new HrManager(new MarriageStrategy()).BuildTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists));
    }

    /// <summary>
    /// При несоответствии количества джунов размеру вишлиста тимлида выбрасывается исключение
    /// </summary>
    [Fact]
    public void TestNumberOfJuniors()
    {
        // ARRANGE
        var teamLeads = new List<Employee> { new(10, "Астафьев Андрей"), new(11, "Демидов Дмитрий") };
        var juniors = new List<Employee> { new(15, "Добрынин Степан"), new(16, "Фомин Никита") };

        var teamLeadsWishlists = new List<Wishlist> { new(10, [15, 16]), new(11, [16]) };
        var juniorsWishlists = new List<Wishlist> { new(15, [10, 11]), new(16, [11, 10]) };

        // ACT & ASSERT
        Assert.Throws<ArgumentException>(() => new HrManager(new MarriageStrategy()).BuildTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists));
    }

    /// <summary>
    /// Количество команд должно совпадать с заранее заданным
    /// </summary>
    [Fact]
    public void TestNumberOfTeams()
    {
        // ARRANGE
        var teamLeads = new List<Employee> { new(10, "Астафьев Андрей"), new(11, "Демидов Дмитрий") };
        var juniors = new List<Employee> { new(15, "Добрынин Степан"), new(16, "Фомин Никита") };

        var teamLeadsWishlists = new List<Wishlist> { new(10, [15, 16]), new(11, [16, 15]) };
        var juniorsWishlists = new List<Wishlist> { new(15, [10, 11]), new(16, [11, 10]) };

        const int expected = 2;

        // ACT
        var teams = new HrManager(new MarriageStrategy()).BuildTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);

        // ASSERT
        Assert.Equal(expected, teams.Count());
    }

    /// <summary>
    /// Стратегия HR менеджера должна быть вызвана ровно один раз
    /// </summary>
    [Fact]
    public void TestNumberOfStrategyCalls()
    {
        // ARRANGE
        var teamLeads = new List<Employee> { new(10, "Астафьев Андрей"), new(11, "Демидов Дмитрий") };
        var juniors = new List<Employee> { new(15, "Добрынин Степан"), new(16, "Фомин Никита") };

        var teamLeadsWishlists = new List<Wishlist> { new(10, [15, 16]), new(11, [16, 15]) };
        var juniorsWishlists = new List<Wishlist> { new(15, [10, 11]), new(16, [11, 10]) };

        var mockStrategy = new Mock<ITeamBuildingStrategy>();
        var hrManager = new HrManager(mockStrategy.Object);

        // ACT
        hrManager.BuildTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);

        // ASSERT
        mockStrategy.Verify(d => d.BuildTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists), Times.Once);
    }
}
