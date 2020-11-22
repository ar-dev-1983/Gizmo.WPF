namespace Gizmo.WPF
{
    /// <summary>
    /// Перечисление, указывающее как именно действовать при выборе элемента UIEnumSwitchItem
    /// </summary>
    /// <remarks>
    /// An enumeration specifying how to act when a UIEnumSwitchItem is selected
    /// </remarks>
    internal enum SelectionAction
    {
        /// <summary>
        /// Действие, в котором IsSelected впервые присваивается значение true
        /// </summary>
        /// <remarks>
        /// Action in which IsSelected is first set to true
        /// </remarks>
        FirstSelection,
        /// <summary>
        /// Действие, в котором IsSelected присваивается значение true
        /// </summary>
        /// <remarks>
        /// Action in which IsSelected is set to true
        /// </remarks>
        Selection
    }
}
