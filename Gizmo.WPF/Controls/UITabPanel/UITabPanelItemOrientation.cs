namespace Gizmo.WPF
{
    /// <summary>
    /// Перечисление определяющее положение в пространстве UITabPanelItem относительно контейнера UITabPanel
    /// </summary>
    /// <remarks>
    /// Enumeration defining the position in space of the UITabPanelItem relative to the UITabPanel container 
    /// </remarks>
    public enum UITabPanelItemOrientation
    {
        /// <summary>
        /// Вкладки располагаются сверху контейнера
        /// </summary>
        /// <remarks>
        /// Tabs are positioned on top of the container
        /// </remarks>
        Top,
        /// <summary>
        /// Вкладки располагаются снизу контейнера
        /// </summary>
        /// <remarks>
        /// Tabs are positioned at the bottom of the container
        /// </remarks>
        Bottom,
        /// <summary>
        /// Вкладки располагаются слева от контейнера
        /// </summary>
        /// <remarks>
        /// Tabs are located to the left of the container
        /// </remarks>
        Left,
        /// <summary>
        /// Вкладки располагаются справа от контейнера
        /// </summary>
        /// <remarks>
        /// Tabs are located to the right of the container
        /// </remarks>
        Right
    }
}
