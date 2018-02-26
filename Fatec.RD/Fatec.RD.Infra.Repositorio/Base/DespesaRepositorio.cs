using Fatec.RD.Dominio.Modelos;
using Fatec.RD.Dominio.Repositorio;
using Fatec.RD.Infra.Repositorio.Contexto;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fatec.RD.Dominio.ViewModel;

namespace Fatec.RD.Infra.Repositorio.Base
{
    public sealed class DespesaRepositorio
    {
        readonly DapperContexto _db;
        readonly IDbConnection _connection;

        public DespesaRepositorio()
        {
            _db = new DapperContexto();
            _connection = _db.Connection;
        }

        public List<DespesaViewModel> Selecionar()
        {
            var sqlCommand = @"SELECT d.Id, td.Descricao as TipoDespesa, tp.Descricao as TipoPagamento, d.Data, d.Valor, d.Comentario
                                    FROM Despesa d
                                        INNER JOIN TipoPagamento tp ON d.IdTipoPagamento = tp.Id
                                        INNER JOIN TipoDespesa td ON d.IdTipoDespesa = td.Id";

            return _connection.Query<DespesaViewModel>(sqlCommand).ToList();
        }

        public DespesaViewModel SelecionarPorId(int Id)
        {
            var sqlCommand = @"SELECT d.Id, td.Descricao as TipoDespesa, tp.Descricao as TipoPagamento,d.Data,d.Valor, d.Comentario FROM Despesa d
                                INNER JOIN TipoDespesa td ON IdTipoDespesa = td.Id
                                INNER JOIN TipoPagamento tp ON IdTipoPagamento = tp.Id 
                                WHERE d.Id = @Id";

            return _connection.Query<DespesaViewModel>(sqlCommand, new { Id }).FirstOrDefault();
        }
        public Despesa SelecionarDespesaTipos(int id)
        {
            var sqlCommand = @"SELECT * FROM Despesa d 
                               INNER JOIN TipoDespesa td ON IdTipoDespesa = td.Id
                                INNER JOIN TipoPagamento tp ON IdTipoPagamento = tp.Id 
                                WHERE d.Id = @Id";

            return _connection.Query<Despesa, TipoDespesa, TipoPagamento, Despesa>(sqlCommand, (d, td, tp) =>
            {
                d.TipoDespesa = td;
                d.TipoPagamento = tp;
                return d;
            },
           param: new { id },
           splitOn: "Id"
           ).FirstOrDefault();

        }

        public int Inserir(Despesa obj)
        {
            return _connection.Query<int>(@"INSERT Despesa(IdTipoDespesa,IdTipoPagamento,Data,Valor,Comentario,DataCriacao)
                                            VALUES(@IdTipoDespesa,@IdTipoPagamento,@Data,@Valor,@Comentario,@DataCriacao)
                                              SELECT CAST (SCOPE_IDENTITY() as int)", obj).First();
        }

        public void Alterar(Despesa obj)
        {
            var sqlCommand = @"UPDATE Despesa
                                      SET IdTipoDespesa = @IdTipoDespesa,
                                          IdTipoPagamento = @IdTipoPagamento,
                                          Data = @Data,
                                          Valor = @Valor,
                                          Comentario = @Comentario
                                        WHERE Id = @Id";

            _connection.Execute(sqlCommand, obj);
        }

        public void Delete(int id)
        {
            _connection.Execute("DELETE FROM DESPESA WHERE ID = @Id", new { Id = id });
        }

    }
}
