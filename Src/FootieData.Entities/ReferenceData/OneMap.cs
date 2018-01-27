﻿namespace FootieData.Entities.ReferenceData
{
    public class OneMap
    {
        public int ClientLeagueId { get; set; }
        public ExternalLeagueCode ExternalLeagueCode { get; set; }
        public string ExternalLeagueCodeDescription { get; set; }
        public InternalLeagueCode InternalLeagueCode { get; set; }
        public string InternalLeagueCodeDescription { get; set; }
    }
}