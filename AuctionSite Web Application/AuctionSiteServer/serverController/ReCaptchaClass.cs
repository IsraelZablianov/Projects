using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSiteServer.serverController
{
   using Newtonsoft.Json;

    public class ReCaptchaClass
    {
        public static string Validate(string EncodedResponse)
        {
            var client = new System.Net.WebClient();

            string PrivateKey = "6LcCjwwUAAAAABc6ERWSsOsL__jbQJu4ciXiZz8c";

            var GoogleReply = client.DownloadString(
                $"https://www.google.com/recaptcha/api/siteverify?secret={PrivateKey}&response={EncodedResponse}");

            var captchaResponse = JsonConvert.DeserializeObject<ReCaptchaClass>(GoogleReply);

            return captchaResponse.Success;
        }

        [JsonProperty("success")]
        public string Success
        {
            get { return m_Success; }
            set { m_Success = value; }
        }
        private string m_Success;

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return m_ErrorCodes; }
            set { m_ErrorCodes = value; }
        }
        private List<string> m_ErrorCodes;
    }
}
