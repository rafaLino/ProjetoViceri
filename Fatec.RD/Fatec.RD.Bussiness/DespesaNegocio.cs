using Fatec.RD.Bussiness.Inputs;
using Fatec.RD.Dominio.Modelos;
using Fatec.RD.Dominio.ViewModel;
using Fatec.RD.Infra.Repositorio.Base;
using Fatec.RD.SharedKernel.Excecoes;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Globalization;

namespace Fatec.RD.Bussiness
{
    public sealed class DespesaNegocio
    {
        DespesaRepositorio _despesaRepositorio;
        TipoDespesaRepositorio _tipoDespesaRepositorio;
        TipoPagamentoRepositorio _tipoPagamentoRepositorio;

        public DespesaNegocio()
        {
            _despesaRepositorio = new DespesaRepositorio();
            _tipoDespesaRepositorio = new TipoDespesaRepositorio();
            _tipoPagamentoRepositorio = new TipoPagamentoRepositorio();
        }

        /// <summary>
        /// Método que seleciona uma lista de despesas...
        /// </summary>
        /// <returns></returns>
        public List<DespesaViewModel> Selecionar()
        {

            return _despesaRepositorio.Selecionar();
        }
        /// <summary>
        /// Método que Seleciona Despesa por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DespesaViewModel SelecionarPorId(int id)
        {
            return _despesaRepositorio.SelecionarPorId(id);
        }
        /// <summary>
        /// Método que adiciona uma despesa...
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>DespesaViewModel</returns>
        public DespesaViewModel Adicionar(DespesaInput obj)
        {
            try
            {
                var objTipoDespesa = _tipoDespesaRepositorio.SelecionarPorId(obj.TipoDespesa);
                var objTipoPagamento = _tipoPagamentoRepositorio.SelecionarPorId(obj.TipoPagamento);

                var _despesa = new Despesa()
                {
                    IdTipoDespesa = objTipoDespesa.Id,
                    IdTipoPagamento = objTipoPagamento.Id,
                    TipoDespesa = objTipoDespesa,
                    TipoPagamento = objTipoPagamento,
                    Data = obj.Data, //DateTime.ParseExact(obj.Data.ToString(),"MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                    Valor = obj.Valor,
                    Comentario = obj.Comentario,
                    DataCriacao = DateTime.Now
                };
                int id = _despesaRepositorio.Inserir(_despesa);

                return _despesaRepositorio.SelecionarPorId(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Método que atualiza despesa...
        /// </summary>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DespesaViewModel Atualizar(int id, DespesaInput obj)
        {
            var _despesa = _despesaRepositorio.SelecionarDespesaTipos(id);

            if (_despesa == null)
                throw new NaoEncontradoException();

            _despesa.TipoDespesa.Id = obj.TipoDespesa;
            _despesa.TipoPagamento.Id = obj.TipoPagamento;
            _despesa.Valor = obj.Valor;
            _despesa.Data = obj.Data;
            _despesa.Comentario = obj.Comentario;

            _despesaRepositorio.Alterar(_despesa);

            return _despesaRepositorio.SelecionarPorId(id);
        }

        public DespesaViewModel Atualizar(int id, JsonPatchDocument<Despesa> obj)
        {
            var _despesa = _despesaRepositorio.SelecionarDespesaTipos(id);
            if (_despesa == null)
                throw new NaoEncontradoException("Despesa não encontrada!", id);

            obj.ApplyTo(_despesa);

            _despesaRepositorio.Alterar(_despesa);

            return _despesaRepositorio.SelecionarPorId(id);
        }
        /// <summary>
        /// Método Para deletar despesa
        /// </summary>
        /// <param name="id"></param>
        public void Deletar(int id)
        {
            var _despesa = _despesaRepositorio.SelecionarPorId(id);

            if (_despesa == null) throw new NaoEncontradoException();
            _despesaRepositorio.Delete(id);
        }
    }
}
