﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Gizmo.WPF
{
    public static class VisualHelper
    {
        //this static method is for most common purpuses
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            return parentObject == null ? (T)null : parentObject is T parent ? parent : FindParent<T>(parentObject);
        }

        //this static method is for items placed in pupups, since child items paced in somewere in popup have Parent Property is Null, not the actual parent;
        //still this method does not give a 100% guarantee that we will still find an Parent of the desired type, but in 99% of cases this method works.
        public static T FindVisulaParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            return parentObject switch
            {
                //is parentObject is null, then we have reached the root element of the popup, which means we need to start a new search cycle already using the Parent property of Pupup;
                //if the Parent property is null, then alas - we return null.
                null => child is FrameworkElement ? (child as FrameworkElement).Parent != null ? FindVisulaParent<T>((child as FrameworkElement).Parent) : null : null,
                //we found an Parent of the desired type - we return it.
                T _ => parentObject as T,
                //we have not yet reached the Parent of the desired type, so we continue to search.
                _ => FindVisulaParent<T>(parentObject)
            };
        }

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

        public static PngBitmapEncoder SnapShotPNG(this UIElement visualSource)
        {
            double actualHeight = visualSource.RenderSize.Height;
            double actualWidth = visualSource.RenderSize.Width;

            RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)actualWidth, (int)actualHeight, 96, 96, PixelFormats.Pbgra32);
            VisualBrush sourceBrush = new VisualBrush(visualSource);

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            using (drawingContext)
            {
                drawingContext.DrawRectangle(sourceBrush, null, new Rect(new Point(0, 0), new Point(actualWidth, actualHeight)));
            }
            renderTarget.Render(drawingVisual);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTarget));
            return encoder;
        }
    }
}