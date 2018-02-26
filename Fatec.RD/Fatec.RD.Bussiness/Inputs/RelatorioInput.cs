using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fatec.RD.Bussiness.Inputs
{
    public class RelatorioInput
    {
        public int TipoRelatorio { get; set; }
       // public string TipoRelatorio { get; set; }
        public string Descricao { get; set; }
        public string Comentario { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
