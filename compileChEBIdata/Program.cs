using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace compileChEBIdata
{
    class Program
    {
        static void Main(string[] args)
        {
            //link ftp://ftp.ebi.ac.uk/pub/databases/chebi/Flat_file_tab_delimited/
            //timestamp 2016-12-01
            if (args.Length == 0)
            {
                Console.WriteLine("[mono] compileChEBIdata.exe chebi_dir output_file");
                Console.WriteLine();
                Console.WriteLine("This script combines multiple flat files from ChEBI and generates one tab-separated file.");
                Console.WriteLine();
                Console.WriteLine("The input is a collection of xml files from HMDB (one for each HMDB identifier)");
                Console.WriteLine("and the output a single tab-separated file that combined all the data.");
                Environment.Exit(0);
            }

            string chebi_dir = args[0];
            string output_file = args[1];
            char ossep = getOSsep();

            string chebi_inchi_file = chebi_dir + ossep + "chebiId_inchi.tsv";
            string names_file = chebi_dir + ossep + "names.tsv";
            string database_accession_file = chebi_dir + ossep + "database_accession.tsv";
            string compounds_file = chebi_dir + ossep + "compounds.tsv";
            string comments_file = chebi_dir + ossep + "comments.tsv";
            string chemical_data_file = chebi_dir + ossep + "chemical_data.tsv";

            int inchi = 0, names = 0, database = 0, compounds = 0, comments = 0, chemical = 0;

            //Step 1: parse the ChEBI InChI file
            ChEBI_compounds.parse_chebi_inchi_file(chebi_inchi_file);
            inchi = ChEBI_compounds.dictOf_ChEBI_compounds.Count();
            //Step 2: parse the ChEBI names file
            ChEBI_compounds.parse_names_file(names_file);
            names = ChEBI_compounds.dictOf_ChEBI_compounds.Count() - inchi;
            //Step 3: parse ChEBI database accession file
            ChEBI_compounds.parse_database_accession_file(database_accession_file);
            database = ChEBI_compounds.dictOf_ChEBI_compounds.Count() - (names + inchi);
            //Step 4: parse ChEBI compounds file
            ChEBI_compounds.parse_compounds_file(compounds_file);
            compounds = ChEBI_compounds.dictOf_ChEBI_compounds.Count() - (names + inchi + database);
            //Step 5: parse ChEBI comments file
            ChEBI_compounds.parse_comments_file(comments_file);
            comments = ChEBI_compounds.dictOf_ChEBI_compounds.Count() - (names + inchi + database + compounds);
            //Step 6: parse ChEBI chemical data file
            ChEBI_compounds.parse_chemical_data_file(chemical_data_file);
            chemical = ChEBI_compounds.dictOf_ChEBI_compounds.Count() - (names + inchi + database + compounds + comments);

            Console.WriteLine("ChEBI InChI file " + inchi);
            Console.WriteLine("ChEBI names file " + names);
            Console.WriteLine("ChEBI database file " + database);
            Console.WriteLine("ChEBI compounds file " + compounds);
            Console.WriteLine("ChEBI comments file " + comments);
            Console.WriteLine("ChEBI chemical file " + chemical);
            Console.WriteLine("ChEBI total " + ChEBI_compounds.dictOf_ChEBI_compounds.Count());

            using (TextWriter output = new StreamWriter(@"" + output_file))
            {
                output.WriteLine(string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}{0}{14}{0}{15}{0}{16}{0}{17}{0}{18}{0}{19}{0}{20}{0}{21}{0}{22}{0}{23}{0}{24}{0}{25}{0}{26}{0}{27}{0}{28}{0}{29}",
                                    "\t",
                                    "ChEBI_ID",
                                    "InChI",
                                    "Synonyms",
                                    "CAS_IDs",
                                    "KEGG_IDs",
                                    "HMDB_IDs",
                                    "LipidMaps_IDs",
                                    "PubChem_IDs",
                                    "Name",
                                    "Description",
                                    "Quality",
                                    "Comment",
                                    "Formula",
                                    "Charge",
                                    "Mass",
                                    "Monoisotopic_mass",
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
                                    "Pathways-Kegg_Rel_pathway"));

                foreach (KeyValuePair<string, ChEBI_compound> kvp in ChEBI_compounds.dictOf_ChEBI_compounds)
                {
                    output.WriteLine(kvp.Value.printLine());
                }
            }

            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        static char getOSsep()
        {
            if ((int)Environment.OSVersion.Platform == 2)
            {
                return '\\';
            }
            else
            {
                return '/';
            }
        }
    }
}