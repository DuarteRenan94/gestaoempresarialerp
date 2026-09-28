using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoEmpresarialERP.Entities
{
    [Table("servico")]
    public class Servico
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; } = null!;

        [Column("preco")]
        public double Preco { get; set; }

        public ICollection<ServicoVenda>? Vendas { get; set; } = new List<ServicoVenda>();
    }
}
