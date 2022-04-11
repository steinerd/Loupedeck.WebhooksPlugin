namespace Loupedeck.WebhooksPlugin
{
    using System;
    using System.IO;

    public class WebhooksPlugin : Plugin
    {
        internal const string DEFAULT_PATH = @".loupedeck\webhooks";
        internal static string UserProfilePath { get => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); }

        public override Boolean HasNoApplication => true;
        public override Boolean UsesApplicationApiOnly => true;

        public override void Load() => this.Init();

        public override void Unload() { }

        private void OnApplicationStarted(Object sender, EventArgs e) { }

        private void OnApplicationStopped(Object sender, EventArgs e) { }

        public override void RunCommand(String commandName, String parameter) { }

        public override void ApplyAdjustment(String adjustmentName, String parameter, Int32 diff) { }

        private void Init()
        {
            this.Info.Icon16x16 = EmbeddedResources.ReadImage("Loupedeck.WebhooksPlugin.Resources.16.png");
            this.Info.Icon32x32 = EmbeddedResources.ReadImage("Loupedeck.WebhooksPlugin.Resources.32.png");
            this.Info.Icon48x48 = EmbeddedResources.ReadImage("Loupedeck.WebhooksPlugin.Resources.48.png");
            this.Info.Icon256x256 = EmbeddedResources.ReadImage("Loupedeck.WebhooksPlugin.Resources.256.png");

            Directory.CreateDirectory(Path.Combine(UserProfilePath, DEFAULT_PATH));
        }
    }
}
