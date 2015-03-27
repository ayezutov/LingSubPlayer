using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using Microsoft.Win32;

namespace LingSubPlayer.Wpf.Core.Controls
{
    /// <summary>
    /// Interaction logic for TextBoxWithButton.xaml
    /// </summary>
    public partial class TextBoxWithButton : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof (string), typeof (TextBoxWithButton), new PropertyMetadata(default(string), new PropertyChangedCallback(OnTextChanged)));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as TextBoxWithButton;
            c.ScrollTextInTextboxToEnd();
        }

        private void ScrollTextInTextboxToEnd()
        {
            Dispatcher.InvokeAsync(() =>
            {
                //FilePath.CaretIndex = FilePath.Text.Length;
                var rect = FilePath.GetRectFromCharacterIndex(FilePath.Text.Length);
                FilePath.ScrollToHorizontalOffset(rect.Right);
            }, DispatcherPriority.Background);
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public OpenFileDialogParameters OpenFileDialogParameters { get; set; }

        public TextBoxWithButton()
        {
            InitializeComponent();

            FilePath.SetBinding(TextBox.TextProperty, new Binding 
            {
                Path = new PropertyPath(TextProperty), 
                Source = this,
                Mode = BindingMode.TwoWay,
            });
        }

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            //ScrollTextInTextboxToEnd();

            var dialog = new OpenFileDialog()
            {
                CheckFileExists = (OpenFileDialogParameters ?? defaultOpenFileDialogParameters).CheckFileExists,
                FileName = Text,
                Filter = (OpenFileDialogParameters ?? defaultOpenFileDialogParameters).Filter,
                Title = (OpenFileDialogParameters ?? defaultOpenFileDialogParameters).Title,
                InitialDirectory = (OpenFileDialogParameters ?? defaultOpenFileDialogParameters).InitialDirectory,
            };

            var showDialogResult = dialog.ShowDialog();
            if (!showDialogResult.HasValue || !showDialogResult.Value)
            {
                return;
            }

            Text = dialog.FileName;
        }

        private readonly OpenFileDialogParameters defaultOpenFileDialogParameters = new OpenFileDialogParameters();
    }
}
