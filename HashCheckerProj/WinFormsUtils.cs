namespace HashChecker.WinForms
{
    using System.Drawing;
    using System.Windows.Forms;

    public class WinFormsUtils
    {
        public static void AppendText(RichTextBox box, Color color, string text)
        {
            int start = box.TextLength;
            box.AppendText(text);
            int end = box.TextLength;

            // Textbox may transform chars, so (end-start) != text.Length
            box.Select(start, end - start);
            {
                box.SelectionColor = color;
                //// Could set box.SelectionBackColor, box.SelectionFont too.
            }

            box.SelectionLength = 0; // clear
        }
    }
}
