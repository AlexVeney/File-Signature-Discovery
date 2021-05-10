# File-Signature-Discovery

Command line application (written in C#) that does the following:

A. Takes two inputs and a flag
	i. A directory that contains the files to be analyzed
	ii. A path for the output file (including file name and extension)
	iii. A flag to determine whether or not to include subdirectories contained (and all subsequently embedded subdirectories) within the input directory (i. above)

B. Processes each of the files in the directory (and subdirectories if the flag is present) and determinesusing a file signature if a given file is a PDF or a JPG

C. For each file that is a PDF or a JPG, creates an entry in the output CSV containing the following information
	i. The full path to the file
	ii. The actual file type (PDF or JPG)
	iii. The MD5 hash of the file contents
