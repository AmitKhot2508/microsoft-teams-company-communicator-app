// <copyright file="TeamDataController.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.CompanyCommunicator.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Teams.Apps.CompanyCommunicator.Authentication;
    using Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.TeamData;
    using Microsoft.Teams.Apps.CompanyCommunicator.Models;

    /// <summary>
    /// Controller for the teams data.
    /// </summary>
    [Route("api/teamData")]
    [Authorize(PolicyNames.MustBeValidUpnPolicy)]
    public class TeamDataController : ControllerBase
    {
        private readonly TeamDataRepository teamDataRepository;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamDataController"/> class.
        /// </summary>
        /// <param name="teamDataRepository">Team data repository instance.</param>
        /// <param name="configuration">configuration</param>
        public TeamDataController(TeamDataRepository teamDataRepository, IConfiguration configuration)
        {
            this.teamDataRepository = teamDataRepository;
            this.configuration = configuration;
        }

        /// <summary>
        /// Get data for all teams.
        /// </summary>
        /// <returns>A list of team data.</returns>
        [HttpGet]
        public async Task<IEnumerable<TeamData>> GetAllTeamDataAsync()
        {
            var entities = await this.teamDataRepository.GetAllSortedAlphabeticallyByNameAsync();
            var result = new List<TeamData>();
            foreach (var entity in entities)
            {
                var team = new TeamData
                {
                    TeamId = entity.TeamId,
                    Name = entity.Name,
                };
                result.Add(team);
            }

            return result;
        }

        /// <summary>
        /// Get value from config.
        /// This represents whether to show/hide 'send to everyone' radiobutton option while composing message.
        /// </summary>
        /// <returns>true or false.</returns>
        [HttpGet("isOptionEnableDisable")]
        public bool GetConfigurationValue()
        {
            var getConfigValue = this.configuration.GetValue<bool>("OptionSetting:IsOptionSendToEveryoneEnabled");
            return getConfigValue;
        }
    }
}
