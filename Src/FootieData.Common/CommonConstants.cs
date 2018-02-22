﻿using System;

namespace FootieData.Common
{
    public static class CommonConstants
    {
        public const int RefreshIntervalInSeconds = 5;//gregt reset to 60
        public const string CategorySubLevel = "General";
        public const string InterestedInLeague = "Set this to 'true' if you are interested in this league.";

        public const string InternalLeagueCodeDescriptionBr1 = "Campeonato Brasileiro Série A";
        public const string InternalLeagueCodeDescriptionDe1 = "1. Bundesliga";
        public const string InternalLeagueCodeDescriptionDe2 = "2. Bundesliga";
        public const string InternalLeagueCodeDescriptionEs1 = "La Liga Primera Division";
        public const string InternalLeagueCodeDescriptionFr1 = "Ligue 1";
        public const string InternalLeagueCodeDescriptionFr2 = "Ligue 2";
        public const string InternalLeagueCodeDescriptionIt1 = "Serie A";
        public const string InternalLeagueCodeDescriptionIt2 = "Serie B";
        public const string InternalLeagueCodeDescriptionNl1 = "Eredivisie";
        public const string InternalLeagueCodeDescriptionPt1 = "Primeira Liga";
        public const string InternalLeagueCodeDescriptionUefa1 = "UEFA Champions League";
        public const string InternalLeagueCodeDescriptionUk1 = "English Premier League";
        public const string InternalLeagueCodeDescriptionUk2 = "English Football League Championship";
        public const string InternalLeagueCodeDescriptionUk3 = "English Football League One";
        public const string InternalLeagueCodeDescriptionUk4 = "English Football League Two";

        public static string TheBossIsCommingText =
            @"CS0016: Could not write to output file 'c:\\WINDOWS\\Microsoft.NET\\Framework\\v4.0.30319\\Temporary ASP.NET Files\\root\\9c4c27ed\\e56c00f7\\App_Web_default.aspx.cdcab7d2.rxmrydy_.dll' . The directory name is invalid." +
            Environment.NewLine +
            @"CarTracker.exe " + Environment.NewLine +
            @"1.0.0.0 " + Environment.NewLine +
            @"4f2bf2f4" + Environment.NewLine +
            @"    KERNELBASE.dll" + Environment.NewLine +
            @"6.1.7600.16850 " + Environment.NewLine +
            @"4e211485 " + Environment.NewLine +
            @"e0434352" + Environment.NewLine +
            @"0000b9bc" + Environment.NewLine +
            @"1114 " + Environment.NewLine +
            @"01cce29b07be5a50" + Environment.NewLine +
            @"    C:\Program Files (x86)\TGS\AutoTracker\Tracker.exe" + Environment.NewLine +
            @"C:\Windows\syswow64\KERNELBASE.dll" + Environment.NewLine +
            @"46533054-4e2e-11e1-9197-66238bf5b722[/ CODE]" + Environment.NewLine +
            @"    2750355945 " + Environment.NewLine +
            @"    5 " + Environment.NewLine +
            @"CLR20r3" + Environment.NewLine +
            @"    Not available" + Environment.NewLine +
            @"0 " + Environment.NewLine +
            @"ERROR: Runtime error: Could not load file or assembly 'System.Data, Version=2.0." + Environment.NewLine +
            @"0.0, Culture = neutral, PublicKeyToken = b77a5c561934e089' or one of its dependencies." +
            Environment.NewLine +
            @"The module was expected to contain an assembly manifest." + Environment.NewLine +
            @"failed at car tracker.exe [1.5.9.88]" + Environment.NewLine +
            @"1.0.0.0 " + Environment.NewLine +
            @"4f2bf2f4" + Environment.NewLine +
            @"failed at car tracker unexpected" + Environment.NewLine +
            @"1.0.0.0 " + Environment.NewLine +
            @"4f2bf2f4" + Environment.NewLine +
            @"    f" + Environment.NewLine +
            @"e9" + Environment.NewLine +
            @"    System.InvalidOperationException" + Environment.NewLine +
            @"C:\Users\PartridgeA\AppData\Local\Temp\WER2F4E.tmp.WERInternalMetadata.xml" + Environment.NewLine +
            @"    C:\Users\PartridgeA\AppData\Local\Microsoft\Windows\WER\ReportArchive\AppCrash_tracker.e_791ba3451e349516de2163964ba4d2ea7d13b81_0dd73b7e" +
            Environment.NewLine +
            @"0 " + Environment.NewLine +
            @"4650f051-4e8e-11e1-9197-00238bf5b722" + Environment.NewLine +
            @"0" + Environment.NewLine +
            @"Error Message" + Environment.NewLine +
            @"Problem signature:" + Environment.NewLine +
            @"Problem Event Name: ArchD" + Environment.NewLine +
            @"Application Name:   cpa.exe" + Environment.NewLine +
            @"Application Version:    0.0.0.0" + Environment.NewLine +
            @"Application Timestamp:  4d264c2d" + Environment.NewLine +
            @"Fault Module Name:  MSVCR90.dll" + Environment.NewLine +
            @"Fault Module Version:   9.0.30729.4940" + Environment.NewLine +
            @"Fault Module Timestamp: 4ca2ef57" + Environment.NewLine +
            @"Exception Offset:   00071f93" + Environment.NewLine +
            @"Exception Code: c0000417" + Environment.NewLine +
            @"Exception Data: 00000000" + Environment.NewLine +
            @"OS Version: 6.1.7601.2.1.0.256.4" + Environment.NewLine +
            @"Locale ID:  2057" + Environment.NewLine +
            @"Additional Information 1:   4c98" + Environment.NewLine +
            @"Additional Information 2:   4c98092e5e2eda594b891f3082316b3d" + Environment.NewLine +
            @"Additional Information 3:   6ad3" + Environment.NewLine +
            @"Additional Information 4:   6ad3940877f665a2ce265b32b2aa8269";
        //@"Lorem ipsum dolor sit amet, consectetur adipiscing elit." + Environment.NewLine +
        //"Sed aliquam, libero eget vehicula aliquam, metus magna rhoncus lectus, ut malesuada tellus felis et nunc.Curabitur at sodales tortor, non tincidunt nisi. "
        //+ Environment.NewLine +
        //@"Quisque auctor bibendum metus et suscipit. Mauris sit amet metus interdum, faucibus metus et, placerat tellus. Suspendisse maximus dui dolor, vel vestibulum nisi porta sit amet.Nulla maximus dui et nisi gravida laoreet.Suspendisse sed tempor mi."
        //+ Environment.NewLine
        //+ @"Curabitur sit amet posuere felis, non sagittis sem.Vivamus pellentesque mi sapien, id elementum diam dictum in."
        //+ Environment.NewLine +
        //@"Nunc ut neque finibus, rutrum diam et, congue eros.Nulla ut metus sit amet tortor finibus mollis tempus eget nibh."
        //+ Environment.NewLine +
        //@"Mauris non rutrum nulla, volutpat eleifend leo. Pellentesque a iaculis est, at volutpat mi. Vestibulum ullamcorper dictum tincidunt. Cras ac enim vel orci accumsan tristique sed mattis ex."
        //+ Environment.NewLine +
        //@"Aliquam erat volutpat.Aenean ut sem nec leo molestie pharetra.Aenean velit ipsum, cursus eget nisl eget, facilisis vehicula nibh. "
        //+ Environment.NewLine +
        //@"Aliquam et metus ornare ante ullamcorper consectetur.Quisque sollicitudin sapien nulla, a mollis ante pellentesque ut. Aliquam erat volutpat.Maecenas condimentum iaculis lobortis. Vivamus non facilisis tortor."
        //+ Environment.NewLine +
        //@"Etiam in viverra purus. Nullam viverra fringilla lacus. Nam laoreet arcu id bibendum accumsan. Curabitur semper quam nisi, ultricies suscipit nibh laoreet nec. "
        //+ Environment.NewLine +
        //@"In turpis metus, venenatis sit amet turpis vel, gravida maximus arcu.Etiam a elit ante. Donec quis odio erat. Aenean vel est quis ligula mattis tristique et at sem. Nulla malesuada, ante vel hendrerit fringilla, diam augue pulvinar nunc, eget consectetur felis orci eu sem.Vestibulum id laoreet ex. "
        //+ Environment.NewLine +
        //@"Pellentesque libero dolor, interdum nec urna at, convallis vehicula purus. Donec elementum mi nulla, a maximus tortor rhoncus vitae. Quisque pellentesque eros nibh. Cras metus velit, aliquet ut volutpat non, eleifend at dolor."
        //+ Environment.NewLine +
        //@"Proin eget sodales mi. Donec volutpat vitae lectus ut efficitur. Integer efficitur eu lorem at tincidunt. Mauris id magna dictum, vulputate turpis sed, euismod enim.Nulla commodo tincidunt blandit."
        //+ Environment.NewLine +
        //@"Pellentesque laoreet justo sed porta dignissim. Quisque vitae erat eget lorem hendrerit semper scelerisque nec dui. Suspendisse vitae nisl ullamcorper nunc sollicitudin dictum ut quis tellus.";
    }
}
