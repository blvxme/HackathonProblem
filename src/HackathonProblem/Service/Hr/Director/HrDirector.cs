using HackathonProblem.Exceptions;
using HackathonProblem.Service.Hr.Director.Calculator;
using Nsu.HackathonProblem.Contracts;

namespace HackathonProblem.Service.Hr.Director;

public class HrDirector(IHarmonyCalculator harmonyCalculator) : IHrDirector
{
    public decimal CalculateHarmonicMean(
        IEnumerable<Team> teams,
        IEnumerable<Wishlist> teamLeadsWishlists, IEnumerable<Wishlist> juniorsWishlists
    )
    {
        if (teams.Count() != teamLeadsWishlists.Count())
        {
            throw new ArgumentException("The number of teams does not match the number of team leads' wishlists");
        }

        if (teams.Count() != juniorsWishlists.Count())
        {
            throw new ArgumentException("The number of teams does not match the number of juniors' wishlists");
        }

        var teamsCount = teams.Count();

        var satisfactions = new List<int>();
        foreach (var team in teams)
        {
            // Индекс удовлетворенности тимлида
            var teamLeadWishlist = teamLeadsWishlists.First(w => w.EmployeeId == team.TeamLead.Id);
            var juniorPosition = GetPosition(teamLeadWishlist, team.Junior.Id);
            satisfactions.Add(teamsCount - juniorPosition);

            // Индекс удовлетворенности джуна
            var juniorWishlist = juniorsWishlists.First(w => w.EmployeeId == team.Junior.Id);
            var teamLeadPosition = GetPosition(juniorWishlist, team.TeamLead.Id);
            satisfactions.Add(teamsCount - teamLeadPosition);
        }

        return harmonyCalculator.CalculateHarmony(satisfactions);
    }

    private int GetPosition(Wishlist wishlist, int employeeId)
    {
        for (var i = 0; i < wishlist.DesiredEmployees.Length; ++i)
        {
            if (wishlist.DesiredEmployees[i] == employeeId)
            {
                return i;
            }
        }

        throw new InvalidWishlistException(wishlist, employeeId);
    }
}
