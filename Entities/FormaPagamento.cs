using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoEmpresarialERP.Entities
{
    [Table("forma_pagamento")]
    public class FormaPagamento
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; } = null!;

        public override string ToString()
        {
            return this.Nome;
        }

        public ICollection<ServicoVenda>? Vendas { get; set; } = new List<ServicoVenda>();
    }
}
