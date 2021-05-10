/* 
Alex Veney Software Engineer
*/


using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;


namespace Program
{

	class FileSignatureDiscovery
	{

		public static string[] GetFileNames1(string directory, int flag)
		{
			GC.Collect();

			IEnumerable<string> fileList;

			// Read file names according to flag
			if (flag == 1)
			{
				fileList = Directory.EnumerateFiles(@directory, "*.*", SearchOption.TopDirectoryOnly);

			}
			else if (flag == 2)
			{
				fileList = Directory.EnumerateFiles(@directory, "*.*", SearchOption.AllDirectories);

			}
			else
			{
				fileList = null;
				Console.WriteLine("Invalid flag entered");
				Environment.Exit(0);
			}

			return fileList.ToArray();

		}

		static string GenerateMD5(string filename)
		{
		    using (var md5 = MD5.Create())
		    {
		        using (var stream = File.OpenRead(filename))
		        {
		            var hash = md5.ComputeHash(stream);
		            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
		        }
		    }
		}

		static void Main(string[] args)
		{


			Console.WriteLine("****File Signature Discovery***\n" + 
								"Author: Alex Veney\n" +
								"Language: C#");	


			string directory;
			int flag;
			string outputFileName;

			// Test for null or empty command line arguments
			if (args == null || args.Length == 0)
			{			
				Console.WriteLine("Provide the following command line arguments: \n" +
					"1) Directory \n" +
					"2) File path of output file\n" +
					"3) Flag - 1 (directory only) or 2 (include subdirectories)");

					Environment.Exit(0);
			} 
			else if ((args.Length < 3) || (args.Length > 3)) 
			{
				// Test for wrong number of arguments
				Console.WriteLine("Wrong number of arguments. You must enter 3 arguments");
				Environment.Exit(0);
			}


			directory = args[0];
			outputFileName = args[1];
			flag = Int32.Parse(args[2]);
			
			Console.WriteLine("1st Arg/directory: " + directory);
			Console.WriteLine("2nd Arg/outputFileName: " + outputFileName);
			Console.WriteLine("3rd Arg/flag: " + flag);


			// Check if directory exists
			if (!Directory.Exists(directory))
			{
				Console.WriteLine("Directory does not exist");
				Environment.Exit(0);
			}

			// Create file if it does not exist
			if(!System.IO.File.Exists(outputFileName))
			{
				System.IO.File.Create(outputFileName);
			}
			else
			{
				Console.WriteLine("Filename \"{0}\" already exists", outputFileName);
				Environment.Exit(0);
			}

			// Get file path/names
			string[] directoryFiles = GetFileNames1(directory, flag);

			// Identify file types
			foreach(string file in directoryFiles)
			{
				byte[] fileBytes = File.ReadAllBytes(file);

				if (fileBytes.Length < 4)
				{
					continue;
				}

				// Create hex string
				string hex = fileBytes[0].ToString("X2") + fileBytes[1].ToString("X2") +
				fileBytes[2].ToString("X2") + fileBytes[3].ToString("X2");

				string type;
				string path = file;
				string hash;

				// Identify file type based on hex signature
				if(hex.Substring(0,4).Equals("FFD8"))
				{
					// * JPG files start with 0xFFD8
					type = "JPG";
				}
				else if (hex.Equals("25504446"))
				{
					// * PDF files start with 0x25504446
					type = "PDF";
				}
				else
				{
					// Not a JPG or PDF
					continue;
				}

				// Create MD5 hash
				hash = GenerateMD5(path);

				// Add file information to csv file
				string fileEntry = path + "," + type + "," + hash + Environment.NewLine;
				Console.WriteLine("File Entry: " + fileEntry);
				File.AppendAllText(outputFileName, fileEntry);

			}


		}
	}

}
