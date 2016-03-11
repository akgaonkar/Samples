using PeopleHubMobile.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace PeopleHubMobile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
        }


        private readonly NavigationHelper navigationHelper;
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
 
       private async void Button_Click(object sender, RoutedEventArgs e)
        { int isLoggedIn=0;
            if(UserName.Text=="Admin" && Password.Password=="Welcome123")
            {
                isLoggedIn=1;
                this.Frame.Navigate(typeof(HubPage),isLoggedIn);
            }
            else
            {
                MessageDialog dialog = new MessageDialog("Invalid username or password");
                await dialog.ShowAsync();
            }   
        }


       #region NavigationHelper registration

       /// <summary>
       /// The methods provided in this section are simply used to allow
       /// NavigationHelper to respond to the page's navigation methods.
       /// <para>
       /// Page specific logic should be placed in event handlers for the
       /// <see cref="NavigationHelper.LoadState"/>
       /// and <see cref="NavigationHelper.SaveState"/>.
       /// The navigation parameter is available in the LoadState method
       /// in addition to page state preserved during an earlier session.
       /// </para>
       /// </summary>
       /// <param name="e">Provides data for navigation methods and event
       /// handlers that cannot cancel the navigation request.</param>
       protected override void OnNavigatedTo(NavigationEventArgs e)
       {
           this.navigationHelper.OnNavigatedTo(e);
       }

       protected override void OnNavigatedFrom(NavigationEventArgs e)
       {
           this.navigationHelper.OnNavigatedFrom(e);
       }

       #endregion
    }
}
