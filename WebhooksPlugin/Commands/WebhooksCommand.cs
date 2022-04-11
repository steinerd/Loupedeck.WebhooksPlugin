namespace Loupedeck.WebhooksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    using Loupedeck.WebhooksPlugin.Extensions;

    public class WebhookCommand : PluginDynamicCommand
    {
        private static readonly Services.HarFileService harFileService = new Services.HarFileService();

        public WebhookCommand() : base("Requests", "Webhook Requests", "Webhook Requests")
        {
            this.MakeProfileAction("tree");
        }

        protected override Boolean OnLoad()
        {
            return base.OnLoad();
        }        

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

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            return string.IsNullOrEmpty(actionParameter) ? "Requests" : actionParameter;
        }

        protected override void RunCommand(String actionParameter)
        {
            harFileService.Webhooks.FirstOrDefault(f => f.Comment.Equals(actionParameter, StringComparison.OrdinalIgnoreCase))?.Request.Run();
        }

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
