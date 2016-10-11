namespace PsMidiProfiler.Behaviors
{
    using System;
    using System.Windows.Controls;

    internal class Capture : IDisposable
    {
        private TextBox textBox;

        public Capture(TextBox textBox)
        {
            this.textBox = textBox;
            this.textBox.TextChanged += this.OnTextBoxOnTextChanged;
        }

        public void Dispose()
        {
            this.textBox.TextChanged -= this.OnTextBoxOnTextChanged;
        }

        private void OnTextBoxOnTextChanged(object sender, TextChangedEventArgs args)
        {
            this.textBox.ScrollToEnd();
        }
    }
}