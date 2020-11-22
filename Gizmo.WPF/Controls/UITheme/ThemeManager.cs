using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.WPF
{
    public static class ThemeManager
    {
        private static ResourceDictionary _BlueDark;
        public static ResourceDictionary BlueDark
        {
            get
            {
                if (_BlueDark == null)
                {
                    _BlueDark = new ResourceDictionary
                    {
                        Source = new Uri("/Gizmo.WPF;Component/Themes/Brushes.BlueDark.xaml", UriKind.Relative)
                    };
                }
                return _BlueDark;
            }
        }
        private static ResourceDictionary _BlueLight;
        public static ResourceDictionary BlueLight
        {
            get
            {
                if (_BlueLight == null)
                {
                    _BlueLight = new ResourceDictionary
                    {
                        Source = new Uri("/Gizmo.WPF;Component/Themes/Brushes.BlueLight.xaml", UriKind.Relative)
                    };
                }
                return _BlueLight;
            }
        }
        private static ResourceDictionary _PurpleDark;
        public static ResourceDictionary PurpleDark
        {
            get
            {
                if (_PurpleDark == null)
                {
                    _PurpleDark = new ResourceDictionary
                    {
                        Source = new Uri("/Gizmo.WPF;Component/Themes/Brushes.PurpleDark.xaml", UriKind.Relative)
                    };
                }
                return _PurpleDark;
            }
        }
        private static ResourceDictionary _PurpleLight;
        public static ResourceDictionary PurpleLight
        {
            get
            {
                if (_PurpleLight == null)
                {
                    _PurpleLight = new ResourceDictionary
                    {
                        Source = new Uri("/Gizmo.WPF;Component/Themes/Brushes.PurpleLight.xaml", UriKind.Relative)
                    };
                }
                return _PurpleLight;
            }
        }
        public static void ApplyTheme(UIThemeEnum _theme)
        {
            if (Application.Current.Resources.MergedDictionaries != null)
            {
                var MergedDictionaries = Application.Current.Resources.MergedDictionaries;
                MergedDictionaries.Remove(BlueDark);
                MergedDictionaries.Remove(BlueLight);
                MergedDictionaries.Remove(PurpleDark);
                MergedDictionaries.Remove(PurpleLight);
                switch (_theme)
                {
                    case UIThemeEnum.BlueDark:
                        MergedDictionaries.Add(BlueDark);
                        break;
                    case UIThemeEnum.BlueLight:
                        MergedDictionaries.Add(BlueLight);
                        break;
                    case UIThemeEnum.PurpleDark:
                        MergedDictionaries.Add(PurpleDark);
                        break;
                    case UIThemeEnum.PurpleLight:
                        MergedDictionaries.Add(PurpleLight);
                        break;
                }
            }
        }

        public static void ApplyThemeToContentControl(ContentControl control, UIThemeEnum _theme)
        {
            if (control.Resources.MergedDictionaries != null)
            {
                var MergedDictionaries = control.Resources.MergedDictionaries;
                MergedDictionaries.Remove(BlueDark);
                MergedDictionaries.Remove(BlueLight);
                MergedDictionaries.Remove(PurpleDark);
                MergedDictionaries.Remove(PurpleLight);
                switch (_theme)
                {
                    case UIThemeEnum.BlueDark:
                        MergedDictionaries.Add(BlueDark);
                        break;
                    case UIThemeEnum.BlueLight:
                        MergedDictionaries.Add(BlueLight);
                        break;
                    case UIThemeEnum.PurpleDark:
                        MergedDictionaries.Add(PurpleDark);
                        break;
                    case UIThemeEnum.PurpleLight:
                        MergedDictionaries.Add(PurpleLight);
                        break;
                }
            }
        }

        public static void ApplyThemeToHeaderedContentControl(HeaderedContentControl control, UIThemeEnum _theme)
        {
            if (control.Resources.MergedDictionaries != null)
            {
                var MergedDictionaries = control.Resources.MergedDictionaries;
                MergedDictionaries.Remove(BlueDark);
                MergedDictionaries.Remove(BlueLight);
                MergedDictionaries.Remove(PurpleDark);
                MergedDictionaries.Remove(PurpleLight);
                switch (_theme)
                {
                    case UIThemeEnum.BlueDark:
                        MergedDictionaries.Add(BlueDark);
                        break;
                    case UIThemeEnum.BlueLight:
                        MergedDictionaries.Add(BlueLight);
                        break;
                    case UIThemeEnum.PurpleDark:
                        MergedDictionaries.Add(PurpleDark);
                        break;
                    case UIThemeEnum.PurpleLight:
                        MergedDictionaries.Add(PurpleLight);
                        break;
                }
            }
        }

        public static void ApplyThemeToItemsControl(ItemsControl control, UIThemeEnum _theme)
        {
            if (control.Resources.MergedDictionaries != null)
            {
                var MergedDictionaries = control.Resources.MergedDictionaries;
                MergedDictionaries.Remove(BlueDark);
                MergedDictionaries.Remove(BlueLight);
                MergedDictionaries.Remove(PurpleDark);
                MergedDictionaries.Remove(PurpleLight);
                switch (_theme)
                {
                    case UIThemeEnum.BlueDark:
                        MergedDictionaries.Add(BlueDark);
                        break;
                    case UIThemeEnum.BlueLight:
                        MergedDictionaries.Add(BlueLight);
                        break;
                    case UIThemeEnum.PurpleDark:
                        MergedDictionaries.Add(PurpleDark);
                        break;
                    case UIThemeEnum.PurpleLight:
                        MergedDictionaries.Add(PurpleLight);
                        break;
                }
            }
        }

        public static void ApplyThemeToHeaderedItemsControl(HeaderedItemsControl control, UIThemeEnum _theme)
        {
            if (control.Resources.MergedDictionaries != null)
            {
                var MergedDictionaries = control.Resources.MergedDictionaries;
                MergedDictionaries.Remove(BlueDark);
                MergedDictionaries.Remove(BlueLight);
                MergedDictionaries.Remove(PurpleDark);
                MergedDictionaries.Remove(PurpleLight);
                switch (_theme)
                {
                    case UIThemeEnum.BlueDark:
                        MergedDictionaries.Add(BlueDark);
                        break;
                    case UIThemeEnum.BlueLight:
                        MergedDictionaries.Add(BlueLight);
                        break;
                    case UIThemeEnum.PurpleDark:
                        MergedDictionaries.Add(PurpleDark);
                        break;
                    case UIThemeEnum.PurpleLight:
                        MergedDictionaries.Add(PurpleLight);
                        break;
                }
            }
        }

        public static void ApplyThemeToTabControl(TabControl control, UIThemeEnum _theme)
        {
            if (control.Resources.MergedDictionaries != null)
            {
                var MergedDictionaries = control.Resources.MergedDictionaries;
                MergedDictionaries.Remove(BlueDark);
                MergedDictionaries.Remove(BlueLight);
                MergedDictionaries.Remove(PurpleDark);
                MergedDictionaries.Remove(PurpleLight);
                switch (_theme)
                {
                    case UIThemeEnum.BlueDark:
                        MergedDictionaries.Add(BlueDark);
                        break;
                    case UIThemeEnum.BlueLight:
                        MergedDictionaries.Add(BlueLight);
                        break;
                    case UIThemeEnum.PurpleDark:
                        MergedDictionaries.Add(PurpleDark);
                        break;
                    case UIThemeEnum.PurpleLight:
                        MergedDictionaries.Add(PurpleLight);
                        break;
                }
            }
        }

        public static void ApplyThemeToWindow(Window control, UIThemeEnum _theme)
        {
            if (control.Resources.MergedDictionaries != null)
            {
                var MergedDictionaries = control.Resources.MergedDictionaries;
                MergedDictionaries.Remove(BlueDark);
                MergedDictionaries.Remove(BlueLight);
                MergedDictionaries.Remove(PurpleDark);
                MergedDictionaries.Remove(PurpleLight);
                switch (_theme)
                {
                    case UIThemeEnum.BlueDark:
                        MergedDictionaries.Add(BlueDark);
                        break;
                    case UIThemeEnum.BlueLight:
                        MergedDictionaries.Add(BlueLight);
                        break;
                    case UIThemeEnum.PurpleDark:
                        MergedDictionaries.Add(PurpleDark);
                        break;
                    case UIThemeEnum.PurpleLight:
                        MergedDictionaries.Add(PurpleLight);
                        break;
                }
            }
        }

        public static object GetResource(UIThemeEnum _theme, string _resourceName)
        {
            object result = null;

            try
            {
                switch (_theme)
                {
                    case UIThemeEnum.BlueDark:
                        result = BlueDark[(from node in BlueDark.Keys.OfType<ComponentResourceKey>() where node.ResourceId.ToString() == _resourceName select node).First()];
                        break;
                    case UIThemeEnum.BlueLight:
                        result = BlueLight[(from node in BlueLight.Keys.OfType<ComponentResourceKey>() where node.ResourceId.ToString() == _resourceName select node).First()];
                        break;
                    case UIThemeEnum.PurpleDark:
                        result = PurpleDark[(from node in PurpleDark.Keys.OfType<ComponentResourceKey>() where node.ResourceId.ToString() == _resourceName select node).First()];
                        break;
                    case UIThemeEnum.PurpleLight:
                        result = PurpleLight[(from node in PurpleLight.Keys.OfType<ComponentResourceKey>() where node.ResourceId.ToString() == _resourceName select node).First()];
                        break;
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}
