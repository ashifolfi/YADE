//#define DEBUG_EXTRA
// uncomment the above to print patch adds
using System;
using System.IO;
using System.Text.RegularExpressions;
using YADE.Resource;
using Vector2 = System.Numerics.Vector2;

namespace YADE.CTexture
{
    public class FileSystem
    {
        public static CTDefResource parseFile(string name, string path)
        {
            CTDefResource resource = new CTDefResource(name, new Dictionary<string, Resource.CTexture>());

            // Attempt to parse the file
            try
            {
                // Read the file into a string
                var filecontents = File.ReadAllLines(path);

				// this is probably stupid inefficient but fuck man it works.
				// go through each line and assemble a list of defs to regex

				// variable we hold the assembled lines in
				List<string> defs = new List<string>();

                // variables to control entry changes
                bool nextline = false;
                bool insidedef = false;
                bool insideparam = false;
                string toadd = "";

                foreach (string line in filecontents)
                {
                    // skip anything containing a comment
                    if (Regex.Match(line, @"//").Value == String.Empty)
                    {
                        // do we match the beginning of a def?
                        if (Regex.Match(line, @"^WallTexture|Texture").Value != string.Empty)
                        {
                            // add it.
                            toadd += line + "\n";
                        }
                        else if (Regex.Match(line, @"^.*{").Value != string.Empty)
                        {
						    if (!insidedef)
                                insidedef = true;
                            else
                                insideparam = true;
                            toadd += line + "\n";
                        }
                        else if (insidedef && Regex.Match(line, @"^.*}").Value != string.Empty)
                        {
                            if (!insideparam)
                            {
                                nextline = true;
                                insidedef = false;
                            }
                            else
                                insideparam = false;
						    toadd += line + "\n";
                        }
                        else if (!(line == String.Empty))
                            toadd += line+"\n";

					    if (nextline)
                        {
                            nextline = false; insidedef = false; insideparam = false;
						    defs.Add(toadd);
                            toadd = "";
					    }
                    }
                }

				// Regex passes on the separated lines
				foreach (String def in defs)
                {
                    // now we assemble a single texture
                    Resource.CTexture texture = new Resource.CTexture("TOCHANGE", new List<CTexPatch>());

                    // pass 1: extract root info from starting line
                    Match rootInfo = Regex.Match(def, @"^(WallTexture|Texture)\s*""(.*)"",\s*(\d+),\s*(\d+)");

                    // theoretically the order should never change here
                    texture.type = rootInfo.Groups[1].Value;
                    texture.ctexName = rootInfo.Groups[2].Value;
					texture.size = new Vector2(
                        Convert.ToInt32(rootInfo.Groups[3].Value), Convert.ToInt32(rootInfo.Groups[4].Value));

                    // pass 2: Return of the individual line crawl: Patch info edition

                    MatchCollection patchinfo = Regex.Matches(def, @"(.*Patch.*""(.*)"",\s(\d+),\s(\d+))");

                    foreach (Match match in patchinfo)
                    {
						CTexPatch patch = new CTexPatch(match.Groups[2].Value,
                            new Vector2(Convert.ToInt32(match.Groups[3].Value), Convert.ToInt32(match.Groups[4].Value)));

                        // TODO: Implement extra parameters for patches
#if DEBUG_EXTRA
                        Console.WriteLine("[CTextureEditor] Added patch " + match.Groups[2].Value + " to texture " + texture.ctexName);
#endif
                        texture.patchList.Add(patch);
                    }

                    // add the texture to the list
                    Console.WriteLine("[CTextureEditor] Added texture " + texture.ctexName);
                    try // a nested trycatch, if an add fails we don't want to lose every other texture
                        // that could have been inside that file.
                    {
						resource.ctexList.Add(texture.ctexName, texture);
					}
                    catch (Exception exc)
                    {
                        Console.WriteLine("[CTextureEditor] Failed to add texture " + texture.ctexName);
                    }
				}
			}
			catch (Exception ex) // Very helpful yes.
            {
                // Failed to open file
                if (ex.GetType() == Type.GetType("FileNotFoundException")
                    || ex.GetType() == Type.GetType("DirectoryNotFoundException"))
                {
					Console.WriteLine("[CTextureEditor] [FS] Texture definition file " + name + " was not found! Attempting to create new definition...");
					return resource;
				}
                else if (ex.GetType() == Type.GetType("IOException"))
                {
					Console.WriteLine("[CTextureEditor] [FS] Unknown IO exception occurred! Printing original message...");
					Console.WriteLine(ex.Message);
					return resource;
				}
                else if (ex.GetType() == Type.GetType("UnauthorizedAccessException"))
                {
					Console.WriteLine("[CTextureEditor] [ACC] Generic Access exception occurred! Printing original message...");
                    Console.WriteLine(ex.Message);
                    return resource;
				}


                // Unknown error
                Console.WriteLine("[CTextureEditor] Unknown error occured! Printing original message...");
                Console.WriteLine(ex.Message);
				return resource;
            }
            return resource;
        }

        public void saveCTexDefinitions(CTDefResource tosave)
        {

        }

        public void locatePatchGraphic(string name)
        {

        }
    }
}

