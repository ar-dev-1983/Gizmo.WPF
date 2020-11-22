using System.Windows;
using System.Windows.Controls;

namespace Gizmo.WPF
{
    public class UITextBox : TextBox, ICorneredControl
    {
        public UITextBox()
            : base()
        {
            DefaultStyleKey = typeof(UITextBox);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public bool Flat
        {
            get => (bool)GetValue(FlatProperty);
            set => SetValue(FlatProperty, value);
        }

        public bool IsValidationError
        {
            get => (bool)GetValue(IsValidationErrorProperty);
            set => SetValue(IsValidationErrorProperty, value);
        }

        public string ValidationError
        {
            get => (string)GetValue(ValidationErrorProperty);
            set => SetValue(ValidationErrorProperty, value);
        }
        public object Watermark
        {
            get => (object)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }
        public DataTemplate WatermarkDataTemplate
        {
            get => (DataTemplate)GetValue(WatermarkDataTemplateProperty);
            set => SetValue(WatermarkDataTemplateProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UITextBox), new UIPropertyMetadata(new CornerRadius(3)));
        public static readonly DependencyProperty FlatProperty = DependencyProperty.Register("Flat", typeof(bool), typeof(UITextBox), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsValidationErrorProperty = DependencyProperty.Register("IsValidationError", typeof(bool), typeof(UITextBox), new UIPropertyMetadata(false));
        public static readonly DependencyProperty ValidationErrorProperty = DependencyProperty.Register("ValidationError", typeof(string), typeof(UITextBox), new UIPropertyMetadata(null));
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(UITextBox), new UIPropertyMetadata(null));
        public static readonly DependencyProperty WatermarkDataTemplateProperty = DependencyProperty.Register("WatermarkDataTemplate", typeof(DataTemplate), typeof(UITextBox), new UIPropertyMetadata(null));
    }
}
