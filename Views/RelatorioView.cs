using System;

namespace GestaoEmpresarialERP.Views
{
    public class RelatorioView
    {
        public string? Nome { get; set; }
        public double Preco { get; set; }
        public int Quantidade { get; set; }
        public DateTime Data { get; set; }
        public double Subtotal { get; set; }
        public string? FormaPagamento { get; set; }
    }
}
