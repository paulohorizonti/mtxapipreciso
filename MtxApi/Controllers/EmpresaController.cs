using MtxApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace MtxApi.Controllers
{
    public class EmpresaController : ApiController
    {
        private MtxApiContext db = new MtxApiContext();

        // GET: api/Empresa
        public IQueryable<Empresa> GetEmpresas()
        {
            return db.Empresas;
        }

        //Retorna a empresa pelo ID
        // GET: api/EmpresaMtx/5
        [ResponseType(typeof(Empresa))]
        public IHttpActionResult GetEmpresa(int id)
        {
            Empresa empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return BadRequest("EMPRESA NÃO ENCONTRADA");
            }

            return Ok(empresa);
        }
    }
}
