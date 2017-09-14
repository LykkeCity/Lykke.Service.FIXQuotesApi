using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.FIXQuotesApi.Core.Domain.Models;
using Lykke.Service.FIXQuotesApi.Core.Services;
using Lykke.Service.FIXQuotesApi.Services.Middleware.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.FIXQuotesApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = KeyAuthHandler.AuthScheme)]
    public class FixQuotesController : Controller
    {
        private readonly IFixQuoteManagerRealtime _realtime;
        private readonly IFixQuoteManagerDelayed _delayed;

        public FixQuotesController(IFixQuoteManagerRealtime realtime, IFixQuoteManagerDelayed delayed)
        {
            _realtime = realtime;
            _delayed = delayed;
        }

        [HttpGet]
        [SwaggerOperation("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<FixQuoteModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get()
        {
            var user = HttpContext.User;
            var quotes = user.Identity.AuthenticationType != "anonymous" ? await _realtime.GetAll() : await _delayed.GetAll();
            if (quotes.Count == 0)
            {
                return NotFound();
            }
            return Ok(quotes);
        }

        [HttpGet]
        [Route("{id:alpha:minlength(3):maxlength(10)}")]
        [SwaggerOperation("GetById")]
        [ProducesResponseType(typeof(FixQuoteModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetById(string id)
        {
            var user = HttpContext.User;
            var quote = user.Identity.AuthenticationType != "anonymous" ? await _realtime.GetById(id) : await _delayed.GetById(id);
            if (quote == null)
            {
                return NotFound();
            }
            return Ok(quote);
        }
    }
}