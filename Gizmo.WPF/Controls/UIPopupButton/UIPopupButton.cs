using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Gizmo.WPF
{
    public static class PopupHelper
    {
        public static bool IsLogicalAncestorOf(this UIElement ancestor, UIElement child)
        {
            if (child != null)
            {
                FrameworkElement obj = child as FrameworkElement;
                while (obj != null)
                {
                    FrameworkElement parent = VisualTreeHelper.GetParent(obj) as FrameworkElement;
                    obj = parent == null ? obj.Parent as FrameworkElement : parent as FrameworkElement;
                    if (obj == ancestor) return true;
                }
            }
            return false;
        }
    }

    [TemplatePart(Name = partPopup)]
    [ContentProperty("Items")]
    public class UIPopupButton : MenuItem, ICorneredControl, ICommandSource
    {
        #region Events
        public static readonly RoutedEvent PopupOpenedEvent = EventManager.RegisterRoutedEvent("PopupOpened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UIPopupButton));
        public static readonly RoutedEvent PopupClosedEvent = EventManager.RegisterRoutedEvent("PopupClosed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UIPopupButton));

        public event RoutedEventHandler PopupOpened
        {
            add { AddHandler(PopupOpenedEvent, value); }
            remove { RemoveHandler(PopupOpenedEvent, value); }
        }
        public event RoutedEventHandler PopupClosed
        {
            add { AddHandler(PopupClosedEvent, value); }
            remove { RemoveHandler(PopupClosedEvent, value); }
        }

        protected virtual void OnPopupOpened(object sender, EventArgs e)
        {
            IsDropDownPressed = true;
            RoutedEventArgs args = new RoutedEventArgs(UIPopupButton.PopupOpenedEvent);
            RaiseEvent(args);
        }
        protected virtual void OnPopupClosed(object sender, EventArgs e)
        {
            IsDropDownPressed = false;
            RoutedEventArgs args = new RoutedEventArgs(UIPopupButton.PopupClosedEvent);
            RaiseEvent(args);
        }
        #endregion

        const string partPopup = "PART_Popup";
        protected Popup popup;
        internal Popup Popup => popup;

        static UIPopupButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIPopupButton), new FrameworkPropertyMetadata(typeof(UIPopupButton)));
        }

        public UIPopupButton()
        {
            AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItemClicked));
        }

        private void MenuItemClicked(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource == this) return;
            IsDropDownPressed = false;
        }

        public override void OnApplyTemplate()
        {
            if (popup != null)
            {
                popup.Closed -= OnPopupClosed;
                popup.Opened -= OnPopupOpened;
            }
            popup = GetTemplateChild(partPopup) as Popup;
            if (popup != null)
            {
                popup.Closed += new EventHandler(OnPopupClosed);
                popup.Opened += new EventHandler(OnPopupOpened);
            }

            base.OnApplyTemplate();
        }

        public object Content
        {
            get => (object)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public bool IsDropDownPressed
        {
            get => (bool)GetValue(IsDropDownPressedProperty);
            set => SetValue(IsDropDownPressedProperty, value);
        }
        public bool Flat
        {
            get => (bool)GetValue(FlatProperty);
            set => SetValue(FlatProperty, value);
        }
        public bool ShowDropDownSymbol
        {
            get => (bool)GetValue(ShowDropDownSymbolProperty);
            set => SetValue(ShowDropDownSymbolProperty, value);
        }
        public double MaxDropDownHeight
        {
            get => (double)GetValue(MaxDropDownHeightProperty);
            set => SetValue(MaxDropDownHeightProperty, value);
        }
        public PopupAnimation PopupAnimation
        {
            get => (PopupAnimation)GetValue(PopupAnimationProperty);
            set => SetValue(PopupAnimationProperty, value);
        }
        public PlacementMode PopupPlacement
        {
            get => (PlacementMode)GetValue(PopupPlacementProperty);
            set => SetValue(PopupPlacementProperty, value);
        }
        public double DropDownWidth
        {
            get => (double)GetValue(DropDownWidthProperty);
            set => SetValue(DropDownWidthProperty, value);
        }
        public object PopupHeader
        {
            get => (object)GetValue(PopupHeaderProperty);
            set => SetValue(PopupHeaderProperty, value);
        }
        public DataTemplate PopupHeaderTemplate
        {
            get => (DataTemplate)GetValue(PopupHeaderTemplateProperty);
            set => SetValue(PopupHeaderTemplateProperty, value);
        }
        public object PopupFooter
        {
            get => (object)GetValue(PopupFooterProperty);
            set => SetValue(PopupFooterProperty, value);
        }
        public DataTemplate PopupFooterTemplate
        {
            get => (DataTemplate)GetValue(PopupFooterTemplateProperty);
            set => SetValue(PopupFooterTemplateProperty, value);
        }
        public double VerticalOffset
        {
            get => (double)GetValue(VerticalOffsetProperty);
            set => SetValue(VerticalOffsetProperty, value);
        }
        public double HorizontalOffset
        {
            get => (double)GetValue(HorizontalOffsetProperty);
            set => SetValue(HorizontalOffsetProperty, value);
        }

        public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register("MaxDropDownHeight", typeof(double), typeof(UIPopupButton), new UIPropertyMetadata(double.NaN));
        public static readonly DependencyProperty ShowDropDownSymbolProperty = DependencyProperty.Register("ShowDropDownSymbol", typeof(bool), typeof(UIPopupButton), new UIPropertyMetadata(true));
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(UIPopupButton), new UIPropertyMetadata(null));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIPopupButton), new UIPropertyMetadata(new CornerRadius(3)));
        public static readonly DependencyProperty IsDropDownPressedProperty = DependencyProperty.Register("IsDropDownPressed", typeof(bool), typeof(UIPopupButton), new UIPropertyMetadata(false, IsDropDownPressedPropertyChangedCallback));
        public static readonly DependencyProperty FlatProperty = DependencyProperty.Register("Flat", typeof(bool), typeof(UIPopupButton), new UIPropertyMetadata(true));
        public static readonly DependencyProperty PopupFooterTemplateProperty = DependencyProperty.Register("PopupFooterTemplate", typeof(DataTemplate), typeof(UIPopupButton), new UIPropertyMetadata(null));
        public static readonly DependencyProperty PopupFooterProperty = DependencyProperty.Register("PopupFooter", typeof(object), typeof(UIPopupButton), new UIPropertyMetadata(null));
        public static readonly DependencyProperty PopupHeaderTemplateProperty = DependencyProperty.Register("PopupHeaderTemplate", typeof(DataTemplate), typeof(UIPopupButton), new UIPropertyMetadata(null));
        public static readonly DependencyProperty PopupHeaderProperty = DependencyProperty.Register("PopupHeader", typeof(object), typeof(UIPopupButton), new UIPropertyMetadata(null));
        public static readonly DependencyProperty DropDownWidthProperty = DependencyProperty.Register("DropDownWidth", typeof(double), typeof(UIPopupButton), new UIPropertyMetadata(double.NaN));
        public static readonly DependencyProperty PopupPlacementProperty = DependencyProperty.Register("PopupPlacement", typeof(PlacementMode), typeof(UIPopupButton), new UIPropertyMetadata(PlacementMode.Bottom));
        public static readonly DependencyProperty PopupAnimationProperty = DependencyProperty.Register("PopupAnimation", typeof(PopupAnimation), typeof(UIPopupButton), new UIPropertyMetadata(PopupAnimation.Slide));
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(UIPopupButton));
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(UIPopupButton));

        static void IsDropDownPressedPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIPopupButton btn = d as UIPopupButton;
            btn.OnDropDownPressedChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        protected virtual void OnDropDownPressedChanged(bool oldValue, bool newValue)
        {
            if (popup != null)
            {
                if (newValue)
                {
                    if (newValue)
                        Mouse.Capture(this, CaptureMode.SubTree);

                    popup.PlacementTarget = this;
                    CloseOpenedPopup(this);
                }
                else
                {

                    CloseOpenedPopup(null);
                    Mouse.Capture(null);
                }
                popup.IsOpen = newValue;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Enter || e.Key == Key.Space) { IsDropDownPressed = true; e.Handled = true; }
            if (e.Key == Key.Escape) { IsDropDownPressed = false; e.Handled = true; }
        }

        private static UIPopupButton DroppedDownButton;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            try
            {
                EnsurePopupRemainsOnMouseUp();
                if (this.IsAncestorOf(e.OriginalSource as DependencyObject))
                {
                    ToggleDropDownState();
                    e.Handled = true;
                }
                base.OnMouseLeftButtonDown(e);
            }
            catch
            {
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            e.Handled = true;
            EnsurePopupDoesNotStayOpen();
            base.OnMouseLeftButtonUp(e);
        }

        protected virtual void ToggleDropDownState()
        {
            IsDropDownPressed ^= true;
        }

        public static void CloseOpenedPopup(UIPopupButton caller)
        {
            UIPopupButton btn = DroppedDownButton;
            if (btn != null && (btn != caller))
            {
                if (caller != null)
                {
                    FrameworkElement parent = caller.Parent as FrameworkElement;
                    while (parent != null)
                    {
                        if (parent == btn) return;
                        FrameworkElement previous = parent;
                        parent = parent.Parent as FrameworkElement;
                        if (parent == null)
                        {
                            parent = previous.TemplatedParent as FrameworkElement;
                        }
                    }

                    if (btn.popup != null)
                        btn.popup.IsOpen = false;
                    btn.IsDropDownPressed = false;
                }
            }
            DroppedDownButton = caller;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is MenuItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MenuItem();
        }

        protected void EnsurePopupRemainsOnMouseUp()
        {
            if (popup != null) popup.StaysOpen = true;
        }

        protected void EnsurePopupDoesNotStayOpen()
        {
            if (popup != null)
            {
                popup.StaysOpen = false;
            }
        }

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);
            if (IsDropDownPressed)
            {
                FrameworkElement fe = e.OriginalSource as FrameworkElement;
                FrameworkElement captured = Mouse.Captured as FrameworkElement;
                if (captured != this && Popup != null)
                {
                    UIElement child = Popup.Child;
                    if (e.OriginalSource == this)
                    {
                        if ((Mouse.Captured == null) || !child.IsLogicalAncestorOf(Mouse.Captured as UIElement))
                        {
                            IsDropDownPressed = false;
                            e.Handled = true;
                        }
                    }
                    else if (child.IsAncestorOf(e.OriginalSource as DependencyObject))
                    {
                        if (this.IsDropDownPressed && (Mouse.Captured == null))
                        {
                            Mouse.Capture(this, CaptureMode.SubTree);
                            e.Handled = true;
                        }
                    }
                    else if (!this.IsLogicalAncestorOf(Mouse.Captured as UIElement))
                    {
                        IsDropDownPressed = false;
                    }
                }
            }
        }
    }

}
