using System.ComponentModel;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gizmo.WPF
{
    /// <summary>
    /// UIPasswordBox - элемент управления для обработки паролей с тем же внешним видом и поведением что и у UITextBox.
    /// </summary>
    /// <remarks>
    /// UIPasswordBox - control for handling password with the same visual behavior that UITextBox had.
    /// </remarks>
    public class UIPasswordBox : TextBox, ICorneredControl
    {
        #region Handlers
        /// <summary>
        /// Предотвращает выполнение команд: копировать, вырезать, вставить. В стиле так же скрыто контекстное меню.
        /// </summary>
        /// <remarks>
        /// Prevents command execution for copy, cut, and paste. In Style also context menu set to collapsed.
        /// </remarks>
        private static void PreviewExecutedHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy || e.Command == ApplicationCommands.Cut || e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию для DependencyObject.
        /// </summary>
        /// <remarks>
        /// Default DependencyObject constructor.
        /// </remarks>
        public UIPasswordBox()
            : base()
        {
            DefaultStyleKey = typeof(UIPasswordBox);
            CommandManager.AddPreviewExecutedHandler(this, PreviewExecutedHandler);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Инициализирует пароль с заданным значение SecureString и показывает, что пароль действительно введен в UIPasswordBox 
        /// </summary>
        /// <remarks>
        /// Initialise private password property with given SecureString, and shows that password is actually entered in UIPasswordBox
        /// </remarks>
        private protected void InitPassword(SecureString secureString)
        {
            if (this != null)
            {
                if (password != null)
                {
                    password = secureString.Copy();
                    Text = new string('*', password.Length);
                }
            }
        }

        /// <summary>
        /// Функция добавляет переданный текст к SecureString.
        /// </summary>
        /// <remarks>
        /// Function that add passed text to SecureString.
        /// </remarks>
        private protected void AddToPassword(string addedText)
        {
            if (SelectionLength > 0)
            {
                RemoveFromPassword(SelectionStart, SelectionLength);
            }

            foreach (char c in addedText)
            {
                var _caretIndex = CaretIndex;
                password.InsertAt(_caretIndex, c);
                HideText();
                Text = Text.Insert(_caretIndex++, "*");
                CaretIndex = _caretIndex;
            }
        }

        /// <summary>
        /// Функция удаляет из SecureString заданное количество символов начиная с указанного индекса.
        /// </summary>
        /// <remarks>
        /// Function that removes from SecureString number of chars at given index.
        /// </remarks>
        private protected void RemoveFromPassword(int index, int length)
        {
            var _caretIndex = CaretIndex;
            for (int i = 0; i < length; ++i)
            {
                password.RemoveAt(index);
            }
            Text = Text.Remove(index, length);
            CaretIndex = _caretIndex;
        }

        /// <summary>
        /// Функция скрывает текст за символом *.
        /// </summary>
        /// <remarks>
        /// Function that hides text behing *.
        /// </remarks>
        private void HideText()
        {
            var _caretIndex = CaretIndex;
            Text = new string('*', Text.Length);
            CaretIndex = _caretIndex;
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Виртуальная функция обрабатывает ввод нового текста до его отображения на экране.
        /// </summary>
        /// <remarks>
        /// The virtual function processes the input of new text before it is displayed on the screen.
        /// </remarks>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            if (e.Text != "\r")//prevent adding to password if Enter key is pressed
                AddToPassword(e.Text);
            e.Handled = true;
        }

        /// <summary>
        /// Виртуальная функция обрабатывает нажатия клавиш, в основном назад и удалить, но так же предотвращает ввод пробелов.
        /// </summary>
        /// <remarks>
        /// The virtual function handles keystrokes, mostly back and forth, but also prevents spaces from being entered.
        /// </remarks>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            Key key = e.Key == Key.System ? e.SystemKey : e.Key;
            switch (key)
            {
                case Key.Space:
                    e.Handled = true;
                    break;
                case Key.Back:
                case Key.Delete:
                    if (SelectionLength > 0)
                    {
                        RemoveFromPassword(SelectionStart, SelectionLength);
                    }
                    else if (key == Key.Delete && CaretIndex < Text.Length)
                    {
                        RemoveFromPassword(CaretIndex, 1);
                    }
                    else if (key == Key.Back && CaretIndex > 0)
                    {
                        var _caretIndex = CaretIndex;
                        if (CaretIndex > 0 && CaretIndex < Text.Length)
                            _caretIndex = _caretIndex - 1;
                        RemoveFromPassword(CaretIndex - 1, 1);
                        CaretIndex = _caretIndex;
                    }
                    e.Handled = true;
                    break;
            }
        }
        #endregion

        #region Private Properties
        private protected SecureString password = new SecureString();
        #endregion

        #region Public Properties
        [Bindable(false)]
        public SecureString Password
        {
            get => password;
            set => InitPassword(value);
        }

        /// <summary>
        /// Задает радиус углов для UIPasswordBox..
        /// </summary>
        /// <remarks>
        /// Sets the corner radius for the UIPasswordBox.
        /// </remarks>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Задает свойство Flat для UIPasswordBox. Если значение true - UIPasswordBox отображается c прозрачным фоном и границей.
        /// </summary>
        /// <remarks>
        /// Sets the Flat property for the UIPasswordBox. If set to true - UIPasswordBox shows with transparent background and border.
        /// </remarks>
        public bool Flat
        {
            get => (bool)GetValue(FlatProperty);
            set => SetValue(FlatProperty, value);
        }

        /// <summary>
        /// Задает подсказку для UIPasswordBox.
        /// </summary>
        /// <remarks>
        /// Sets the Watermark property for the UIPasswordBox..
        /// </remarks>
        public object Watermark
        {
            get => (object)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        /// <summary>
        /// Задает визуальный шаблон подсказки для UIPasswordBox.
        /// </summary>
        /// <remarks>
        /// Sets the Watermark DataTemplate property for the UIPasswordBox..
        /// </remarks>
        public DataTemplate WatermarkDataTemplate
        {
            get => (DataTemplate)GetValue(WatermarkDataTemplateProperty);
            set => SetValue(WatermarkDataTemplateProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(SecureString), typeof(UIPasswordBox), new UIPropertyMetadata(new SecureString()));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIPasswordBox), new UIPropertyMetadata(new CornerRadius(3)));
        public static readonly DependencyProperty FlatProperty = DependencyProperty.Register("Flat", typeof(bool), typeof(UIPasswordBox), new UIPropertyMetadata(false));
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(UIPasswordBox), new UIPropertyMetadata(null));
        public static readonly DependencyProperty WatermarkDataTemplateProperty = DependencyProperty.Register("WatermarkDataTemplate", typeof(DataTemplate), typeof(UIPasswordBox), new UIPropertyMetadata(null));
        #endregion
    }
}
