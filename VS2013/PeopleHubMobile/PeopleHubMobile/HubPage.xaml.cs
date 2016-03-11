using PeopleHubMobile.Common;
using PeopleHubMobile.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Hub Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace PeopleHubMobile
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");
        private static int isLoggedIn = 0;
        private static int visits = 0;

        public HubPage()
        {
            this.InitializeComponent();

            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var sampleDataGroups = await SampleDataSource.GetGroupsAsync();
            this.DefaultViewModel["Groups"] = sampleDataGroups;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        /// <summary>
        /// Shows the details of a clicked group in the <see cref="SectionPage"/>.
        /// </summary>
        private void GroupSection_ItemClick(object sender, ItemClickEventArgs e)
        {
            var groupId = ((SampleDataGroup)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(SectionPage), groupId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            //var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            //if (!Frame.Navigate(typeof(ItemPage), itemId))
            var url = ((SampleDataItem)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(ItemPage), url))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private void ItemView1_ItemClick(object sender, ItemClickEventArgs e)
        {
            var url = ((SampleDataItem)e.ClickedItem);
            if (!Frame.Navigate(typeof(BlogDetailPage), url))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private async void ItemView2_ItemClick(object sender, ItemClickEventArgs e)
        {
            var url = ((SampleDataItem)e.ClickedItem);
            IEnumerable<SampleDataGroup> groupsInfo = await SampleDataSource.GetGroupsAsync();

            if (url.Content.Equals("Group-6"))
            {
                if (isLoggedIn == 0)
                {
                    Frame.Navigate(typeof(LoginPage));
                }
                else if (isLoggedIn == 1)
                {
                    isLoggedIn = 0;
                    Frame.Navigate(typeof(HubPage), isLoggedIn);
                }
            }

            else
            {
                foreach (var grp in groupsInfo)
                {
                    if (grp.UniqueId.Equals(url.Content))
                    {
                        hub4.DataContext = grp;
                        break;
                    }
                }
                Hub.ScrollToSection(hub4);
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
        /// <param name="e">Event data that describes how this page was reached.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (e.Parameter != null && e.Parameter != String.Empty)
            {
                isLoggedIn = int.Parse(e.Parameter.ToString());
                if (isLoggedIn == 1)
                {
                    
                    hub1.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    hub2.Visibility = Windows.UI.Xaml.Visibility.Visible;

                    IEnumerable<SampleDataGroup> groupsInfo = await SampleDataSource.GetGroupsAsync();
                    foreach (var grp in groupsInfo)
                    {
                        if (grp.UniqueId.Equals("Group-6"))
                        {
                            grp.Items.RemoveAt(3);
                            grp.Items.Insert(3, new SampleDataItem("Group-6-Item-4", "Logout", "Item Subtitle: 4", "Assets/DarkGray.png", "Item Description: ", "Group-6"));

                            if (visits == 0)
                            {
                                visits++;
                                Hub.ScrollToSection(hub1);
                            }
                            break;
                        }
                    }
                }

            }
            if (e.Parameter == null || isLoggedIn == 0)
            {
                hub1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                hub2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                IEnumerable<SampleDataGroup> groupsInfo = await SampleDataSource.GetGroupsAsync();
                foreach (var grp in groupsInfo)
                {
                    if (grp.UniqueId.Equals("Group-6"))
                    {
                        grp.Items.RemoveAt(3);
                        grp.Items.Insert(3, new SampleDataItem("Group-6-Item-4", "Employee Login", "Item Subtitle: 4", "Assets/DarkGray.png", "Item Description: ", "Group-6"));
                        break;
                    }
                }
                Frame.BackStack.Clear();
                Hub.ScrollToSection(hub4);
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
