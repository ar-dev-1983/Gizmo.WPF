using System;
using System.Windows;

namespace Gizmo.WPF
{
    public interface ICorneredControl
    {
        CornerRadius CornerRadius { get; set; }
        Thickness BorderThickness { set; get; }
    }

    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }

    public interface IGroupable
    {
        Guid Id { get; }
        Guid ParentId { get; set; }
        bool IsGroup { get; set; }
    }
}
