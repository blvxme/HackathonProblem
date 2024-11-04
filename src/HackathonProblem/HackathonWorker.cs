using HackathonProblem.Config;
using HackathonProblem.Exceptions;
using HackathonProblem.Service.Hackathon;
using HackathonProblem.Service.Hr.Director;
using HackathonProblem.Service.Hr.Manager;
using HackathonProblem.Service.Registrar;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace HackathonProblem;

public class HackathonWorker(
    IHostApplicationLifetime lifetime,
    IOptions<HackathonConfig> hackathonConfigOptions,
    IRegistrar registrar,
    IHrManager hrManager,
    IHrDirector hrDirector
) : IHostedService
{
    private readonly int _hackathonCount = hackathonConfigOptions.Value.HackathonCount;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () => await OrganizeHackathons());

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task OrganizeHackathons()
    {
        // "Регистрация" участников хакатона (парсинг csv-файлов)
        var (teamLeads, juniors) = await Task.Run(registrar.Register);
        Log.Debug("teamLeads: {teamLeads}", teamLeads);
        Log.Debug("juniors: {juniors}", juniors);

        Log.Debug("_hackathonCount: {_hackathonCount}", _hackathonCount);

        var average = 0.0m;
        for (var i = 0; i < _hackathonCount; ++i)
        {
            try
            {
                // Проведение хакатона (составление вишлистов)
                var hackathon = new Hackathon();
                var (teamLeadsWishlists, juniorsWishlists) = await Task.Run(() => hackathon.GenerateWishlists(teamLeads, juniors));
                Log.Debug("teamLeadsWishlists: {teamLeadsWishlists}", teamLeadsWishlists);
                Log.Debug("juniorsWishlists: {juniorsWishlists}", juniorsWishlists);

                // Формирование команд
                var teams = await Task.Run(() => hrManager.BuildTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists));
                Log.Debug("teams: {teams}", teams);

                // Подсчет среднего гармонического
                var harmony = await Task.Run(() => hrDirector.CalculateHarmonicMean(teams, teamLeadsWishlists, juniorsWishlists));
                Log.Information("harmony: {harmony}", harmony);

                average += harmony;
            }
            catch (InvalidWishlistException exception)
            {
                Log.Error(exception, "An error occurred while creating the wishlist. {Message}", exception.Message);
            }
            catch (ArgumentException exception)
            {
                Log.Error(exception, "An error occured during the hackathon. {Message}", exception.Message);
            }
        }

        average /= _hackathonCount;
        Log.Information("average: {average}", average);

        lifetime.StopApplication();
    }
}
