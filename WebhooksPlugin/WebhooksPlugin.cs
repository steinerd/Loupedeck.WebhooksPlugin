namespace Loupedeck.WebhooksPlugin
{
    using System;
    using System.IO;

    /// <summary>
    /// Encapsulates the Webhook Plugin members
    /// </summary>
    public class WebhooksPlugin : Plugin
    {
        /// <summary>
        /// Default path to search for webhook .har files.
        /// </summary>
        internal static readonly string DEFAULT_PATH = Path.Combine(".loupedeck","webhooks");

        /// <summary>
        /// Gets thread-safe readonly User Profile path.
        /// </summary>
        internal static string UserProfilePath => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        /// <summary>
        /// Gets <see cref="Plugin"/>'s flag property to check if plugin maps to a specific application. Always TRUE, for all applications
        /// </summary>
        public override Boolean HasNoApplication => true;

        /// <summary>
        /// Sets <see cref="Plugin"/>'s flag property to check if plugin uses ApplicationApi only. Always TRUE.
        /// </summary>
        public override Boolean UsesApplicationApiOnly => true;

        /// <summary>
        /// <see cref="Plugin"/> inherited initialization routine. 
        /// </summary>
        public override void Load() => this.Init();

        /// <summary>
        /// <see cref="Plugin"/> inherited tear-down routine.
        /// </summary>
        public override void Unload()
        {
            // Do nothing because we're not loading anything into the plugin that requires this.
        }

        /// <summary>
        /// <see cref="Plugin"/> inherited plugin-level run command routine. Ignored.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="parameter"></param>
        public override void RunCommand(String commandName, String parameter)
        {
            // Do nothing because we're <em>not</em> handeling commands at this level. 
        }

        /// <summary>
        /// <see cref="Plugin"/> inherited plugin-level apply adjustment routine. Ignored
        /// </summary>
        /// <param name="adjustmentName"></param>
        /// <param name="parameter"></param>
        /// <param name="diff"></param>
        public override void ApplyAdjustment(String adjustmentName, String parameter, Int32 diff)
        {
            // Do nothing, because we're not writing any adjustments
        }

        /// <summary>
        /// Webhook Plugin initialization routine
        /// </summary>
        private void Init()
        {
            this.Info.Icon16x16 = EmbeddedResources.ReadImage("Loupedeck.WebhooksPlugin.Resources.16.png");
            this.Info.Icon32x32 = EmbeddedResources.ReadImage("Loupedeck.WebhooksPlugin.Resources.32.png");
            this.Info.Icon48x48 = EmbeddedResources.ReadImage("Loupedeck.WebhooksPlugin.Resources.48.png");
            this.Info.Icon256x256 = EmbeddedResources.ReadImage("Loupedeck.WebhooksPlugin.Resources.256.png");

            Directory.CreateDirectory(Path.Combine(UserProfilePath, DEFAULT_PATH));
        }

        /// <summary>
        /// <see cref="Plugin"/> inherited Application Start event. Ignored
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationStarted(Object sender, EventArgs e)
        {
            // Do nothing because we're not writing an application specific plugin
        }

        /// <summary>
        /// <see cref="Plugin"/> inherited Application Stop event. Ignored
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationStopped(Object sender, EventArgs e)
        {
            // Do nothing because we're not writing an application specific plugin
        }

    }
}
