using Efficy.Models;

namespace Efficy.Interfaces
{
    public interface ITeamService
    {
        Task<Team> CreateTeamAsync(string name);
        Task<StepCounter> AddStepCounterAsync(string teamId, string counterName);
        Task<StepCounter> IncrementCounterAsync(string teamId, string counterId, int steps);
        Task<int> GetTeamStepsAsync(string teamId);
        Task<List<Team>> GetAllTeamsAsync();
        Task<List<StepCounter>> GetTeamCountersAsync(string teamId);
        Task<Boolean> DeleteTeamAsync(string teamId);
        Task<Boolean> DeleteStepCounterAsync(string teamId, string counterId);
    }
}
