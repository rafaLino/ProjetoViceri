using Fatec.RD.Bussiness;
using Fatec.RD.Bussiness.Inputs;
using Fatec.RD.Dominio.Modelos;
using Fatec.RD.Dominio.ViewModel;
using Microsoft.AspNetCore.JsonPatch;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace Fatec.RD.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/Despesa")]
    public class DespesaController : ApiController
    {
        DespesaNegocio _appDespesa;

        /// <summary>
        /// 
        /// </summary>
        public DespesaController()
        {
            _appDespesa = new DespesaNegocio();
        }

        /// <summary>
        /// Método que obtem uma lista de despesa....
        /// </summary>
        /// <returns>Lista de Despesa</returns>
        /// <remarks>Obtem lista de depesa</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response>
        /// <response code="500">InternalServerError</response>
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest, "BadRequest")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(_appDespesa.Selecionar());
        }
        /// <summary>
        /// Método para selecionar despesa...
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response>
        /// <response code="500">InternalServerError</response>
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Bad Request")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [Route("{id}")]
        [ResponseType(typeof(DespesaViewModel))]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            return Ok(_appDespesa.SelecionarPorId(id));

        }


        /// <summary>
        /// Método que insere uma despesa...
        /// </summary>
        /// <returns></returns>
        /// <response code="201">Created</response>
        /// <response code="400">BadRequest</response>
        /// <response code="500">InternalServerError</response>
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Bad Request")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [ResponseType(typeof(DespesaViewModel))]
        [HttpPost]
        public IHttpActionResult Post([FromBody] DespesaInput objInput)
        {
            var obj = _appDespesa.Adicionar(objInput);
            return Created($"{Request.RequestUri}/{ obj.Id}",obj);
        }

        /// <summary>
        /// Método para atualizar Despesa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <response code="202">Accepted</response>
        /// <response code="404">BadRequest</response>
        /// <response code="500">InternalServerError</response>
        [SwaggerResponse(HttpStatusCode.Accepted)]
        [SwaggerResponse(HttpStatusCode.NotFound, "Not Found")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [ResponseType(typeof(DespesaViewModel))]
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(int id, DespesaInput input)
        {
            var obj = _appDespesa.Atualizar(id, input);
            return Content(HttpStatusCode.Accepted, obj);
        }
        /// <summary>
        /// Método para atualizar Despesa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        ///  /// <response code="200">Accepted</response>
        /// <response code="404">BadRequest</response>
        /// <response code="500">InternalServerError</response>
        [SwaggerResponse(HttpStatusCode.Accepted)]
        [SwaggerResponse(HttpStatusCode.NotFound, "Not Found")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [ResponseType(typeof(DespesaViewModel))]
        [HttpPatch]
        [Route("{id}")]
        public IHttpActionResult Patch(int id, [FromBody] JsonPatchDocument<Despesa> input)
        {
            var obj = _appDespesa.Atualizar(id, input);
            return Content(HttpStatusCode.Accepted, obj);
        }

        /// <summary>
        /// Método para Deletar Despesa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///<response code="200">OK</response>
        ///<response code="404">BadRequest</response>
        ///<response code="500">InternalServerError</response>
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest, "BadRequest")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [ResponseType(typeof(DespesaViewModel))]
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            _appDespesa.Deletar(id);
            return Ok();
        }
    }
}
