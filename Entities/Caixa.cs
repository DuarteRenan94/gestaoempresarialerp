using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoEmpresarialERP.Entities
{
    [Table("caixa")]
    public class Caixa
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("movimento")]
        public double Movimento { get; set; }
        [Column("saldo_anterior")]
        public double SaldoAnterior { get; set; }

        [Column("descricao")]
        public string? Descricao { get; set; }
        [Column("saldo_atual")]
        public double SaldoAtual { get; set; }
        [Column("dt_movimentacao")]
        public DateTime DtMovimentacao { get; set; }
    }
}
