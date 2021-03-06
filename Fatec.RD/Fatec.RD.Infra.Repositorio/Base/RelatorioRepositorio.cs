﻿using Dapper;
using Fatec.RD.Dominio.Modelos;
using Fatec.RD.Dominio.ViewModel;
using Fatec.RD.Infra.Repositorio.Contexto;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Fatec.RD.Infra.Repositorio.Base
{
    public sealed class RelatorioRepositorio
    {
        readonly DapperContexto _db;
        readonly IDbConnection _connection;

        public RelatorioRepositorio()
        {
            _db = new DapperContexto();
            _connection = _db.Connection;
        }

        /// <summary>
        /// Método que seleciona uma lista de relatórios..
        /// </summary>
        /// <returns>Lista de relatórios</returns>
        public List<RelatorioViewModel> Selecionar()
        {
            var sqlCommand = @"SELECT r.Id, tp.Descricao as TipoRelatorio, r.Descricao, r.Comentario, r.DataCriacao
                                    FROM Relatorio r
                                    INNER JOIN TipoRelatorio tp ON r.IdTipoRelatorio = tp.Id";

            return _connection.Query<RelatorioViewModel>(sqlCommand).ToList();
        }

        /// <summary>
        /// Método que seleciona um relatorio pelo Id...
        /// </summary>
        /// <param name="id">Id do relatorio</param>
        /// <returns>Objeto de relatorio</returns>
        public RelatorioViewModel SelecionarPorId(int id)
        {
            var sqlCommand = @"SELECT r.Id, tp.Descricao as TipoRelatorio, r.Descricao, r.Comentario, r.DataCriacao
                                    FROM Relatorio r
                                    INNER JOIN TipoRelatorio tp ON r.IdTipoRelatorio = tp.Id
                                        WHERE r.id = @id";

            return _connection.Query<RelatorioViewModel>(sqlCommand, new { id }).FirstOrDefault();
        }
        /// <summary>
        /// Método que retorna Relatório por Tipo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Relatorio SelecionarRelatorioTipoRelatorio(int id)
        {
            var sqlCommand = @"SELECT * FROM Relatorios r
                                INNER JOIN TipoRelatorio tp ON r.IdTipoRelatorio = tp.Id
                                WHERE r.Id = @Id";
            return _connection.Query<Relatorio, TipoRelatorio, Relatorio>(sqlCommand, (r, tp) =>
                   {
                       r.TipoRelatorio = tp;
                       return r;
                   },
                 param: new { id },
                 splitOn: "Id"
         ).FirstOrDefault();
        }

        /// <summary>
        /// Método que retorna as despesas de um relatorio...
        /// </summary>
        /// <param name="idRelatorio">Id do relatorio</param>
        /// <returns>Uma lista de despesas</returns>
        public List<DespesaViewModel> SelecionarPorRelatorio(int idRelatorio)
        {
            var sqlCommand = @"SELECT d.Id, td.Descricao as TipoDespesa, tp.Descricao as TipoPagamento, d.Data, d.Valor, d.Comentario
                                    FROM Despesa d
                                        INNER JOIN RelatorioDespesa rd ON rd.IdDespesa = d.Id
                                        INNER JOIN Relatorio r ON rd.IdRelatorio = r.Id
                                        INNER JOIN TipoPagamento tp ON d.IdTipoPagamento = tp.Id
                                        INNER JOIN TipoDespesa td ON d.IdTipoDespesa = td.Id
                                    WHERE r.Id = @IdRelatorio";

            return _connection.Query<DespesaViewModel>(sqlCommand, new { IdRelatorio = idRelatorio }).ToList();
        }
       
        /// <summary>
        /// Método que retorna despesas que não estão atreladas a algum relatorio...
        /// </summary>
        /// <returns>Lista de despesas</returns>
        public List<DespesaViewModel> SelecionarDespesasSemRelatorio()
        {
            var sqlCommand = @"SELECT d.Id, td.Descricao as TipoDespesa, tp.Descricao as TipoPagamento, d.Data, d.Valor, d.Comentario
                                    FROM Despesa d
                                        LEFT JOIN RelatorioDespesa rd ON rd.IdDespesa = d.Id
                                        INNER JOIN TipoPagamento tp ON d.IdTipoPagamento = tp.Id
                                        INNER JOIN TipoDespesa td ON d.IdTipoDespesa = td.Id
                                    WHERE rd.IdRelatorio is null";

            return _connection.Query<DespesaViewModel>(sqlCommand).ToList();
        }
        
        public void Alterar(Relatorio obj)
        {
            var sqlCommand = @"UPDATE Relatorio SET
	                            IdTipoRelatorio = @IdTipoRelatorio,
	                            Descricao = @Descricao,
	                            Comentario = @Comentario
	                            WHERE Id = @Id";
            _connection.Execute(sqlCommand, obj);
        }

        /// <summary>
        /// Método que insere um novo Relatório
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Inserir(Relatorio obj)
        {
            var sqlCommand = @"INSERT Relatorio(IdTipoRelatorio,Descricao,Comentario,DataCriacao)
	                            VALUES (@IdTipoRelatorio,@Descricao,@Comentario,@DataCriacao)
                                     SELECT CAST (SCOPE_IDENTITY() as int)";

            return _connection.Query<int>(sqlCommand, obj).First();

        }
        /// <summary>
        /// Método que deleta um relatório
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var sqlCommand = @"DELETE FROM Relatorio WHERE ID=@Id";

            _connection.Execute(sqlCommand, new { Id = id });
        }

    }
}
