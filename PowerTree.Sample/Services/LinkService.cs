
using PowerTree.Sample.Interfaces;
using PowerTree.Sample.Models;
using System;
using System.Diagnostics;
using System.Net.Http.Headers;


namespace PowerTree.Sample.Services
{

    public class LinkService: ILinkService
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Non Async Methods
        public void DeleteLink(int linkId)
        {
            _unitOfWork.Links.Remove(new Link { LinkId = linkId });
            _unitOfWork.Complete();
        }



        public LinkIcon CreateLinkIcon(LinkIcon linkIcon)
        {
            _unitOfWork.LinkIcons.Add(linkIcon);
            _unitOfWork.Complete();
            _unitOfWork.ClearTracking();

            return linkIcon;

        }

        public void SaveLink(Link link)
        {
            _unitOfWork.Links.Update(link);
            _unitOfWork.Complete();
            _unitOfWork.ClearTracking();

        }
        public Link GetLink(int linkId)
        {
            var l = _unitOfWork.Links.Find(x => x.LinkId == linkId).FirstOrDefault(); 
            return l;
        }

        
        #endregion

        #region Async Methods

        public async Task<Link> CreateLink(Link link)
        {
            try
            {
                var rootDomain = GetURLRootDomain(link.LinkURL);

                var l = await GetFavIconDetailListAsync(rootDomain);
                foreach (var item in l)
                {
                    item.Link = link;
                }
                link.LinkIcons = l;

            }
            catch (Exception ex)
            {

                // For now, just ignore this and add the Link without any Favicons, the TreeView will use a default icon
            }

            _unitOfWork.Links.Add(link);
            _unitOfWork.Complete();
            _unitOfWork.ClearTracking();

            return link;
        }
        private string GetURLRootDomain(string url)
        {
            var slashLocation = url.IndexOf("/", 8);
            if (slashLocation == -1)
            {
                // Assume this is already a root domain
                return url;
            }
            return url.Substring(0, slashLocation);
        }
        private async System.Threading.Tasks.Task<List<LinkIcon>> GetFavIconDetailListAsync(string rootUrl)
        {
            Uri myUri = new Uri(rootUrl);

            //  ***********************************************************
            //    This is only a quick hack in getting FavIcons and does not work in many scenarios
            //
            //   This Method could be implemented as a standalone NUGET package by itself due to complexity
            //
            //   There are probably over 25 different ways FavIcons may be implemented by websites
            //   including different Icon sets for different platforms and browsers.
            //   Instead of the basic string manipulation seen below, a much better implementation would include:
            //   - use REGEX
            //   - use of strong UnitTesting
            //   - Better exception handling and HttpClient usage
            //   - support for the modern use of .SVG icons
            //  ***********************************************************

            List<LinkIcon> iconList = new List<LinkIcon>();
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.AllowAutoRedirect = false;

                using (HttpClient client = new HttpClient(httpClientHandler))
                {

                    using (HttpResponseMessage response = await client.GetAsync(myUri))
                    {
                        if (!response.IsSuccessStatusCode) 
                        {
                            if (response.Headers.Location != null)
                            {
                                myUri = response.Headers.Location;

                            }

                        }
                    }
                    try
                    {
                        byte[] fileContent = await client.GetByteArrayAsync(myUri + "favicon.ico");

                        LinkIcon li = new LinkIcon() { IconImage = fileContent, Rel = "shortcut icon", Href = "//favicon.ico", MimeType = "image/x-icon", Size = "32x32" };
                        iconList.Add(li);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("FavIcon not found at root of " + myUri.ToString() + "  " + ex.InnerException);
                    }
                    using (HttpResponseMessage response = await client.GetAsync(myUri))
                    {
                        if (response.IsSuccessStatusCode)
                        {

                            var content = response.Content.ReadAsStringAsync().Result;

                            var l = GetHeaderLinkHrefs(content);

                            foreach (var item in l)
                            {
                                string urlToUse;
                                if (myUri.ToString().EndsWith("/") && item.Href.StartsWith("/"))
                                {
                                    urlToUse = myUri + item.Href.Substring(1);
                                }
                                else
                                {
                                    urlToUse = myUri + item.Href;

                                }
                                if (item.Href.StartsWith("http"))
                                {
                                    urlToUse = item.Href;
                                }
                                else if (item.Href.StartsWith("//"))
                                {
                                    urlToUse = "https:" + item.Href;
                                }
                                byte[] fileContent = await client.GetByteArrayAsync(urlToUse);

                                item.Size = "48x48";
                                item.IconImage = fileContent;

                            }

                            iconList.AddRange(l);
                        }
                        else
                        {
                            
                            Debug.WriteLine("Site Appears to be blocking a basic GET on base Uri " + myUri);
                        }
                    }
                }

            }

            return iconList;
        }
        private List<LinkIcon> GetHeaderLinkHrefs(string input)
        {
            List<LinkIcon> result = new List<LinkIcon>();
            try
            {
                if (input.Contains("rel=\"shortcut icon\""))
                {
                    var rellocation = input.IndexOf("rel=\"shortcut icon\"");
                    var tmpInput = input.Substring(0, rellocation);
                    var startlocation = tmpInput.LastIndexOf("<link");

                    var linkInput = input.Substring(startlocation);
                    var endoffset = linkInput.IndexOf(">");

                    string link = input.Substring(startlocation, endoffset);


                    var hrefLocation = link.IndexOf("href=") + 6;
                    var endhrefLocation = link.IndexOf(".ico", hrefLocation);
                    string hrefResult = link.Substring(hrefLocation, endhrefLocation + 4 - hrefLocation);

                    var typeLocation = link.IndexOf("type=") + 6;
                    var endtypeLocation = link.IndexOf("\"", typeLocation);
                    string typeResult = link.Substring(typeLocation, endtypeLocation - typeLocation);

                    LinkIcon h = new LinkIcon() { Href = hrefResult, MimeType = typeResult, Rel = "shortcut icon" };
                    result.Add(h);



                }
                else if (input.Contains("rel=\"icon\""))
                {
                    var rellocation = input.IndexOf("rel=\"icon\"");
                    var tmpInput = input.Substring(0, rellocation);
                    var startlocation = tmpInput.LastIndexOf("<link");

                    var linkInput = input.Substring(startlocation);
                    var endoffset = linkInput.IndexOf(">");

                    string link = input.Substring(startlocation, endoffset);


                    var hrefLocation = link.IndexOf("href=") + 6;
                    var endhrefLocation = link.IndexOf(".png", hrefLocation); 
                    string hrefResult = link.Substring(hrefLocation, endhrefLocation + 4 - hrefLocation);

                    var typeLocation = link.IndexOf("type=") + 6;
                    var endtypeLocation = link.IndexOf("\"", typeLocation);
                    string typeResult = link.Substring(typeLocation, endtypeLocation - typeLocation);

                    var sizeLocation = link.IndexOf("sizes=") + 7;
                    var endtsizeLocation = link.IndexOf("\"", sizeLocation);
                    string typesizeResult = link.Substring(sizeLocation, endtsizeLocation - sizeLocation);

                    LinkIcon h = new LinkIcon() { Href = hrefResult, MimeType = typeResult, Rel = "shortcut icon", Size = typesizeResult };
                    result.Add(h);

                }
                else if (input.Contains("rel=\"apple-touch-icon\""))
                {
                    var rellocation = input.IndexOf("rel=\"apple-touch-icon\"");
                    var tmpInput = input.Substring(0, rellocation);
                    var startlocation = tmpInput.LastIndexOf("<link");

                    var linkInput = input.Substring(startlocation);
                    var endoffset = linkInput.IndexOf(">");

                    string link = input.Substring(startlocation, endoffset);


                    var hrefLocation = link.IndexOf("href=") + 6;
                    var endhrefLocation = link.IndexOf(".png", hrefLocation); 
                    string hrefResult = link.Substring(hrefLocation, endhrefLocation + 4 - hrefLocation);


                    var sizeLocation = link.IndexOf("sizes=") + 7;
                    var endtsizeLocation = link.IndexOf("\"", sizeLocation);
                    string typesizeResult = link.Substring(sizeLocation, endtsizeLocation - sizeLocation);

                    LinkIcon h = new LinkIcon() { Href = hrefResult, MimeType = "image/png", Rel = "apple-touch-icon", Size = typesizeResult };
                    result.Add(h);

                }

            }
            catch (Exception ex)
            {

                Debug.WriteLine("Error parsing the Header... can be fixed!!!!!!!!!!!!!!!!!");
            }

            return result;

        }



        #endregion


        #region Xtors
        public LinkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;


        }


        #endregion
    }

}