using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoEmpresarialERP.Entities
{
    [Table("servico_venda")]
    public class ServicoVenda
    {
        [Column("id_servico")]
        public int ServicoId { get; set; }

        public Servico? Servico { get; set; }

        [Column("id_venda")]
        public int VendaId { get; set; }

        public Venda? Venda { get; set; }

        [Column("quantidade")]
        public int Quantidade { get; set; }

        [Column("id_forma_pgto")]
        public int FormaPagamentoId { get; set; }

        public FormaPagamento? FormaPagamento { get; set; }
    }
}
