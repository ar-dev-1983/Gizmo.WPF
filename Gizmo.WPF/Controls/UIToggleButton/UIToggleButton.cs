using System.Windows;
using System.Windows.Controls.Primitives;

namespace Gizmo.WPF
{
    /// <summary>
    /// UIToggleButton - это элемент управления для представления ToggleButton с закругленными углами и иконкой.
    /// </summary>
    /// <remarks>
    /// UIToggleButton is a control for presenting a ToggleButton with rounded corners and an icon.
    /// </remarks>
    public class UIToggleButton : ToggleButton, ICorneredControl
    {
        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию для DependencyObject
        /// </summary>
        /// <remarks>
        /// Default DependencyObject constructor
        /// </remarks>
        static UIToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIToggleButton), new FrameworkPropertyMetadata(typeof(UIToggleButton)));
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
        /// Признак устанавливающий видимость части визуального элемента. Когда true - отрисовывается только контент и иконка, если false - отрисовывается полностью.
        /// </summary>
        /// <remarks>
        /// A property that sets the visibility of a part of the visual element. When true - only the content and the icon are drawn, if false - the entire control is drawn.
        /// </remarks>
        public bool Flat
        {
            get => (bool)GetValue(FlatProperty);
            set => SetValue(FlatProperty, value);
        }

        /// <summary>
        /// Иконка
        /// </summary>
        /// <remarks>
        /// Icon
        /// </remarks>
        public object Icon
        {
            get => (object)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIToggleButton), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty FlatProperty = DependencyProperty.Register("Flat", typeof(bool), typeof(UIToggleButton), new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(UIToggleButton), new FrameworkPropertyMetadata(null));
        #endregion
    }
}
