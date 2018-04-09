using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace compileHMDBdata
{
    public class HMDB_metabolite
    {
        public string Hmdb_accession { get; set; }
        public List<string> Hmdb_secondary_accessions { get; set; }
        public string Name { get; set; }
        public List<string> Synonym_names { get; set; }
        public string Description { get; set; }
        public string Formula { get; set; }
        public double Average_molecular_weight { get; set; }
        public double Monisotopic_molecular_weight { get; set; }
        public string Iupac_name { get; set; }
        public string Traditional_iupac { get; set; }
        public string Cas_registry_number { get; set; }
        public List<string> Cts_cas { get; set; }
        public string Smiles { get; set; }
        public string Inchi { get; set; }
        public string Inchikey { get; set; }
        public ontology My_onotology { get; set; }
        public string State { get; set; }
        public List<string> Biofluid_locations { get; set; }
        public List<string> Tissue_locations { get; set; }
        public List<pathway> List_of_pathways { get; set; }
        public string Drugbank_id { get; set; }
        public string Drugbank_metabolite_id { get; set; }
        public string Chemspider_id { get; set; }
        public string Kegg_id { get; set; }
        public List<string> Cts_kegg { get; set; }
        public Dictionary<string, KEGG_entry_details> Dict_kegg_details { get; set; }
        public string Metlin_id { get; set; }
        public string Pubchem_compound_id { get; set; }
        public string Chebi_id { get; set; }
        public List<string> Cts_chebi { get; set; }
        public string Synthesis_reference { get; set; }
        public List<protein> List_of_proteins { get; set; }

        public class ontology
        {
            private string status;
            private List<string> origins;
            private List<string> biofunctions;
            private List<string> applications;
            private List<string> cellular_locations;

            public string Status { get { return status; } set { status = value; } }
            public List<string> Origins { get { return origins; } set { origins = value; } }
            public List<string> Biofunctions { get { return biofunctions; } set { biofunctions = value; } }
            public List<string> Applications { get { return applications; } set { applications = value; } }
            public List<string> Cellular_locations { get { return cellular_locations; } set { cellular_locations = value; } }
        }

        public class protein
        {
            private string protein_accession;
            private string name;
            private string uniprot_id;
            private string gene_name;
            private string protein_type;

            public string Protein_accession { get { return protein_accession; } set { protein_accession = value; } }
            public string Name { get { return name; } set { name = value; } }
            public string Uniprot_id { get { return uniprot_id; } set { uniprot_id = value; } }
            public string Gene_name { get { return gene_name; } set { gene_name = value; } }
            public string Protein_type { get { return protein_type; } set { protein_type = value; } }
                 }

        public void addKeggDetails(string v)
        {
			if (!Dict_kegg_details.ContainsKey(v))
			{
				Dict_kegg_details.Add(v, new KEGG_entry_details(v));
				if (Dict_kegg_details[v].metadata)
				{
					foreach (pathway p in Dict_kegg_details[v].listOfPathway)
					{
						if (!List_of_pathways.Any(x => x.Kegg_map_id == p.Kegg_map_id))
						{
							List_of_pathways.Add(p);
						}
					}
				}
			}
        }
    }
}
