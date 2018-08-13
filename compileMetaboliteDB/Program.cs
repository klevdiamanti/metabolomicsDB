using System;

namespace compileMetaboliteDB
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("[mono] compileMetaboliteDB.exe hmdb_tsv_file chebi_tsv_file duplicates_file output_file problem_file single_multiple_xml_files_boolean");
                Console.WriteLine();
                Console.WriteLine("This script combines the compiled HMDB database with the ChEBI database.");
                Console.WriteLine();
                Console.WriteLine("Input:");
                Console.WriteLine("1. tsv file with compounds from HMDB as compiled by compileHMDBdata.exe");
                Console.WriteLine("2. tsv file with compounds from ChEBI as compiled by compileChEBIdata.exe");
                Console.WriteLine("3. tsv file with of manually resolved duplicates that have been detected from this script in a previous run");
                Console.WriteLine("4. output database target tsv file");
                Console.WriteLine("5. output tsv file with duplicates and problematic cases");
                Console.WriteLine("6. boolean value denoting if the HMDB file comes from a single or multiple XML file(s)");
                Console.WriteLine("\t\tTRUE: single xml file; FALSE: multiple xml files;");
                Environment.Exit(0);
            }

            //input variables
            string hmdb_file = args[0];
            string chebi_file = args[1];
            string dups_file = args[2];
            string output_file = args[3];
            string problems_file = args[4];
            bool singleOrMultipleXml = Convert.ToBoolean(args[5]);

            metabolitesDB.process_database(hmdb_file, chebi_file, dups_file, output_file, problems_file, singleOrMultipleXml);

            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}
