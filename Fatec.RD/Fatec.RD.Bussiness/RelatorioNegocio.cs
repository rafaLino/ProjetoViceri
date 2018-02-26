using Fatec.RD.Bussiness.Inputs;
using Fatec.RD.Dominio.Modelos;
using Fatec.RD.Dominio.ViewModel;
using Fatec.RD.Infra.Repositorio.Base;
using Fatec.RD.SharedKernel.Excecoes;
using System;
using System.Collections.Generic;

namespace Fatec.RD.Bussiness
{
    public sealed class RelatorioNegocio
    {
        RelatorioRepositorio _relatorioRepositorio;
        RelatorioDespesaRepositorio _relatorioDespesaRepositorio;
        TipoRelatorioRepositorio _tipoRelatorio;

        public RelatorioNegocio()
        {
            _relatorioRepositorio = new RelatorioRepositorio();
            _relatorioDespesaRepositorio = new RelatorioDespesaRepositorio();
            _tipoRelatorio = new TipoRelatorioRepositorio();
        }

        /// <summary>
        /// Método que seleciona uma lista de relatorio...
        /// </summary>
        /// <returns></returns>
        public List<RelatorioViewModel> Selecionar()
        {
            return _relatorioRepositorio.Selecionar();
        }

        /// <summary>
        /// Método que seleciona despesas pelo relatorio...
        /// </summary>
        /// <param name="idRelatorio">Id do relatório</param>
        /// <returns></returns>
        public List<DespesaViewModel> SelecionarDespesasPorRelatorio(int idRelatorio)
        {
            this.SelecionarPorId(idRelatorio);

            return _relatorioRepositorio.SelecionarPorRelatorio(idRelatorio);
        }

        /// <summary>
        /// Método que seleciona despesas sem ser atrelado com o relatorio
        /// </summary>
        /// <returns></returns>
        public List<DespesaViewModel> SelecionarDespesasSemRelatorio()
        {
            return _relatorioRepositorio.SelecionarDespesasSemRelatorio();
        }

        /// <summary>
        /// Método que seleciona um relatorio pelo Id
        /// </summary>
        /// <param name="id">ID do relatório</param>
        /// <returns>Objeto de relatorio</returns>
        public RelatorioViewModel SelecionarPorId(int id)
        {
            var retorno = _relatorioRepositorio.SelecionarPorId(id);

            if (retorno.Id <= 0)
                throw new NaoEncontradoException("Relatório não encontrado", id);

            return retorno;
        }
        /// <summary>
        /// Método para atualizar relatório
        /// </summary>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public RelatorioViewModel Atualizar(int id, RelatorioInput obj)
        {
            var relatorio = _relatorioRepositorio.SelecionarRelatorioTipoRelatorio(id);
            if (relatorio == null)
                throw new NaoEncontradoException();

            relatorio.TipoRelatorio.Id = obj.TipoRelatorio;
            relatorio.Descricao = obj.Descricao;
            relatorio.Comentario = obj.Comentario;


            _relatorioRepositorio.Alterar(relatorio);
            return _relatorioRepositorio.SelecionarPorId(id);
        }
        /// <summary>
        /// Método para adicionar Relatório
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public RelatorioViewModel Adicionar(RelatorioInput obj)
        {
            var objTipoRelatorio = _tipoRelatorio.SelecionarPorId(obj.TipoRelatorio);
            if (objTipoRelatorio == null)
                throw new NaoEncontradoException();
            var relatorio = new Relatorio()
            {
                IdTipoRelatorio = objTipoRelatorio.Id,
                TipoRelatorio = objTipoRelatorio,
                Descricao = obj.Descricao,
                Comentario = obj.Comentario,
                DataCriacao = DateTime.Now

            };
            var id = _relatorioRepositorio.Inserir(relatorio);

            return _relatorioRepositorio.SelecionarPorId(id);
        }
        /// <summary>
        /// Método que deleta um relatório
        /// </summary>
        /// <param name="id"></param>
        public void Deletar(int id)
        {
            var obj = _relatorioRepositorio.SelecionarPorId(id);
            if (obj == null)
                throw new NaoEncontradoException("Relatório não encontrado", id);

            _relatorioRepositorio.Delete(id);
        }

        /// <summary>
        /// Método que insere a relação de Despesa com relatório...
        /// </summary>
        /// <param name="obj">Obj de Input</param>
        public void InserirRelatorioDespesa(int idRelatorio, RelatorioDespesaInput obj)
        {
            foreach (var item in obj.Chave)
            {
                _relatorioDespesaRepositorio.Inserir(item.IdDespesa, idRelatorio);
            }
        }

        /// <summary>
        /// Método que deleta a relação de Despesa com relatório
        /// </summary>
        /// <param name="idRelatorio"></param>
        /// <param name="obj"></param>
        public void DeletarRelatorioDespesa(int idRelatorio, RelatorioDespesaInput obj)
        {
            foreach (var item in obj.Chave)
            {
                _relatorioDespesaRepositorio.Deletar(item.IdDespesa, idRelatorio);
            }
        }
    }
}
