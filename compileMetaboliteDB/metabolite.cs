using System;
using System.Collections.Generic;
using System.Linq;

namespace compileMetaboliteDB
{
    public class metabolite
    {
        public string Hmdb_accession { get; set; }
        public List<string> Hmdb_secondary_accessions { get; set; }
        public string Name { get; set; }
        public List<string> Synonym_names { get; set; }
        public string Description { get; set; }
        public string Description_chebi { get; set; }
        public string Quality { get; set; }
        public string Comment { get; set; }
        public string Charge { get; set; }
        public string Formula { get; set; }
        public string Formula_chebi { get; set; }
        public double Average_molecular_weight { get; set; }
        public double Monisotopic_molecular_weight { get; set; }
        public double Mass { get; set; }
        public double Monoisotopic_mass { get; set; }
        public string Iupac_name { get; set; }
        public string Traditional_iupac { get; set; }
        public string Cas_registry_number { get; set; }
        public List<string> Cts_cas { get; set; }
        public string Smiles { get; set; }
        public string Inchi { get; set; }
        public string Inchi_chebi { get; set; }
        public string Inchikey { get; set; }
        public taxonomy My_taxonomy { get; set; }
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
        public string Metlin_id { get; set; }
        public string Lipidmaps_id { get; set; }
        public List<string> Cts_Lipidmaps { get; set; }
        public string Pubchem_compound_id { get; set; }
        public List<string> Cts_Pubchem { get; set; }
        public string Chebi_id { get; set; }
        public List<string> Cts_chebi { get; set; }
        public string Synthesis_reference { get; set; }
        public List<protein> List_of_proteins { get; set; }
        public List<disease> List_of_diseases { get; set; }
        public bool IsProblematic { get; set; }
        public bool SignleXmlFile { get; set; }

        public void add_data_from_HMDB_singe_file(string hmdb_compiled_file_line)
        {
            List<string> breakAtSep = hmdb_compiled_file_line.Split('\t').ToList();

            Cts_Lipidmaps = new List<string>();
            Cts_Pubchem = new List<string>();

            Hmdb_accession = breakAtSep.First(); //0
            Hmdb_secondary_accessions = breakAtSep.ElementAt(1).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(); //1
            Name = breakAtSep.ElementAt(2); //2
            Synonym_names = breakAtSep.ElementAt(3).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(); //3
            Description = breakAtSep.ElementAt(4); //4
            Formula = breakAtSep.ElementAt(5); //5
            Average_molecular_weight = Convert.ToDouble(breakAtSep.ElementAt(6)); //6
            Monisotopic_molecular_weight = Convert.ToDouble(breakAtSep.ElementAt(7)); //7
            Iupac_name = breakAtSep.ElementAt(8); //8
            Traditional_iupac = breakAtSep.ElementAt(9); //9
            Cas_registry_number = breakAtSep.ElementAt(10); //10
            Cts_cas = breakAtSep.ElementAt(11).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(); //11
            Smiles = breakAtSep.ElementAt(12); //12
            Inchi = breakAtSep.ElementAt(13); //13
            Inchikey = breakAtSep.ElementAt(14); //14
            My_taxonomy = new taxonomy()
            {
                Description = breakAtSep.ElementAt(15), //15
                Direct_parent = breakAtSep.ElementAt(16), //16
                Kingdom = breakAtSep.ElementAt(17), //17
                Super_class = breakAtSep.ElementAt(18), //18
                Tclass = breakAtSep.ElementAt(19), //19
                Molecular_framework = breakAtSep.ElementAt(20), //20
                Alternative_parents = breakAtSep.ElementAt(21).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(), //21
                Substituents = breakAtSep.ElementAt(22).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList() //22
            };
            My_onotology = new ontology()
            {
                Status = breakAtSep.ElementAt(23), //23
                Origins = breakAtSep.ElementAt(24).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(), //24
                Biofunctions = breakAtSep.ElementAt(25).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(), //25
                Applications = breakAtSep.ElementAt(26).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(), //26
                Cellular_locations = breakAtSep.ElementAt(27).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList() //27
            };
            State = breakAtSep.ElementAt(28); //28
            Biofluid_locations = breakAtSep.ElementAt(29).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(); //29
            Tissue_locations = breakAtSep.ElementAt(30).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(); //30
            List_of_pathways = returnListOfPathways(breakAtSep.GetRange(31, 16), true); //31-46
            Drugbank_id = breakAtSep.ElementAt(47); //47
            Drugbank_metabolite_id = breakAtSep.ElementAt(48); //48
            Chemspider_id = breakAtSep.ElementAt(49); //49
            Kegg_id = breakAtSep.ElementAt(50); //50
            Cts_kegg = breakAtSep.ElementAt(51).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(); //51
            Metlin_id = breakAtSep.ElementAt(52); //52
            Pubchem_compound_id = breakAtSep.ElementAt(53); //53
            Chebi_id = breakAtSep.ElementAt(54); //54
            Cts_chebi = breakAtSep.ElementAt(55).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(); //55
            Synthesis_reference = breakAtSep.ElementAt(56); //56
            List_of_proteins = returnListOfProteins(breakAtSep.GetRange(57, 5)); //57-61
            List_of_diseases = returnListOfDiseases(breakAtSep.GetRange(62, 3)); //62-64
        }

        public void add_data_from_HMDB_multiple_files(string hmdb_compiled_file_line)
        {
            List<string> breakAtSep = hmdb_compiled_file_line.Split('\t').ToList();

            Cts_Lipidmaps = new List<string>();
            Cts_Pubchem = new List<string>();

            Hmdb_accession = breakAtSep.First(); //0
            Hmdb_secondary_accessions = breakAtSep.ElementAt(1).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(); //1
            Name = breakAtSep.ElementAt(2); //2
            Synonym_names = breakAtSep.ElementAt(3).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(); //3
            Description = breakAtSep.ElementAt(4); //4
            Formula = breakAtSep.ElementAt(5); //5
            Average_molecular_weight = Convert.ToDouble(breakAtSep.ElementAt(6)); //6
            Monisotopic_molecular_weight = Convert.ToDouble(breakAtSep.ElementAt(7)); //7
            Iupac_name = breakAtSep.ElementAt(8); //8
            Traditional_iupac = breakAtSep.ElementAt(9); //9
            Cas_registry_number = breakAtSep.ElementAt(10); //10
            Cts_cas = breakAtSep.ElementAt(11).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(); //11
            Smiles = breakAtSep.ElementAt(12); //12
            Inchi = breakAtSep.ElementAt(13); //13
            Inchikey = breakAtSep.ElementAt(14); //14
            My_taxonomy = new taxonomy()
            {
                Direct_parent = breakAtSep.ElementAt(15), //15
                Kingdom = breakAtSep.ElementAt(16), //16
                Super_class = breakAtSep.ElementAt(17), //17
                Tclass = breakAtSep.ElementAt(18), //18
                Substituents = breakAtSep.ElementAt(19).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(), //19
                Other_descriptors = breakAtSep.ElementAt(20).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList() //20
            };
            My_onotology = new ontology()
            {
                Status = breakAtSep.ElementAt(21), //21
                Origins = breakAtSep.ElementAt(22).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(), //22
                Biofunctions = breakAtSep.ElementAt(23).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(), //23
                Applications = breakAtSep.ElementAt(24).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(), //24
                Cellular_locations = breakAtSep.ElementAt(25).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList() //25
            };
            State = breakAtSep.ElementAt(26); //26
            Biofluid_locations = breakAtSep.ElementAt(27).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(); //27
            Tissue_locations = breakAtSep.ElementAt(28).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(); //28
            List_of_pathways = returnListOfPathways(breakAtSep.GetRange(29, 16), true); //29-44
            Drugbank_id = breakAtSep.ElementAt(45); //45
            Drugbank_metabolite_id = breakAtSep.ElementAt(46); //46
            Chemspider_id = breakAtSep.ElementAt(47); //47
            Kegg_id = breakAtSep.ElementAt(48); //48
            Cts_kegg = breakAtSep.ElementAt(49).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(); //49
            Metlin_id = breakAtSep.ElementAt(50); //50
            Pubchem_compound_id = breakAtSep.ElementAt(51); //51
            Chebi_id = breakAtSep.ElementAt(52); //52
            Cts_chebi = breakAtSep.ElementAt(53).Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(); //53
            Synthesis_reference = breakAtSep.ElementAt(54); //54
            List_of_proteins = returnListOfProteins(breakAtSep.GetRange(55, 5));
        }

        public void add_data_from_ChEBI(string chebi_compiled_file_line)
        {
            List<string> breakAtSep = chebi_compiled_file_line.Split('\t').ToList();
            double tmpDouble = -1;

            //add ChEBI id
            //if the primary ID does not exist then just add it
            //if the primary ID exists but it does not agree with the current new one then add the older primary to the Cts_ChEBI and set the new one as primary
            //in any other case proceed
            if (string.IsNullOrEmpty(Chebi_id) && string.IsNullOrWhiteSpace(Chebi_id))
            {
                Chebi_id = breakAtSep.First();
            }
            else if (!string.IsNullOrEmpty(Chebi_id) && !string.IsNullOrWhiteSpace(Chebi_id) && Chebi_id != breakAtSep.First())
            {
                if (!Cts_chebi.Contains(Chebi_id))
                {
                    Cts_chebi.Add(Chebi_id);
                }
                Chebi_id = breakAtSep.First();
            }

            //add inchi
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(1)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(1)))
            {
                if (Inchi != breakAtSep.ElementAt(1))
                {
                    Inchi_chebi = breakAtSep.ElementAt(1);
                }
            }

            //add synonyms
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(2)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(2)))
            {
                foreach (string s in breakAtSep.ElementAt(2).Split('|'))
                {
                    if (!Synonym_names.Contains(s))
                    {
                        Synonym_names.Add(s);
                    }
                }
            }

            //add cas
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(3)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(3)))
            {
                foreach (string s in breakAtSep.ElementAt(3).Split('|'))
                {
                    if (Cas_registry_number != s && !Cts_cas.Contains(s))
                    {
                        Cts_cas.Add(s);
                    }
                }
            }

            //add kegg
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(4)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(4)))
            {
                foreach (string s in breakAtSep.ElementAt(4).Split('|'))
                {
                    if (Kegg_id != s && !Cts_kegg.Contains(s))
                    {
                        Cts_kegg.Add(s);
                    }
                }
            }

            //add hmdb id
            //if the primary ID does not exist then just add it
            //if the primary ID exists but it does not agree with the current new one then add the new one to the secondary list of ids
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(5)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(5)))
            {
                if (string.IsNullOrEmpty(Hmdb_accession) && string.IsNullOrWhiteSpace(Hmdb_accession))
                {
                    Hmdb_accession = breakAtSep.ElementAt(5).Split('|').First();
                    if (breakAtSep.ElementAt(5).Split('|').Skip(1).Count() > 0)
                    {
                        foreach (string s in breakAtSep.ElementAt(5).Split('|').Skip(1))
                        {
                            if (Hmdb_accession != s && !Hmdb_secondary_accessions.Contains(s))
                            {
                                Hmdb_secondary_accessions.Add(s);
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(Hmdb_accession) && !string.IsNullOrWhiteSpace(Hmdb_accession))
                {
                    foreach (string s in breakAtSep.ElementAt(5).Split('|'))
                    {
                        if (Hmdb_accession != s && !Hmdb_secondary_accessions.Contains(s))
                        {
                            Hmdb_secondary_accessions.Add(s);
                        }
                    }
                }
            }

            //add lipidmaps id
            //if the primary ID does not exist then just add it
            //if the primary ID exists but it does not agree with the current new one then add the new one to the secondary list of ids
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(6)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(6)))
            {
                if (string.IsNullOrEmpty(Lipidmaps_id) && string.IsNullOrWhiteSpace(Lipidmaps_id))
                {
                    Lipidmaps_id = breakAtSep.ElementAt(6).Split('|').First();
                    if (breakAtSep.ElementAt(6).Split('|').Skip(1).Count() > 0)
                    {
                        foreach (string s in breakAtSep.ElementAt(6).Split('|').Skip(1))
                        {
                            if (Lipidmaps_id != s && !Cts_Lipidmaps.Contains(s))
                            {
                                Cts_Lipidmaps.Add(s);
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(Lipidmaps_id) && !string.IsNullOrWhiteSpace(Lipidmaps_id))
                {
                    foreach (string s in breakAtSep.ElementAt(6).Split('|'))
                    {
                        if (Lipidmaps_id != s && !Cts_Lipidmaps.Contains(s))
                        {
                            Cts_Lipidmaps.Add(s);
                        }
                    }
                }
            }

            //add pubchem id
            //if the primary ID does not exist then just add it
            //if the primary ID exists but it does not agree with the current new one then add the new one to the secondary list of ids
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(7)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(7)))
            {
                if (string.IsNullOrEmpty(Pubchem_compound_id) && string.IsNullOrWhiteSpace(Pubchem_compound_id))
                {
                    Pubchem_compound_id = breakAtSep.ElementAt(7).Split('|').First();
                    if (breakAtSep.ElementAt(7).Split('|').Skip(1).Count() > 0)
                    {
                        foreach (string s in breakAtSep.ElementAt(7).Split('|').Skip(1))
                        {
                            if (Pubchem_compound_id != s && !Cts_Pubchem.Contains(s))
                            {
                                Cts_Pubchem.Add(s);
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(Pubchem_compound_id) && !string.IsNullOrWhiteSpace(Pubchem_compound_id))
                {
                    foreach (string s in breakAtSep.ElementAt(7).Split('|'))
                    {
                        if (Pubchem_compound_id != s && !Cts_Pubchem.Contains(s))
                        {
                            Cts_Pubchem.Add(s);
                        }
                    }
                }
            }

            //add name
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(8)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(8)))
            {
                foreach (string s in breakAtSep.ElementAt(8).Split('|'))
                {
                    if (Name != s && !Synonym_names.Contains(s))
                    {
                        Synonym_names.Add(s);
                    }
                }
            }

            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(9)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(9)))
            {
                Description_chebi = breakAtSep.ElementAt(9);
            }
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(10)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(10)))
            {
                Quality = breakAtSep.ElementAt(10);
            }
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(11)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(11)))
            {
                Comment = breakAtSep.ElementAt(11);
            }

            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(12)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(12)))
            {
                Formula_chebi = breakAtSep.ElementAt(12);
            }

            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(13)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(13)))
            {
                Charge = breakAtSep.ElementAt(13);
            }
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(14)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(14)))
            {
                tmpDouble = 1000000;
                foreach (string s in breakAtSep.ElementAt(14).Split('|'))
                {
                    if (Math.Abs(Convert.ToDouble(s) - Average_molecular_weight) < tmpDouble)
                    {
                        Mass = Convert.ToDouble(s);
                    }
                }
            }
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(15)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(15)))
            {
                tmpDouble = 1000000;
                foreach (string s in breakAtSep.ElementAt(15).Split('|'))
                {
                    if (Math.Abs(Convert.ToDouble(s) - Monisotopic_molecular_weight) < tmpDouble)
                    {
                        Monoisotopic_mass = Convert.ToDouble(s);
                    }
                }
            }

            //add pathways
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(16)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(16)))
            {
                foreach (pathway ptw in returnListOfPathways(breakAtSep.GetRange(16, 13), false))
                {
                    if (!List_of_pathways.Any(x => x.Kegg_map_id == ptw.Kegg_map_id))
                    {
                        List_of_pathways.Add(ptw); ;
                    }
                }
            }
        }

        public void add_secondary_data_from_ChEBI(string chebi_compiled_file_line)
        {
            List<string> breakAtSep = chebi_compiled_file_line.Split('\t').ToList();

            //add ChEBI id to the additional ones
            if (!Cts_chebi.Contains(breakAtSep.First()))
            {
                Cts_chebi.Add(breakAtSep.First());
            }

            //add synonyms
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(2)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(2)))
            {
                foreach (string s in breakAtSep.ElementAt(2).Split('|'))
                {
                    if (!Synonym_names.Contains(s))
                    {
                        Synonym_names.Add(s);
                    }
                }
            }

            //add cas
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(3)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(3)))
            {
                foreach (string s in breakAtSep.ElementAt(3).Split('|'))
                {
                    if (Cas_registry_number != s && !Cts_cas.Contains(s))
                    {
                        Cts_cas.Add(s);
                    }
                }
            }

            //add kegg
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(4)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(4)))
            {
                foreach (string s in breakAtSep.ElementAt(4).Split('|'))
                {
                    if (Kegg_id != s && !Cts_kegg.Contains(s))
                    {
                        Cts_kegg.Add(s);
                    }
                }
            }

            //add hmdb
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(5)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(5)))
            {
                foreach (string s in breakAtSep.ElementAt(5).Split('|'))
                {
                    if (Hmdb_accession != s && !Hmdb_secondary_accessions.Contains(s))
                    {
                        Hmdb_secondary_accessions.Add(s);
                    }
                }
            }

            //add name
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(8)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(8)))
            {
                foreach (string s in breakAtSep.ElementAt(8).Split('|'))
                {
                    if (Name != s && !Synonym_names.Contains(s))
                    {
                        Synonym_names.Add(s);
                    }
                }
            }

            //add pathways
            if (!string.IsNullOrEmpty(breakAtSep.ElementAt(16)) && !string.IsNullOrWhiteSpace(breakAtSep.ElementAt(16)))
            {
                foreach (pathway ptw in returnListOfPathways(breakAtSep.GetRange(16, 13), false))
                {
                    if (!List_of_pathways.Any(x => x.Kegg_map_id == ptw.Kegg_map_id))
                    {
                        List_of_pathways.Add(ptw); ;
                    }
                }
            }
        }

        public void new_instance_from_ChEBI_single_file(string chebi_compiled_file_line)
        {
            add_data_from_HMDB_singe_file(string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}{0}{14}{0}{15}{0}{16}{0}{17}{0}{18}{0}{19}{0}{20}{0}{21}" +
                "{0}{22}{0}{23}{0}{24}{0}{25}{0}{26}{0}{27}{0}{28}{0}{29}{0}{30}{0}{31}{0}{32}{0}{33}{0}{34}{0}{35}{0}{36}{0}{37}{0}{38}{0}{39}{0}{40}{0}{41}{0}{42}{0}{43}{0}{44}{0}{45}{0}{46}{0}{47}{0}{48}{0}{49}{0}{50}{0}{51}" +
                "{0}{52}{0}{53}{0}{54}{0}{55}{0}{56}{0}{57}{0}{58}{0}{59}{0}{60}{0}{61}{0}{62}{0}{63}{0}{64}{0}{65}",
                    '\t', "", "", "", "", "", "", "-1", "-1", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
            add_data_from_ChEBI(chebi_compiled_file_line);
        }

        public void new_instance_from_ChEBI_multiple_files(string chebi_compiled_file_line)
        {
            add_data_from_HMDB_multiple_files(string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}{0}{14}{0}{15}{0}{16}{0}{17}{0}{18}{0}{19}{0}{20}{0}{21}" +
                "{0}{22}{0}{23}{0}{24}{0}{25}{0}{26}{0}{27}{0}{28}{0}{29}{0}{30}{0}{31}{0}{32}{0}{33}{0}{34}{0}{35}{0}{36}{0}{37}{0}{38}{0}{39}{0}{40}{0}{41}{0}{42}{0}{43}{0}{44}{0}{45}{0}{46}{0}{47}{0}{48}{0}{49}{0}{50}{0}{51}" +
                "{0}{52}{0}{53}{0}{54}{0}{55}{0}{56}{0}{57}{0}{58}{0}{59}{0}{60}",
                    '\t', "", "", "", "", "", "", "-1", "-1", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
            add_data_from_ChEBI(chebi_compiled_file_line);
        }

        private static List<pathway> returnListOfPathways(List<string> lst, bool mode) //mode true for HMDB and false for ChEBI
        {
            List<pathway> retrn = new List<pathway>();
            for (int i = 0; i < lst.First().Split('|').Length; i++)
            {
                retrn.Add(new pathway()
                {
                    Kegg_map_id = lst.First().Split('|').ElementAt(i), //29 or 31
                    List_of_names = lst.ElementAt(1).Split('|').ElementAt(i).Split(';').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList(), //30 or 32
                    Super_class = lst.ElementAt(2).Split('|').ElementAt(i), //31 or 33
                    Pathway_map = (!string.IsNullOrEmpty(lst.ElementAt(3).Split('|').ElementAt(i)) && !string.IsNullOrWhiteSpace(lst.ElementAt(3).Split('|').ElementAt(i))) ?
                    new Tuple<string, string>(lst.ElementAt(3).Split('|').ElementAt(i).Split('-').First(), lst.ElementAt(3).Split('|').ElementAt(i).Split('-').ElementAt(1)) :
                    new Tuple<string, string>("", ""), //32 or 34
                    List_of_modules = (!string.IsNullOrEmpty(lst.ElementAt(4).Split('|').ElementAt(i)) && !string.IsNullOrWhiteSpace(lst.ElementAt(4).Split('|').ElementAt(i))) ?
                    lst.ElementAt(4).Split('|').ElementAt(i).Split(';').Select(x => new Tuple<string, string>(x.Split('-').First(), string.Join("-", x.Split('-').Skip(1)))).ToList() :
                    new List<Tuple<string, string>>(), //33 or 35
                    List_of_diseases = (!string.IsNullOrEmpty(lst.ElementAt(5).Split('|').ElementAt(i)) && !string.IsNullOrWhiteSpace(lst.ElementAt(5).Split('|').ElementAt(i))) ?
                    lst.ElementAt(5).Split('|').ElementAt(i).Split(';').Select(x => new Tuple<string, string>(x.Split('-').First(), string.Join("-", x.Split('-').Skip(1)))).ToList() :
                    new List<Tuple<string, string>>(), //34 or 36
                    Organism = lst.ElementAt(6).Split('|').ElementAt(i), //35 or 37
                    Gene = lst.ElementAt(7).Split('|').ElementAt(i), //36 or 38
                    Enzyme = lst.ElementAt(8).Split('|').ElementAt(i), //37 or 39
                    Reaction = lst.ElementAt(9).Split('|').ElementAt(i), //38 or 40
                    Compound = lst.ElementAt(10).Split('|').ElementAt(i), //39 or 41
                    Ko_pathway = lst.ElementAt(11).Split('|').ElementAt(i), //40 or 42
                    Rel_pathway = lst.ElementAt(12).Split('|').ElementAt(i), //41 or 43
                    Smpdb_map_id = (mode ? lst.ElementAt(13).Split('|').ElementAt(i) : ""), //42 or 55
                    Smpdb_map_name = (mode ? lst.ElementAt(14).Split('|').ElementAt(i) : ""), //43 or 45
                    Smpadb_map_description = (mode ? lst.ElementAt(15).Split('|').ElementAt(i) : "") //44 or 46
                });
            }
            return retrn;
        }

        private static List<protein> returnListOfProteins(List<string> lst)
        {
            List<protein> retrn = new List<protein>();
            for (int i = 0; i < lst.First().Split('|').Length; i++)
            {
                retrn.Add(new protein()
                {
                    Protein_accession = lst.First().Split('|').ElementAt(i), //55 or 57
                    Name = lst.ElementAt(1).Split('|').ElementAt(i), //56 or 58
                    Uniprot_id = lst.ElementAt(2).Split('|').ElementAt(i), //57 or 59
                    Gene_name = lst.ElementAt(3).Split('|').ElementAt(i), //58 or 60
                    Protein_type = lst.ElementAt(4).Split('|').ElementAt(i) //59 or 61
                });
            }
            return retrn;
        }

        private static List<disease> returnListOfDiseases(List<string> lst)
        {
            List<disease> retrn = new List<disease>();
            for (int i = 0; i < lst.First().Split('|').Length; i++)
            {
                retrn.Add(new disease()
                {
                    Name = lst.First().Split('|').ElementAt(i), //56
                    Omim_id = lst.ElementAt(1).Split('|').ElementAt(i), //57
                    List_of_pubmed_ids = lst.ElementAt(2).Split('|').ElementAt(i).Split(';').ToList() //
                });
            }
            return retrn;
        }

        public class taxonomy
        {
            public string Description { get; set; }
            public string Direct_parent { get; set; }
            public string Kingdom { get; set; }
            public string Super_class { get; set; }
            public string Tclass { get; set; }
            public string Molecular_framework { get; set; }
            public List<string> Alternative_parents { get; set; }
            public List<string> Substituents { get; set; }
            public List<string> Other_descriptors { get; set; }
        }

        public class ontology
        {
            public string Status { get; set; }
            public List<string> Origins { get; set; }
            public List<string> Biofunctions { get; set; }
            public List<string> Applications { get; set; }
            public List<string> Cellular_locations { get; set; }
        }

        public class protein
        {
            public string Protein_accession { get; set; }
            public string Name { get; set; }
            public string Uniprot_id { get; set; }
            public string Gene_name { get; set; }
            public string Protein_type { get; set; }
        }

        public class disease
        {
            public string Name { get; set; }
            public string Omim_id { get; set; }
            public List<string> List_of_pubmed_ids { get; set; }
        }

        public string printLine()
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}{0}{14}{0}{15}{0}{16}{0}{17}{0}{18}{0}{19}{0}{20}{0}{21}" +
                "{0}{22}{0}{23}{0}{24}{0}{25}{0}{26}{0}{27}{0}{28}{0}{29}{0}{30}{0}{31}{0}{32}{0}{33}{0}{34}{0}{35}{0}{36}{0}{37}{0}{38}{0}{39}{0}{40}{0}{41}{0}{42}" +
                "{0}{43}{0}{44}{0}{45}{0}{46}{0}{47}{0}{48}{0}{49}{0}{50}{0}{51}{0}{52}{0}{53}{0}{54}{0}{55}{0}{56}{0}{57}{0}{58}{0}{59}{0}{60}{0}{61}{0}{62}{0}{63}" +
                "{0}{64}{0}{65}{0}{66}{0}{67}{0}{68}{0}{69}{0}{70}{0}{71}{0}{72}{0}{73}{0}{74}{0}{75}{0}{76}{0}{77}{0}{78}",
                    '\t',
                    Hmdb_accession,
                    (Hmdb_secondary_accessions == null ? "" : string.Join("|", Hmdb_secondary_accessions)),
                    Name,
                    (IsProblematic ? "TRUE" : "FALSE"),
                    (Synonym_names == null ? "" : string.Join("|", Synonym_names)),
                    Description,
                    Description_chebi,
                    Quality,
                    Comment,
                    Charge,
                    Formula,
                    Formula_chebi,
                    Average_molecular_weight,
                    Monisotopic_molecular_weight,
                    Mass,
                    Monoisotopic_mass,
                    Iupac_name,
                    Traditional_iupac,
                    Cas_registry_number,
                    (Cts_cas == null ? "" : string.Join("|", Cts_cas)),
                    Smiles,
                    Inchi,
                    Inchi_chebi,
                    Inchikey,
                    (My_taxonomy == null ? "" : My_taxonomy.Description),
                    (My_taxonomy == null ? "" : My_taxonomy.Direct_parent),
                    (My_taxonomy == null ? "" : My_taxonomy.Kingdom),
                    (My_taxonomy == null ? "" : My_taxonomy.Super_class),
                    (My_taxonomy == null ? "" : My_taxonomy.Tclass),
                    (My_taxonomy == null ? "" : My_taxonomy.Molecular_framework),
                    (My_taxonomy == null ? "" : (My_taxonomy.Alternative_parents == null ? "" : string.Join("|", My_taxonomy.Alternative_parents))),
                    (My_taxonomy == null ? "" : (My_taxonomy.Substituents == null ? "" : string.Join("|", My_taxonomy.Substituents))),
                    (My_taxonomy == null ? "" : (My_taxonomy.Other_descriptors == null ? "" : string.Join("|", My_taxonomy.Other_descriptors))),
                    (My_onotology == null ? "" : My_onotology.Status),
                    (My_onotology == null ? "" : (My_onotology.Origins == null ? "" : string.Join("|", My_onotology.Origins))),
                    (My_onotology == null ? "" : (My_onotology.Biofunctions == null ? "" : string.Join("|", My_onotology.Biofunctions))),
                    (My_onotology == null ? "" : (My_onotology.Applications == null ? "" : string.Join("|", My_onotology.Applications))),
                    (My_onotology == null ? "" : (My_onotology.Cellular_locations == null ? "" : string.Join("|", My_onotology.Cellular_locations))),
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
                    (Cts_Pubchem == null ? "" : string.Join("|", Cts_Pubchem)),
                    Lipidmaps_id,
                    (Cts_Lipidmaps == null ? "" : string.Join("|", Cts_Lipidmaps)),
                    Chebi_id,
                    (Cts_chebi == null ? "" : string.Join("|", Cts_chebi)),
                    Synthesis_reference,
                    (List_of_proteins == null ? "" : string.Join("|", List_of_proteins.Select(x => x.Protein_accession))),
                    (List_of_proteins == null ? "" : string.Join("|", List_of_proteins.Select(x => x.Name))),
                    (List_of_proteins == null ? "" : string.Join("|", List_of_proteins.Select(x => x.Uniprot_id))),
                    (List_of_proteins == null ? "" : string.Join("|", List_of_proteins.Select(x => x.Gene_name))),
                    (List_of_proteins == null ? "" : string.Join("|", List_of_proteins.Select(x => x.Protein_type))),
                    (List_of_diseases == null ? "" : string.Join("|", List_of_diseases.Select(x => x.Name))),
                    (List_of_diseases == null ? "" : string.Join("|", List_of_diseases.Select(x => x.Omim_id))),
                    (List_of_diseases == null ? "" : string.Join("|", List_of_diseases.Select(x => string.Join(",", x.List_of_pubmed_ids)))));
        }
    }
}
