﻿

#pragma checksum "C:\Users\m1025951\documents\visual studio 2013\Projects\PeopleHubMobile\PeopleHubMobile\HubPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "20630626AC3B0BC575471C4B2877CC4D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PeopleHubMobile
{
    partial class HubPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 354 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ItemView2_ItemClick;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 315 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ItemView1_ItemClick;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 101 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ItemView_ItemClick;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


