﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace compileMetaboliteDB
{
	public static class metabolitesDB
	{
		private static Dictionary<string, metabolite> dictOfMetabolitesFromDB = new Dictionary<string, metabolite>();
        private static Dictionary<string, duplicatesToUnique> dictDuplicates = new Dictionary<string, duplicatesToUnique>();
        private static Dictionary<string, primarySecondaryIDs> dictionaryOfIDs = new Dictionary<string, primarySecondaryIDs>();
        private static Dictionary<string, string> dictionaryOfUniqueIDs = new Dictionary<string, string>();

        public static void process_database(string hmdb_file, string chebi_file, string dups_file, string output_file, string problems_file, bool singleOrMultipleXml)
        {
            read_from_hmdb_database(hmdb_file, singleOrMultipleXml);

            Console.WriteLine("####HMDB INPUT DATA");
            Console.WriteLine("TOTAL METABOLITES: " + dictOfMetabolitesFromDB.Count);
            Console.WriteLine("EMPTY OR MULTIPLE RECORDS: " + dictionaryOfIDs.Count(x => x.Value.listOfHMDB.Count != 1 && string.IsNullOrEmpty(x.Value.primaryHMDB) && string.IsNullOrWhiteSpace(x.Value.primaryHMDB)));
            Console.WriteLine("EMPTY RECORDS: " + dictionaryOfIDs.Count(x => x.Value.listOfHMDB.Count < 1 && string.IsNullOrEmpty(x.Value.primaryHMDB) && string.IsNullOrWhiteSpace(x.Value.primaryHMDB)));
            Console.WriteLine("MULTIPLE RECORDS: " + dictionaryOfIDs.Count(x => x.Value.listOfHMDB.Count > 1 && string.IsNullOrEmpty(x.Value.primaryHMDB) && string.IsNullOrWhiteSpace(x.Value.primaryHMDB)));
            Console.WriteLine();

            dictionaryOfUniqueIDs = dictionaryOfIDs.ToDictionary(x => x.Key, x => ((x.Value.listOfHMDB.Count > 0) ? x.Value.listOfHMDB.First() : "-1"));

            read_from_chebi_database(chebi_file, singleOrMultipleXml);
            detect_duplicate_ids();

            Console.WriteLine("####ChEBI INPUT DATA");
            Console.WriteLine("TOTAL METABOLITES: " + dictOfMetabolitesFromDB.Count);
            Console.WriteLine("EMPTY OR MULTIPLE RECORDS: " + dictionaryOfIDs.Count(x => x.Value.listOfHMDB.Count != 1 && string.IsNullOrEmpty(x.Value.primaryHMDB) && string.IsNullOrWhiteSpace(x.Value.primaryHMDB)));
            Console.WriteLine("EMPTY RECORDS: " + dictionaryOfIDs.Count(x => x.Value.listOfHMDB.Count < 1 && string.IsNullOrEmpty(x.Value.primaryHMDB) && string.IsNullOrWhiteSpace(x.Value.primaryHMDB)));
            Console.WriteLine("MULTIPLE RECORDS: " + dictionaryOfIDs.Count(x => x.Value.listOfHMDB.Count > 1 && string.IsNullOrEmpty(x.Value.primaryHMDB) && string.IsNullOrWhiteSpace(x.Value.primaryHMDB)));
            Console.WriteLine();

            read_from_duplicates_file(dups_file);
            remove_non_matching_duplicates();
            remove_duplicate_hmdb();
            add_new_IDs();
            remove_duplicates();
            fill_in_empty_IDs();

            Console.WriteLine("####PROCESSED DATA");
            Console.WriteLine("TOTAL METABOLITES: " + dictOfMetabolitesFromDB.Count);
            Console.WriteLine("EMPTY OR MULTIPLE RECORDS: " + dictionaryOfIDs.Count(x => x.Value.listOfHMDB.Count != 1 && string.IsNullOrEmpty(x.Value.primaryHMDB) && string.IsNullOrWhiteSpace(x.Value.primaryHMDB)));
            Console.WriteLine("EMPTY RECORDS: " + dictionaryOfIDs.Count(x => x.Value.listOfHMDB.Count < 1 && string.IsNullOrEmpty(x.Value.primaryHMDB) && string.IsNullOrWhiteSpace(x.Value.primaryHMDB)));
            Console.WriteLine("MULTIPLE RECORDS: " + dictionaryOfIDs.Count(x => x.Value.listOfHMDB.Count > 1 && string.IsNullOrEmpty(x.Value.primaryHMDB) && string.IsNullOrWhiteSpace(x.Value.primaryHMDB)));
            Console.WriteLine();

            printProblematicMetabolites(problems_file);

            printDatabase(output_file);
        }

        private static void read_from_hmdb_database(string hmdb_file, bool singleOrMultipleXml) //true if single XML
		{
			//read the all hmdb compounds file
			using (TextReader input = new StreamReader(@"" + hmdb_file))
			{
				string line = input.ReadLine();
                metabolite hmdb_metabolite;
				while ((line = input.ReadLine()) != null)
				{
                    hmdb_metabolite = new metabolite();
                    if (singleOrMultipleXml)
                    {
                        hmdb_metabolite.add_data_from_HMDB_singe_file(line);
                    }
                    else
                    {
                        hmdb_metabolite.add_data_from_HMDB_multiple_files(line);
                    }
                    dictOfMetabolitesFromDB.Add(hmdb_metabolite.Hmdb_accession, hmdb_metabolite);

                    //HMDB
                    if (!string.IsNullOrEmpty(hmdb_metabolite.Hmdb_accession) && !string.IsNullOrWhiteSpace(hmdb_metabolite.Hmdb_accession))
                    {
                        if (!dictionaryOfIDs.ContainsKey(hmdb_metabolite.Hmdb_accession))
                        {
                            dictionaryOfIDs.Add(hmdb_metabolite.Hmdb_accession, new primarySecondaryIDs(hmdb_metabolite.Hmdb_accession));
                        }
                        else
                        {
                            if (!dictionaryOfIDs[hmdb_metabolite.Hmdb_accession].listOfHMDB.Contains(hmdb_metabolite.Hmdb_accession))
                            {
                                dictionaryOfIDs[hmdb_metabolite.Hmdb_accession].listOfHMDB.Add(hmdb_metabolite.Hmdb_accession);
                            }
                            //Console.WriteLine("WARNING: HMDB ID " + hmdb_metabolite.Hmdb_accession);
                        }
                    }
                    foreach (string sah in hmdb_metabolite.Hmdb_secondary_accessions)
                    {
                        if (!string.IsNullOrEmpty(sah) && !string.IsNullOrWhiteSpace(sah))
                        {
                            if (!dictionaryOfIDs.ContainsKey(sah))
                            {
                                dictionaryOfIDs.Add(sah, new primarySecondaryIDs(hmdb_metabolite.Hmdb_accession));
                            }
                            else
                            {
                                if (!dictionaryOfIDs[sah].listOfHMDB.Contains(hmdb_metabolite.Hmdb_accession))
                                {
                                    dictionaryOfIDs[sah].listOfHMDB.Add(hmdb_metabolite.Hmdb_accession);
                                }
                                //Console.WriteLine("WARNING: add HMDB ID " + hmdb_metabolite.Hmdb_accession);
                            }
                        }
                    }

                    //CAS
                    if (!string.IsNullOrEmpty(hmdb_metabolite.Cas_registry_number) && !string.IsNullOrWhiteSpace(hmdb_metabolite.Cas_registry_number))
                    {
                        if (!dictionaryOfIDs.ContainsKey(hmdb_metabolite.Cas_registry_number))
                        {
                            dictionaryOfIDs.Add(hmdb_metabolite.Cas_registry_number, new primarySecondaryIDs(hmdb_metabolite.Hmdb_accession));
                        }
                        else
                        {
                            if (!dictionaryOfIDs[hmdb_metabolite.Cas_registry_number].listOfHMDB.Contains(hmdb_metabolite.Hmdb_accession))
                            {
                                dictionaryOfIDs[hmdb_metabolite.Cas_registry_number].listOfHMDB.Add(hmdb_metabolite.Hmdb_accession);
                            }
                            //Console.WriteLine("WARNING: CAS ID " + hmdb_metabolite.Cas_registry_number);
                        }
                    }
                    foreach (string cc in hmdb_metabolite.Cts_cas)
                    {
                        if (!string.IsNullOrEmpty(cc) && !string.IsNullOrWhiteSpace(cc))
                        {
                            if (!dictionaryOfIDs.ContainsKey(cc))
                            {
								dictionaryOfIDs.Add(cc, new primarySecondaryIDs(hmdb_metabolite.Hmdb_accession));
                            }
                            else
                            {
                                if (!dictionaryOfIDs[cc].listOfHMDB.Contains(hmdb_metabolite.Hmdb_accession))
								{
									dictionaryOfIDs[cc].listOfHMDB.Add(hmdb_metabolite.Hmdb_accession);
								}
								//Console.WriteLine("WARNING: add CAS ID " + cc);
                            }
                        }
                    }

                    //KEGG
                    if (!string.IsNullOrEmpty(hmdb_metabolite.Kegg_id) && !string.IsNullOrWhiteSpace(hmdb_metabolite.Kegg_id))
                    {
                    	if (!dictionaryOfIDs.ContainsKey(hmdb_metabolite.Kegg_id))
                    	{
							dictionaryOfIDs.Add(hmdb_metabolite.Kegg_id, new primarySecondaryIDs(hmdb_metabolite.Hmdb_accession));
                    	}
                    	else
                    	{
                    		if (!dictionaryOfIDs[hmdb_metabolite.Kegg_id].listOfHMDB.Contains(hmdb_metabolite.Hmdb_accession))
							{
								dictionaryOfIDs[hmdb_metabolite.Kegg_id].listOfHMDB.Add(hmdb_metabolite.Hmdb_accession);
							}
							//Console.WriteLine("WARNING: KEGG ID " + hmdb_metabolite.Kegg_id);
						}
                    }
                    foreach (string ck in hmdb_metabolite.Cts_kegg)
                    {
                    	if (!string.IsNullOrEmpty(ck) && !string.IsNullOrWhiteSpace(ck))
                    	{
                    		if (!dictionaryOfIDs.ContainsKey(ck))
                    		{
								dictionaryOfIDs.Add(ck, new primarySecondaryIDs(hmdb_metabolite.Hmdb_accession));
                    		}
                    		else
                    		{
                    			if (!dictionaryOfIDs[ck].listOfHMDB.Contains(hmdb_metabolite.Hmdb_accession))
								{
									dictionaryOfIDs[ck].listOfHMDB.Add(hmdb_metabolite.Hmdb_accession);
								}
								//Console.WriteLine("WARNING: add KEGG ID " + ck);
							}
                    	}
                    }

                    //ChEBI
                    if (!string.IsNullOrEmpty(hmdb_metabolite.Chebi_id) && !string.IsNullOrWhiteSpace(hmdb_metabolite.Chebi_id))
                    {
                    	if (!dictionaryOfIDs.ContainsKey(hmdb_metabolite.Chebi_id))
                    	{
							dictionaryOfIDs.Add(hmdb_metabolite.Chebi_id, new primarySecondaryIDs(hmdb_metabolite.Hmdb_accession));
                    	}
                    	else
                    	{
                    		if (!dictionaryOfIDs[hmdb_metabolite.Chebi_id].listOfHMDB.Contains(hmdb_metabolite.Hmdb_accession))
							{
								dictionaryOfIDs[hmdb_metabolite.Chebi_id].listOfHMDB.Add(hmdb_metabolite.Hmdb_accession);
							}
							//Console.WriteLine("WARNING: ChEBI ID " + hmdb_metabolite.Chebi_id);
						}
                    }
                    foreach (string cc in hmdb_metabolite.Cts_chebi)
                    {
                    	if (!string.IsNullOrEmpty(cc) && !string.IsNullOrWhiteSpace(cc))
                    	{
                    		if (!dictionaryOfIDs.ContainsKey(cc))
                    		{
								dictionaryOfIDs.Add(cc, new primarySecondaryIDs(hmdb_metabolite.Hmdb_accession));
                    		}
                    		else
                    		{
                    			if (!dictionaryOfIDs[cc].listOfHMDB.Contains(hmdb_metabolite.Hmdb_accession))
								{
									dictionaryOfIDs[cc].listOfHMDB.Add(hmdb_metabolite.Hmdb_accession);
								}
								//Console.WriteLine("WARNING: add ChEBI ID " + cc);
							}
                    	}
                    }
                }
			}
        }

        //read from the curated file that fixes duplicates
        private static void read_from_duplicates_file(string dups_file)
        {
            //read the curated list of metabolites to fix duplicates
            using (TextReader input = new StreamReader(@"" + dups_file))
            {
                string line = input.ReadLine();
                List<string> breakAtSep;
                int randCnt = 1;
                while ((line = input.ReadLine()) != null)
                {
                    breakAtSep = line.Split('\t').ToList();
                    if (dictDuplicates.ContainsKey(breakAtSep.First()))
                    {
                        Console.WriteLine("WARNING: duplicate in duplicates file!");
                    }
                    else
                    {
                        dictDuplicates.Add(((!string.IsNullOrEmpty(breakAtSep.First()) && !string.IsNullOrWhiteSpace(breakAtSep.First())) ? breakAtSep.First() : "random" + randCnt++), new duplicatesToUnique()
                        {
                            hmdb_additional = (!string.IsNullOrEmpty(breakAtSep.ElementAt(1)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(1))) ? breakAtSep.ElementAt(1).Split('|').ToList() : new List<string>(),
                            cas = (!string.IsNullOrEmpty(breakAtSep.ElementAt(2)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(2))) ? breakAtSep.ElementAt(2) : "",
                            cas_additional = (!string.IsNullOrEmpty(breakAtSep.ElementAt(3)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(3))) ? breakAtSep.ElementAt(3).Split('|').ToList() : new List<string>(),
                            kegg = (!string.IsNullOrEmpty(breakAtSep.ElementAt(4)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(4))) ? breakAtSep.ElementAt(4) : "",
                            chebi = (!string.IsNullOrEmpty(breakAtSep.ElementAt(5)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(5))) ? breakAtSep.ElementAt(5) : "",
                            chebi_additional= (!string.IsNullOrEmpty(breakAtSep.ElementAt(6)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(6))) ? breakAtSep.ElementAt(6) : "",
                            action = (!string.IsNullOrEmpty(breakAtSep.ElementAt(7)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(7))) ? breakAtSep.ElementAt(7) : "",
                            comment = (!string.IsNullOrEmpty(breakAtSep.ElementAt(8)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(8))) ? breakAtSep.ElementAt(8) : "",
                            isProblematic = (breakAtSep.ElementAt(7) == "NO") ? false : true
                        });
                    }
                }
            }
        }

        //remove IDs from duplicates that do not really match to the suggested ones
        private static void remove_non_matching_duplicates()
        {
            //Dictionary<string, duplicatesToUnique> toRemove = dictDuplicates.Where(x => x.Value.action == "REMOVE").ToDictionary(x => x.Key, x => x.Value);
            foreach (KeyValuePair<string, duplicatesToUnique> kvp_dtu_remove in dictDuplicates.Where(x => x.Value.action == "REMOVE").ToDictionary(x => x.Key, x => x.Value))
            {
                if (!string.IsNullOrEmpty(kvp_dtu_remove.Value.cas) && !string.IsNullOrEmpty(kvp_dtu_remove.Value.cas))
                {
                    if (dictionaryOfIDs.ContainsKey(kvp_dtu_remove.Value.cas))
                    {
                        dictionaryOfIDs.Remove(kvp_dtu_remove.Value.cas);
                    }
                    else
                    {
                        Console.WriteLine("WARNING: CAS is missing " + kvp_dtu_remove.Value.cas);
                    }
                }
                else if (!string.IsNullOrEmpty(kvp_dtu_remove.Value.kegg) && !string.IsNullOrEmpty(kvp_dtu_remove.Value.kegg))
                {
                    if (dictionaryOfIDs.ContainsKey(kvp_dtu_remove.Value.kegg))
                    {
                        dictionaryOfIDs.Remove(kvp_dtu_remove.Value.kegg);
                    }
                    else
                    {
                        Console.WriteLine("WARNING: KEGG is missing " + kvp_dtu_remove.Value.kegg);
                    }
                }
                else if (!string.IsNullOrEmpty(kvp_dtu_remove.Value.chebi) && !string.IsNullOrEmpty(kvp_dtu_remove.Value.chebi))
                {
                    if (dictionaryOfIDs.ContainsKey(kvp_dtu_remove.Value.chebi))
                    {
                        dictionaryOfIDs.Remove(kvp_dtu_remove.Value.chebi);
                    }
                    else
                    {
                        Console.WriteLine("WARNING: ChEBI is missing " + kvp_dtu_remove.Value.chebi);
                    }
                }
            }
        }

        private static void remove_duplicate_hmdb()
        {
            foreach (KeyValuePair<string,primarySecondaryIDs> kvp_dp in dictionaryOfIDs.Where(x => x.Value.listOfHMDB.Count > 1))
            {
                if (kvp_dp.Key.StartsWith("HMDB"))
                {
                    kvp_dp.Value.listOfHMDB.RemoveAll(x => x != kvp_dp.Key);
                }
            }
        }

        //add the IDs the do not match to the suggested metabolites but to others
        private static void add_new_IDs()
        {
            foreach (KeyValuePair<string, duplicatesToUnique> kvp_dtu_add in dictDuplicates.Where(x => x.Value.action == "ADD").ToDictionary(x => x.Key, x => x.Value))
            {
                if (!string.IsNullOrEmpty(kvp_dtu_add.Value.cas) && !string.IsNullOrEmpty(kvp_dtu_add.Value.cas))
                {
                    if (dictionaryOfIDs.ContainsKey(kvp_dtu_add.Value.cas))
                    {
                        dictionaryOfIDs[kvp_dtu_add.Value.cas].listOfHMDB.Add(kvp_dtu_add.Key);
                        dictionaryOfIDs[kvp_dtu_add.Value.cas].secondaryHMDB.Add(kvp_dtu_add.Key);
                        Console.WriteLine("WARNING: CAS already exists " + kvp_dtu_add.Value.cas);
                    }
                    else
                    {
                        dictionaryOfIDs.Add(kvp_dtu_add.Value.cas, new primarySecondaryIDs(kvp_dtu_add.Key));
                    }
                }
                else if (!string.IsNullOrEmpty(kvp_dtu_add.Value.kegg) && !string.IsNullOrEmpty(kvp_dtu_add.Value.kegg))
                {
                    if (dictionaryOfIDs.ContainsKey(kvp_dtu_add.Value.kegg))
                    {
                        dictionaryOfIDs[kvp_dtu_add.Value.kegg].listOfHMDB.Add(kvp_dtu_add.Key);
                        dictionaryOfIDs[kvp_dtu_add.Value.kegg].secondaryHMDB.Add(kvp_dtu_add.Key);
                        Console.WriteLine("WARNING: KEGG already exists " + kvp_dtu_add.Value.kegg);
                    }
                    else
                    {
                        dictionaryOfIDs.Add(kvp_dtu_add.Value.kegg, new primarySecondaryIDs(kvp_dtu_add.Key));
                    }
                }
                else if (!string.IsNullOrEmpty(kvp_dtu_add.Value.chebi) && !string.IsNullOrEmpty(kvp_dtu_add.Value.chebi))
                {
                    if (dictionaryOfIDs.ContainsKey(kvp_dtu_add.Value.chebi))
                    {
                        dictionaryOfIDs[kvp_dtu_add.Value.chebi].listOfHMDB.Add(kvp_dtu_add.Key);
                        dictionaryOfIDs[kvp_dtu_add.Value.chebi].secondaryHMDB.Add(kvp_dtu_add.Key);
                        Console.WriteLine("WARNING: ChEBI already exists " + kvp_dtu_add.Value.chebi);
                    }
                    else
                    {
                        dictionaryOfIDs.Add(kvp_dtu_add.Value.chebi, new primarySecondaryIDs(kvp_dtu_add.Key));
                    }
                }
            }
        }

        //remove duplicates from curated file
        private static void remove_duplicates()
        {
            //remove duplicates
            foreach (KeyValuePair<string, duplicatesToUnique> kvp_dtu in dictDuplicates.Where(x => x.Value.action != "ADD" && x.Value.action != "REMOVE").ToDictionary(x => x.Key, x => x.Value))
            {
                if (kvp_dtu.Value.hmdb_additional.Count != 0)
                {
                    foreach (string s in kvp_dtu.Value.hmdb_additional)
                    {
                        if (dictionaryOfIDs.ContainsKey(s) && dictionaryOfIDs[s].listOfHMDB.Count > 1)
                        {
                            dictionaryOfIDs[s].primaryHMDB = kvp_dtu.Key;
                            dictionaryOfIDs[s].secondaryHMDB = kvp_dtu.Value.hmdb_additional;
                            dictionaryOfIDs[s].removeIDs();
                            dictOfMetabolitesFromDB[kvp_dtu.Key].IsProblematic = kvp_dtu.Value.isProblematic;
                        }
                        else if (!dictionaryOfIDs.ContainsKey(s))
                        {
                            Console.WriteLine("WARNING: HMDB is missing " + s);
                        }
                    }

                    remove_all_non_matching(kvp_dtu.Value.cas, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "CAS", kvp_dtu.Value.isProblematic);
                    foreach (string s in kvp_dtu.Value.cas_additional)
                    {
                        remove_all_non_matching(s, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "additional CAS", kvp_dtu.Value.isProblematic);
                    }
                    remove_all_non_matching(kvp_dtu.Value.kegg, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "KEGG", kvp_dtu.Value.isProblematic);
                    remove_all_non_matching(kvp_dtu.Value.chebi, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "ChEBI", kvp_dtu.Value.isProblematic);
                    remove_all_non_matching(kvp_dtu.Value.chebi_additional, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "additional ChEBI", kvp_dtu.Value.isProblematic);
                }
                else if (!string.IsNullOrEmpty(kvp_dtu.Value.cas) && !string.IsNullOrWhiteSpace(kvp_dtu.Value.cas))
                {
                    remove_all_non_matching(kvp_dtu.Value.cas, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "CAS", kvp_dtu.Value.isProblematic);
                    foreach (string s in kvp_dtu.Value.cas_additional)
                    {
                        remove_all_non_matching(s, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "additional CAS", kvp_dtu.Value.isProblematic);
                    }
                    remove_all_non_matching(kvp_dtu.Value.kegg, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "KEGG", kvp_dtu.Value.isProblematic);
                    remove_all_non_matching(kvp_dtu.Value.chebi, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "ChEBI", kvp_dtu.Value.isProblematic);
                    remove_all_non_matching(kvp_dtu.Value.chebi_additional, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "additional ChEBI", kvp_dtu.Value.isProblematic);
                }
                else if (kvp_dtu.Value.cas_additional.Count > 0)
                {
                    foreach (string s in kvp_dtu.Value.cas_additional)
                    {
                        remove_all_non_matching(s, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "additional CAS", kvp_dtu.Value.isProblematic);
                    }
                    remove_all_non_matching(kvp_dtu.Value.kegg, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "KEGG", kvp_dtu.Value.isProblematic);
                    remove_all_non_matching(kvp_dtu.Value.chebi, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "ChEBI", kvp_dtu.Value.isProblematic);
                    remove_all_non_matching(kvp_dtu.Value.chebi_additional, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "additional ChEBI", kvp_dtu.Value.isProblematic);
                }
                else if (!string.IsNullOrEmpty(kvp_dtu.Value.kegg) && !string.IsNullOrWhiteSpace(kvp_dtu.Value.kegg))
                {
                    remove_all_non_matching(kvp_dtu.Value.kegg, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "KEGG", kvp_dtu.Value.isProblematic);
                    remove_all_non_matching(kvp_dtu.Value.chebi, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "ChEBI", kvp_dtu.Value.isProblematic);
                    remove_all_non_matching(kvp_dtu.Value.chebi_additional, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "additional ChEBI", kvp_dtu.Value.isProblematic);
                }
                else if (!string.IsNullOrEmpty(kvp_dtu.Value.chebi) && !string.IsNullOrWhiteSpace(kvp_dtu.Value.chebi))
                {
                    remove_all_non_matching(kvp_dtu.Value.chebi, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "ChEBI", kvp_dtu.Value.isProblematic);
                    remove_all_non_matching(kvp_dtu.Value.chebi_additional, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "additional ChEBI", kvp_dtu.Value.isProblematic);
                }
                else if (!string.IsNullOrEmpty(kvp_dtu.Value.chebi_additional) && !string.IsNullOrWhiteSpace(kvp_dtu.Value.chebi_additional))
                {
                    remove_all_non_matching(kvp_dtu.Value.chebi_additional, kvp_dtu.Key, kvp_dtu.Value.hmdb_additional, "additional ChEBI", kvp_dtu.Value.isProblematic);
                }
            }
        }

        private static void remove_all_non_matching(string secondary_accession, string hmdb_accession, List<string> secondary_hmdb_accession, string print_, bool isprobl)
        {
            if (!string.IsNullOrEmpty(secondary_accession) && !string.IsNullOrWhiteSpace(secondary_accession))
            {
                if (dictionaryOfIDs.ContainsKey(secondary_accession))
                {
                    if (dictionaryOfIDs[secondary_accession].listOfHMDB.Count > 1)
                    {
                        dictionaryOfIDs[secondary_accession].primaryHMDB = hmdb_accession;
                        dictionaryOfIDs[secondary_accession].secondaryHMDB = secondary_hmdb_accession;
                        dictionaryOfIDs[secondary_accession].removeIDs();

                        if (!dictOfMetabolitesFromDB.ContainsKey(hmdb_accession))
                        {
                            Console.WriteLine("WARNING: " + hmdb_accession + " does not exist in the HMDB database");
                        }
                        else
                        {
                            dictOfMetabolitesFromDB[hmdb_accession].IsProblematic = isprobl;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("WARNING: " + print_ + " is missing " + secondary_accession);
                }
            }
        }

        private static void fill_in_empty_IDs()
        {
            foreach (KeyValuePair<string, primarySecondaryIDs> empty_record in dictionaryOfIDs.Where(x => x.Value.listOfHMDB.Count < 1))
            {
                if (dictDuplicates.Any(x => x.Value.cas == empty_record.Key))
                {
                    dictionaryOfIDs[empty_record.Key].listOfHMDB.Add(dictDuplicates.First(x => x.Value.cas == empty_record.Key).Key);
                    dictionaryOfIDs[empty_record.Key].primaryHMDB = dictDuplicates.First(x => x.Value.cas == empty_record.Key).Key;
                }
                else if (dictDuplicates.Any(x => x.Value.cas_additional.Contains(empty_record.Key)))
                {
                    dictionaryOfIDs[empty_record.Key].listOfHMDB.Add(dictDuplicates.First(x => x.Value.cas_additional.Contains(empty_record.Key)).Key);
                    dictionaryOfIDs[empty_record.Key].primaryHMDB = dictDuplicates.First(x => x.Value.cas_additional.Contains(empty_record.Key)).Key;
                }
                else if (dictDuplicates.Any(x => x.Value.chebi == empty_record.Key))
                {
                    dictionaryOfIDs[empty_record.Key].listOfHMDB.Add(dictDuplicates.First(x => x.Value.chebi == empty_record.Key).Key);
                    dictionaryOfIDs[empty_record.Key].primaryHMDB = dictDuplicates.First(x => x.Value.chebi == empty_record.Key).Key;
                }
                else if (dictDuplicates.Any(x => x.Value.chebi_additional.Contains(empty_record.Key)))
                {
                    dictionaryOfIDs[empty_record.Key].listOfHMDB.Add(dictDuplicates.First(x => x.Value.chebi_additional.Contains(empty_record.Key)).Key);
                    dictionaryOfIDs[empty_record.Key].primaryHMDB = dictDuplicates.First(x => x.Value.chebi_additional.Contains(empty_record.Key)).Key;
                }
                else if (dictDuplicates.Any(x => x.Value.kegg == empty_record.Key))
                {
                    dictionaryOfIDs[empty_record.Key].listOfHMDB.Add(dictDuplicates.First(x => x.Value.kegg == empty_record.Key).Key);
                    dictionaryOfIDs[empty_record.Key].primaryHMDB = dictDuplicates.First(x => x.Value.kegg == empty_record.Key).Key;
                }
            }
        }

        private static void read_from_chebi_database(string chebi_file, bool singleOrMultipleXml)
        {
            //read the all chebi compounds file
            using (TextReader input = new StreamReader(@"" + chebi_file))
            {
                string line = input.ReadLine();
                List<string> breakAtSep;
                chebi_metabolite chm;
                bool check_break = false;
                while ((line = input.ReadLine()) != null)
                {
                    breakAtSep = line.Split('\t').ToList();
                    check_break = false;

                    chm = new chebi_metabolite()
                    {
                        chebiid = ((!string.IsNullOrEmpty(breakAtSep.First()) && !string.IsNullOrWhiteSpace(breakAtSep.First())) ? breakAtSep.First() : ""),
                        inchi = ((!string.IsNullOrEmpty(breakAtSep.ElementAt(1)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(2))) ? breakAtSep.ElementAt(1) : ""),
                        synonyms = ((!string.IsNullOrEmpty(breakAtSep.ElementAt(2)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(2))) ? breakAtSep.ElementAt(2).Split('|').ToList() : new List<string>()),
                        casid = ((!string.IsNullOrEmpty(breakAtSep.ElementAt(3)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(3))) ? breakAtSep.ElementAt(3).Split('|').ToList() : new List<string>()),
                        keggid = ((!string.IsNullOrEmpty(breakAtSep.ElementAt(4)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(4))) ? breakAtSep.ElementAt(4).Split('|').ToList() : new List<string>()),
                        hmdbid = ((!string.IsNullOrEmpty(breakAtSep.ElementAt(5)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(5))) ? breakAtSep.ElementAt(5).Split('|').ToList() : new List<string>()),
                        lipidmaps_id = ((!string.IsNullOrEmpty(breakAtSep.ElementAt(6)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(6))) ? breakAtSep.ElementAt(6).Split('|').ToList() : new List<string>()),
                        pubchem_id = ((!string.IsNullOrEmpty(breakAtSep.ElementAt(7)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(7))) ? breakAtSep.ElementAt(7).Split('|').ToList() : new List<string>()),
                        name = ((!string.IsNullOrEmpty(breakAtSep.ElementAt(8)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(8))) ? breakAtSep.ElementAt(8) : ""),
                        description = ((!string.IsNullOrEmpty(breakAtSep.ElementAt(9)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(9))) ? breakAtSep.ElementAt(9) : ""),
                        quality = ((!string.IsNullOrEmpty(breakAtSep.ElementAt(10)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(10))) ? breakAtSep.ElementAt(10) : ""),
                        comment = ((!string.IsNullOrEmpty(breakAtSep.ElementAt(11)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(11))) ? breakAtSep.ElementAt(11) : ""),
                        formula = ((!string.IsNullOrEmpty(breakAtSep.ElementAt(12)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(12))) ? breakAtSep.ElementAt(12) : ""),
                        charge = ((!string.IsNullOrEmpty(breakAtSep.ElementAt(13)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(13))) ? breakAtSep.ElementAt(13) : ""),
                        mass = ((!string.IsNullOrEmpty(breakAtSep.ElementAt(14)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(14))) ? breakAtSep.ElementAt(14) : ""),
                        monoisotopic_mass = ((!string.IsNullOrEmpty(breakAtSep.ElementAt(15)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(15))) ? breakAtSep.ElementAt(15) : "")
                    };

                    if (!string.IsNullOrEmpty(breakAtSep.ElementAt(16)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(16)))
                    {
                        chm.getListOfPathways(breakAtSep.GetRange(16, 13), false);
                    }

                    #region hmdb
                    //check if there is any hmdb id in the chebi record
                    if (chm.hmdbid.Count != 0) //search based on HMDB ID
                    {
                        //check if any of the hmdb ids in the chebi record are primary keys
                        if (chm.hmdbid.Any(x => dictOfMetabolitesFromDB.ContainsKey(x)))
                        {
                            //if the hmdb ids, coming from chebi, that match keys are more then one
                            if (chm.hmdbid.Count(x => dictOfMetabolitesFromDB.ContainsKey(x)) > 1)
                            {
                                //check how many of them are secondary accessions and how many primary only
                                List<Tuple<string, bool>> primary_secondary_metabolite = new List<Tuple<string, bool>>();
                                foreach (string s in chm.hmdbid)
                                {
                                    if (!primary_secondary_metabolite.Select(x => x.Item1).Contains(s))
                                    {
                                        //if secondary mark them false
                                        if (dictOfMetabolitesFromDB.Select(x => x.Value).SelectMany(x => x.Hmdb_secondary_accessions).Contains(s))
                                        {
                                            primary_secondary_metabolite.Add(new Tuple<string, bool>(s, false));
                                        }
                                        else //if not mark them true
                                        {
                                            primary_secondary_metabolite.Add(new Tuple<string, bool>(s, true));
                                        }
                                    }
                                }

                                //if exactly one primary
                                if (primary_secondary_metabolite.Select(x => x.Item2).Where(x => x == true).Count() == 1)
                                {
                                    dictOfMetabolitesFromDB[primary_secondary_metabolite.First(x => x.Item2 == true).Item1].add_data_from_ChEBI(line);
                                }
                                //if none primary and all secondary
                                else if (primary_secondary_metabolite.Select(x => x.Item2).Where(x => x == true).Count() == 0)
                                {
                                    foreach (Tuple<string, bool> sb in primary_secondary_metabolite.Where(x => x.Item2 == true))
                                    {
                                        dictOfMetabolitesFromDB[sb.Item1].add_secondary_data_from_ChEBI(line);
                                    }
                                }
                                //if more than one primary
                                else
                                {
                                    foreach (Tuple<string, bool> sb in primary_secondary_metabolite.Where(x => x.Item2 == true))
                                    {
                                        dictOfMetabolitesFromDB[sb.Item1].add_data_from_ChEBI(line);
                                    }
                                }
                            }
                            else //if there is exactly one
                            {
                                dictOfMetabolitesFromDB[chm.hmdbid.First(x => dictOfMetabolitesFromDB.ContainsKey(x))].add_data_from_ChEBI(line);
                            }
                        }
                        else //if there is no record
                        {
                            //check if they are recorded as secondary accessions
                            if (chm.hmdbid.Any(y => dictOfMetabolitesFromDB.Select(x => x.Value).SelectMany(x => x.Hmdb_secondary_accessions).Contains(y)))
                            {
                                //if the hmdb ids, coming from chebi, that match more then one secondary accessions from hmdb
                                if (chm.hmdbid.Count(y => dictOfMetabolitesFromDB.Select(x => x.Value).SelectMany(x => x.Hmdb_secondary_accessions).Contains(y)) > 1)
                                {
                                    //for all the rest secondary accessions do add additional information
                                    foreach (string s in chm.hmdbid)
                                    {
                                        foreach (KeyValuePair<string, metabolite> mtb in dictOfMetabolitesFromDB)
                                        {
                                            if (mtb.Value.Hmdb_secondary_accessions.Contains(s))
                                            {
                                                mtb.Value.add_secondary_data_from_ChEBI(line);
                                            }
                                        }
                                    }
                                }
                                //if there is exactly one
                                else
                                {
                                    string s = chm.hmdbid.First(y => dictOfMetabolitesFromDB.Select(x => x.Value).SelectMany(x => x.Hmdb_secondary_accessions).Contains(y));
                                    foreach (KeyValuePair<string, metabolite> mtb in dictOfMetabolitesFromDB)
                                    {
                                        if (mtb.Value.Hmdb_secondary_accessions.Contains(s))
                                        {
                                            mtb.Value.add_secondary_data_from_ChEBI(line);
                                        }
                                    }
                                }
                            }
                            //if not add the new metabolite
                            else
                            {
                                metabolite chebi_metabolite = new metabolite();
                                if (singleOrMultipleXml)
                                {
                                    chebi_metabolite.new_instance_from_ChEBI_single_file(line);
                                }
                                else
                                {
                                    chebi_metabolite.new_instance_from_ChEBI_multiple_files(line);
                                }
                                dictOfMetabolitesFromDB.Add(chebi_metabolite.Chebi_id, chebi_metabolite);
                            }
                        }
                    }
                    #endregion
                    else if (chm.casid.Count != 0)
                    {
                        foreach (string cid in chm.casid)
                        {
                            if (check_break)
                                break;

                            if (dictionaryOfUniqueIDs.ContainsKey(cid))
                            {
                                dictOfMetabolitesFromDB[dictionaryOfUniqueIDs[cid]].add_data_from_ChEBI(line);
                                check_break = true;
                            }
                        }

                        if (chm.keggid.Count != 0 && !check_break)
                        {
                            foreach (string kid in chm.keggid)
                            {
                                if (check_break)
                                    break;

                                if (dictionaryOfUniqueIDs.ContainsKey(kid))
                                {
                                    dictOfMetabolitesFromDB[dictionaryOfUniqueIDs[kid]].add_data_from_ChEBI(line);
                                    check_break = true;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(chm.chebiid) && !string.IsNullOrWhiteSpace(chm.chebiid) && dictionaryOfUniqueIDs.ContainsKey(chm.chebiid) && !check_break)
                        {
                            dictOfMetabolitesFromDB[dictionaryOfUniqueIDs[chm.chebiid]].add_data_from_ChEBI(line);
                            check_break = true;
                        }
                        else if (!check_break)
                        {
                            metabolite chebi_metabolite = new metabolite();
                            if (singleOrMultipleXml)
                            {
                                chebi_metabolite.new_instance_from_ChEBI_single_file(line);
                            }
                            else
                            {
                                chebi_metabolite.new_instance_from_ChEBI_multiple_files(line);
                            }
                            dictOfMetabolitesFromDB.Add(chebi_metabolite.Chebi_id, chebi_metabolite);
                        }
                    }
                    else if (chm.keggid.Count != 0)
                    {
                        foreach (string kid in chm.keggid)
                        {
                            if (check_break)
                                break;

                            if (dictionaryOfUniqueIDs.ContainsKey(kid))
                            {
                                dictOfMetabolitesFromDB[dictionaryOfUniqueIDs[kid]].add_data_from_ChEBI(line);
                                check_break = true;
                            }
                        }
                        if (!string.IsNullOrEmpty(chm.chebiid) && !string.IsNullOrWhiteSpace(chm.chebiid) && dictionaryOfUniqueIDs.ContainsKey(chm.chebiid) && !check_break)
                        {
                            dictOfMetabolitesFromDB[dictionaryOfUniqueIDs[chm.chebiid]].add_data_from_ChEBI(line);
                            check_break = true;
                        }
                        else if (!check_break)
                        {
                            metabolite chebi_metabolite = new metabolite();
                            if (singleOrMultipleXml)
                            {
                                chebi_metabolite.new_instance_from_ChEBI_single_file(line);
                            }
                            else
                            {
                                chebi_metabolite.new_instance_from_ChEBI_multiple_files(line);
                            }
                            dictOfMetabolitesFromDB.Add(chebi_metabolite.Chebi_id, chebi_metabolite);
                        }
                    }
                    else if (!string.IsNullOrEmpty(chm.chebiid) && !string.IsNullOrWhiteSpace(chm.chebiid))
                    {
                        if (dictionaryOfUniqueIDs.ContainsKey(chm.chebiid))
                        {
                            dictOfMetabolitesFromDB[dictionaryOfUniqueIDs[chm.chebiid]].add_data_from_ChEBI(line);
                        }
                        else
                        {
                            metabolite chebi_metabolite = new metabolite();
                            if (singleOrMultipleXml)
                            {
                                chebi_metabolite.new_instance_from_ChEBI_single_file(line);
                            }
                            else
                            {
                                chebi_metabolite.new_instance_from_ChEBI_multiple_files(line);
                            }
                            dictOfMetabolitesFromDB.Add(chebi_metabolite.Chebi_id, chebi_metabolite);
                        }
                    }
                }
            }
        }

        private static void detect_duplicate_ids()
        {
            //Dictionary<string, List<string>> dictOfIDs = new Dictionary<string, List<string>>();
            foreach (KeyValuePair<string, metabolite> kvp_mb in dictOfMetabolitesFromDB)
            {
                //HMDB
                if (!string.IsNullOrEmpty(kvp_mb.Value.Hmdb_accession) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Hmdb_accession))
                {
                    if (!dictionaryOfIDs.ContainsKey(kvp_mb.Value.Hmdb_accession))
                    {
                        dictionaryOfIDs.Add(kvp_mb.Value.Hmdb_accession, new primarySecondaryIDs(kvp_mb.Value.Hmdb_accession));
                    }
                    else
                    {
                        if (!dictionaryOfIDs[kvp_mb.Value.Hmdb_accession].listOfHMDB.Contains(kvp_mb.Value.Hmdb_accession))
                        {
                            dictionaryOfIDs[kvp_mb.Value.Hmdb_accession].listOfHMDB.Add(kvp_mb.Value.Hmdb_accession);
                        }
                        //Console.WriteLine("WARNING: HMDB ID " + kvp_mb.Value.Hmdb_accession);
                    }
                }
                foreach (string sah in kvp_mb.Value.Hmdb_secondary_accessions)
                {
                    if (!string.IsNullOrEmpty(sah) && !string.IsNullOrWhiteSpace(sah))
                    {
                        if (!dictionaryOfIDs.ContainsKey(sah))
                        {
                            if (!string.IsNullOrEmpty(kvp_mb.Value.Hmdb_accession) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Hmdb_accession))
                            {
                                dictionaryOfIDs.Add(sah, new primarySecondaryIDs(kvp_mb.Value.Hmdb_accession));
                            }
                        }
                        else
                        {
                            if (!dictionaryOfIDs[sah].listOfHMDB.Contains(kvp_mb.Value.Hmdb_accession))
                            {
                                if (!string.IsNullOrEmpty(kvp_mb.Value.Hmdb_accession) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Hmdb_accession))
                                {
                                    dictionaryOfIDs[sah].listOfHMDB.Add(kvp_mb.Value.Hmdb_accession);
                                }
                            }
                            //Console.WriteLine("WARNING: add HMDB ID " + kvp_mb.Value.Hmdb_accession);
                        }
                    }
                }

                //CAS
                if (!string.IsNullOrEmpty(kvp_mb.Value.Cas_registry_number) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Cas_registry_number))
                {
                    if (!dictionaryOfIDs.ContainsKey(kvp_mb.Value.Cas_registry_number))
                    {
                        if (!string.IsNullOrEmpty(kvp_mb.Value.Hmdb_accession) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Hmdb_accession))
                        {
                            dictionaryOfIDs.Add(kvp_mb.Value.Cas_registry_number, new primarySecondaryIDs(kvp_mb.Value.Hmdb_accession));
                        }
                    }
                    else
                    {
                        if (!dictionaryOfIDs[kvp_mb.Value.Cas_registry_number].listOfHMDB.Contains(kvp_mb.Value.Hmdb_accession))
                        {
                            if (!string.IsNullOrEmpty(kvp_mb.Value.Hmdb_accession) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Hmdb_accession))
                            {
                                dictionaryOfIDs[kvp_mb.Value.Cas_registry_number].listOfHMDB.Add(kvp_mb.Value.Hmdb_accession);
                            }
                        }
                        //Console.WriteLine("WARNING: CAS ID " + kvp_mb.Value.Cas_registry_number);
                    }
                }
                foreach (string cc in kvp_mb.Value.Cts_cas)
                {
                    if (!string.IsNullOrEmpty(cc) && !string.IsNullOrWhiteSpace(cc))
                    {
                        if (!dictionaryOfIDs.ContainsKey(cc))
                        {
                            if (!string.IsNullOrEmpty(kvp_mb.Value.Hmdb_accession) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Hmdb_accession))
                            {
                                dictionaryOfIDs.Add(cc, new primarySecondaryIDs(kvp_mb.Value.Hmdb_accession));
                            }
                        }
                        else
                        {
                            if (!dictionaryOfIDs[cc].listOfHMDB.Contains(kvp_mb.Value.Hmdb_accession))
                            {
                                if (!string.IsNullOrEmpty(kvp_mb.Value.Hmdb_accession) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Hmdb_accession))
                                {
                                    dictionaryOfIDs[cc].listOfHMDB.Add(kvp_mb.Value.Hmdb_accession);
                                }
                            }
                            //Console.WriteLine("WARNING: add CAS ID " + cc);
                        }
                    }
                }

                //KEGG
                if (!string.IsNullOrEmpty(kvp_mb.Value.Kegg_id) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Kegg_id))
                {
                    if (!dictionaryOfIDs.ContainsKey(kvp_mb.Value.Kegg_id))
                    {
                        if (!string.IsNullOrEmpty(kvp_mb.Value.Hmdb_accession) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Hmdb_accession))
                        {
                            dictionaryOfIDs.Add(kvp_mb.Value.Kegg_id, new primarySecondaryIDs(kvp_mb.Value.Hmdb_accession));
                        }
                    }
                    else
                    {
                        if (!dictionaryOfIDs[kvp_mb.Value.Kegg_id].listOfHMDB.Contains(kvp_mb.Value.Hmdb_accession))
                        {
                            if (!string.IsNullOrEmpty(kvp_mb.Value.Hmdb_accession) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Hmdb_accession))
                            {
                                dictionaryOfIDs[kvp_mb.Value.Kegg_id].listOfHMDB.Add(kvp_mb.Value.Hmdb_accession);
                            }
                        }
                        //Console.WriteLine("WARNING: KEGG ID " + kvp_mb.Value.Kegg_id);
                    }
                }
                foreach (string ck in kvp_mb.Value.Cts_kegg)
                {
                    if (!string.IsNullOrEmpty(ck) && !string.IsNullOrWhiteSpace(ck))
                    {
                        if (!dictionaryOfIDs.ContainsKey(ck))
                        {
                            if (!string.IsNullOrEmpty(kvp_mb.Value.Hmdb_accession) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Hmdb_accession))
                            {
                                dictionaryOfIDs.Add(ck, new primarySecondaryIDs(kvp_mb.Value.Hmdb_accession));
                            }
                        }
                        else
                        {
                            if (!dictionaryOfIDs[ck].listOfHMDB.Contains(kvp_mb.Value.Hmdb_accession))
                            {
                                if (!string.IsNullOrEmpty(kvp_mb.Value.Hmdb_accession) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Hmdb_accession))
                                {
                                    dictionaryOfIDs[ck].listOfHMDB.Add(kvp_mb.Value.Hmdb_accession);
                                }
                            }
                            //Console.WriteLine("WARNING: add KEGG ID " + ck);
                        }
                    }
                }

                //ChEBI
                if (!string.IsNullOrEmpty(kvp_mb.Value.Chebi_id) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Chebi_id))
                {
                    if (!dictionaryOfIDs.ContainsKey(kvp_mb.Value.Chebi_id))
                    {
                        if (!string.IsNullOrEmpty(kvp_mb.Value.Hmdb_accession) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Hmdb_accession))
                        {
                            dictionaryOfIDs.Add(kvp_mb.Value.Chebi_id, new primarySecondaryIDs(kvp_mb.Value.Hmdb_accession));
                        }
                    }
                    else
                    {
                        if (!dictionaryOfIDs[kvp_mb.Value.Chebi_id].listOfHMDB.Contains(kvp_mb.Value.Hmdb_accession))
                        {
                            if (!string.IsNullOrEmpty(kvp_mb.Value.Hmdb_accession) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Hmdb_accession))
                            {
                                dictionaryOfIDs[kvp_mb.Value.Chebi_id].listOfHMDB.Add(kvp_mb.Value.Hmdb_accession);
                            }
                        }
                        //Console.WriteLine("WARNING: ChEBI ID " + kvp_mb.Value.Chebi_id);
                    }
                }
                foreach (string cc in kvp_mb.Value.Cts_chebi)
                {
                    if (!string.IsNullOrEmpty(cc) && !string.IsNullOrWhiteSpace(cc))
                    {
                        if (!dictionaryOfIDs.ContainsKey(cc))
                        {
                            if (!string.IsNullOrEmpty(kvp_mb.Value.Hmdb_accession) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Hmdb_accession))
                            {
                                dictionaryOfIDs.Add(cc, new primarySecondaryIDs(kvp_mb.Value.Hmdb_accession));
                            }
                        }
                        else
                        {
                            if (!dictionaryOfIDs[cc].listOfHMDB.Contains(kvp_mb.Value.Hmdb_accession))
                            {
                                if (!string.IsNullOrEmpty(kvp_mb.Value.Hmdb_accession) && !string.IsNullOrWhiteSpace(kvp_mb.Value.Hmdb_accession))
                                {
                                    dictionaryOfIDs[cc].listOfHMDB.Add(kvp_mb.Value.Hmdb_accession);
                                }
                            }
                            //Console.WriteLine("WARNING: add ChEBI ID " + cc);
                        }
                    }
                }
            }
        }

        private static void printProblematicMetabolites(string problems_file)
        {
            if (!dictionaryOfIDs.Any(x => x.Value.listOfHMDB.Count != 1 && string.IsNullOrEmpty(x.Value.primaryHMDB) && string.IsNullOrWhiteSpace(x.Value.primaryHMDB)))
            {
                Console.WriteLine("No problematic metabolites!");
                return;
            }

            using (TextWriter output = new StreamWriter(@"" + problems_file))
            {
                foreach (var item in dictionaryOfIDs.Where(x => x.Value.listOfHMDB.Count != 1 && string.IsNullOrEmpty(x.Value.primaryHMDB) && string.IsNullOrWhiteSpace(x.Value.primaryHMDB)))
                {
                    output.WriteLine(item.Key + "\t" + string.Join("\t", item.Value.listOfHMDB));
                }
            }
        }

        private static void printDatabase(string output_file)
        {
            using (TextWriter output = new StreamWriter(@"" + output_file))
            {
                output.WriteLine(string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}{0}{14}{0}{15}{0}{16}{0}{17}{0}{18}{0}{19}{0}{20}{0}{21}" +
                "{0}{22}{0}{23}{0}{24}{0}{25}{0}{26}{0}{27}{0}{28}{0}{29}{0}{30}{0}{31}{0}{32}{0}{33}{0}{34}{0}{35}{0}{36}{0}{37}{0}{38}{0}{39}{0}{40}{0}{41}{0}{42}" +
                "{0}{43}{0}{44}{0}{45}{0}{46}{0}{47}{0}{48}{0}{49}{0}{50}{0}{51}{0}{52}{0}{53}{0}{54}{0}{55}{0}{56}{0}{57}{0}{58}{0}{59}{0}{60}{0}{61}{0}{62}{0}{63}" +
                "{0}{64}{0}{65}{0}{66}{0}{67}{0}{68}{0}{69}{0}{70}{0}{71}{0}{72}{0}{73}{0}{74}{0}{75}{0}{76}{0}{77}{0}{78}",
                    '\t',
                    "HMDB_ID",
                    "secondary_sccession",
                    "name",
                    "is_problematic",
                    "synonyms",
                    "description",
                    "description_ChEBI",
                    "quality",
                    "comment",
                    "charge",
                    "formula",
                    "formula_ChEBI",
                    "average_molecular_weight",
                    "monoisotopic_molecular_weight",
                    "mass",
                    "monoisotopic_mass",
                    "iupac_name",
                    "traditional_iupac",
                    "CAS_ID",
                    "addittional_CAS_ID",
                    "smiles",
                    "inchi",
                    "inchi_ChEBI",
                    "inchikey",
                    "Taxonomy-Description",
                    "Taxonomy-Direct_Parent",
                    "Taxonomy-Kingdom",
                    "Taxonomy-Super_Class",
                    "Taxonomy-Class",
                    "Taxonomy-Molecular_Framework",
                    "Taxonomy-Alternative_Parents",
                    "Taxonomy-Substituents",
                    "Taxonomy-Other_Descreptors",
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
                    "Add_pubchem_id",
                    "lipidmaps_id",
                    "add_lipidmaps_id",
                    "chebi_id",
                    "Add_chebi_id",
                    "synthesis_reference",
                    "Protein-accession",
                    "Protein-name",
                    "Protein-uniprot_id",
                    "Protein-gene_name",
                    "Protein-protein_type",
                    "Disease-Name",
                    "Disease-ImimID",
                    "Disease-PubMedID"));

                foreach (KeyValuePair<string, metabolite> mtbts in dictOfMetabolitesFromDB)
                {
                    output.WriteLine(mtbts.Value.printLine());
                }
            }
        }
    }

    public class duplicatesToUnique
    {
        public List<string> hmdb_additional { get; set; }
        public string cas { get; set; }
        public List<string> cas_additional { get; set; }
        public string kegg { get; set; }
        public string chebi { get; set; }
        public string chebi_additional { get; set; }
        public string action { get; set; }
        public string comment { get; set; }
        public bool isProblematic { get; set; }

        public duplicatesToUnique()
        {
            hmdb_additional = new List<string>();
        }
    }

    public class primarySecondaryIDs
    {
        public List<string> listOfHMDB { get; set; }
        public string primaryHMDB { get; set; }
        public List<string> secondaryHMDB { get; set; }

        public primarySecondaryIDs(string hmdbID)
        {
            listOfHMDB = new List<string>() { hmdbID };
            secondaryHMDB = new List<string>();
        }

        public void removeIDs()
        {
            listOfHMDB.RemoveAll(x => x != primaryHMDB && !secondaryHMDB.Contains(x));
        }
    }

    public class chebi_metabolite
    {
        public string chebiid { get; set; }
        public string inchi { get; set; }
        public List<string> synonyms { get; set; }
        public List<string> casid { get; set; }
        public List<string> keggid { get; set; }
        public List<string> hmdbid { get; set; }
        public List<string> lipidmaps_id { get; set; }
        public List<string> pubchem_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string quality { get; set; }
        public string comment { get; set; }
        public string formula { get; set; }
        public string charge { get; set; }
        public string mass { get; set; }
        public string monoisotopic_mass { get; set; }
        public List<pathway> list_of_pathways { get; set; }

        public chebi_metabolite()
        {
            synonyms = new List<string>();
            keggid = new List<string>();
            casid = new List<string>();
            hmdbid = new List<string>();
            lipidmaps_id = new List<string>();
            pubchem_id = new List<string>();
            list_of_pathways = new List<pathway>();
        }

        public void getListOfPathways(List<string> lst, bool mode) //mode true for HMDB and false for ChEBI
        {
            List<pathway> retrn = new List<pathway>();
            for (int i = 0; i < lst.First().Split('|').Length; i++)
            {
                list_of_pathways.Add(new pathway()
                {
                    Kegg_map_id = lst.First().Split('|').ElementAt(i), //29
                    List_of_names = lst.ElementAt(1).Split('|').ElementAt(i).Split(';').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(), //30
                    Super_class = lst.ElementAt(2).Split('|').ElementAt(i), //31
                    Pathway_map = (!string.IsNullOrEmpty(lst.ElementAt(3).Split('|').ElementAt(i)) && !string.IsNullOrWhiteSpace(lst.ElementAt(3).Split('|').ElementAt(i))) ?
                    new Tuple<string, string>(lst.ElementAt(3).Split('|').ElementAt(i).Split('-').First(), lst.ElementAt(3).Split('|').ElementAt(i).Split('-').ElementAt(1)) :
                    new Tuple<string, string>("", ""), //32
                    List_of_modules = (!string.IsNullOrEmpty(lst.ElementAt(4).Split('|').ElementAt(i)) && !string.IsNullOrWhiteSpace(lst.ElementAt(4).Split('|').ElementAt(i))) ?
                    lst.ElementAt(4).Split('|').ElementAt(i).Split(';').Select(x => new Tuple<string, string>(x.Split('-').First(), string.Join("-", x.Split('-').Skip(1)))).ToList() :
                    new List<Tuple<string, string>>(), //33
                    List_of_diseases = (!string.IsNullOrEmpty(lst.ElementAt(5).Split('|').ElementAt(i)) && !string.IsNullOrWhiteSpace(lst.ElementAt(5).Split('|').ElementAt(i))) ?
                    lst.ElementAt(5).Split('|').ElementAt(i).Split(';').Select(x => new Tuple<string, string>(x.Split('-').First(), string.Join("-", x.Split('-').Skip(1)))).ToList() :
                    new List<Tuple<string, string>>(), //34
                    Organism = lst.ElementAt(6).Split('|').ElementAt(i), //35
                    Gene = lst.ElementAt(7).Split('|').ElementAt(i), //36
                    Enzyme = lst.ElementAt(8).Split('|').ElementAt(i), //37
                    Reaction = lst.ElementAt(9).Split('|').ElementAt(i), //38
                    Compound = lst.ElementAt(10).Split('|').ElementAt(i), //39
                    Ko_pathway = lst.ElementAt(11).Split('|').ElementAt(i), //40
                    Rel_pathway = lst.ElementAt(12).Split('|').ElementAt(i), //41
                    Smpdb_map_id = (mode ? lst.ElementAt(13).Split('|').ElementAt(i) : ""), //42
                    Smpdb_map_name = (mode ? lst.ElementAt(14).Split('|').ElementAt(i) : ""), //43
                    Smpadb_map_description = (mode ? lst.ElementAt(15).Split('|').ElementAt(i) : "") //44
                });
            }
        }
    }
}

