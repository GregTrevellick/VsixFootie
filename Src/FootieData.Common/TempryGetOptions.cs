﻿using System.Collections.Generic;
using FootieData.Common.Options;
using FootieData.Entities.ReferenceData;

namespace FootieData.Common
{
    public class TempryGetOptions
    {
        private static GeneralOptions _generalOptions;

        public GeneralOptions GetGeneralOptions()
        {
            _generalOptions = new GeneralOptions
            {
                LeagueOptions = new List<LeagueOption>()
            };

            AddThem(InternalLeagueCode.BR1);
            //AddThem(InternalLeagueCode.DE1);
            //AddThem(InternalLeagueCode.DE2);
            AddThem(InternalLeagueCode.ES1);
            //AddThem(InternalLeagueCode.FR1);
            //AddThem(InternalLeagueCode.FR2);
            //AddThem(InternalLeagueCode.IT1);
            //AddThem(InternalLeagueCode.IT2);
            //AddThem(InternalLeagueCode.NL1);
            //AddThem(InternalLeagueCode.PT1);
            //AddThem(InternalLeagueCode.UEFA1);
            AddThem(InternalLeagueCode.UK1);
            //AddThem(InternalLeagueCode.UK2);
            //AddThem(InternalLeagueCode.UK3);
            //AddThem(InternalLeagueCode.UK4);
            
            return _generalOptions;
        }

        private void AddThem(InternalLeagueCode internalLeagueCode)
        {
            _generalOptions.LeagueOptions.Add(
                new LeagueOption
                {
                    InternalLeagueCode = internalLeagueCode,
                    ShowLeague = true,
                    LeagueSubOptions = new List<LeagueSubOption>
                    {
                        //from false/true/true to true/true/true gives error
                        new LeagueSubOption {Expand = true, GridType = GridType.Standing},
                        new LeagueSubOption {Expand = true, GridType = GridType.Result},
                        new LeagueSubOption {Expand = true, GridType = GridType.Fixture}
                    }
                });
        }
    }
}