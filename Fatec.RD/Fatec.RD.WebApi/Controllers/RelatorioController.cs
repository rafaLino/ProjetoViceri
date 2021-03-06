﻿using Fatec.RD.Bussiness;
using Fatec.RD.Bussiness.Inputs;
using Fatec.RD.Dominio.ViewModel;
using Swashbuckle.Swagger.Annotations;

using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Fatec.RD.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/Relatorio")]
    public class RelatorioController : ApiController
    {
        RelatorioNegocio _appRelatorio;

        /// <summary>
        /// 
        /// </summary>
        public RelatorioController()
        {
            _appRelatorio = new RelatorioNegocio();
        }

        /// <summary>
        /// Método que obtem uma lista de relatorio....
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
            return Ok(_appRelatorio.Selecionar());
        }

        /// <summary>
        /// Método que obtem um relatorio por Id
        /// </summary>
        /// <returns>Lista de Despesa</returns>
        /// <param name="id"></param>
        /// <remarks>Obtem lista de depesa</remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response>
        /// <response code="500">InternalServerError</response>
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest, "BadRequest")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [Route("{id}")]
        [ResponseType(typeof(RelatorioViewModel))]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            return Ok(_appRelatorio.SelecionarPorId(id));
        }


        /// <summary>
        /// Método que obtem todas as despesas do relatório...
        /// </summary>
        /// <param name="id">Id do relatório</param>
        /// <returns>Lista de Despesas</returns>
        /// <remarks>Obtem lista de depesa</remarks>
        /// <response code="200">Ok</response>
        /// <response code="404">NotFound</response>      
        /// <response code="500">InternalServerError</response>
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound, "NotFound")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [Route("{id}/Despesas")]
        [HttpGet]
        public IHttpActionResult GetDespesaRelatorio(int id)
        {
            return Ok(_appRelatorio.SelecionarDespesasPorRelatorio(id));
        }

       
        /// <summary>
        /// Método que obtem todas as despesas sem relatório...
        /// </summary>
        /// <returns>Lista de Despesas</returns>
        /// <remarks>Obtem lista de depesa</remarks>
        /// <response code="200">Ok</response>
        /// <response code="404">NotFound</response>
        /// <response code="500">InternalServerError</response>
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound, "NotFound")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [Route("Despesas")]
        [HttpGet]
        public IHttpActionResult GetDespesaRelatorio()
        {
            return Ok(_appRelatorio.SelecionarDespesasSemRelatorio());
        }

        /// <summary>
        /// Método para atualizar relatório
        /// </summary>
        /// <returns></returns>
        /// <response code="202">Accepted</response>
        /// <response code="404">BadRequest</response>
        /// <response code="500">InternalServerError</response>
        [SwaggerResponse(HttpStatusCode.Accepted)]
        [SwaggerResponse(HttpStatusCode.NotFound, "Not Found")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [ResponseType(typeof(RelatorioViewModel))]
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(int id, RelatorioInput input)
        {
            _appRelatorio.Atualizar(id, input);
            return Content(HttpStatusCode.Accepted, input);
        }

        /// <summary>
        /// Método para Deletar Relatório
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
            _appRelatorio.Deletar(id);
            return Ok();
        }

        /// <summary>
        /// Método para Adicionar um Relatório
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Created</response>
        /// <response code="400">BadRequest</response>
        /// <response code="500">InternalServerError</response>
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest, "BadRequest")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [ResponseType(typeof(RelatorioViewModel))]
        [HttpPost]
        public IHttpActionResult Post([FromBody] RelatorioInput input)
        {
            var obj = _appRelatorio.Adicionar(input);
            return Created($"{Request?.RequestUri}/{obj.Id}", obj);
        }

        /// <summary>
        /// Método que insere a relacao de relatório e despesa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        /// <param name="input">Input com lista da relação de relatório e despesa</param>
        /// <remarks>Insere vínculo entre as entidade</remarks>
        /// <response code="201">Created</response>
        /// <response code="400">BadRequest</response>
        /// <response code="500">InternalServerError</response>
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest, "BadRequest")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [Route("{id}/Despesas")]
        [HttpPost]
        public HttpResponseMessage PostRelatorioDespesa(int id, RelatorioDespesaInput obj)
        {
            _appRelatorio.InserirRelatorioDespesa(id, obj);
            return new HttpResponseMessage(HttpStatusCode.Created);
        }


        /// <summary>
        /// Método que Deleta a relacao de relatório e despesa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        /// <remarks>Insere vínculo entre as entidade</remarks>
        /// <response code="200">OK</response>
        /// <response code="404">BadRequest</response>
        /// <response code="500">InternalServerError</response>
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest, "BadRequest")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [Route("{id}/Despesas")]
        [HttpDelete]
        public HttpResponseMessage DeleteRelatorioDespesa(int id, RelatorioDespesaInput obj)
        {
            _appRelatorio.DeletarRelatorioDespesa(id, obj);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }



    }
}
