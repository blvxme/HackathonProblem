using Nsu.HackathonProblem.Contracts;

namespace HackathonProblem.Service.Hackathon;

public class Hackathon : IHackathon
{
    private readonly Random _random = new();

    public (IEnumerable<Wishlist> TeamLeadsWishlists, IEnumerable<Wishlist> JuniorsWishlists) GenerateWishlists(
        IEnumerable<Employee> teamLeads, IEnumerable<Employee> juniors
    )
    {
        if (teamLeads.Count() != juniors.Count())
        {
            throw new ArgumentException("The number of team leads does not match the number of juniors");
        }

        return (GenerateWishlistsFor(teamLeads, juniors), GenerateWishlistsFor(juniors, teamLeads));
    }

    private IEnumerable<Wishlist> GenerateWishlistsFor(IEnumerable<Employee> owners, IEnumerable<Employee> members)
    {
        var wishlists = new List<Wishlist>();

        foreach (var owner in owners)
        {
            var preferenceIds = members.Select(m => m.Id).ToArray();
            var count = preferenceIds.Length;

            while (count > 1)
            {
                --count;

                var rand = _random.Next(count + 1);
                (preferenceIds[rand], preferenceIds[count]) = (preferenceIds[count], preferenceIds[rand]);
            }

            wishlists.Add(new Wishlist(owner.Id, preferenceIds));
        }

        return wishlists;
    }
}
