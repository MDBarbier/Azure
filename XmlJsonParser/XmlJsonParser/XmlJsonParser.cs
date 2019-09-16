using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XmlJsonParser
{
    public static class XmlJsonParser
    {
        [FunctionName("XmlJsonParser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {   

            log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];
            //string mode = req.Query["mode"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if (requestBody.StartsWith('<'))
            {
                //XML
                XDocument xml = XDocument.Parse(requestBody.Replace("\n", "").Replace("\t", ""));
                string resultXmlString = string.Empty;

                foreach (var item in xml.Root.Elements())
                {
                    string quoted = HttpUtility.JavaScriptStringEncode(item.Value);
                    item.Value = quoted;
                }
                string xmlString = xml.ToString();
               // return new OkObjectResult(xmlString);

                StringBuilder output = new StringBuilder();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                
                using (var sw = new StringWriter(output))
                { doc.WriteTo(new XmlTextWriter(sw)); }

                return new ContentResult
                {
                    Content = output.ToString(),
                    ContentType = @"application/xml",
                    StatusCode = StatusCodes.Status200OK
                };

            }
            else if (requestBody.StartsWith('{'))
            {
                //JSON
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                
                JObject json = JObject.Parse(requestBody);
                JObject returnData = new JObject();

                foreach (var token in json)
                {
                    string escapedString = XmlEncode(token.Value.ToString());
                    returnData.Add(token.Key.ToString(), escapedString);
                }

                return new OkObjectResult(returnData);
            }
            else
            {
                //Invalid
                return new BadRequestObjectResult("Please ensure the input is either an XML or JSON formatted string");
            }
        }

        public static string XmlEncode(string value)
        {
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings
            {
                ConformanceLevel = System.Xml.ConformanceLevel.Fragment
            };

            StringBuilder builder = new StringBuilder();

            using (var writer = System.Xml.XmlWriter.Create(builder, settings))
            {
                writer.WriteString(value);
            }

            return builder.ToString();
        }
    }

    public class PostData
    {
        public string name { get; set; }
    }
}
