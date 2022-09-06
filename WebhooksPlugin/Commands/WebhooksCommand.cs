namespace Loupedeck.WebhooksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    using Loupedeck.WebhooksPlugin.Extensions;

    /// <summary>
    /// Enacapsulates Webhook Command object's method and members. 
    /// </summary>
    public class WebhookCommand : PluginDynamicCommand
    {
        /// <summary>
        /// HAR File Service 
        /// </summary>
        internal static readonly Services.HarFileService harFileService = new Services.HarFileService();

        /// <summary>
        /// Default Constructor, with <see cref="PluginDynamicCommand"/> interited base params
        /// </summary>
        public WebhookCommand() : base("Requests", "Webhook Requests", "Webhook Requests")
        {
            this.MakeProfileAction("tree");
        }

        /// <summary>
        /// <see cref="PluginDynamicCommand"/> inherited Load Event handler
        /// </summary>
        /// <returns></returns>
        protected override Boolean OnLoad()
        {
            return base.OnLoad();
        }        

        /// <summary>
        /// Gets the different "hooks" via the HAR files's <see cref="HarSharp.Entry"/> type.
        /// <see cref="PluginDynamicCommand"/> inherited Profile Action Data fetch routine.
        /// </summary>
        /// <returns></returns>
        protected override PluginProfileActionData GetProfileActionData()
        {
            var tree = new PluginProfileActionTree("Select HTTP Request to Run");

            var webHooks = harFileService.Webhooks;

            var wehHooksByHosts = webHooks.GroupBy(gb => gb.Request.Method);

            tree.AddLevel("HTTP Method");
            tree.AddLevel("Hook");

            foreach (var group in wehHooksByHosts)
            {
                var node = tree.Root.AddNode(group.Key);

                foreach (var entry in group)
                {
                    node.AddItem(entry.Request.Comment ?? entry.Comment, entry.Request.Comment ?? entry.Comment, $"{entry.Request.Method} {entry.Request.Url}");
                }
            }

            return tree;
        }

        /// <summary>
        /// <see cref="PluginDynamicCommand"/> inherited command displayname fetch routine
        /// </summary>
        /// <param name="actionParameter">Name of action</param>
        /// <param name="imageSize">ref-ed in image size for loupedeck</param>
        /// <returns></returns>
        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            return string.IsNullOrEmpty(actionParameter) ? "Requests" : actionParameter;
        }

        /// <summary>
        /// <see cref="PluginDynamicCommand"/> inherited run command routine. Runs the HAR file's entry via <see cref="HarExtensions.Run(HarSharp.Request)"/>
        /// </summary>
        /// <param name="actionParameter"></param>
        protected override void RunCommand(String actionParameter)
        {
            harFileService.Webhooks.FirstOrDefault(f => f.Comment.Equals(actionParameter, StringComparison.OrdinalIgnoreCase))?.Request.Run();
        }

        /// <summary>
        /// <see cref="PluginDynamicCommand"/> inherited command image fetch router. 
        /// </summary>
        /// <param name="actionParameter"></param>
        /// <param name="imageSize"></param>
        /// <returns></returns>
        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            using (var bitmapBuilder = new BitmapBuilder(imageSize))
            {
                bitmapBuilder.Clear(BitmapColor.Black);

                bitmapBuilder.DrawText(actionParameter, BitmapColor.White, 15, 13, 3);
                return bitmapBuilder.ToImage();
            }
        }
    }
}
