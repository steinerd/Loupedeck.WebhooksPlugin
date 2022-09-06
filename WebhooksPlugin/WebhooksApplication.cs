namespace Loupedeck.WebhooksPlugin
{
    using System;

    /// <summary>
    /// Webhook Application stub for Loupedeck <see cref="ClientApplication"/> inheritance
    /// </summary>
    public class WebhooksApplication : ClientApplication
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public WebhooksApplication() { }

        /// <summary>
        /// Gets the Proccess Name. Always empty, we want this to run on ALL process.
        /// </summary>
        /// <returns></returns>
        protected override String GetProcessName() => String.Empty;

        /// <summary>
        /// Gets the Bundle Name. Always empty, we don't want to catagorize this
        /// </summary>
        /// <returns></returns>
        protected override String GetBundleName() => String.Empty;
    }
}