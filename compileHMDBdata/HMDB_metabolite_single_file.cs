using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace compileHMDBdata
{
    class HMDB_metabolite_single_file : HMDB_metabolite
    {
        public taxonomy My_taxonomy { get; set; }
        public List<disease> List_of_diseases { get; set; }

        public HMDB_metabolite_single_file(XElement metaboliteElement)
        {
            Cts_cas = new List<string>();
            Cts_kegg = new List<string>();
            Cts_chebi = new List<string>();

            string tmp_string = "";

            foreach (XElement item in metaboliteElement.Elements())
            {
                switch (item.Name.LocalName)
                {
                    case "accession":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Hmdb_accession = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "secondary_accessions":
                        Hmdb_secondary_accessions = new List<string>();
                        foreach (XElement secondary_accession in item.Elements().Where(x => x.Name.LocalName == "accession"))
                        {
                            if (string.IsNullOrEmpty(secondary_accession.Value) || string.IsNullOrWhiteSpace(secondary_accession.Value) || secondary_accession.Value == "\n")
                            {
                                continue;
                            }
                            else
                            {
                                Hmdb_secondary_accessions.Add(secondary_accession.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim());
                            }
                        }
                        break;
                    case "name":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Name = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "description":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Description = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "synonyms":
                        Synonym_names = new List<string>();
                        foreach (XElement synonym_content in item.Elements().Where(x => x.Name.LocalName == "synonym"))
                        {
                            if (string.IsNullOrEmpty(synonym_content.Value) || string.IsNullOrWhiteSpace(synonym_content.Value) || synonym_content.Value == "\n")
                            {
                                continue;
                            }
                            else
                            {
                                Synonym_names.Add(synonym_content.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim());
                            }
                        }
                        break;
                    case "chemical_formula":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Formula = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "average_molecular_weight":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Average_molecular_weight = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? -1 : Convert.ToDouble(tmp_string);
                        break;
                    case "monisotopic_moleculate_weight":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Monisotopic_molecular_weight = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? -1 : Convert.ToDouble(tmp_string);
                        break;
                    case "iupac_name":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Iupac_name = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "traditional_iupac":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Traditional_iupac = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "cas_registry_number":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Cas_registry_number = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "smiles":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Smiles = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "inchi":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Inchi = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "inchikey":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Inchikey = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "taxonomy":
                        My_taxonomy = new taxonomy()
                        {
                            Description = (string.IsNullOrEmpty(item.Elements().First(x => x.Name.LocalName == "description").Value) ||
                                        string.IsNullOrWhiteSpace(item.Elements().First(x => x.Name.LocalName == "description").Value) ||
                                        item.Elements().First(x => x.Name.LocalName == "description").Value == "\n") ?
                                            "" : item.Elements().First(x => x.Name.LocalName == "description").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                            Direct_parent = (string.IsNullOrEmpty(item.Elements().First(x => x.Name.LocalName == "direct_parent").Value) ||
                                        string.IsNullOrWhiteSpace(item.Elements().First(x => x.Name.LocalName == "direct_parent").Value) ||
                                        item.Elements().First(x => x.Name.LocalName == "direct_parent").Value == "\n") ?
                                            "" : item.Elements().First(x => x.Name.LocalName == "direct_parent").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                            Kingdom = (string.IsNullOrEmpty(item.Elements().First(x => x.Name.LocalName == "kingdom").Value) ||
                                        string.IsNullOrWhiteSpace(item.Elements().First(x => x.Name.LocalName == "kingdom").Value) ||
                                        item.Elements().First(x => x.Name.LocalName == "kingdom").Value == "\n") ?
                                            "" : item.Elements().First(x => x.Name.LocalName == "kingdom").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                            Super_class = (string.IsNullOrEmpty(item.Elements().First(x => x.Name.LocalName == "super_class").Value) ||
                                        string.IsNullOrWhiteSpace(item.Elements().First(x => x.Name.LocalName == "super_class").Value) ||
                                        item.Elements().First(x => x.Name.LocalName == "super_class").Value == "\n") ?
                                            "" : item.Elements().First(x => x.Name.LocalName == "super_class").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                            Tclass = (string.IsNullOrEmpty(item.Elements().First(x => x.Name.LocalName == "class").Value) ||
                                        string.IsNullOrWhiteSpace(item.Elements().First(x => x.Name.LocalName == "class").Value) ||
                                        item.Elements().First(x => x.Name.LocalName == "class").Value == "\n") ?
                                            "" : item.Elements().First(x => x.Name.LocalName == "class").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                            Molecular_framework = (string.IsNullOrEmpty(item.Elements().First(x => x.Name.LocalName == "direct_parent").Value) ||
                                        string.IsNullOrWhiteSpace(item.Elements().First(x => x.Name.LocalName == "direct_parent").Value) ||
                                        item.Elements().First(x => x.Name.LocalName == "direct_parent").Value == "\n") ?
                                            "" : item.Elements().First(x => x.Name.LocalName == "direct_parent").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                            Alternative_parents = item.Elements().First(x => x.Name.LocalName == "alternative_parents").Elements().Where(x => x.Name.LocalName == "alternative_parent").
                                                                    Select(x => x.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim()).ToList(),
                            Substituents = item.Elements().First(x => x.Name.LocalName == "substituents").Elements().Where(x => x.Name.LocalName == "substituent").
                                                                    Select(x => x.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim()).ToList()

                        };
                        break;
                    case "ontology":
                        My_onotology = new ontology()
                        {
                            Status = (string.IsNullOrEmpty(item.Elements().First(x => x.Name.LocalName == "status").Value) ||
                                        string.IsNullOrWhiteSpace(item.Elements().First(x => x.Name.LocalName == "status").Value) ||
                                        item.Elements().First(x => x.Name.LocalName == "status").Value == "\n") ?
                                            "" : item.Elements().First(x => x.Name.LocalName == "status").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                            Origins = item.Elements().First(x => x.Name.LocalName == "origins").Elements().Where(x => x.Name.LocalName == "origin").
                                                                    Select(x => x.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim()).ToList(),
                            Biofunctions = item.Elements().First(x => x.Name.LocalName == "biofunctions").Elements().Where(x => x.Name.LocalName == "biofunction").
                                                                    Select(x => x.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim()).ToList(),
                            Applications = item.Elements().First(x => x.Name.LocalName == "applications").Elements().Where(x => x.Name.LocalName == "application").
                                                                    Select(x => x.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim()).ToList(),
                            Cellular_locations = item.Elements().First(x => x.Name.LocalName == "cellular_locations").Elements().Where(x => x.Name.LocalName == "cellular_location").
                                                                    Select(x => x.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim()).ToList()
                        };
                        break;
                    case "state":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        State = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "biofluid_locations":
                        Biofluid_locations = new List<string>();
                        foreach (XElement biofluid in item.Elements().Where(x => x.Name.LocalName == "biofluid"))
                        {
                            if (string.IsNullOrEmpty(biofluid.Value) || string.IsNullOrWhiteSpace(biofluid.Value) || biofluid.Value == "\n")
                            {
                                continue;
                            }
                            else
                            {
                                Biofluid_locations.Add(biofluid.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim());
                            }
                        }
                        break;
                    case "tissue_locations":
                        Tissue_locations = new List<string>();
                        foreach (XElement tissue in item.Elements().Where(x => x.Name.LocalName == "tissue"))
                        {
                            if (string.IsNullOrEmpty(tissue.Value) || string.IsNullOrWhiteSpace(tissue.Value) || tissue.Value == "\n")
                            {
                                continue;
                            }
                            else
                            {
                                Tissue_locations.Add(tissue.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim());
                            }
                        }
                        break;
                    case "pathways":
                        List_of_pathways = new List<pathway>();
                        string kegg_map_id = "", smpdb_id = "";
                        pathway ptwy;
                        foreach (XElement cpathway in item.Elements().Where(x => x.Name.LocalName == "pathway"))
                        {
                            kegg_map_id = (string.IsNullOrEmpty(cpathway.Elements().First(x => x.Name.LocalName == "kegg_map_id").Value) ||
                                string.IsNullOrWhiteSpace(cpathway.Elements().First(x => x.Name.LocalName == "kegg_map_id").Value) ||
                                cpathway.Elements().First(x => x.Name.LocalName == "kegg_map_id").Value == "\n") ?
                                "" : cpathway.Elements().First(x => x.Name.LocalName == "kegg_map_id").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                            smpdb_id = (string.IsNullOrEmpty(cpathway.Elements().First(x => x.Name.LocalName == "smpdb_id").Value) ||
                                string.IsNullOrWhiteSpace(cpathway.Elements().First(x => x.Name.LocalName == "smpdb_id").Value) ||
                                cpathway.Elements().First(x => x.Name.LocalName == "smpdb_id").Value == "\n") ?
                                "" : cpathway.Elements().First(x => x.Name.LocalName == "smpdb_id").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();

                            ptwy = new pathway()
                            {
                                Kegg_map_id = kegg_map_id,
                                Smpdb_map_id = smpdb_id
                            };
                            ptwy.get_details();
                            List_of_pathways.Add(ptwy);
                        }
                        foreach (SMPDB_pathway smpdb_ptwy in SMPDB_pathways.list_of_smpdb_pathways.Where(x => x.Hmdb_id == Hmdb_accession))
                        {
                            if (!List_of_pathways.Any(x => x.Smpdb_map_id == smpdb_ptwy.Id))
                            {
                                ptwy = new pathway()
                                {
                                    Kegg_map_id = "",
                                    Smpdb_map_id = smpdb_ptwy.Id
                                };
                                ptwy.get_details();
                                List_of_pathways.Add(ptwy);
                            }
                        }
                        break;
                    case "drugbank_id":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Drugbank_id = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "drugbank_metabolite_id":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Drugbank_metabolite_id = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "chemspider_id":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Chemspider_id = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "kegg_id":
                        Dict_kegg_details = new Dictionary<string, KEGG_entry_details>();
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Kegg_id = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        addKeggDetails((string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string);
                        break;
                    case "metlin_id":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Metlin_id = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "pubchem_compound_id":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Pubchem_compound_id = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "chebi_id":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Chebi_id = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "synthesis_reference":
                        tmp_string = item.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        Synthesis_reference = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
                        break;
                    case "protein_associations":
                        List_of_proteins = new List<protein>();
                        foreach (XElement protein in item.Elements().Where(x => x.Name.LocalName == "protein"))
                        {
                            List_of_proteins.Add(new protein()
                            {
                                Protein_accession = (string.IsNullOrEmpty(protein.Elements().First(x => x.Name.LocalName == "protein_accession").Value) ||
                                                string.IsNullOrWhiteSpace(protein.Elements().First(x => x.Name.LocalName == "protein_accession").Value) ||
                                                protein.Elements().First(x => x.Name.LocalName == "protein_accession").Value == "\n") ?
                                                    "" : protein.Elements().First(x => x.Name.LocalName == "protein_accession").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                                Name = (string.IsNullOrEmpty(protein.Elements().First(x => x.Name.LocalName == "name").Value) ||
                                                string.IsNullOrWhiteSpace(protein.Elements().First(x => x.Name.LocalName == "name").Value) ||
                                                protein.Elements().First(x => x.Name.LocalName == "name").Value == "\n") ?
                                                    "" : protein.Elements().First(x => x.Name.LocalName == "name").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                                Uniprot_id = (string.IsNullOrEmpty(protein.Elements().First(x => x.Name.LocalName == "uniprot_id").Value) ||
                                                string.IsNullOrWhiteSpace(protein.Elements().First(x => x.Name.LocalName == "uniprot_id").Value) ||
                                                protein.Elements().First(x => x.Name.LocalName == "uniprot_id").Value == "\n") ?
                                                    "" : protein.Elements().First(x => x.Name.LocalName == "uniprot_id").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                                Gene_name = (string.IsNullOrEmpty(protein.Elements().First(x => x.Name.LocalName == "gene_name").Value) ||
                                                string.IsNullOrWhiteSpace(protein.Elements().First(x => x.Name.LocalName == "gene_name").Value) ||
                                                protein.Elements().First(x => x.Name.LocalName == "gene_name").Value == "\n") ?
                                                    "" : protein.Elements().First(x => x.Name.LocalName == "gene_name").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                                Protein_type = (string.IsNullOrEmpty(protein.Elements().First(x => x.Name.LocalName == "protein_type").Value) ||
                                                string.IsNullOrWhiteSpace(protein.Elements().First(x => x.Name.LocalName == "protein_type").Value) ||
                                                protein.Elements().First(x => x.Name.LocalName == "protein_type").Value == "\n") ?
                                                    "" : protein.Elements().First(x => x.Name.LocalName == "protein_type").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim()
                            });
                        }
                        break;
                    case "diseases":
                        List_of_diseases = new List<disease>();
                        disease ds;
                        foreach (XElement cdisease in item.Elements().Where(x => x.Name.LocalName == "disease"))
                        {
                            ds = new disease()
                            {
                                Name = (string.IsNullOrEmpty(cdisease.Elements().First(x => x.Name.LocalName == "name").Value) ||
                                        string.IsNullOrWhiteSpace(cdisease.Elements().First(x => x.Name.LocalName == "name").Value) ||
                                        cdisease.Elements().First(x => x.Name.LocalName == "name").Value == "\n") ?
                                        "" : cdisease.Elements().First(x => x.Name.LocalName == "name").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                                Omim_id = (string.IsNullOrEmpty(cdisease.Elements().First(x => x.Name.LocalName == "omim_id").Value) ||
                                        string.IsNullOrWhiteSpace(cdisease.Elements().First(x => x.Name.LocalName == "omim_id").Value) ||
                                        cdisease.Elements().First(x => x.Name.LocalName == "omim_id").Value == "\n") ?
                                        "" : cdisease.Elements().First(x => x.Name.LocalName == "omim_id").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                                List_of_pubmed_ids = new List<string>()
                            };
                            foreach (XElement references in cdisease.Elements().Where(x => x.Name.LocalName == "references"))
                            {
                                foreach (XElement reference in references.Elements().Where(x => x.Name.LocalName == "reference"))
                                {
                                    if (reference.Elements().Any(x => x.Name.LocalName == "pubmed_id") &&
                                        !string.IsNullOrEmpty(reference.Elements().First(x => x.Name.LocalName == "pubmed_id").Value) &&
                                        !string.IsNullOrWhiteSpace(reference.Elements().First(x => x.Name.LocalName == "pubmed_id").Value) &&
                                        reference.Elements().First(x => x.Name.LocalName == "pubmed_id").Value != "\n")
                                    {
                                        ds.List_of_pubmed_ids.Add(reference.Elements().First(x => x.Name.LocalName == "pubmed_id").Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim());
                                    }
                                }
                            }
                            List_of_diseases.Add(ds);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public class taxonomy
        {
            private string description;
            private string direct_parent;
            private string kingdom;
            private string super_class;
            private string tclass;
            private string molecular_framework;
            private List<string> alternative_parents;
            private List<string> substituents;

            public string Description { get { return description; } set { description = value; } }
            public string Direct_parent { get { return direct_parent; } set { direct_parent = value; } }
            public string Kingdom { get { return kingdom; } set { kingdom = value; } }
            public string Super_class { get { return super_class; } set { super_class = value; } }
            public string Tclass { get { return tclass; } set { tclass = value; } }
            public string Molecular_framework { get { return molecular_framework; } set { molecular_framework = value; } }
            public List<string> Alternative_parents { get { return alternative_parents; } set { alternative_parents = value; } }
            public List<string> Substituents { get { return substituents; } set { substituents = value; } }
        }

        public class disease
        {
            private string name;
            private string omim_id;
            private List<string> list_of_pubmed_ids;

            public string Name { get { return name; } set { name = value; } }
            public string Omim_id { get { return omim_id; } set { omim_id = value; } }
            public List<string> List_of_pubmed_ids { get { return list_of_pubmed_ids; } set { list_of_pubmed_ids = value; } }
        }

        public string printLine()
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}{0}{14}{0}{15}{0}{16}{0}{17}{0}{18}{0}{19}{0}{20}{0}{21}" +
                "{0}{22}{0}{23}{0}{24}{0}{25}{0}{26}{0}{27}{0}{28}{0}{29}{0}{30}{0}{31}{0}{32}{0}{33}{0}{34}{0}{35}{0}{36}{0}{37}{0}{38}{0}{39}{0}{40}{0}{41}{0}{42}{0}{43}{0}{44}{0}{45}{0}{46}{0}{47}{0}{48}{0}{49}{0}{50}{0}{51}" +
                "{0}{52}{0}{53}{0}{54}{0}{55}{0}{56}{0}{57}{0}{58}{0}{59}{0}{60}{0}{61}{0}{62}{0}{63}{0}{64}{0}{65}",
                    '\t',
                    Hmdb_accession,
                    (Hmdb_secondary_accessions == null ? "" : string.Join("|", Hmdb_secondary_accessions)),
                    Name,
                    (Synonym_names == null ? "" : string.Join("|", Synonym_names)),
                    Description,
                    Formula,
                    Convert.ToString(Average_molecular_weight),
                    Convert.ToString(Monisotopic_molecular_weight),
                    Iupac_name,
                    Traditional_iupac,
                    Cas_registry_number,
                    (Cts_cas == null ? "" : string.Join("|", Cts_cas)),
                    Smiles,
                    Inchi,
                    Inchikey,
                    (My_taxonomy == null ? "" : My_taxonomy.Description),
                    (My_taxonomy == null ? "" : My_taxonomy.Direct_parent),
                    (My_taxonomy == null ? "" : My_taxonomy.Kingdom),
                    (My_taxonomy == null ? "" : My_taxonomy.Super_class),
                    (My_taxonomy == null ? "" : My_taxonomy.Tclass),
                    (My_taxonomy == null ? "" : My_taxonomy.Molecular_framework),
                    (My_taxonomy == null ? "" : string.Join("|", My_taxonomy.Alternative_parents)),
                    (My_taxonomy == null ? "" : string.Join("|", My_taxonomy.Substituents)),
                    My_onotology.Status,
                    string.Join("|", My_onotology.Origins),
                    string.Join("|", My_onotology.Biofunctions),
                    string.Join("|", My_onotology.Applications),
                    string.Join("|", My_onotology.Cellular_locations),
                    State,
                    (Biofluid_locations == null ? "" : string.Join("|", Biofluid_locations)),
                    (Tissue_locations == null ? "" : string.Join("|", Tissue_locations)),
                    string.Join("|", List_of_pathways.Select(x => x.Kegg_map_id)),
                    string.Join("|", List_of_pathways.Select(x => string.Join(",", x.List_of_names))),
                    string.Join("|", List_of_pathways.Select(x => x.Super_class)),
                    string.Join("|", List_of_pathways.Select(x => x.Pathway_map.Item1 + "-" + x.Pathway_map.Item2)),
                    string.Join("|", List_of_pathways.Select(x => string.Join(",", x.List_of_modules.Select(y => y.Item1 + "-" + y.Item2)))),
                    string.Join("|", List_of_pathways.Select(x => string.Join(",", x.List_of_diseases.Select(y => y.Item1 + "-" + y.Item2)))),
                    string.Join("|", List_of_pathways.Select(x => x.Organism)),
                    string.Join("|", List_of_pathways.Select(x => x.Gene)),
                    string.Join("|", List_of_pathways.Select(x => x.Enzyme)),
                    string.Join("|", List_of_pathways.Select(x => x.Reaction)),
                    string.Join("|", List_of_pathways.Select(x => x.Compound)),
                    string.Join("|", List_of_pathways.Select(x => x.Ko_pathway)),
                    string.Join("|", List_of_pathways.Select(x => x.Rel_pathway)),
                    string.Join("|", List_of_pathways.Select(x => x.Smpdb_map_id)),
                    string.Join("|", List_of_pathways.Select(x => x.Smpdb_map_name)),
                    string.Join("|", List_of_pathways.Select(x => x.Smpadb_map_description)),
                    Drugbank_id,
                    Drugbank_metabolite_id,
                    Chemspider_id,
                    Kegg_id,
                    (Cts_kegg == null ? "" : string.Join("|", Cts_kegg)),
                    Metlin_id,
                    Pubchem_compound_id,
                    Chebi_id,
                    (Cts_chebi == null ? "" : string.Join("|", Cts_chebi)),
                    Synthesis_reference,
                    string.Join("|", List_of_proteins.Select(x => x.Protein_accession)),
                    string.Join("|", List_of_proteins.Select(x => x.Name)),
                    string.Join("|", List_of_proteins.Select(x => x.Uniprot_id)),
                    string.Join("|", List_of_proteins.Select(x => x.Gene_name)),
                    string.Join("|", List_of_proteins.Select(x => x.Protein_type)),
                    string.Join("|", List_of_diseases.Select(x => x.Name)),
                    string.Join("|", List_of_diseases.Select(x => x.Omim_id)),
                    string.Join("|", List_of_diseases.Select(x => string.Join(",", x.List_of_pubmed_ids))));
        }
    }
}
