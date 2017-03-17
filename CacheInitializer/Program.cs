using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Qlik.Engine;
using Qlik.Engine.Communication;
using Qlik.Sense.Client;

// Title:       Qlik Sense Cache Initializer 
// Date:        19.08.2016
// Version:     0.11
// Author:      Joe Bickley,Roland Vecera
// Summary:     This tool will "warm" the cache of a Qlik Sense server so that when using large apps the users get good performance right away.  
//              You can use it to load all apps, a single app, and you can get it to just open the app to RAM or cycle through all the objects 
//              so that it will pre calculate expressions so users get rapid performance.
// Credits:     Thanks to Øystein Kolsrud for helping with the Qlik Sense .net SDK steps
//              Uses the commandline.codeplex.com for processing parameters


// Usage:       cacheinitiazer.exe -s https://server.domain.com [-a appname] [-o] [-f fieldname] [-v "value 1,value 2"] [-p virtualproxyprefix]
// Notes:       This projects use the Qlik Sense .net SDK, you must use the right version of the SDK to match the server you are connecting too. 
//              To swap version   simply replace the .net SDK files in the BIN directory of this project, if you dont match them, it wont work!


namespace CacheInitializer
{
    class Program
    {

        static void Main(string[] args)
        {

            //////Setup 
            Options options = new Options();
            Uri serverURL;
            string appname;
            string virtualProxy;

            //// process the parameters using the https://commandline.codeplex.com/           
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                serverURL = new Uri(options.server);
                appname = options.appname;
                virtualProxy = !string.IsNullOrEmpty(options.virtualProxy) ? options.virtualProxy : "" ;
                //TODO need to validate the params ideally
            }
            else
            {
                throw new Exception("Check parameters are correct");
            }


            ////connect to the server (using windows credentials
            QlikConnection.Timeout = Int32.MaxValue;
            var d = DateTime.Now;
            ILocation remoteQlikSenseLocation = Qlik.Engine.Location.FromUri(serverURL);
            if (virtualProxy.Length > 0)
            {
                remoteQlikSenseLocation.VirtualProxyPath = virtualProxy;
            }
            bool isHTTPs = (serverURL.Scheme == Uri.UriSchemeHttps);
            remoteQlikSenseLocation.AsNtlmUserViaProxy(isHTTPs);
            bool createSearchIndex = options.createsearchindex;


            ////Start to cache the apps
            if (appname != null)
            {
                //Open up and cache one app
                IAppIdentifier appidentifier = remoteQlikSenseLocation.AppWithNameOrDefault(appname);

                LoadCache(remoteQlikSenseLocation, appidentifier, createSearchIndex);
            }
            else
            {
                //Get all apps, open them up and cache them
                remoteQlikSenseLocation.GetAppIdentifiers().ToList().ForEach(id => LoadCache(
                    remoteQlikSenseLocation, id, createSearchIndex));
            }

            ////Wrap it up
            var dt = DateTime.Now - d;
            Print("Cache initialization complete. Total time: {0}", dt.ToString());


        }

        static void LoadCache(ILocation location, IAppIdentifier id,
                              bool createSearchIndex)
        {
            //open up the app
            Print("{0}: Opening app", id.AppName);
            IApp app = location.App(id);
            Print("{0}: App open", id.AppName);

            if (createSearchIndex)
            {
                cacheSearchIndex(app);
            }

            Print("{0}: App cache completed", id.AppName);

        }

        static void cacheSearchIndex(IApp app)
        {
            Print("{0}: Search indexing started");
            List<string> searchTerms = new List<string>();
            searchTerms.Add("mydummysearch");
            app.SearchSuggest(new SearchCombinationOptions(), searchTerms);
            Print("{0}: Search indexing completed");
        }

        private static void Print(string txt)
        {
            Console.WriteLine("{0} - {1}", DateTime.Now.ToString("hh:mm:ss"), txt);
        }

        private static void Print(string txt, params object[] os)
        {
            Print(String.Format(txt, os));
        }
    }

}
