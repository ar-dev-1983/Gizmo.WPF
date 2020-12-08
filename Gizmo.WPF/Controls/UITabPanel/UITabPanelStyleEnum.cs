namespace Gizmo.WPF
{
    /// <summary>
    /// Перечисление определяющее внешний вид UITabPanelItem
    /// </summary>
    /// <remarks>
    /// An enumeration that determines the appearance of UITabPanelItem
    /// </remarks>
    public enum UITabPanelStyleEnum
    {
        /// <summary>
        /// Значение по умолчанию для внешнего вида UITabPanelItem
        /// </summary>
        /// <remarks>
        /// Default UITabPanelItem appearance
        /// </remarks>
        Tabs,
        /// <summary>
        /// Вкладки со стилями по умолчанию перекрывают друг друга
        /// </summary>
        /// <remarks>
        /// Default styled tabs overlap each other
        /// </remarks>
        CompactTabs,
        /// <summary>
        /// Вкладки с упрощенным стилем
        /// </summary>
        /// <remarks>
        /// Simplified style tabs
        /// </remarks>
        SimpleTabs
    }
}
