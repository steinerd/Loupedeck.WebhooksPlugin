namespace Loupedeck.WebhooksPlugin.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using System.Net.Http;

    public static class HarExtensions
    {
        public static Task Run(this HarSharp.Request request)
        {
            return request.Method != HttpMethod.Get.Method
                && (
                    string.IsNullOrEmpty(request.PostData.MimeType)
                    || string.IsNullOrEmpty(request.Headers.FirstOrDefault(h => h.Name.ToLower() == "content-type")?.Value)
                    )
                ? Task.Factory.StartNew(() => { })
                : Task.Factory.StartNew(() =>
                {
                    using (var client = new HttpClient())
                    using (var req = new HttpRequestMessage(new HttpMethod(request.Method), request.Url))
                    {
                        request.Headers.ToList().ForEach(h =>
                        {
                            req.Headers.TryAddWithoutValidation(h.Name, h.Value);
                        });

                        if (request.Method != HttpMethod.Get.Method)
                        {
                            req.Content = new StringContent(request.PostData.Text, System.Text.Encoding.UTF8, request.PostData.MimeType);
                        }

                        using (var response = client.SendAsync(req).Result)
                        {
                            if ((int)response.StatusCode > 400)
                            {
                                Console.Error.WriteLine($"Webhoook threw an undesired status code of {(int)response.StatusCode} | Message: {response.Content.ReadAsStringAsync().Result}");
                                return;
                            }                            
                        }
                    }
                });
        }
    }
}
