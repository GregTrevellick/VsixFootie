﻿using FootieData.Entities;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using FootballDataSDK;
using FootballDataSDK.Results;
using Standing = FootieData.Entities.Standing;

namespace FootieData.Gateway
{
    public class FootballDataSdkGateway
    {
        //gregt for testing
        private static CultureInfo enUS = new CultureInfo("en-US");
        private static CultureInfo enGB = new CultureInfo("en-GB");
        private static CultureInfo frFR = new CultureInfo("fr-FR");
        private static CultureInfo deDE = new CultureInfo("de-DE");

        private readonly FootDataServices _footDataServices;
        private readonly CompetitionResultSingleton _soccerSeasonResultSingleton;

        public FootballDataSdkGateway(FootDataServices footDataServices, CompetitionResultSingleton soccerSeasonResultSingletonInstance)
        {
            _footDataServices = footDataServices;
            _soccerSeasonResultSingleton = soccerSeasonResultSingletonInstance;
        }

        public IEnumerable<Standing> GetFromClientStandings(string leagueIdentifier)
        {
            IEnumerable<Standing> result = null;
            var idSeason = GetIdSeason(leagueIdentifier);
            if (idSeason > 0)
            {
                var leagueTableResult = _footDataServices.GetLeagueTableResult(idSeason);
                if (leagueTableResult != null)
                {
                    result = GetResultMatchStandings(leagueTableResult);
                }
            }
            return result;
        }

        public IEnumerable<FixturePast> GetFromClientFixturePasts(string leagueIdentifier, string timeFrame)
        {
            IEnumerable<FixturePast> result = null;
            var idSeason = GetIdSeason(leagueIdentifier);
            if (idSeason > 0)
            {
                var fixturesResult = _footDataServices.GetFixturesResult(idSeason, timeFrame);
                if (fixturesResult != null)
                {
                    result = GetFixturePasts(fixturesResult);
                }
            }
            return result;
        }

        public IEnumerable<FixtureFuture> GetFromClientFixtureFutures(string leagueIdentifier, string timeFrame)
        {
            IEnumerable<FixtureFuture> result = null;
            var idSeason = GetIdSeason(leagueIdentifier);
            if (idSeason > 0)
            {
                var fixturesResult = _footDataServices.GetFixturesResult(idSeason, timeFrame);
                if (fixturesResult != null)
                {
                    result = GetFixtureFutures(fixturesResult);
                }
            }
            return result;
        }

        private int GetIdSeason(string leagueIdentifier)
        {
            //gregt get from new entity here =========================================================================================
            var league = _soccerSeasonResultSingleton?.CompetitionResult?.Competitions?.SingleOrDefault(x => x.league == leagueIdentifier);
            return league?.id ?? 0;
        }

        private static IEnumerable<Standing> GetResultMatchStandings(LeagueTableResult leagueTableResult)
        {
            if (!string.IsNullOrEmpty(leagueTableResult?.error))
            {
                return new List<Standing>
                {
                    new Standing
                    {
                        Team = leagueTableResult.error
                    }
                };
            }
            else
            {                
                return leagueTableResult?.standing?.Select(x => new Standing
                {
                    //CrestURI = x.crestURI,
                    Against = x.goalsAgainst,
                    Diff = x.goalDifference,
                    For = x.goals,
                    Played = x.playedGames,
                    Points = x.points,
                    Rank = x.position,
                    Team = x.teamName,
                });
            }
        }

        private static IEnumerable<FixturePast> GetFixturePasts(FixturesResult fixturesResult)
        {
            if (!string.IsNullOrEmpty(fixturesResult?.error))
            {
                return new List<FixturePast>
                {
                    new FixturePast
                    {
                        HomeName = fixturesResult.error
                    }
                };
            }
            else
            {
                return fixturesResult?.fixtures?.Select(x => new FixturePast
                {
                    AwayName = x.awayTeamName,
                    Date = x.date.ToString("d", enGB),//gregt unit test & remove culture
                    HomeName = x.homeTeamName,
                    GoalsAway = x.ResultMatch?.goalsAwayTeam,
                    GoalsHome = x.ResultMatch?.goalsHomeTeam,
                });
            }
        }

        private static IEnumerable<FixtureFuture> GetFixtureFutures(FixturesResult fixturesResult)
        {
            if (!string.IsNullOrEmpty(fixturesResult?.error))
            {
                return new List<FixtureFuture>
                {
                    new FixtureFuture
                    {
                        HomeName = fixturesResult.error
                    }
                };
            }
            else
            {
                return fixturesResult?.fixtures?.Select(x => new FixtureFuture
                {
                    AwayName = x.awayTeamName,
                    Date = x.date.ToString("d", enGB),//gregt unit test & remove culture
                    HomeName = x.homeTeamName,
                    Time = x.date.ToString("t", enUS),//gregt unit test & remove culture
                });
            }
        }
    }
}


//https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings
//2009-06-15T13:45:30 -> 1:45 PM(en-US)
//2009-06-15T13:45:30 -> 13:45 (hr-HR)
//2009-06-15T13:45:30 -> 01:45 م(ar-EG)























































//public async Task<LeagueStandings> GetLeagueResponse_Standings(string leagueIdentifier)
//{
//    var result = await GetLeagueResponseFromClient_Standings(leagueIdentifier);
//    return result;
//}

//public async Task<LeagueMatchesResults> GetLeagueResponse_Results(string leagueIdentifier)
//{
//    var result = await GetLeagueResponseFromClient_MatchesResult(leagueIdentifier, "p7");
//    return result;
//}

//public async Task<LeagueMatchesFixtures> GetLeagueResponse_Fixtures(string leagueIdentifier)
//{
//    var result = await GetLeagueResponseFromClient_MatchesFixture(leagueIdentifier, "n7");
//    return result;
//}

//private async Task<int> GetLeagueIdResult(string leagueIdentifier)
//{
//    var taskSeasons = await GetLeagueId2r(leagueIdentifier);
//    return taskSeasons;
//}
//private int GetLeagueIdResult(string leagueIdentifier)
//{
//    var taskSeasons = GetLeagueId4(leagueIdentifier);
//    return taskSeasons;
//}

//private async Task<int> GetLeagueIdFixture(string leagueIdentifier)
//{
//    var taskSeasons = await GetLeagueId2f(leagueIdentifier);
//    return taskSeasons;
//}
//private int GetLeagueIdFixture(string leagueIdentifier)
//{
//    var taskSeasons = GetLeagueId4(leagueIdentifier);
//    return taskSeasons;
//}

//private int GetLeagueId5(string leagueIdentifier)
//{
//    var taskSeasons = GetLeagueId4(leagueIdentifier);
//    return taskSeasons;
//}
//private async Task<SoccerSeasonResult> GetLeagueId1b()
//{
//    var result = await _footDataServices.SoccerSeasonsAsync();
//    return result;
//}
//private SoccerSeasonResult GetLeagueId1b()
//{
//    var result = GetLeagueId3();//_footDataServices.SoccerSeasons();
//    return result;
//}

//private async Task<int> GetLeagueId2r(string leagueIdentifier)
//{
//    var taskSeasons = await GetLeagueId2rb();
//    var league = taskSeasons.Seasons.First(x => x.league == leagueIdentifier);
//    return league.id;
//}
//private int GetLeagueId2r(string leagueIdentifier)
//{
//    var taskSeasons = GetLeagueId3();
//    var league = taskSeasons.Seasons.First(x => x.league == leagueIdentifier);
//    return league.id;
//}

//private async Task<SoccerSeasonResult> GetLeagueId2rb()
//{
//    return await _footDataServices.SoccerSeasonsAsync();
//}
//private SoccerSeasonResult GetLeagueId2rb()
//{
//    return GetLeagueId3();//_footDataServices.SoccerSeasons();
//}

//private async Task<int> GetLeagueId2f(string leagueIdentifier)
//{
//    var taskSeasons = await GetLeagueId2fb();
//    var league = taskSeasons.Seasons.First(x => x.league == leagueIdentifier);
//    return league.id;
//}
//private int GetLeagueId2f(string leagueIdentifier)
//{
//    var taskSeasons = GetLeagueId3();
//    var league = taskSeasons?.Seasons?.First(x => x.league == leagueIdentifier);
//    return league == null ? 0 : league.id;
//}

//private async Task<SoccerSeasonResult> GetLeagueId2fb()
//{
//    return await _footDataServices.SoccerSeasonsAsync();
//}
//private SoccerSeasonResult GetLeagueId2fb()
//{
//    return GetLeagueId3();//_footDataServices.SoccerSeasons();
//}


//public LeagueStandings GetLeagueResponse_Standings(string leagueIdentifier)
//{
//    var result = GetLeagueResponseFromClient_Standings(leagueIdentifier);
//    return result;
//}
//public LeagueMatchesResults GetLeagueResponse_Results(string leagueIdentifier)
//{
//    var result =  GetLeagueResponseFromClient_MatchesResult(leagueIdentifier, "p7");
//    return result;
//}

//public LeagueMatchesFixtures GetLeagueResponse_Fixtures(string leagueIdentifier)
//{
//    var result = GetLeagueResponseFromClient_MatchesFixture(leagueIdentifier, "n7");
//    return result;
//}





//private int GetLeagueId4(string leagueIdentifier)
//{
//    var soccerSeasonResult = GetLeagueId3();
//    var league = soccerSeasonResult?.Seasons?.First(x => x.league == leagueIdentifier);
//    return league == null ? 0 : league.id;
//}




//private int GetLeagueId1a(string leagueIdentifier)
//{
//    if (_soccerSeasonResult == null)
//    {
//        _soccerSeasonResult = GetLeagueId3();
//   }

//    if (_soccerSeasonResult == null ||
//        _soccerSeasonResult.Seasons == null||
//        _soccerSeasonResult.Seasons.Length == 0)
//    {
//        //throw new Exception("yep");
//        return int.MinValue;
//    }
//    else
//    {
//        var league = _soccerSeasonResult.Seasons.First(x => x.league == leagueIdentifier);
//        return league.id;
//    }            
//}

//private SoccerSeasonResult GetLeagueId3()
//{
//    return _footDataServices.SoccerSeasons();
//}
