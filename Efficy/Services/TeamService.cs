using Efficy.Interfaces;
using Efficy.Models;
using System.Collections.Concurrent;

namespace Efficy.Services
{
    public class TeamService : ITeamService
    {
        private static ConcurrentDictionary<string, Team> teams = new ConcurrentDictionary<string, Team>();

        public async Task<Team> CreateTeamAsync(string name)
        {
            var team = new Team { Id = Guid.NewGuid().ToString(), Name = name };
            teams.TryAdd(team.Id, team);
            return await Task.FromResult(team);
        }

        public async Task<StepCounter> AddStepCounterAsync(string teamId, string counterName)
        {
            var counter = new StepCounter { };
            await Task.Run(() =>
            {
            var teamX = teams.FirstOrDefault(t => t.Key == teamId);
            if (teamX.Equals(default(KeyValuePair<string, Team>)))
            {
                throw new KeyNotFoundException("Team not found");
            }
            counter = new StepCounter { Id = Guid.NewGuid().ToString(), Name = counterName, Steps = 0 };
                teamX.Value.StepCounters.Add(counter);
            });

            return counter;
        }

        public async Task<StepCounter> IncrementCounterAsync(string teamId, string counterId, int steps)
        {
            var counter = new StepCounter { };

            await Task.Run(() =>
            {
                var team = teams.FirstOrDefault(t => t.Value.Id == teamId);
            if (team.Equals(default(KeyValuePair<string, Team>)))
            {
                throw new KeyNotFoundException("Team not found");
            }
                counter = team.Value.StepCounters.FirstOrDefault(c => c.Id == counterId);
            if (counter == null)
            {
                throw new KeyNotFoundException("Counter not found");
            }
                counter.Steps += steps;       
            });

            return counter;
        }

        public async Task<int> GetTeamStepsAsync(string teamId)
        {
            int count = 0;
            await Task.Run(() =>
            {
                var team = teams.FirstOrDefault(t => t.Value.Id == teamId);
            if (team.Equals(default(KeyValuePair<string, Team>)))
            {
                throw new KeyNotFoundException("Team not found");
            }
            count = team.Value.StepCounters.Sum(c => c.Steps);
            });

            return count;
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            var listOfTeams = new List<Team>();

            await Task.Run(() =>
            {
                listOfTeams = teams.Values
                    .Select(t => new Team
                    {
                        Id = t.Id,
                        Name = t.Name,
                        StepCounters = t.StepCounters.ToList()
                    })
                    .ToList();
            });

            return listOfTeams;
        }


        public async Task<List<StepCounter>> GetTeamCountersAsync(string teamId)
        {
            List<StepCounter> stepCounters = new List<StepCounter>();
            await Task.Run(() =>
            {
                    var team = teams.FirstOrDefault(t => t.Value.Id == teamId);
                if (team.Equals(default(KeyValuePair<string, Team>)))
                {
                    throw new KeyNotFoundException("Team not found");
                }
                stepCounters = team.Value.StepCounters;
            });

            return stepCounters;
        }

        public async Task<Boolean> DeleteTeamAsync(string teamId)
        {
            var isSuccess = false;
            await Task.Run(() =>
            {
                    var team = teams.FirstOrDefault(t => t.Value.Id == teamId);
                if (team.Equals(default(KeyValuePair<string, Team>)))
                {
                    throw new KeyNotFoundException("Team not found");
                }

                teams.TryRemove(team);
                isSuccess = true;
            });

            return isSuccess;
        }

        public async Task<Boolean> DeleteStepCounterAsync(string teamId, string counterId)
        {
            var isSuccess = false;

            await Task.Run(() =>
            {
                    var team = teams.FirstOrDefault(t => t.Value.Id == teamId);
                if (team.Equals(default(KeyValuePair<string, Team>)))
                {
                    throw new KeyNotFoundException("Team not found");
                }

                var counter = team.Value.StepCounters.FirstOrDefault(c => c.Id == counterId);
                if (counter == null)
                {
                    throw new KeyNotFoundException("Counter not found");
                }

                team.Value.StepCounters.Remove(counter);
                isSuccess = true;
            });

            return isSuccess;
        }
    }
}
