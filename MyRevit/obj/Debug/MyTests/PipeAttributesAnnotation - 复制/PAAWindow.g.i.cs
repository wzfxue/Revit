﻿#pragma checksum "..\..\..\..\MyTests\PipeAttributesAnnotation - 复制\PAAWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D4C44812F56E338982BF182A6AFEF3F6"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using MyRevit.MyTests.VLBase;
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


namespace MyRevit.MyTests.PAA {
    
    
    /// <summary>
    /// PAAWindow
    /// </summary>
    public partial class PAAWindow : MyRevit.MyTests.VLBase.VLWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 100 "..\..\..\..\MyTests\PipeAttributesAnnotation - 复制\PAAWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Single;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\..\MyTests\PipeAttributesAnnotation - 复制\PAAWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Multiple;
        
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
            System.Uri resourceLocater = new System.Uri("/MyRevit;component/mytests/pipeattributesannotation%20-%20%e5%a4%8d%e5%88%b6/paaw" +
                    "indow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\MyTests\PipeAttributesAnnotation - 复制\PAAWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            this.Btn_Single = ((System.Windows.Controls.Button)(target));
            
            #line 100 "..\..\..\..\MyTests\PipeAttributesAnnotation - 复制\PAAWindow.xaml"
            this.Btn_Single.Click += new System.Windows.RoutedEventHandler(this.Btn_Single_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Btn_Multiple = ((System.Windows.Controls.Button)(target));
            
            #line 101 "..\..\..\..\MyTests\PipeAttributesAnnotation - 复制\PAAWindow.xaml"
            this.Btn_Multiple.Click += new System.Windows.RoutedEventHandler(this.Btn_Multiple_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

