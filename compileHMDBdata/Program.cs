using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace compileHMDBdata
{
    class Program
    {
        static void Main(string[] args)
        {
            //link http://www.hmdb.ca/downloads
            //timestamp 2016-12-18
            if (args.Length == 0)
            {
                Console.WriteLine("[mono] compileHMDBdata.exe (hmdb_xml_file OR collection_of_hmdb_xml_files) smpdb_pathway_file output_file");
                Console.WriteLine();
                Console.WriteLine("This script combines multiple files from HMDB and generates one tab-separated file.");
                Console.WriteLine();
                Console.WriteLine("The input is a collection of xml files from HMDB (one for each HMDB identifier) OR ");
                Console.WriteLine("a single xml file with all the metabolites, and a tsv file with the information from SMPDB, ");
                Console.WriteLine("and the output a single tab-separated file that combined all the data.");
                Environment.Exit(0);
            }

            string hmdb_xml = args[0];
            string smpdb_pathways_file = args[1];
            string output_file = args[2];

            SMPDB_pathways.parse_SMPDB_tsv_file(smpdb_pathways_file);

            using (TextWriter output = new StreamWriter(@"" + output_file))
            {
                string header;
                if (File.Exists(@"" + hmdb_xml))
                {
                    #region header line
                    header = string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}{0}{14}{0}{15}{0}{16}{0}{17}{0}{18}{0}{19}{0}{20}{0}{21}" +
                                            "{0}{22}{0}{23}{0}{24}{0}{25}{0}{26}{0}{27}{0}{28}{0}{29}{0}{30}{0}{31}{0}{32}{0}{33}{0}{34}{0}{35}{0}{36}{0}{37}{0}{38}{0}{39}{0}{40}{0}" +
                                            "{41}{0}{42}{0}{43}{0}{44}{0}{45}{0}{46}{0}{47}{0}{48}{0}{49}{0}{50}{0}{51}{0}{52}{0}{53}{0}{54}{0}{55}{0}{56}{0}{57}{0}{58}{0}{59}{0}" +
                                            "{60}{0}{61}{0}{62}{0}{63}{0}{64}{0}{65}",
                    '\t',
                    "HMDB_ID",
                    "Secondary_Accession",
                    "Name",
                    "Synonym_Names",
                    "Description",
                    "Formula",
                    "Avg_Mol_Weight",
                    "MonoIsotopic_Mol_Weight",
                    "iupac_name",
                    "traditional_iupac",
                    "CAS_ID",
                    "Add_CAS_ID",
                    "smiles",
                    "inchi",
                    "inchikey",
                    "Taxonomy-Description",
                    "Taxonomy-Direct_Parent",
                    "Taxonomy-Kingdom",
                    "Taxonomy-Super_Class",
                    "Taxonomy-Class",
                    "Taxonomy-Molecular_Framework",
                    "Taxonomy-Alternative_Parents",
                    "Taxonomy-Substituents",
                    "Ontology-Status",
                    "Ontology-Origins",
                    "Ontology-Biofunctions",
                    "Ontology-Applications",
                    "Ontology-Cellular_Locations",
                    "state",
                    "biofluid_locations",
                    "tissue_locations",
                    "Pathways-Kegg",
                    "Pathways-Kegg_id",
                    "Pathways-Kegg_Class",
                    "Pathways-Kegg_Map",
                    "Pathways-Kegg_Module",
                    "Pathways-Kegg_Disease",
                    "Pathways-Kegg_Organism",
                    "Pathways-Kegg_Gene",
                    "Pathways-Kegg_Enzyme",
                    "Pathways-Kegg_Reaction",
                    "Pathways-Kegg_Compound",
                    "Pathways-Kegg_Ko_pathway",
                    "Pathways-Kegg_Rel_pathway",
                       "Pathways-Smpdb_id",
                       "Pathways-Smpdb_name",
                       "Pathways-Smpdb_description",
                    "drugbank_id",
                    "drugbank_metabolite_id",
                    "chemspider_id",
                    "kegg_id",
                    "Add_kegg_id",
                    "metlin_id",
                    "pubchem_compound_id",
                    "chebi_id",
                    "Add_chebi_id",
                    "synthesis_reference",
                    "Protein-accession",
                    "Protein-name",
                    "Protein-uniprot_id",
                    "Protein-gene_name",
                    "Protein-protein_type",
                    "Disease-name",
                    "Disease-omim_id",
                    "Disease-references");
                    output.WriteLine(header);
                    #endregion

                    using (XmlReader reader = XmlReader.Create(@"" + hmdb_xml))
                    {
                        reader.MoveToContent();
                        HMDB_metabolite_single_file m;
                        int cnt = 1;
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                Console.WriteLine(cnt++);
                                m = new HMDB_metabolite_single_file(XNode.ReadFrom(reader) as XElement);
                                mine_additional_ids.retriveInfoFromCts(m);
                                output.WriteLine(m.printLine());
                            }
                        }
                    }
                }
                else
                {
                    #region header line
                    header = string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}{0}{14}{0}{15}{0}{16}{0}{17}{0}{18}{0}{19}{0}{20}{0}{21}" +
                                            "{0}{22}{0}{23}{0}{24}{0}{25}{0}{26}{0}{27}{0}{28}{0}{29}{0}{30}{0}{31}{0}{32}{0}{33}{0}{34}{0}{35}{0}{36}{0}{37}{0}{38}{0}{39}{0}{40}{0}" +
                                            "{41}{0}{42}{0}{43}{0}{44}{0}{45}{0}{46}{0}{47}{0}{48}{0}{49}{0}{50}{0}{51}{0}{52}{0}{53}{0}{54}{0}{55}{0}{56}{0}{57}{0}{58}{0}{59}{0}{60}",
                    '\t',
                    "HMDB_ID",
                    "Secondary_Accession",
                    "Name",
                    "Synonym_Names",
                    "Description",
                    "Formula",
                    "Avg_Mol_Weight",
                    "MonoIsotopic_Mol_Weight",
                    "iupac_name",
                    "traditional_iupac",
                    "CAS_ID",
                    "Add_CAS_ID",
                    "smiles",
                    "inchi",
                    "inchikey",
                    "Taxonomy-Direct_Parent",
                    "Taxonomy-Kingdom",
                    "Taxonomy-Super_Class",
                    "Taxonomy-Class",
                    "Taxonomy-Substituents",
                    "Taxonomy-Other_Descriptors",
                    "Ontology-Status",
                    "Ontology-Origins",
                    "Ontology-Biofunctions",
                    "Ontology-Applications",
                    "Ontology-Cellular_Locations",
                    "state",
                    "biofluid_locations",
                    "tissue_locations",
                    "Pathways-Kegg",
                    "Pathways-Kegg_id",
                    "Pathways-Kegg_Class",
                    "Pathways-Kegg_Map",
                    "Pathways-Kegg_Module",
                    "Pathways-Kegg_Disease",
                    "Pathways-Kegg_Organism",
                    "Pathways-Kegg_Gene",
                    "Pathways-Kegg_Enzyme",
                    "Pathways-Kegg_Reaction",
                    "Pathways-Kegg_Compound",
                    "Pathways-Kegg_Ko_pathway",
                    "Pathways-Kegg_Rel_pathway",
                    "Pathways-Smpdb_id",
                    "Pathways-Smpdb_name",
                    "Pathways-Smpdb_description",
                    "drugbank_id",
                    "drugbank_metabolite_id",
                    "chemspider_id",
                    "kegg_id",
                    "Add_kegg_id",
                    "metlin_id",
                    "pubchem_compound_id",
                    "chebi_id",
                    "Add_chebi_id",
                    "synthesis_reference",
                    "Protein-accession",
                    "Protein-name",
                    "Protein-uniprot_id",
                    "Protein-gene_name",
                    "Protein-protein_type");
                    output.WriteLine(header);
                    #endregion

                    HMDB_metabolite_multiple_files m;
                    int cnt = 1;
                    foreach (string metaboliteXmlFile in Directory.GetFiles(@"" + hmdb_xml))
                    {
                        Console.WriteLine(cnt++);
                        m = new HMDB_metabolite_multiple_files(metaboliteXmlFile);
                        mine_additional_ids.retriveInfoFromCts(m);
                        output.WriteLine(m.printLine());
                    }
                }
            }
        }
    }
}
