using Microsoft.UI.Xaml.Controls;
using static GestaoEmpresarialERP.App;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoEmpresarialERP
{

    public enum QuantidadeResult
    {
        CONFIRMAR,
        CANCELAR
    }

    /// <summary>
    /// Diálogo para selecionar quantidade de serviço requerido
    /// </summary>
    public sealed partial class QuantidadeDialog : ContentDialog
    {
        public QuantidadeResult quantidadeResult { get; private set; }
        public int Quantidade { get; private set; } = 0;

        public QuantidadeDialog()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (NmQtd.Value <= 0) args.Cancel = true;
            quantidadeResult = QuantidadeResult.CONFIRMAR;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            quantidadeResult = QuantidadeResult.CANCELAR;
        }
    }
}
