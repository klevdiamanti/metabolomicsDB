using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace compileHMDBdata
{
    public class pathway
    {
        private string kegg_map_id;

		private string smpdb_map_id;
		private string smpdb_map_name;
		private string smpadb_map_description;

		private string entry;
        private List<string> list_of_names;
        private string description;
		private string super_class;
		private Tuple<string, string> pathway_map;
        private List<Tuple<string, string>> list_of_modules;
        private List<Tuple<string, string>> list_of_diseases;
        private List<string> list_of_drug;
        private List<Tuple<string, List<string>>> list_of_dblinks;
        private string organism;
        private string orthology;
        private string gene;
		private string enzyme;
		private string reaction;
		private string compound;
        private List<Tuple<string, string, string, string, string>> list_of_references;
        private string rel_pathway;
        private string ko_pathway;

        public string Kegg_map_id { get { return kegg_map_id; } set { kegg_map_id = value; } }

		public string Smpdb_map_id { get { return smpdb_map_id; } set { smpdb_map_id = value; } }
		public string Smpdb_map_name { get { return smpdb_map_name; } }
		public string Smpadb_map_description { get { return smpadb_map_description; } }

        public string Entry { get { return entry; } }
        public List<string> List_of_names { get { return list_of_names; } }
		public string Description { get { return description; } }
		public string Super_class { get { return super_class; } }
		public Tuple<string, string> Pathway_map { get { return pathway_map; } set { pathway_map = value;} }
        public List<Tuple<string, string>> List_of_modules { get { return list_of_modules; } }
        public List<Tuple<string, string>> List_of_diseases { get { return list_of_diseases; } }
        public List<string> List_of_drug { get { return list_of_drug; } }
        public List<Tuple<string, List<string>>> List_of_dblinks { get { return list_of_dblinks; } }
        public string Organism { get { return organism; } }
        public string Orthology { get { return orthology; } }
        public string Gene { get { return gene; } }
		public string Enzyme { get { return enzyme; } }
		public string Reaction { get { return reaction; } }
		public string Compound { get { return compound; } }
        public List<Tuple<string, string, string, string, string>> List_of_references { get { return list_of_references; } set { list_of_references = value; } }
        public string Rel_pathway { get { return rel_pathway; } }
        public string Ko_pathway { get { return ko_pathway; } }

		public pathway()
		{
			list_of_names = new List<string>();
			list_of_modules = new List<Tuple<string, string>>();
			list_of_diseases = new List<Tuple<string, string>>();
            list_of_drug = new List<string>();
            list_of_dblinks = new List<Tuple<string, List<string>>>();
            list_of_references = new List<Tuple<string, string, string, string, string>>();
		}

		public void get_details()
		{
			if (smpdb_map_id != "" && SMPDB_pathways.list_of_smpdb_pathways.Any(x => x.Id == smpdb_map_id))
			{
				smpdb_map_name = SMPDB_pathways.list_of_smpdb_pathways.First(x => x.Id == smpdb_map_id).Name;
				smpadb_map_description = SMPDB_pathways.list_of_smpdb_pathways.First(x => x.Id == smpdb_map_id).Description;
			}

			if (kegg_map_id != "")
			{
				string KEGGrecord = httpRequestToKEGG("http://rest.kegg.jp/get/" + kegg_map_id);
				if (!string.IsNullOrEmpty(KEGGrecord) && !string.IsNullOrWhiteSpace(KEGGrecord))
				{
					parseREST(KEGGrecord);
				}
			}

			try
			{
				if (string.IsNullOrEmpty(pathway_map.Item1) && string.IsNullOrEmpty(pathway_map.Item2))
				{
					pathway_map = new Tuple<string, string>("", "");
				}
			}
			catch (Exception ex)
			{
				string k = ex.Message;
				pathway_map = new Tuple<string, string>("", "");
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
            list_of_names = new List<string>();
            list_of_modules = new List<Tuple<string, string>>();
            list_of_diseases = new List<Tuple<string, string>>();
            list_of_drug = new List<string>();
            list_of_dblinks = new List<Tuple<string, List<string>>>();
            list_of_references = new List<Tuple<string, string, string, string, string>>();

            string check = "";
            Tuple<string, string, string, string, string> tmp_tuple;

            //http://www.kegg.jp/kegg/rest/dbentry.html
            foreach (string recordLine in KEGGrecord.Split('\n'))
            {
                if (recordLine.StartsWith("ENTRY"))
                {
                    entry = recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ElementAt(1);
                    check = "ENTRY";
                }
                else if (recordLine.StartsWith("NAME"))
                {
                    list_of_names.Add(string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Skip(1)));
                    check = "NAME";
                }
				else if (recordLine.StartsWith("DESCRIPTION"))
				{
					description = string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Skip(1));
					check = "DESCRIPTION";
                }
                else if (recordLine.StartsWith("CLASS"))
                {
                    super_class = string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Skip(1));
                    check = "CLASS";
                }
                else if (recordLine.StartsWith("PATHWAY_MAP"))
                {
                    pathway_map = new Tuple<string, string>
                    (
                        recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ElementAt(1),
                        string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Skip(2))
                    );
                    check = "PATHWAY_MAP";
                }
                else if (recordLine.StartsWith("MODULE"))
                {
                    list_of_modules.Add(new Tuple<string, string>
                    (
                        recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ElementAt(1),
                        string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Skip(2))
                    ));
                    check = "MODULE";
                }
                else if (recordLine.StartsWith("DISEASE"))
                {
                    list_of_diseases.Add(new Tuple<string, string>
                    (
                        recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ElementAt(1),
                        string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Skip(2))
                    ));
                    check = "DISEASE";
                }
                else if (recordLine.StartsWith("DRUG"))
                {
                    list_of_drug.Add(string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Skip(1)));
                    check = "DRUG";
                }
                else if (recordLine.StartsWith("DBLINKS"))
                {
                    list_of_dblinks.Add(new Tuple<string, List<string>>
                    (
                        recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ElementAt(1),
                        recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Skip(2).ToList()
                    ));
                    check = "DBLINKS";
                }
                else if (recordLine.StartsWith("ORGANISM"))
				{
					organism = recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ElementAt(1);
					check = "ORGANISM";
                }
                else if (recordLine.StartsWith("ORTHOLOGY"))
                {
                    orthology = recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ElementAt(1);
                    check = "ORTHOLOGY";
                }
                else if (recordLine.StartsWith("GENE"))
				{
					gene = recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ElementAt(1);
					check = "GENE";
				}
				else if (recordLine.StartsWith("ENZYME"))
				{
					enzyme = recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ElementAt(1);
					check = "ENZYME";
				}
				else if (recordLine.StartsWith("REACTION"))
				{
					reaction = recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ElementAt(1);
					check = "REACTION";
				}
				else if (recordLine.StartsWith("COMPOUND"))
				{
					compound = recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ElementAt(1);
					check = "COMPOUND";
                }
                else if (recordLine.StartsWith("REFERENCE"))
                {
                    list_of_references.Add(new Tuple<string, string, string, string, string>
                    (
                        string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Skip(1)),
                        "", "", "", ""
                    ));
                    check = "REFERENCE";
                }
                else if (recordLine.StartsWith("REL_PATHWAY"))
				{
                    rel_pathway = string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Skip(1));
					check = "REL_PATHWAY";
				}
                else if (recordLine.StartsWith("KO_PATHWAY"))
                {
                    ko_pathway = string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Skip(1));
                    check = "KO_PATHWAY";
                }
                else
                {
                    switch (check)
                    {
                        case "NAME":
                            list_of_names.Add(string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))));
                            break;
                        case "MODULE":
                            list_of_modules.Add(new Tuple<string, string>
                            (
                                recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).First(),
                                string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Skip(1))
                            ));
                            break;
                        case "DISEASE":
                            list_of_diseases.Add(new Tuple<string, string>
                            (
                                recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).First(),
                                string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Skip(1))
                            ));
                            break;
                        case "DRUG":
                            list_of_drug.Add(string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))));
                            break;
                        case "DBLINKS":
                            list_of_dblinks.Add(new Tuple<string, List<string>>
                            (
                                recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).First(),
                                recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Skip(1).ToList()
                            ));
                            break;
						case "REFERENCE":
                            list_of_references.Add(new Tuple<string, string, string, string, string>
                            (
                                string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Skip(1)),
                                "", "", "", ""
                            ));
                            check = "REFERENCE";
                            break;
                        case "AUTHORS":
                            tmp_tuple = new Tuple<string, string, string, string, string>
                                (
                                    list_of_references.Last().Item1,
                                    string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))),
                                    "", "", ""
                                );
                            list_of_references.RemoveAt(list_of_references.Count - 1);
                            list_of_references.Add(tmp_tuple);
                            break;
                        case "TITLE":
                            tmp_tuple = new Tuple<string, string, string, string, string>
                            (
                                list_of_references.Last().Item1,
                                list_of_references.Last().Item2,
                                string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))),
                                "", ""
                            );
                            list_of_references.RemoveAt(list_of_references.Count - 1);
                            list_of_references.Add(tmp_tuple);
                            break;
                        case "JOURNAL":
                            if (string.IsNullOrEmpty(list_of_references.Last().Item4) && string.IsNullOrWhiteSpace(list_of_references.Last().Item4))
                            {
                                tmp_tuple = new Tuple<string, string, string, string, string>
                                (
                                    list_of_references.Last().Item1,
                                    list_of_references.Last().Item2,
                                    list_of_references.Last().Item3,
                                    string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))),
                                    ""
                                );
                                list_of_references.RemoveAt(list_of_references.Count - 1);
                                list_of_references.Add(tmp_tuple);
                            }
                            else
                            {
                                tmp_tuple = new Tuple<string, string, string, string, string>
                                (
                                    list_of_references.Last().Item1,
                                    list_of_references.Last().Item2,
                                    list_of_references.Last().Item3,
                                    list_of_references.Last().Item4,
                                    string.Join(" ", recordLine.Split(' ').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)))
                                );
                                list_of_references.RemoveAt(list_of_references.Count - 1);
                                list_of_references.Add(tmp_tuple);
                            }
                            break;
                        default:
                            break;

                    }
                }
            }
        }
    }
}
