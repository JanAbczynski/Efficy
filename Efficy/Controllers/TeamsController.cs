using Efficy.Interfaces;
using Efficy.Models;
using Microsoft.AspNetCore.Mvc;

namespace Efficy.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {

        private readonly ITeamService _teamService;
        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        // 0 - Init new team
        [HttpPost]
        public async Task<ActionResult<Team>> CreateTeam([FromBody] string name)
        {
            try
            {
                var team = await _teamService.CreateTeamAsync(name);
                return Ok(team);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }



        //    1    As a User
        //I want to be able to create a new counter
        //So that steps can be accumulated for a team of one or multiple employees

        [HttpPost("counters")]
        public async Task<ActionResult<StepCounter>> AddStepCounter([FromQuery] string teamId, [FromBody] string counterName)
        {
            try
            {
                var counter = await _teamService.AddStepCounterAsync(teamId, counterName);
                return Ok(counter);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }



//    2    As a User
//I want to be able to increment the value of a stored counter
//So that I can get steps counted towards my team's score

        [HttpPut("counters/increment")]
        public async Task<ActionResult> IncrementCounter([FromQuery] string teamId, [FromQuery] string counterId, [FromBody] int steps)
        {
            try
            {
                var counter = await _teamService.IncrementCounterAsync(teamId, counterId, steps);
                return Ok(counter);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }


            //    3    As a User
            //I want to get the current total steps taken by a team
            //So that I can see how much that team have walked in total


        [HttpGet("steps")]
        public async Task<ActionResult<int>> GetTeamSteps([FromQuery] string teamId)
        {
            try
            {
                var totalSteps = await _teamService.GetTeamStepsAsync(teamId);
                return Ok(totalSteps);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }




            //     4   As a User
            //I want to list all teams and see their step counts
            //So that I can compare my team with the others

        [HttpGet]
        public async  Task<ActionResult<List<Team>>> GetAllTeams()
        {
            try
            {
                var teams = await _teamService.GetAllTeamsAsync();
                return Ok(teams);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }




            //    5    As a User
            //I want to list all counters in a team
            //So that I can see how much each team member have walked

        [HttpGet("counters")]
        public async Task<ActionResult<List<StepCounter>>> GetTeamCounters([FromQuery] string teamId)
        {
            try
            {
                var counters = await _teamService.GetTeamCountersAsync(teamId);
                return Ok(counters);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }




            //    6    As a User
            //I want to be able to add/delete teams
            //So that I can manage teams
        [HttpDelete]
        public async Task<IActionResult> DeleteTeam([FromQuery] string teamId)
        {
            try
            {
                var result = await _teamService.DeleteTeamAsync(teamId);

                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound($"Team with ID {teamId} not found.");
                }

            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }




        //    7    As a User
        //I want to be able to add/delete counters
        //So that I can manage team member's counters

        [HttpDelete("counters")]
        public async Task<IActionResult> DeleteStepCounter([FromQuery] string teamId, [FromQuery] string counterId)
        {
            try
            {
                var result = await _teamService.DeleteStepCounterAsync(teamId, counterId);

                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound($"Stepcounter with ID {counterId} not found.");
                }
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

    }


}
