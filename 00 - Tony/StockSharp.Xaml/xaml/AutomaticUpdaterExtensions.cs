﻿using Ecng.Localization;
using StockSharp.Localization;
using System;
using wyDay.Controls;

namespace StockSharp.Xaml
{
    public static class AutomaticUpdaterExtensions
    {
        public static void Translate( this AutomaticUpdater automaticUpdater )
        {
            if ( automaticUpdater == null )
            {
                throw new ArgumentNullException( nameof( automaticUpdater ) );
            }
                
            if ( LocalizedStrings.ActiveLanguage == Languages.English )
            {
                return;
            }
                
            automaticUpdater.Translation.AlreadyUpToDate      = LocalizedStrings.Str1466;
            automaticUpdater.Translation.CancelCheckingMenu   = LocalizedStrings.Str1467;
            automaticUpdater.Translation.CancelUpdatingMenu   = LocalizedStrings.Str1468;
            automaticUpdater.Translation.ChangesInVersion     =  LocalizedStrings.Str1469;
            automaticUpdater.Translation.CheckForUpdatesMenu  = LocalizedStrings.Str1470;
            automaticUpdater.Translation.Checking             = LocalizedStrings.Str1471;
            automaticUpdater.Translation.CloseButton          = LocalizedStrings.Str1472;
            automaticUpdater.Translation.Downloading          = LocalizedStrings.Str1473;
            automaticUpdater.Translation.DownloadUpdateMenu   = LocalizedStrings.Str1474;
            automaticUpdater.Translation.ErrorTitle           = LocalizedStrings.Str152;
            automaticUpdater.Translation.Extracting           = LocalizedStrings.Str1475;
            automaticUpdater.Translation.FailedToCheck        = LocalizedStrings.Str1476;
            automaticUpdater.Translation.FailedToDownload     = LocalizedStrings.Str1477;
            automaticUpdater.Translation.FailedToExtract      = LocalizedStrings.Str1478;
            automaticUpdater.Translation.HideMenu             = LocalizedStrings.Str1479;
            automaticUpdater.Translation.InstallOnNextStart   = LocalizedStrings.Str1480;
            automaticUpdater.Translation.InstallUpdateMenu    = LocalizedStrings.Str1481;
            automaticUpdater.Translation.PrematureExitMessage = string.Empty;
            automaticUpdater.Translation.PrematureExitTitle   = string.Empty;
            automaticUpdater.Translation.StopChecking         = LocalizedStrings.Str1482;
            automaticUpdater.Translation.StopDownloading      = LocalizedStrings.Str1483;
            automaticUpdater.Translation.StopExtracting       = LocalizedStrings.Str1484;
            automaticUpdater.Translation.SuccessfullyUpdated  = LocalizedStrings.Str1485;
            automaticUpdater.Translation.TryAgainLater        = LocalizedStrings.Str1486;
            automaticUpdater.Translation.TryAgainNow          = LocalizedStrings.Str1487;
            automaticUpdater.Translation.UpdateAvailable      = LocalizedStrings.Str1488;
            automaticUpdater.Translation.UpdateFailed         = LocalizedStrings.Str1489;
            automaticUpdater.Translation.UpdateNowButton      = LocalizedStrings.Str1490;
            automaticUpdater.Translation.ViewChangesMenu      = LocalizedStrings.Str1491;
            automaticUpdater.Translation.ViewError            = LocalizedStrings.Str1492;
        }
    }
}
