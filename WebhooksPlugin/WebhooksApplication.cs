namespace Loupedeck.WebhooksPlugin
{
    using System;

    public class WebhooksApplication : ClientApplication
    {
        public WebhooksApplication() { }

        protected override String GetProcessName() => String.Empty;
        protected override String GetBundleName() => String.Empty;
    }
}