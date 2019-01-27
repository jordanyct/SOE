using Business;
using SOE.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SOE.Controllers
{
    public class SOEController : Controller
    {
        // GET: SOE
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Index(SOEModel soe)
        {
            SOEResponseModel res = new SOEResponseModel();
            if (ModelState.IsValid)
            {
                List<string> stopWordsList = new List<string>() { "all", "at", "of" };
                SOEService svc = new SOEService();
                string InputText = soe.txtArea ?? string.Empty;
                if (!String.IsNullOrWhiteSpace(soe.txtURL))
                {
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(soe.txtURL);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Stream receiveStream = response.GetResponseStream();
                            StreamReader readStream = null;

                            if (response.CharacterSet == null)
                            {
                                readStream = new StreamReader(receiveStream);
                            }
                            else
                            {
                                readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                            }

                            InputText = readStream.ReadToEnd();
                            response.Close();
                            readStream.Close();
                        }
                    }
                    catch (System.UriFormatException uex)
                    {
                        res.ErrorMessage = "There was an error in the format of the url: " + soe.txtURL;

                    }
                    catch (System.Net.WebException wex)
                    {
                        res.ErrorMessage = string.Format("There was an error connecting to the url [{0}] [{1}] ", soe.txtURL, wex.Message);
                    }
                }
                if (!string.IsNullOrEmpty(InputText) && string.IsNullOrEmpty(res.ErrorMessage)) //No Error and InputText is not empty
                {
                    if (soe.SOEOption != null && soe.SOEOption.Any())
                    {
                        if (soe.SOEOption.Contains("StopwordCheck"))
                        {
                            res.OutputText = svc.RemoveWords(stopWordsList, InputText);
                        }
                        if (soe.SOEOption.Contains("OccurrenceCheck"))
                        {
                            res.CountOccurrence = svc.CountOccurrences(InputText);
                        }
                        if (soe.SOEOption.Contains("MetaCheck"))
                        {
                            res.MetaTagOccurrences = svc.CountMetaTagOccurrences(InputText);
                        }
                        if (soe.SOEOption.Contains("URLCheck"))
                        {
                            res.ExternaLinkCount = svc.ExternalLink(InputText);
                        }
                    }
                    else
                    {
                        res.ErrorMessage = "Please tick at least one check box";
                    }
                }
                else
                {
                    res.ErrorMessage = "Please enter Text or URL";
                }
            }
            else
            { res.ErrorMessage = "Invalid Input"; }
            return View(res);

        }
    }
}