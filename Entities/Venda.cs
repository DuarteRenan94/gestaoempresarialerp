using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoEmpresarialERP.Entities
{
    [Table("venda")]
    public class Venda
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("total")]
        public double Total { get; set; }

        [Column("dt_venda")]
        public DateTime DtVenda { get; set; }

        public ICollection<ServicoVenda>? Vendas { get; set; } = new List<ServicoVenda>();
    }
}
