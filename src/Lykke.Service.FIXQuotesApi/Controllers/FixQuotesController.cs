using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.FIXQuotesApi.Core.Services;
using Lykke.Service.FIXQuotesApi.Models;
using Lykke.Service.FIXQuotesApi.Services.Middleware.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.FIXQuotesApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = KeyAuthHandler.AuthScheme)]
    [Produces("application/json")]
    public class FixQuotesController : Controller
    {
        private readonly IFixQuoteManagerRealtime _realtime;
        private readonly IFixQuoteManagerDelayed _delayed;
        private readonly ITimeService _timeService;

        public FixQuotesController(IFixQuoteManagerRealtime realtime, IFixQuoteManagerDelayed delayed, ITimeService timeService)
        {
            _realtime = realtime;
            _delayed = delayed;
            _timeService = timeService;
        }


        /// <summary>
        /// Returns actual fixing quotes
        /// </summary>
        /// <returns>Fixing quotes</returns>
        [HttpGet]
        [SwaggerOperation("GetAllForToday")]
        [ProducesResponseType(typeof(IEnumerable<FixQuoteResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public Task<IActionResult> GetAll()
        {
            var date = _timeService.UtcNow;
            return GetAllByDate(date);
        }

        /// <summary>
        /// Returns an actual fixing quote for the specified asset pair
        /// </summary>
        /// <param name="assetPair">An asset pair</param>
        /// <returns>Fixing quotes</returns>
        [HttpGet("{assetPair:alpha:minlength(3):maxlength(10)}")]
        [SwaggerOperation("GetByAssetPairForToday")]
        [ProducesResponseType(typeof(FixQuoteResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public Task<IActionResult> GetByAssetPair([FromRoute] string assetPair)
        {
            var date = _timeService.UtcNow;
            return GetByDateAndAssetPair(date, assetPair);
        }

        /// <summary>
        /// Returns actual fixing quotes for the specified <paramref name="date"/>
        /// </summary>
        /// <param name="date">A date when the fixing qutes were published</param>
        /// <returns>Fixing quotes</returns>
        [HttpGet("{date}")]
        [SwaggerOperation("GetAllByDate")]
        [ProducesResponseType(typeof(IEnumerable<FixQuoteResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllByDate([FromRoute]DateTime date)
        {
            date = date.ToUniversalTime();
            var user = HttpContext.User;
            var quotes = user.Identity.AuthenticationType != "anonymous" ? await _realtime.GetAll(date) : await _delayed.GetAll(date);
            if (quotes.Count == 0)
            {
                return NotFound();
            }
            return Ok(quotes.Select(FixQuoteResponse.Create));
        }


        /// <summary>
        /// Returns fixing quotes filtered by <paramref name="date"/> and <paramref name="assetPair"/>
        /// </summary>
        /// <param name="date">A date when the fixing qutes were published</param>
        /// <param name="assetPair">An asset pair</param>
        /// <returns>Fixing quotes</returns>
        [HttpGet("{date}/{assetPair:alpha:minlength(3):maxlength(10)}")]
        [SwaggerOperation("GetByDateAndAssetPair")]
        [ProducesResponseType(typeof(FixQuoteResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByDateAndAssetPair([FromRoute]DateTime date, [FromRoute] string assetPair)
        {
            date = date.ToUniversalTime();
            var user = HttpContext.User;
            var quote = user.Identity.AuthenticationType != "anonymous" ? await _realtime.GetById(date, assetPair) : await _delayed.GetById(date, assetPair);
            if (quote == null)
            {
                return NotFound();
            }
            return Ok(FixQuoteResponse.Create(quote));
        }


    }
}
