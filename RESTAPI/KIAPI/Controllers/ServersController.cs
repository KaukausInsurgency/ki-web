using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace KIAPI.Controllers
{
    public class ServersController : ApiController
    {
        // GET api/<controller>
        [Route("api/servers")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return Ok(new { JsonProperty = "SomeVal" });
        }

        [Route("api/servers/serverid")]
        [HttpGet]
        public IHttpActionResult GetSingle(int serverid)
        {
            return Ok(new { Result = "value" });
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}