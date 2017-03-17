using System;
using System.IO;
using AuctionSiteServer.DTO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace AuctionSiteServer.serverController
{

    [BasicAuthenticationFilter]
    [RoutePrefix("api")]
    public class AuctionsController : ApiController
    {
        private DBManager _DBManager = new DBManager();

        [BasicAuthenticationFilter(active: false, loginMode: false)]
        [Route("users")]
        [HttpPost]
        public bool AddUser(UserRegistrationDTO userRegistration)
        {
            var succeed = false;
            if (!_DBManager.IsUserExists(userRegistration.ID, userRegistration.Password))
            {
                var user = Mapper.ToUser(userRegistration);
                _DBManager.AddUser(user);
                succeed = true;
            }
            return succeed;
        }

        [BasicAuthenticationFilter(active: false, loginMode: false)]
        [Route("captcha")]
        [HttpPost]
        public bool VerifyCaptcha([FromBody] CaptchaInfo captcha)
        {
           return ReCaptchaClass.Validate(captcha.Captcha) == "True";
        }

        [BasicAuthenticationFilter(active: true, loginMode: true)]
        [Route("users")]
        [HttpGet]
        public void Login()
        {
            //filter validation
        }

        [Route("auctions")]
        [HttpGet]
        public AuctionDTO[] GetAuctions()
        {
            var allAuctions = _DBManager.GetAuctions();
            var allAuctionsDTO = allAuctions.Select(Mapper.ToAuctionDTO).ToArray();
            return allAuctionsDTO;
        }

        [Route("auctions")]
        [HttpGet]
        public AuctionDTO[] GetAuctions(string partNumber)
        {
            int intPartNumber;
            if (!int.TryParse(partNumber, out intPartNumber)) return null;
            var auctions = _DBManager.GetAuctions(intPartNumber);
            var auctionsDto = auctions?.Select(Mapper.ToAuctionDTO).ToArray();
            return auctionsDto;

        }

        [Route("auctions")]
        [HttpGet]
        public AuctionDTO GetAuction(string id)
        {
            var auction = _DBManager.GetAuction(id);
            var auctionDTO = Mapper.ToAuctionDTO(auction);
            return auctionDTO;
        }

        [Route("auctions")]
        [HttpDelete]
        public void DeleteAuction(string auctionId)
        {
            _DBManager.DeleteAuction(auctionId, true);
        }

        [Route("auctions")]
        [HttpPost]
        public void addAuction(AuctionAddingDTO auctionAddingDto)
        {
            var identifier = BasicAutenticationParser.ParseAuthorizationHeader(HttpContext.Current.Request.Headers["Authorization"]);
            var auction = Mapper.ToAuction(auctionAddingDto, identifier.Name);
            _DBManager.AddAuction(auction);
        }

        [Route("categories")]
        [HttpGet]
        public CategoryDTO[] GelCategories()
        {
            var allCategories = _DBManager.GetCategories();
            var allCategoriesDTO = allCategories.Select(Mapper.ToCategoryDTO).ToArray();
            return allCategoriesDTO;
        }

        [Route("bids")]
        [HttpPost]
        public bool addBid(HighestBidDTO highestBidDTO, string auctionId)
        {
            var identifier = BasicAutenticationParser.ParseAuthorizationHeader(HttpContext.Current.Request.Headers["Authorization"]);
            var bid = Mapper.ToBid(highestBidDTO, auctionId, identifier.Name);
            return _DBManager.AddBid(bid);
        }
    }
}