﻿

#pragma checksum "C:\Users\Eduardo\Documents\win8\app\Win8Catalogo\Win8Catalogo\ItemPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0199CEAEE7F33A35FD9BE44013ED0B77"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Win8Catalogo
{
    partial class ItemPage : global::Win8Catalogo.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 19 "..\..\ItemPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnHome_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 214 "..\..\ItemPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 162 "..\..\ItemPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.StartLayoutUpdates;
                 #line default
                 #line hidden
                #line 162 "..\..\ItemPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Unloaded += this.StopLayoutUpdates;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 81 "..\..\ItemPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.StartLayoutUpdates;
                 #line default
                 #line hidden
                #line 81 "..\..\ItemPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Unloaded += this.StopLayoutUpdates;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


