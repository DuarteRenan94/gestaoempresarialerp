namespace GestaoEmpresarialERP.Views
{
    public class ServicoView
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public double Preco { get; set; }
        public int Quantidade { get; set; }

        public double Total { get; set; }
    }
}
