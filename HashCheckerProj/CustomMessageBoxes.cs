namespace HashChecker.WinForms
{
    using System.Windows.Forms;

    internal class CustomMessageBoxes
    {
        #region Message Boxes

        public static void Simple(string msg)
        {
            MessageBox.Show(msg, Application.ProductName);
        }

        public static void Error(string msg)
        {
            MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Info(string msg)
        {
            MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Warning(string msg)
        {
            MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Exclamation(string msg)
        {
            MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void Question(string msg)
        {
            MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        #endregion
    }
}
