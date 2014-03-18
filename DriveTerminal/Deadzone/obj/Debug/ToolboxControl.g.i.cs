﻿#pragma checksum "..\..\ToolboxControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BFD70F0966A9F8D751BC3DC8464C784C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Deadzone {
    
    
    /// <summary>
    /// ToolboxControl
    /// </summary>
    public partial class ToolboxControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\ToolboxControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider minSlider;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\ToolboxControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider maxSlider;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\ToolboxControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label minValue;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\ToolboxControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label maxValue;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\ToolboxControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sensitivitySlider;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\ToolboxControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label sensitivityValue;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Deadzone;component/toolboxcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ToolboxControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.minSlider = ((System.Windows.Controls.Slider)(target));
            
            #line 12 "..\..\ToolboxControl.xaml"
            this.minSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.Slider_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.maxSlider = ((System.Windows.Controls.Slider)(target));
            
            #line 13 "..\..\ToolboxControl.xaml"
            this.maxSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.maxSlider_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.minValue = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.maxValue = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.sensitivitySlider = ((System.Windows.Controls.Slider)(target));
            
            #line 17 "..\..\ToolboxControl.xaml"
            this.sensitivitySlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.sensitivitySlider_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.sensitivityValue = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

