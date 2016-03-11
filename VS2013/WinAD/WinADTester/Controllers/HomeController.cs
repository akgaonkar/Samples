using System;
using System.Collections.Generic;
using System.Net;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Security.Permissions;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices.AccountManagement;

namespace WinADTester.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string test = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;
            UserPrincipal userData = null;
            
            try
            {
                string ldapServer = "LDAP://mindtree.com/";

                LdapConnection ldapConnection = new LdapConnection(ldapServer);
                ldapConnection.Credential = CredentialCache.DefaultNetworkCredentials;
                    //new NetworkCredential("m100XXXX", "******", "mindtree");

                var res = new List<UserPrincipal>();
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "mindtree"))
                {
                    // Gets the specified user
                    UserPrincipal currentUser = UserPrincipal.FindByIdentity(ctx, test);
                    if (currentUser!=null && currentUser.Enabled.Value)
                    {
                        userData = currentUser;
                        ViewBag.UserDetails = userData.GetUnderlyingObject() as DirectoryEntry; 
                    }


                    // Loops through all users in AD
                    //using (UserPrincipal qbeUser = new UserPrincipal(ctx))
                    //{
                    // Need to use DirectorySearcher as the below syntax is not being supported; and also propertywise search is not supported in the new PrincipalSearcher
                    //    //qbeUser.AdvancedSearchFilter = "(&(division=C5)(description=Core Solutions Group))";


                    //    using (PrincipalSearcher srch = new PrincipalSearcher(qbeUser))
                    //    {

                    //        foreach (var found in srch.FindAll())
                    //        {
                    //            var user = found as UserPrincipal;
                    //            Console.WriteLine(user.GivenName + " " + user.Surname + " " + user.EmailAddress);
                    //            var groups = user.GetAuthorizationGroups();

                    //            res.Add(user);
                    //        }
                    //    }
                    //}
                }

                int resultsCount  = res.Count;
                ViewBag.SampleData = res;
            }
            catch (Exception ex)
            {
                int x = 0;
                x = 10;
                throw ex;
            }
            ViewBag.Message = "test";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetImage(string mid)
        {
            try
            {
                string ldapServer = "LDAP://mindtree.com/";
                LdapConnection ldapConnection = new LdapConnection(ldapServer);
                ldapConnection.Credential = CredentialCache.DefaultNetworkCredentials;
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "mindtree"))
                {
                    UserPrincipal currentUser = UserPrincipal.FindByIdentity(ctx, mid);
                    if (currentUser != null && currentUser.Enabled.Value)
                    {
                        byte[] thumbnailPhoto = (byte[])(currentUser.GetUnderlyingObject() as DirectoryEntry).Properties["thumbnailPhoto"].Value;

                        System.Drawing.Image thumbnailImage;
                        using (var ms = new System.IO.MemoryStream(thumbnailPhoto))
                        {
                            thumbnailImage = System.Drawing.Image.FromStream(ms);
                            using (var streak = new System.IO.MemoryStream())
                            {
                                thumbnailImage.Save(streak, System.Drawing.Imaging.ImageFormat.Jpeg);
                                return File(streak.ToArray(), "image/jpg");
                            }
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}