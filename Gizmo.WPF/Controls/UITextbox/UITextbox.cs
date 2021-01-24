using System.Windows;
using System.Windows.Controls;

namespace Gizmo.WPF
{
    /// <summary>
    /// UITextBox - это элемент управления для представления TextBox с закругленными углами и подсказкой.
    /// </summary>
    /// <remarks>
    /// UITextBox is a control for presenting a TextBox with rounded corners and a watermark.
    /// </remarks>
    public class UITextBox : TextBox, ICorneredControl
    {
        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию для DependencyObject
        /// </summary>
        /// <remarks>
        /// Default DependencyObject constructor
        /// </remarks>
        public UITextBox()
            : base()
        {
            DefaultStyleKey = typeof(UITextBox);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Радиус скругления
        /// </summary>
        /// <remarks>
        /// Corner Radius
        /// </remarks>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Признак устанавливающий видимость части визуального элемента. Когда true - отрисовывается только контент, если false - отрисовывается полностью.
        /// </summary>
        /// <remarks>
        /// A property that sets the visibility of a part of the visual element. When true - only the content are drawn, if false - the entire control is drawn.
        /// </remarks>
        public bool Flat
        {
            get => (bool)GetValue(FlatProperty);
            set => SetValue(FlatProperty, value);
        }

        /// <summary>
        /// Признак устанавливающий состояние ошибки валидации.
        /// </summary>
        /// <remarks>
        /// A property that sets the state of a validation error.
        /// </remarks>
        public bool IsValidationError
        {
            get => (bool)GetValue(IsValidationErrorProperty);
            set => SetValue(IsValidationErrorProperty, value);
        }

        /// <summary>
        /// Ошибка валидации.
        /// </summary>
        /// <remarks>
        /// Validation error.
        /// </remarks>
        public string ValidationError
        {
            get => (string)GetValue(ValidationErrorProperty);
            set => SetValue(ValidationErrorProperty, value);
        }

        /// <summary>
        /// Подсказка.
        /// </summary>
        /// <remarks>
        /// Watermark.
        /// </remarks>
        public object Watermark
        {
            get => (object)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        /// <summary>
        /// Шаблон подсказки.
        /// </summary>
        /// <remarks>
        /// Data template for Watermark.
        /// </remarks>
        public DataTemplate WatermarkDataTemplate
        {
            get => (DataTemplate)GetValue(WatermarkDataTemplateProperty);
            set => SetValue(WatermarkDataTemplateProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UITextBox), new UIPropertyMetadata(new CornerRadius(3)));
        public static readonly DependencyProperty FlatProperty = DependencyProperty.Register("Flat", typeof(bool), typeof(UITextBox), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsValidationErrorProperty = DependencyProperty.Register("IsValidationError", typeof(bool), typeof(UITextBox), new UIPropertyMetadata(false));
        public static readonly DependencyProperty ValidationErrorProperty = DependencyProperty.Register("ValidationError", typeof(string), typeof(UITextBox), new UIPropertyMetadata(null));
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(UITextBox), new UIPropertyMetadata(null));
        public static readonly DependencyProperty WatermarkDataTemplateProperty = DependencyProperty.Register("WatermarkDataTemplate", typeof(DataTemplate), typeof(UITextBox), new UIPropertyMetadata(null));
        #endregion
    }
}
