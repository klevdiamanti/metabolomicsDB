using System.Collections.Generic;
using compileHMDBdata;
using System.Linq;

namespace compileChEBIdata
{
    public class ChEBI_compound
    {
        //from chebi_inchi.tsv
        private string id;
        private string inchi;
        //from names.tsv
        private List<string> names;
        //from database_accession.tsv
        private List<string> cas_id;
        private List<string> kegg_id;
        private List<string> hmdb_id;
        private List<string> lipidmaps_id;
        private List<string> pubchem_id;
        //from compounds.tsv
        private string name;
        private string descrption;
        private string quality;
        //from comments.tsv
        private string comment;
        //chemical_data.tsv
        private List<string> formula;
        private List<string> charge;
        private List<string> mass;
        private List<string> monoisotopic_mass;

        private List<pathway> list_of_pathways;
        private Dictionary<string, KEGG_entry_details> dict_kegg_details;

        public string Id { get { return id; } set { id = value; } }
        public string Inchi { get { return inchi; } set { inchi = value; } }
        public List<string> Names { get { return names; } }
        public List<string> Cas_id { get { return cas_id; } }
        public List<string> Kegg_id { get { return kegg_id; } }
        public List<string> Hmdb_id { get { return hmdb_id; } }
        public List<string> Lipidmaps_id { get { return lipidmaps_id; } }
        public List<string> Pubchem_id { get { return pubchem_id; } }
        public string Name { get { return name; } set { name = value; } }
        public string Description { get { return descrption; } set { descrption = value; } }
        public string Quality { get { return quality; } set { quality = value; } }
        public string Comment { get { return comment; } set { comment = value; } }
        public List<string> Formula { get { return formula; } }
        public List<string> Charge { get { return charge; } }
        public List<string> Mass { get { return mass; } }
        public List<string> Monoisotopic_mass { get { return monoisotopic_mass; } }

        /// <summary>
        /// constructor
        /// </summary>
        public ChEBI_compound()
        {
            names = new List<string>();

            cas_id = new List<string>();
            kegg_id = new List<string>();
            hmdb_id = new List<string>();
            lipidmaps_id = new List<string>();
            pubchem_id = new List<string>();

            formula = new List<string>();
            charge = new List<string>();
            mass = new List<string>();
            monoisotopic_mass = new List<string>();

            list_of_pathways = new List<pathway>();
        }

        /// <summary>
        /// Add the information from the names file to one common list of names.
        /// All the values are added in one lists since different sources have slightly different names for the same compound.
        /// Here we are mainly interested in connecting HMDB to ChEBI to retrieve more pathways, so we do not care much about names.
        /// </summary>
        /// <param name="val">What the name is</param>
        public void add_from_names(string val)
        {
            if (!names.Contains(val))
            {
                names.Add(val);
            }
        }

        /// <summary>
        /// Add the information from the database accession ids to the proper variable.
        /// All the values are added in lists since different sources have different accessions for the same compound.
        /// Here we are mainly interested in connecting HMDB to ChEBI to retrieve more pathways, so we do not care much about names.
        /// </summary>
        /// <param name="_type">What kind of information this is: BRAND NAME, INN, IUPAC NAME, NAME, SYNONYM, SYSTEMATIC NAME</param>
        /// <param name="_name">What the accession is</param>
        public void add_from_database_accessions(string _type, string _name)
        {
            switch (_type)
            {
                case "CAS Registry Number":
                    if (!string.IsNullOrEmpty(_name) && !string.IsNullOrWhiteSpace(_name) && !cas_id.Contains(_name))
                    {
                        cas_id.Add(_name);
                    }
                    break;
                case "HMDB accession":
                    if (!string.IsNullOrEmpty(_name) && !string.IsNullOrWhiteSpace(_name) && !hmdb_id.Contains(_name))
                    {
                        hmdb_id.Add(_name);
                    }
                    break;
                case "KEGG COMPOUND accession":
                    if (!string.IsNullOrEmpty(_name) && !string.IsNullOrWhiteSpace(_name) && !kegg_id.Contains(_name))
                    {
                        kegg_id.Add(_name);
                        dict_kegg_details = new Dictionary<string, KEGG_entry_details>();
                        addKeggDetails(_name);

                    }
                    break;
                case "LIPID MAPS instance accession":
                    if (!string.IsNullOrEmpty(_name) && !string.IsNullOrWhiteSpace(_name) && !hmdb_id.Contains(_name))
                    {
                        lipidmaps_id.Add(_name);
                    }
                    break;
                default:
                    break;
            }
        }

        private void addKeggDetails(string v)
        {
            if (!dict_kegg_details.ContainsKey(v))
            {
                dict_kegg_details.Add(v, new KEGG_entry_details(v));
                if (dict_kegg_details[v].metadata)
                {
                    foreach (pathway p in dict_kegg_details[v].listOfPathway)
                    {
                        if (!list_of_pathways.Any(x => x.Kegg_map_id == p.Kegg_map_id))
                        {
                            list_of_pathways.Add(p);
                        }
                    }

                    if (!cas_id.Any(x => x == dict_kegg_details[v].casID))
                    {
                        cas_id.Add(dict_kegg_details[v].casID);
                    }
                    if (!pubchem_id.Any(x => x == dict_kegg_details[v].pubchemID))
                    {
                        pubchem_id.Add(dict_kegg_details[v].pubchemID);
                    }
                    if (!lipidmaps_id.Any(x => x == dict_kegg_details[v].lipidmapsID))
                    {
                        lipidmaps_id.Add(dict_kegg_details[v].lipidmapsID);
                    }
                }
            }
        }

        public void add_from_chemical_data(string _type, string _name)
        {
            switch (_type)
            {
                case "CHARGE":
                    if (!charge.Contains(_name))
                    {
                        charge.Add(_name);
                    }
                    break;
                case "FORMULA":
                    if (!formula.Contains(_name))
                    {
                        formula.Add(_name);
                    }
                    break;
                case "MASS":
                    if (!mass.Contains(_name))
                    {
                        mass.Add(_name);
                    }
                    break;
                case "MONOISOTOPIC MASS":
                    if (!monoisotopic_mass.Contains(_name))
                    {
                        monoisotopic_mass.Add(_name);
                    }
                    break;
                default:
                    break;
            }
        }

        public string printLine()
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}{0}{14}{0}{15}{0}{16}{0}{17}{0}{18}{0}{19}{0}{20}{0}{21}{0}{22}{0}{23}{0}{24}{0}{25}{0}{26}{0}{27}{0}{28}{0}{29}",
                "\t",
                id,
                inchi,
                string.Join("|", names),
                string.Join("|", cas_id),
                string.Join("|", kegg_id),
                string.Join("|", hmdb_id),
                string.Join("|", lipidmaps_id),
                string.Join("|", pubchem_id),
                name,
                descrption,
                quality,
                comment,
                string.Join("|", formula),
                string.Join("|", charge),
                string.Join("|", mass),
                string.Join("|", monoisotopic_mass),
                string.Join("|", list_of_pathways.Select(x => x.Kegg_map_id)),
                string.Join("|", list_of_pathways.Select(x => string.Join(";", x.List_of_names))),
                string.Join("|", list_of_pathways.Select(x => x.Super_class)),
                string.Join("|", list_of_pathways.Select(x => x.Pathway_map.Item1 + "-" + x.Pathway_map.Item2)),
                string.Join("|", list_of_pathways.Select(x => string.Join(";", x.List_of_modules.Select(y => y.Item1 + "-" + y.Item2)))),
                string.Join("|", list_of_pathways.Select(x => string.Join(";", x.List_of_diseases.Select(y => y.Item1 + "-" + y.Item2)))),
                string.Join("|", list_of_pathways.Select(x => x.Organism)),
                string.Join("|", list_of_pathways.Select(x => x.Gene)),
                string.Join("|", list_of_pathways.Select(x => x.Enzyme)),
                string.Join("|", list_of_pathways.Select(x => x.Reaction)),
                string.Join("|", list_of_pathways.Select(x => x.Compound)),
                string.Join("|", list_of_pathways.Select(x => x.Ko_pathway)),
                string.Join("|", list_of_pathways.Select(x => x.Rel_pathway)));
        }
    }
}
