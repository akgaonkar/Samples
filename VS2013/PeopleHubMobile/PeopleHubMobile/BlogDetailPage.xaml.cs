using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PeopleHubMobile.Data;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PeopleHubMobile.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace PeopleHubMobile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlogDetailPage : Page
    {
        public BlogDetailPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }



        private readonly NavigationHelper navigationHelper;
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
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
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string content = (e.Parameter as SampleDataItem).Content;

            // ObservableCollection<SampleDataItem> Items= 
            string[] groups = content.Split('@');
            Title.Text = (e.Parameter as SampleDataItem).Title;
            string paraText;
            int length;
            string uriElement;
            //Add elements
            foreach (var element in groups)
            {

                if (element.Contains("<para>"))
                {
                    //add textblock
                    TextBlock tb = new TextBlock();
                    tb.Margin = new Thickness(0, 15, 0, 0);
                    paraText = element.Remove(0, 6);
                    length = paraText.Length;
                    tb.Text = paraText.Substring(0, length - 7);
                    tb.FontSize = 20;
                    tb.TextWrapping = TextWrapping.Wrap;
                    containerSp.Children.Add(tb);
                }
                else if (element.Contains("<img>"))
                {
                    //add image
                    Image img = new Image();
                    img.Margin = new Thickness(0, 15, 0, 0);
                    uriElement = element.Remove(0, 5);
                    length = uriElement.Length;
                    uriElement = uriElement.Substring(0, length - 6);
                    uriElement = uriElement.Remove(6, 0);
                    var u = new Uri(this.BaseUri, uriElement);
                    BitmapImage bi = new BitmapImage(u);
                    img.Source = bi;
                    containerSp.Children.Add(img);
                }

            }
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
