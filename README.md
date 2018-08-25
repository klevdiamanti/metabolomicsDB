# metabolomicsDB
metabolomicsDB is a collection of three open-source command-line tools that when used step-wise create a meta-databse of the Human Metabolome Database (HMDB) and Chemical Entities of Biological Interest (ChEBI). The pipelines are written in C# and run in all platforms. For Windows you can run it through cmd.exe, in OSX and in Linux through the terminal.

The output of these three tools can be used as the database file of MS_targeted repository.

metabolomicsDB is an API that allows to load the database generated from the three previous tools. You just need to provide the database tab-separated file and next continue using the collection of metabolites from the static class metabolites.

## Run metabolomicsDB tools
You might use the three attahced .exe files under the exec directory of each tool to run the tools.
You will also find a sample_files directory under the directory of each tool.

**For any questions or issues please use the Issues in github or contact Klev Diamanti.**

metabolomicsDB contans the following three tools:
- compileChEBIdata
- compileHMDBdata
- compileMetaboliteDB

## Run compileChEBIdata
```
[mono] compileChEBIdata.exe input_chebi_dir output_file
```
This tool takes as input the collection of flat files from ChEBI (ftp://ftp.ebi.ac.uk/pub/databases/chebi/Flat_file_tab_delimited/) and outputs the same file with the addition of information from the Kyoto Encyclopedia of Genes and Genomes (KEGG) which is queries on-the-fly. The output is a tab-separated file.

## Run compileHMDBdata
```
[mono] compileHMDBdata.exe (hmdb_xml_file OR collection_of_hmdb_xml_files) smpdb_pathway_file output_file
```
This tool takes as input the a single XML file or a collection of XML files (one for each metabolite) from HMDB (http://www.hmdb.ca/downloads) and a database flat file from the the Small Molecule Pathway Database (SMPDB) (http://smpdb.ca/downloads), and outputs the same file with the addition of information from the Kyoto Encyclopedia of Genes and Genomes (KEGG) which is queries on-the-fly. The output is a tab-separated file.

## Run compileMetaboliteDB
```
[mono] compileMetaboliteDB.exe hmdb_tsv_file chebi_tsv_file duplicates_file output_file problem_file single_multiple_xml_files_boolean
```
This tool takes as input the two files generated from the previous two steps we described (compileChEBIdata and compileHMDBdata). The file is given to resolve duplicates that are caused because of inaccurate cross-references between the public databases. These cases are reported by the tool in the problem_file and the user should manually resolve them. The output file is a tab-separated file. The last option should be always TRUE.
Samples of duplicates_file and corrections_file can be found under compileMetaboliteDB/sample_data. Please decompress the fles before using them.

## Citation
Klev Diamanti, Marco Cavalli, Gang Pan, Maria João Pereira, Chanchal Kumar, Stanko Skrtic, Manfred Grabherr, Ulf Risérus, Jan W Eriksson, Jan Komorowski and Claes Wadelius (2018). "Metabolic landscape of five tissues suggests new molecular defects in type-2 diabetes". Submitted.
