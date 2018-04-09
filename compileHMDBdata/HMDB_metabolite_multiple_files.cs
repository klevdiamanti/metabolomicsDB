using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace compileHMDBdata
{
    class HMDB_metabolite_multiple_files : HMDB_metabolite
    {
        public taxonomy My_taxonomy { get; set; }
        
        public HMDB_metabolite_multiple_files(string inputFile)
        {
            Cts_cas = new List<string>();
            Cts_kegg = new List<string>();
            Cts_chebi = new List<string>();

            string tmp_string = "";

            XmlReader reader = XmlReader.Create(@"" + inputFile);
            XmlReader subTree;
            XElement subTreeContent;

            #region accession
            reader.ReadToFollowing("accession");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Hmdb_accession = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region secondary_accessions
            Hmdb_secondary_accessions = new List<string>();
            reader.ReadToFollowing("secondary_accessions");
            subTree = reader.ReadSubtree();
            subTreeContent = XElement.Load(subTree);
            foreach (XElement synonym_content in subTreeContent.Descendants("accession"))
            {
                if (string.IsNullOrEmpty(synonym_content.Value) || string.IsNullOrWhiteSpace(synonym_content.Value) || synonym_content.Value == "\n")
                {
                    continue;
                }
                else
                {
                    Hmdb_secondary_accessions.Add(synonym_content.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim());
                }
            }
            #endregion

            #region name
            reader.ReadToFollowing("name");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Name = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region description
            reader.ReadToFollowing("description");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Description = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region synonyms
            Synonym_names = new List<string>();
            reader.ReadToFollowing("synonyms");
            subTree = reader.ReadSubtree();
            subTreeContent = XElement.Load(subTree);
            foreach (XElement synonym_content in subTreeContent.Descendants("synonym"))
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
            #endregion

            #region chemical_formula
            reader.ReadToFollowing("chemical_formula");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Formula = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region average_molecular_weight
            reader.ReadToFollowing("average_molecular_weight");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Average_molecular_weight = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? -1 : Convert.ToDouble(tmp_string);
            #endregion

            #region monisotopic_moleculate_weight
            reader.ReadToFollowing("monisotopic_moleculate_weight");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Monisotopic_molecular_weight = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? -1 : Convert.ToDouble(tmp_string);
            #endregion

            #region iupac_name
            reader.ReadToFollowing("iupac_name");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Iupac_name = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region traditional_iupac
            reader.ReadToFollowing("traditional_iupac");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Traditional_iupac = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region cas_registry_number
            reader.ReadToFollowing("cas_registry_number");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Cas_registry_number = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region smiles
            reader.ReadToFollowing("smiles");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Smiles = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region inchi
            reader.ReadToFollowing("inchi");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim().Split('=').Last();
            Inchi = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region inchikey
            reader.ReadToFollowing("inchikey");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim().Split('=').Last();
            Inchikey = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region taxonomy
            reader.ReadToFollowing("taxonomy");
            subTree = reader.ReadSubtree();
            subTreeContent = XElement.Load(subTree);
            My_taxonomy = new taxonomy()
            {
                Direct_parent = (string.IsNullOrEmpty(subTreeContent.Descendants("direct_parent").First().Value) ||
                                        string.IsNullOrWhiteSpace(subTreeContent.Descendants("direct_parent").First().Value) || subTreeContent.Descendants("direct_parent").First().Value == "\n") ?
                                            "" : subTreeContent.Descendants("direct_parent").First().Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                Kingdom = (string.IsNullOrEmpty(subTreeContent.Descendants("kingdom").First().Value) ||
                                        string.IsNullOrWhiteSpace(subTreeContent.Descendants("kingdom").First().Value) || subTreeContent.Descendants("kingdom").First().Value == "\n") ?
                                            "" : subTreeContent.Descendants("kingdom").First().Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                Super_class = (string.IsNullOrEmpty(subTreeContent.Descendants("super_class").First().Value) ||
                                        string.IsNullOrWhiteSpace(subTreeContent.Descendants("super_class").First().Value) || subTreeContent.Descendants("super_class").First().Value == "\n") ?
                                            "" : subTreeContent.Descendants("super_class").First().Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                Tclass = (string.IsNullOrEmpty(subTreeContent.Descendants("class").First().Value) ||
                                        string.IsNullOrWhiteSpace(subTreeContent.Descendants("class").First().Value) || subTreeContent.Descendants("class").First().Value == "\n") ?
                                            "" : subTreeContent.Descendants("class").First().Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                Substituents = subTreeContent.Descendants("substituents").First().Descendants("substituent").Select(x => x.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim()).ToList(),
                Other_descriptors = subTreeContent.Descendants("other_descriptors").First().Descendants("descriptor").Select(x => x.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim()).ToList()
            };
            #endregion

            #region ontology
            reader.ReadToFollowing("ontology");
            subTree = reader.ReadSubtree();
            subTreeContent = XElement.Load(subTree);
            My_onotology = new ontology()
            {
                Status = (string.IsNullOrEmpty(subTreeContent.Descendants("status").First().Value) ||
                                        string.IsNullOrWhiteSpace(subTreeContent.Descendants("status").First().Value) || subTreeContent.Descendants("status").First().Value == "\n") ?
                                            "" : subTreeContent.Descendants("status").First().Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                Origins = subTreeContent.Descendants("origins").First().Descendants("origin").Select(x => x.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim()).ToList(),
                Biofunctions = subTreeContent.Descendants("biofunctions").First().Descendants("biofunction").Select(x => x.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim()).ToList(),
                Applications = subTreeContent.Descendants("applications").First().Descendants("application").Select(x => x.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim()).ToList(),
                Cellular_locations = subTreeContent.Descendants("cellular_locations").First().Descendants("cellular_location").Select(x => x.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim()).ToList()
            };
            #endregion

            #region state
            reader.ReadToFollowing("state");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            State = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region biofluid_locations
            Biofluid_locations = new List<string>();
            reader.ReadToFollowing("biofluid_locations");
            subTree = reader.ReadSubtree();
            subTreeContent = XElement.Load(subTree);
            foreach (XElement synonym_content in subTreeContent.Descendants("biofluid"))
            {
                if (string.IsNullOrEmpty(synonym_content.Value) || string.IsNullOrWhiteSpace(synonym_content.Value) || synonym_content.Value == "\n")
                {
                    continue;
                }
                else
                {
                    Biofluid_locations.Add(synonym_content.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim());
                }
            }
            #endregion

            #region tissue_locations
            Tissue_locations = new List<string>();
            reader.ReadToFollowing("tissue_locations");
            subTree = reader.ReadSubtree();
            subTreeContent = XElement.Load(subTree);
            foreach (XElement synonym_content in subTreeContent.Descendants("tissue"))
            {
                if (string.IsNullOrEmpty(synonym_content.Value) || string.IsNullOrWhiteSpace(synonym_content.Value) || synonym_content.Value == "\n")
                {
                    continue;
                }
                else
                {
                    Tissue_locations.Add(synonym_content.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim());
                }
            }
            #endregion

            #region pathways
            List_of_pathways = new List<pathway>();
            reader.ReadToFollowing("pathways");
            subTree = reader.ReadSubtree();
            subTreeContent = XElement.Load(subTree);
            string kegg_map_id = "", smpdb_id = "";
            pathway ptwy;
            foreach (XElement pathway_content in subTreeContent.Descendants("pathway"))
            {
                kegg_map_id = (string.IsNullOrEmpty(pathway_content.Descendants("kegg_map_id").First().Value) ||
                    string.IsNullOrWhiteSpace(pathway_content.Descendants("kegg_map_id").First().Value) ||
                    pathway_content.Descendants("kegg_map_id").First().Value == "\n") ?
                    "" : pathway_content.Descendants("kegg_map_id").First().Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                smpdb_id = (string.IsNullOrEmpty(pathway_content.Descendants("smpdb_id").First().Value) ||
                    string.IsNullOrWhiteSpace(pathway_content.Descendants("smpdb_id").First().Value) ||
                    pathway_content.Descendants("smpdb_id").First().Value == "\n") ?
                    "" : pathway_content.Descendants("smpdb_id").First().Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();

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
            #endregion

            #region drugbank_id
            reader.ReadToFollowing("drugbank_id");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Drugbank_id = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region drugbank_metabolite_id
            reader.ReadToFollowing("drugbank_metabolite_id");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Drugbank_metabolite_id = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region chemspider_id
            reader.ReadToFollowing("chemspider_id");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Chemspider_id = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region kegg_id
            Dict_kegg_details = new Dictionary<string, KEGG_entry_details>();
            reader.ReadToFollowing("kegg_id");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Kegg_id = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            addKeggDetails((string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string);
            #endregion

            #region metlin_id
            reader.ReadToFollowing("metlin_id");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Metlin_id = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region pubchem_compound_id
            reader.ReadToFollowing("pubchem_compound_id");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Pubchem_compound_id = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region chebi_id
            reader.ReadToFollowing("chebi_id");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Chebi_id = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region synthesis_reference
            reader.ReadToFollowing("synthesis_reference");
            tmp_string = reader.ReadElementContentAsString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            Synthesis_reference = (string.IsNullOrEmpty(tmp_string) || string.IsNullOrWhiteSpace(tmp_string) || tmp_string == "\n") ? "" : tmp_string;
            #endregion

            #region protein_associations
            List_of_proteins = new List<protein>();
            reader.ReadToFollowing("protein_associations");
            subTree = reader.ReadSubtree();
            subTreeContent = XElement.Load(subTree);
            foreach (XElement pathway_content in subTreeContent.Descendants("protein"))
            {
                List_of_proteins.Add(new protein()
                {
                    Protein_accession = (string.IsNullOrEmpty(pathway_content.Descendants("protein_accession").First().Value) ||
                                        string.IsNullOrWhiteSpace(pathway_content.Descendants("protein_accession").First().Value) || pathway_content.Descendants("protein_accession").First().Value == "\n") ?
                                            "" : pathway_content.Descendants("protein_accession").First().Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                    Name = (string.IsNullOrEmpty(pathway_content.Descendants("name").First().Value) ||
                                        string.IsNullOrWhiteSpace(pathway_content.Descendants("name").First().Value) || pathway_content.Descendants("name").First().Value == "\n") ?
                                            "" : pathway_content.Descendants("name").First().Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                    Uniprot_id = (string.IsNullOrEmpty(pathway_content.Descendants("uniprot_id").First().Value) ||
                                        string.IsNullOrWhiteSpace(pathway_content.Descendants("uniprot_id").First().Value) || pathway_content.Descendants("uniprot_id").First().Value == "\n") ?
                                            "" : pathway_content.Descendants("uniprot_id").First().Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                    Gene_name = (string.IsNullOrEmpty(pathway_content.Descendants("gene_name").First().Value) ||
                                        string.IsNullOrWhiteSpace(pathway_content.Descendants("gene_name").First().Value) || pathway_content.Descendants("gene_name").First().Value == "\n") ?
                                            "" : pathway_content.Descendants("gene_name").First().Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim(),
                    Protein_type = (string.IsNullOrEmpty(pathway_content.Descendants("protein_type").First().Value) ||
                                        string.IsNullOrWhiteSpace(pathway_content.Descendants("protein_type").First().Value) || pathway_content.Descendants("protein_type").First().Value == "\n") ?
                                            "" : pathway_content.Descendants("protein_type").First().Value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim()
                });
            }
            #endregion
        }

        public class taxonomy
        {
            private string direct_parent;
            private string kingdom;
            private string super_class;
            private string tclass;
            private List<string> substituents;
            private List<string> other_descriptors;

            public string Direct_parent { get { return direct_parent; } set { direct_parent = value; } }
            public string Kingdom { get { return kingdom; } set { kingdom = value; } }
            public string Super_class { get { return super_class; } set { super_class = value; } }
            public string Tclass { get { return tclass; } set { tclass = value; } }
            public List<string> Substituents { get { return substituents; } set { substituents = value; } }
            public List<string> Other_descriptors { get { return other_descriptors; } set { other_descriptors = value; } }
        }

        public string printLine()
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}{0}{14}{0}{15}{0}{16}{0}{17}{0}{18}{0}{19}{0}{20}{0}{21}" +
                "{0}{22}{0}{23}{0}{24}{0}{25}{0}{26}{0}{27}{0}{28}{0}{29}{0}{30}{0}{31}{0}{32}{0}{33}{0}{34}{0}{35}{0}{36}{0}{37}{0}{38}{0}{39}{0}{40}{0}{41}{0}{42}{0}{43}{0}{44}{0}{45}{0}{46}{0}{47}{0}{48}{0}{49}{0}{50}{0}{51}" +
                "{0}{52}{0}{53}{0}{54}{0}{55}{0}{56}{0}{57}{0}{58}{0}{59}{0}{60}",
                    '\t',
                    Hmdb_accession,
                    (Hmdb_secondary_accessions == null ? "" : string.Join("|", Hmdb_secondary_accessions)),
                    Name,
                    (Synonym_names == null ? "" : string.Join("|", Synonym_names)),
                    Description,
                    Formula,
                    Average_molecular_weight,
                    Monisotopic_molecular_weight,
                    Iupac_name,
                    Traditional_iupac,
                    Cas_registry_number,
                    (Cts_cas == null ? "" : string.Join("|", Cts_cas)),
                    Smiles,
                    Inchi,
                    Inchikey,
                    (My_taxonomy == null ? "" : My_taxonomy.Direct_parent),
                    (My_taxonomy == null ? "" : My_taxonomy.Kingdom),
                    (My_taxonomy == null ? "" : My_taxonomy.Super_class),
                    (My_taxonomy == null ? "" : My_taxonomy.Tclass),
                    (My_taxonomy == null ? "" : string.Join("|", My_taxonomy.Substituents)),
                    (My_taxonomy == null ? "" : string.Join("|", My_taxonomy.Other_descriptors)),
                    (My_onotology == null ? "" : My_onotology.Status),
                    (My_onotology == null ? "" : string.Join("|", My_onotology.Origins)),
                    (My_onotology == null ? "" : string.Join("|", My_onotology.Biofunctions)),
                    (My_onotology == null ? "" : string.Join("|", My_onotology.Applications)),
                    (My_onotology == null ? "" : string.Join("|", My_onotology.Cellular_locations)),
                    State,
                    (Biofluid_locations == null ? "" : string.Join("|", Biofluid_locations)),
                    (Tissue_locations == null ? "" : string.Join("|", Tissue_locations)),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => x.Kegg_map_id))),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => string.Join(",", x.List_of_names)))),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => x.Super_class))),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => x.Pathway_map.Item1 + "-" + x.Pathway_map.Item2))),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => string.Join(",", x.List_of_modules.Select(y => y.Item1 + "-" + y.Item2))))),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => string.Join(",", x.List_of_diseases.Select(y => y.Item1 + "-" + y.Item2))))),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => x.Organism))),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => x.Gene))),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => x.Enzyme))),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => x.Reaction))),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => x.Compound))),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => x.Ko_pathway))),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => x.Rel_pathway))),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => x.Smpdb_map_id))),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => x.Smpdb_map_name))),
                    (List_of_pathways == null ? "" : string.Join("|", List_of_pathways.Select(x => x.Smpadb_map_description))),
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
                    (List_of_proteins == null ? "" : string.Join("|", List_of_proteins.Select(x => x.Protein_accession))),
                    (List_of_proteins == null ? "" : string.Join("|", List_of_proteins.Select(x => x.Name))),
                    (List_of_proteins == null ? "" : string.Join("|", List_of_proteins.Select(x => x.Uniprot_id))),
                    (List_of_proteins == null ? "" : string.Join("|", List_of_proteins.Select(x => x.Gene_name))),
                    (List_of_proteins == null ? "" : string.Join("|", List_of_proteins.Select(x => x.Protein_type))));
        }
    }
}
