using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LingSubPlayer.Common;
using LingSubPlayer.Common.Data;
using LingSubPlayer.Common.Logging;
using LingSubPlayer.Install;
using Splat;
using Squirrel;

namespace LingSubPlayer.Wpf.Core.Controllers
{
    public class ApplicationUpdateController
    {
        private readonly IConfiguration configuration;
        private readonly ILog log;

        public ApplicationUpdateController(IConfiguration configuration, ILog log)
        {
            this.configuration = configuration;
            this.log = log;
        }

        public async Task CheckForUpdatesAndDownload(IUpdatesAvailableView view, AvailableUpdatesInformation availableUpdatesInformation)
        {
            if (!string.IsNullOrEmpty(configuration.UpdateUrl))
            {
                log.Write.Trace("Looking for updates at {0}...", configuration.UpdateUrl);
                using (var splatLogger = new SplatLogger("Logs\\LingSubPlayer-Update.log"))
                {
                    Locator.CurrentMutable.Register(() =>
                    {

                        return splatLogger;
                    }, typeof (ILogger));

                    using (
                        var mgr = new UpdateManager(configuration.UpdateUrl, configuration.ApplicationPackageName,
                            FrameworkVersion.Net45))
                    {
                        try
                        {
                            var info = await mgr.CheckForUpdate();

                            if (info.ReleasesToApply.Any())
                            {
                                //var availableUpdatesInformation = new AvailableUpdatesInformation();
                                availableUpdatesInformation.CurrentVersion = info.CurrentlyInstalledVersion != null
                                    ? info.CurrentlyInstalledVersion.Version
                                    : Assembly.GetEntryAssembly().GetName().Version;
                                availableUpdatesInformation.FutureVersion = info.FutureReleaseEntry.Version;
                                availableUpdatesInformation.State = AvailableUpdatesInformation.UpdateState.UpdatesAvailable;
                                availableUpdatesInformation.DownloadPercentage = 0;

                                view.ShowUpdatesAvailable(availableUpdatesInformation);

                                log.Write.Trace(
                                    "Updates were found. Current version is {0}. The following versions to be applied: {1}",
                                    info.CurrentlyInstalledVersion != null
                                        ? info.CurrentlyInstalledVersion.Version.ToString()
                                        : "<not installed>",
                                    string.Join(", ", info.ReleasesToApply.Select(r => r.Version.ToString())));

                                log.Write.Trace("Downloading updates...");

                                availableUpdatesInformation.State = AvailableUpdatesInformation.UpdateState.Downloading;
                                await mgr.DownloadReleases(info.ReleasesToApply, i =>
                                {
                                    try
                                    {
                                        availableUpdatesInformation.DownloadPercentage = i;
                                        log.Write.Trace("Downloading... {0}%", new[] {i});
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Write.Error("Error while downloading", ex);
                                    }
                                });

                                availableUpdatesInformation.DownloadPercentage = 100;
                                availableUpdatesInformation.State = AvailableUpdatesInformation.UpdateState.Applying;

                                log.Write.Trace("Successfully downloaded updates.");

                                log.Write.Trace("Applying updates...");

                                await mgr.ApplyReleases(info, i =>
                                {
                                    availableUpdatesInformation.ApplicationPercentage = i;
                                    log.Write.Trace("Applying updates... {0}%", new[] {i});
                                });

                                log.Write.Trace("Successfully applied updates.");

                                log.Write.Trace("Creating uninstall shortcuts...");

                                await mgr.CreateUninstallerRegistryEntry();

                                log.Write.Trace("Created uninstall shortcuts.");

                                log.Write.Trace("Update is completed.");

                                availableUpdatesInformation.State =
                                    AvailableUpdatesInformation.UpdateState.RestartRequired;
                            }
                            else
                            {
                                log.Write.Trace("No updates were found. Current version is {0}",
                                    info.CurrentlyInstalledVersion.Version.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Write.Error("There was an error while checking for updates", ex);
                            return;
                        }
                    }
                }
            }
            else
            {
                log.Write.Trace("Update is skipped as there is no update URL specified");
            }

            log.Write.Trace("End!");
        }
    }
}