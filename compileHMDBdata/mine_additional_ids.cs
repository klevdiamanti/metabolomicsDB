using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace compileHMDBdata
{
    public static class mine_additional_ids
    {
        public static void retriveInfoFromCts(HMDB_metabolite hmdb_metab)
        {
            List<cts_response> lstcr;
            #region CAS
            string jsonText = httpRequestToKEGG("http://cts.fiehnlab.ucdavis.edu/service/convert/Human Metabolome Database/" + "CAS" + "/" + hmdb_metab.Hmdb_accession);
            if (!string.IsNullOrEmpty(jsonText) && !string.IsNullOrWhiteSpace(jsonText))
            {
                lstcr = JsonConvert.DeserializeObject<List<cts_response>>(jsonText);
                if (lstcr.First().result.Count != 0)
                {
                    foreach (string s in lstcr.First().result)
                    {
                        if (!hmdb_metab.Cts_cas.Contains(s) && hmdb_metab.Cas_registry_number != s)
                        {
                            hmdb_metab.Cts_cas.Add(s);
                        }
                    }
                }
            }
            foreach (string hmdb_alt_id in hmdb_metab.Hmdb_secondary_accessions)
            {
                jsonText = httpRequestToKEGG("http://cts.fiehnlab.ucdavis.edu/service/convert/Human Metabolome Database/" + "CAS" + "/" + hmdb_alt_id);
                if (!string.IsNullOrEmpty(jsonText) && !string.IsNullOrWhiteSpace(jsonText))
                {
                    lstcr = JsonConvert.DeserializeObject<List<cts_response>>(jsonText);
                    if (lstcr.First().result.Count != 0)
                    {
                        foreach (string s in lstcr.First().result)
                        {
                            if (!hmdb_metab.Cts_cas.Contains(s) && hmdb_metab.Cas_registry_number != s)
                            {
                                hmdb_metab.Cts_cas.Add(s);
                            }
                        }
                    }
                }
            }
            #endregion

            #region KEGG
            jsonText = httpRequestToKEGG("http://cts.fiehnlab.ucdavis.edu/service/convert/Human Metabolome Database/" + "KEGG" + "/" + hmdb_metab.Hmdb_accession);
            if (!string.IsNullOrEmpty(jsonText) && !string.IsNullOrWhiteSpace(jsonText))
            {
                lstcr = JsonConvert.DeserializeObject<List<cts_response>>(jsonText);
                if (lstcr.First().result.Count != 0)
                {
                    foreach (string s in lstcr.First().result)
                    {
                        if (!hmdb_metab.Cts_kegg.Contains(s) && hmdb_metab.Kegg_id != s)
                        {
                            hmdb_metab.Cts_kegg.Add(s);
                            hmdb_metab.addKeggDetails(s);
                        }
                    }
                }
            }
            foreach (string hmdb_alt_id in hmdb_metab.Hmdb_secondary_accessions)
            {
                jsonText = httpRequestToKEGG("http://cts.fiehnlab.ucdavis.edu/service/convert/Human Metabolome Database/" + "KEGG" + "/" + hmdb_alt_id);
                if (!string.IsNullOrEmpty(jsonText) && !string.IsNullOrWhiteSpace(jsonText))
                {
                    lstcr = JsonConvert.DeserializeObject<List<cts_response>>(jsonText);
                    if (lstcr.First().result.Count != 0)
                    {
                        foreach (string s in lstcr.First().result)
                        {
                            if (!hmdb_metab.Cts_kegg.Contains(s) && hmdb_metab.Kegg_id != s)
                            {
                                hmdb_metab.Cts_kegg.Add(s);
                                hmdb_metab.addKeggDetails(s);
                            }
                        }
                    }
                }
            }
            #endregion

            #region ChEBI
            jsonText = httpRequestToKEGG("http://cts.fiehnlab.ucdavis.edu/service/convert/Human Metabolome Database/" + "ChEBI" + "/" + hmdb_metab.Hmdb_accession);
            if (!string.IsNullOrEmpty(jsonText) && !string.IsNullOrWhiteSpace(jsonText))
            {
                lstcr = JsonConvert.DeserializeObject<List<cts_response>>(jsonText);
                if (lstcr.First().result.Count != 0)
                {
                    foreach (string s in lstcr.First().result.Select(x => x.Split(':').Last()))
                    {
                        if (!hmdb_metab.Cts_chebi.Contains(s) && hmdb_metab.Chebi_id != s)
                        {
                            hmdb_metab.Cts_chebi.Add(s);
                        }
                    }
                }
            }
            foreach (string hmdb_alt_id in hmdb_metab.Hmdb_secondary_accessions)
            {
                jsonText = httpRequestToKEGG("http://cts.fiehnlab.ucdavis.edu/service/convert/Human Metabolome Database/" + "ChEBI" + "/" + hmdb_alt_id);
                if (!string.IsNullOrEmpty(jsonText) && !string.IsNullOrWhiteSpace(jsonText))
                {
                    lstcr = JsonConvert.DeserializeObject<List<cts_response>>(jsonText);
                    if (lstcr.First().result.Count != 0)
                    {
                        foreach (string s in lstcr.First().result)
                        {
                            if (!hmdb_metab.Cts_chebi.Contains(s) && hmdb_metab.Chebi_id != s)
                            {
                                hmdb_metab.Cts_chebi.Add(s);
                            }
                        }
                    }
                }
            }
            #endregion
        }

        private static string httpRequestToKEGG(string requestUrl)
        {
            string result = null;
            WebResponse response = null;
            StreamReader reader = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                request.Method = "GET";
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = reader.ReadToEnd();
            }
            catch (Exception)
            {
                // handle error
                //outputToLog.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();
            }
            return result;
        }
    }

    public class lists_of_ids
    {
        public List<string> cas { get; set; }
        public List<string> chebi { get; set; }
        public List<string> kegg { get; set; }
    }

    public class cts_response
    {
        public string fromIdentifier { get; set; }
        public string searchTerm { get; set; }
        public string toIdentifier { get; set; }
        public IList<string> result { get; set; }
    }
}
