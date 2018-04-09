using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace compileHMDBdata
{
	public static class SMPDB_pathways
	{
		public static List<SMPDB_pathway> list_of_smpdb_pathways = new List<SMPDB_pathway>();

		public static void parse_SMPDB_tsv_file(string SMPDB_file)
		{
			using (TextReader input = new StreamReader(@"" + SMPDB_file))
			{
				string line = input.ReadLine();
				while ((line = input.ReadLine()) != null)
				{
					list_of_smpdb_pathways.Add(new SMPDB_pathway()
					{
						Id = line.Split('\t').First(),
						Name = line.Split('\t').ElementAt(1),
						Subject = line.Split('\t').ElementAt(2),
						Description = line.Split('\t').ElementAt(3),
						Metabolite_id = line.Split('\t').ElementAt(4),
						Metabolite_name = line.Split('\t').ElementAt(5),
						Hmdb_id = line.Split('\t').ElementAt(6),
						Kegg_id = line.Split('\t').ElementAt(7),
						Chebi_id = line.Split('\t').ElementAt(8),
						Drugbank_id = line.Split('\t').ElementAt(9),
						Cas_id = line.Split('\t').ElementAt(10),
						Fromula = line.Split('\t').ElementAt(11),
						Iupac = line.Split('\t').ElementAt(12),
						Smiles = line.Split('\t').ElementAt(13),
						Inchi = line.Split('\t').ElementAt(14),
						Inchi_key = line.Split('\t').ElementAt(15)
					});
				}
			}
		}
	}

	public class SMPDB_pathway
	{
		private string id;
		private string name;
		private string subject;
		private string description;
		private string metabolite_id;
		private string metabolite_name;
		private string hmdb_id;
		private string kegg_id;
		private string chebi_id;
		private string drugbank_id;
		private string cas_id;
		private string fromula;
		private string iupac;
		private string smiles;
		private string inchi;
		private string inchi_key;

		public string Id { get { return id; } set { id = value; } }
		public string Name { get { return name; } set { name = value; } }
		public string Subject { get { return subject; } set { subject = value; } }
		public string Description { get { return description; } set { description = value; } }
		public string Metabolite_id { get { return metabolite_id; } set { metabolite_id = value; } }
		public string Metabolite_name { get { return metabolite_name; } set { metabolite_name = value; } }
		public string Hmdb_id { get { return hmdb_id; } set { hmdb_id = value; } }
		public string Kegg_id { get { return kegg_id; } set { kegg_id = value; } }
		public string Chebi_id { get { return chebi_id; } set { chebi_id = value; } }
		public string Drugbank_id { get { return drugbank_id; } set { drugbank_id = value; } }
		public string Cas_id { get { return cas_id; } set { cas_id = value; } }
		public string Fromula { get { return fromula; } set { fromula = value; } }
		public string Iupac { get { return iupac; } set { iupac = value; } }
		public string Smiles { get { return smiles; } set { smiles = value; } }
		public string Inchi { get { return inchi; } set { inchi = value; } }
		public string Inchi_key { get { return inchi_key; } set { inchi_key = value; } }
	}
}
