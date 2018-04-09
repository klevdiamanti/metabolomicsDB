using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace compileHMDBdata
{
    public class KEGG_entry_details
    {
        public string KEGG_id { get; set; }
        public string entry { get; set; }
        public List<string> listOfName { get; set; }
        public string formula { get; set; }
        public double exact_mass { get; set; }
        public double mol_wight { get; set; }
        public List<pathway> listOfPathway { get; set; }
        public string casID { get; set; }
        public string pubchemID { get; set; }
        public string lipidmapsID { get; set; }

		public bool metadata { get; set; } //a variable to know if the kegg entry was found in the database or not

        public KEGG_entry_details(string kid)
        {
            KEGG_id = kid;
			metadata = false;
			string KEGGrecord = httpRequestToKEGG("http://rest.kegg.jp/get/cpd:" + kid);
			if (!string.IsNullOrEmpty(KEGGrecord) && !string.IsNullOrWhiteSpace(KEGGrecord))
            {
                parseREST(KEGGrecord);
				metadata = true;
            }
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

        private void parseREST(string KEGGrecord)
        {
			listOfName = new List<string>();
            listOfPathway = new List<pathway>();

            string check = "", correctRecordLine = "";

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);

            foreach (string recordLine in KEGGrecord.Split('\n'))
            {
                correctRecordLine = regex.Replace(recordLine, " ");

                if (correctRecordLine.StartsWith("ENTRY"))
                {
                    entry = correctRecordLine.Split(' ').ElementAt(1);
                    check = "ENTRY";
                }
                else if (correctRecordLine.StartsWith("NAME"))
                {
                    if (correctRecordLine.Split(' ').ElementAt(1).EndsWith(";"))
                    {
                        listOfName.Add(correctRecordLine.Split(' ').ElementAt(1).Substring(0, correctRecordLine.Split(' ').ElementAt(1).Length - 1));
                    }
                    else
                    {
                        listOfName.Add(correctRecordLine.Split(' ').ElementAt(1));
                    }
                    check = "NAME";
                }
                else if (correctRecordLine.StartsWith("FORMULA"))
                {
                    formula = correctRecordLine.Split(' ').ElementAt(1);
                    check = "FORMULA";
                }
                else if (correctRecordLine.StartsWith("EXACT_MASS"))
                {
                    exact_mass = Convert.ToDouble(correctRecordLine.Split(' ').ElementAt(1));
                    check = "EXACT_MASS";
                }
                else if (correctRecordLine.StartsWith("MOL_WEIGHT"))
                {
                    mol_wight = Convert.ToDouble(correctRecordLine.Split(' ').ElementAt(1));
                    check = "MOL_WEIGHT";
                }
                else if (correctRecordLine.StartsWith("PATHWAY"))
                {
					listOfPathway.Add(new pathway()
					{
						Kegg_map_id = correctRecordLine.Split(' ').ElementAt(1),
						Pathway_map = new Tuple<string, string>("", "")
					});
                    check = "PATHWAY";
                }
                else if (correctRecordLine.StartsWith("DBLINKS"))
                {
                    if (correctRecordLine.Split(' ').ElementAt(1).StartsWith("CAS"))
                    {
                        casID = correctRecordLine.Split(' ').Last();
                    }
                    else if (correctRecordLine.Split(' ').ElementAt(1).StartsWith("PubChem"))
                    {
                        pubchemID = correctRecordLine.Split(' ').Last();
                    }
                    else if (correctRecordLine.Split(' ').ElementAt(1).StartsWith("LIPIDMAPS"))
                    {
                        lipidmapsID = correctRecordLine.Split(' ').Last();
                    }
                    check = "DBLINKS";
                }
                else if (correctRecordLine.StartsWith("REMARK") || correctRecordLine.StartsWith("REACTION") || correctRecordLine.StartsWith("MODULE") ||
                    correctRecordLine.StartsWith("ENZYME") || correctRecordLine.StartsWith("BRITE") || correctRecordLine.StartsWith("ATOM") || correctRecordLine.StartsWith("BOND") ||
                    correctRecordLine.StartsWith("COMMENT") || correctRecordLine.StartsWith("SEQUENCE") || correctRecordLine.StartsWith("REFERENCE") || correctRecordLine.StartsWith("BRACKET"))
                {
                    check = correctRecordLine.Split(' ').First();
                }
                else
                {
                    switch (check)
                    {
                        case "NAME":
                            if (correctRecordLine.Split(' ').ElementAt(1).EndsWith(";"))
                            {
                                listOfName.Add(correctRecordLine.Split(' ').ElementAt(1).Substring(0, correctRecordLine.Split(' ').ElementAt(1).Length - 1));
                            }
                            else
                            {
                                listOfName.Add(correctRecordLine.Split(' ').ElementAt(1));
                            }
                            break;
                        case "PATHWAY":
							listOfPathway.Add(new pathway()
							{
								Kegg_map_id = correctRecordLine.Split(' ').ElementAt(1),
								Pathway_map = new Tuple<string, string>("", "")
							});
                            break;
                        case "DBLINKS":
							if (correctRecordLine.Split(' ').Length <= 1)
							{
								break;
							}
							else if (correctRecordLine.Split(' ').ElementAt(1).StartsWith("CAS"))
                            {
                                casID = correctRecordLine.Split(' ').Last();
                            }
                            else if (correctRecordLine.Split(' ').ElementAt(1).StartsWith("PubChem"))
                            {
                                pubchemID = correctRecordLine.Split(' ').Last();
                            }
                            else if (correctRecordLine.Split(' ').ElementAt(1).StartsWith("LIPIDMAPS"))
                            {
                                lipidmapsID = correctRecordLine.Split(' ').Last();
                            }
                            break;
                        case "REMARK":
                        case "REACTION":
                        case "MODULE":
                        case "ENZYME":
                        case "BRITE":
                        case "ATOM":
                        case "BOND":
                        case "COMMENT":
                        case "SEQUENCE":
                        case "REFERENCE":
                        case "BRACKET":
                            break;
                        default:
                            break;

                    }
                }
            }
        }
    }
}
