using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace compileChEBIdata
{
    public static class ChEBI_compounds
    {
        public static Dictionary<string, ChEBI_compound> dictOf_ChEBI_compounds = new Dictionary<string, ChEBI_compound>();

        /// <summary>
        /// This method parses the ChEBI InChI file that contains 2 tab-separated columns
        /// If the compound exists in the list of compounds it checks if the InChI name is the same with the previous one
        /// Otherwise it will just add the compound to the list.
        /// If there are duplicates and they do not have the same InChI then keep the one with the smaller number of '?'
        /// </summary>
        /// <param name="chebi_inchi_file">the ChEBI InChI tab-separated file</param>
        public static void parse_chebi_inchi_file(string chebi_inchi_file)
        {
            using (TextReader input = new StreamReader(@"" + chebi_inchi_file))
            {
                string line = input.ReadLine();
                while ((line = input.ReadLine()) != null)
                {
                    if (dictOf_ChEBI_compounds.ContainsKey(line.Split('\t').First()))
                    {
                        if (dictOf_ChEBI_compounds[line.Split('\t').First()].Inchi != line.Split('\t').Last().Split('=').Last())
                        {
                            Console.WriteLine("ChEBI InChI Warning: duplicate ID " + line.Split('\t').First());
                            //Keep the one with the fewer '?'
                            if (line.Split('\t').Last().Split('=').Last().Count(x => x == '?') < dictOf_ChEBI_compounds[line.Split('\t').First()].Inchi.Count(x => x == '?'))
                            {
                                dictOf_ChEBI_compounds[line.Split('\t').First()].Inchi = line.Split('\t').Last().Split('=').Last();
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        ChEBI_compounds.dictOf_ChEBI_compounds.Add(line.Split('\t').First(), new ChEBI_compound()
                        {
                            Id = line.Split('\t').First(),
                            Inchi = line.Split('\t').Last().Split('=').Last()
                        });
                    }
                }
            }
        }

        /// <summary>
        /// This method parses the ChEBI names file that contains 7 tab-separated columns
        /// If the compound exists in the list of compounds it add the information to the correct variable.
        /// Otherwise it will just add the compound to the list and add the information to the correct variable.
        /// </summary>
        /// <param name="names_file"></param>
        public static void parse_names_file(string names_file)
        {
            using (TextReader input = new StreamReader(@"" + names_file))
            {
                string line = input.ReadLine();
                while ((line = input.ReadLine()) != null)
                {
                    if (dictOf_ChEBI_compounds.ContainsKey(line.Split('\t').ElementAt(1)))
                    {
                        dictOf_ChEBI_compounds[line.Split('\t').ElementAt(1)].add_from_names(line.Split('\t').ElementAt(4));
                    }
                    else
                    {
                        dictOf_ChEBI_compounds.Add(line.Split('\t').ElementAt(1), new ChEBI_compound()
                        {
                            Id = line.Split('\t').ElementAt(1)
                        });
                        dictOf_ChEBI_compounds[line.Split('\t').ElementAt(1)].add_from_names(line.Split('\t').ElementAt(4));
                    }
                }
            }
        }

        /// <summary>
        /// This method parses the ChEBI database accessions file that contains 5 tab-separated columns
        /// If the compound exists in the list of compounds it add the information to the correct variable.
        /// Otherwise it will just add the compound to the list and add the information to the correct variable.
        /// </summary>
        /// <param name="names_file"></param>
        public static void parse_database_accession_file(string database_accession_file)
        {
            int totalLines = File.ReadAllLines(@"" + database_accession_file).Count();
            int cnt = 1;

            using (TextReader input = new StreamReader(@"" + database_accession_file))
            {
                string line = input.ReadLine();
                while ((line = input.ReadLine()) != null)
                {
                    if (dictOf_ChEBI_compounds.ContainsKey(line.Split('\t').ElementAt(1)))
                    {
                        dictOf_ChEBI_compounds[line.Split('\t').ElementAt(1)].add_from_database_accessions(line.Split('\t').ElementAt(3), line.Split('\t').ElementAt(4));
                    }
                    else
                    {
                        dictOf_ChEBI_compounds.Add(line.Split('\t').ElementAt(1), new ChEBI_compound()
                        {
                            Id = line.Split('\t').ElementAt(1)
                        });
                        dictOf_ChEBI_compounds[line.Split('\t').ElementAt(1)].add_from_database_accessions(line.Split('\t').ElementAt(3), line.Split('\t').ElementAt(4));
                    }

                    if ((cnt % 1000) == 0)
                    {
                        Console.WriteLine(cnt++ + "/" + totalLines);
                    }
                }
            }
        }

        /// <summary>
        /// This method parses the ChEBI compounds file that contains 10 tab-separated columns
        /// If the compound exists in the list of compounds it add the information to the correct variable.
        /// Otherwise it will just add the compound to the list and add the information to the correct variable.
        /// Here we will keep only the name, definition and quality of the compound.
        /// </summary>
        /// <param name="names_file"></param>
        public static void parse_compounds_file(string compounds_file)
        {
            using (TextReader input = new StreamReader(@"" + compounds_file))
            {
                string line = input.ReadLine();
                while ((line = input.ReadLine()) != null)
                {
                    if (dictOf_ChEBI_compounds.ContainsKey(line.Split('\t').First()))
                    {
                        dictOf_ChEBI_compounds[line.Split('\t').First()].Name = line.Split('\t').ElementAt(5);
                        dictOf_ChEBI_compounds[line.Split('\t').First()].Description = line.Split('\t').ElementAt(6);
                        dictOf_ChEBI_compounds[line.Split('\t').First()].Quality = line.Split('\t').Last();
                    }
                    else
                    {
                        dictOf_ChEBI_compounds.Add(line.Split('\t').First(), new ChEBI_compound()
                        {
                            Id = line.Split('\t').First(),
                            Name = line.Split('\t').ElementAt(5),
                            Description = line.Split('\t').ElementAt(6),
                            Quality = line.Split('\t').Last()
                        });
                    }
                }
            }
        }

        /// <summary>
        /// This method parses the ChEBI comments file that contains 6 tab-separated columns
        /// If the compound exists in the list of compounds it add the information to the correct variable.
        /// Otherwise it will just add the compound to the list and add the information to the correct variable.
        /// Here we will keep only the comment field of the compound.
        /// </summary>
        /// <param name="names_file"></param>
        public static void parse_comments_file(string comments_file)
        {
            using (TextReader input = new StreamReader(@"" + comments_file))
            {
                string line = input.ReadLine();
                while ((line = input.ReadLine()) != null)
                {
                    if (dictOf_ChEBI_compounds.ContainsKey(line.Split('\t').ElementAt(1)))
                    {
                        dictOf_ChEBI_compounds[line.Split('\t').ElementAt(1)].Comment = line.Split('\t').ElementAt(5);
                    }
                    else
                    {
                        dictOf_ChEBI_compounds.Add(line.Split('\t').ElementAt(1), new ChEBI_compound()
                        {
                            Id = line.Split('\t').ElementAt(1),
                            Comment = line.Split('\t').ElementAt(5)
                        });
                    }
                }
            }
        }

        /// <summary>
        /// This method parses the ChEBI chemical data file that contains 5 tab-separated columns
        /// If the compound exists in the list of compounds it add the information to the correct variable.
        /// Otherwise it will just add the compound to the list and add the information to the correct variable.
        /// Here we will keep only the comment field of the compound.
        /// </summary>
        /// <param name="names_file"></param>
        public static void parse_chemical_data_file(string chemical_data_file)
        {
            using (TextReader input = new StreamReader(@"" + chemical_data_file))
            {
                string line = input.ReadLine();
                while ((line = input.ReadLine()) != null)
                {
                    if (dictOf_ChEBI_compounds.ContainsKey(line.Split('\t').ElementAt(1)))
                    {
                        dictOf_ChEBI_compounds[line.Split('\t').ElementAt(1)].add_from_chemical_data(line.Split('\t').ElementAt(3), line.Split('\t').Last());
                    }
                    else
                    {
                        dictOf_ChEBI_compounds.Add(line.Split('\t').ElementAt(1), new ChEBI_compound()
                        {
                            Id = line.Split('\t').ElementAt(1)
                        });
                        dictOf_ChEBI_compounds[line.Split('\t').ElementAt(1)].add_from_chemical_data(line.Split('\t').ElementAt(3), line.Split('\t').Last());
                    }
                }
            }
        }
    }
}
