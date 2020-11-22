namespace Gizmo.WPF
{
    /// <summary>
    /// Перечисление, определяющее режим выбора элементов для UIEnumSwitch
    /// </summary>
    /// <remarks>
    /// Enumeration that specifies selection mode for UIEnumSwitch
    /// </remarks>
    public enum SelectionModeEnum
    {
        /// <summary>
        /// Режим в котором может быть выбран только один элемент одновременно.
        /// </summary>
        /// <remarks>
        /// Mode in which only one item can be selected at a time.
        /// </remarks>
        Single,
        /// <summary>
        /// Режим в котором может быть выбрано несколько элементов одновременно либо элемент по умолчанию, если ни один  из элементов не выбран - автоматически выбирается элемент по умолчанию (первый в списке).
        /// </summary>
        /// <remarks>
        /// The mode in which several items can be selected at the same time or the default item, if none of the items is selected - the default item is automatically selected (the first in the list).
        /// </remarks>
        MultipleWhithDefault,
        /// <summary>
        /// Режим в котором может быть выбрано несколько элементов одновременно.
        /// </summary>
        /// <remarks>
        /// The mode in which several items can be selected at the same time.
        /// </remarks>
        Multiple
    }
}
