using HackathonProblem.Service.Hackathon;
using Nsu.HackathonProblem.Contracts;

namespace HackathonProblemTest;

public class WishlistTest
{
    /// <summary>
    /// При несоответствии количества тимлидов количеству джунов выбрасывается исключение
    /// </summary>
    [Fact]
    public void TestNumberOfEmployees()
    {
        // ARRANGE
        var teamLeads = new List<Employee> { new(10, "Астафьев Андрей"), new(11, "Демидов Дмитрий"), new(12, "Климов Михаил") };
        var juniors = new List<Employee> { new(15, "Добрынин Степан"), new(16, "Фомин Никита") };

        // ACT & ASSERT
        Assert.Throws<ArgumentException>(() => new Hackathon().GenerateWishlists(teamLeads, juniors));
    }

    /// <summary>
    /// Размер списка должен совпадать с количеством тимлидов/джунов
    /// </summary>
    [Fact]
    public void TestWishlistSize()
    {
        // ARRANGE
        var teamLeads = new List<Employee> { new(10, "Астафьев Андрей"), new(11, "Демидов Дмитрий"), new(12, "Климов Михаил") };
        var juniors = new List<Employee> { new(15, "Добрынин Степан"), new(16, "Фомин Никита"), new(17, "Маркина Кристина") };

        // ACT
        var (teamLeadsWishlists, juniorsWishlists) = new Hackathon().GenerateWishlists(teamLeads, juniors);

        // ASSERT
        foreach (var teamLeadWishlist in teamLeadsWishlists)
        {
            Assert.Equal(juniors.Count, teamLeadWishlist.DesiredEmployees.Length);
        }

        foreach (var juniorWishlist in juniorsWishlists)
        {
            Assert.Equal(teamLeads.Count, juniorWishlist.DesiredEmployees.Length);
        }
    }

    /// <summary>
    /// Заранее определенный сотрудник должен присутствовать в списке
    /// </summary>
    [Fact]
    public void TestWishlistContent()
    {
        // ARRANGE
        var certainTeamLead = new Employee(11, "Демидов Дмитрий");
        var teamLeads = new List<Employee> { new(10, "Астафьев Андрей"), certainTeamLead, new(12, "Климов Михаил") };

        var certainJunior = new Employee(16, "Фомин Никита");
        var juniors = new List<Employee> { new(15, "Добрынин Степан"), certainJunior, new(17, "Маркина Кристина") };

        // ACT
        var (teamLeadsWishlists, juniorsWishlists) = new Hackathon().GenerateWishlists(teamLeads, juniors);

        // ASSERT
        foreach (var teamLeadWishlist in teamLeadsWishlists)
        {
            Assert.Contains(certainJunior.Id, teamLeadWishlist.DesiredEmployees);
        }

        foreach (var juniorWishlist in juniorsWishlists)
        {
            Assert.Contains(certainTeamLead.Id, juniorWishlist.DesiredEmployees);
        }
    }
}
