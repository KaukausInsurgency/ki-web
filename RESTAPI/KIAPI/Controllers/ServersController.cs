using KIAPI.Classes;
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
        private IDAL dal;
        private IDAL_Rpt rpt_dal;
        public ServersController()
        {
            dal = new DAL();
            rpt_dal = new DAL_Rpt();
        }

        // GET api/<controller>
        [Route("api/servers")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return Ok(dal.GetServers());
        }

        [Route("api/servers/serverid")]
        [HttpGet]
        public IHttpActionResult GetSingle(int serverid)
        {
            return Ok(new { Result = "value" });
        }
    }
}