namespace WebScrapping.Utils
{
    public static class Constants
    {
        public struct Messages
        {
            public static readonly string ERROR_LOGIN = "Incorrect password or user does not exist.";
            public static readonly string QUERY_TOO_SHORT = "You must enter at least 3 characters to perform the query";
            public static readonly string OFFSHORE_LEAKS_ERROR = "There was an error querying the data from Offshore Leaks";
            public static readonly string WB_DEBARRED_FIRMS_ERROR = "There was an error querying the data from World Bank Debarred Firms.";
            public static readonly string OFAC_SANCTIONS_ERROR = "There was an error querying the data from OFAC Sanctions List.";
            public static readonly string REGEX_PATTERN_DBS = @"^\d+(,\d+)*$";
            public static readonly string ERROR_PATTERN_DBS = "You must enter dbs correctly (numbers separated with commas and without spaces).";
            public static readonly string ERROR_NUMBERS_DBS = "You can only enter numbers from 1 to 3 in dbs.";
        }
    }
}
