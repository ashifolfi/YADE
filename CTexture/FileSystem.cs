//#define DEBUG_EXTRA
// uncomment the above to print patch adds

using Microsoft.Xna.Framework.Graphics;
using FileTypeChecker;
using System.Text.RegularExpressions;
using YADE.Resource;
using Vector2 = System.Numerics.Vector2;
using FileTypeChecker.Abstracts;
using FileTypeChecker.Types;

namespace YADE.CTexture
{
    /// <summary></summary>
    public class FileSystem
    {
        /// <summary>
        /// Parse Composite Texture Definition files
        /// </summary>
        /// <param name="name">name of the definition</param>
        /// <param name="path">Path to look for said definition</param>
        /// <returns>CTexture Resource data</returns>
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

        /// <summary>
        /// TODO: Fill this out!
        /// </summary>
        /// <param name="tosave"></param>
        public void saveCTexDefinitions(CTDefResource tosave)
        {

        }

        /// <summary>
        /// Locate a patch/graphic and load it into a texture
        /// </summary>
        /// <param name="name">Patch name to look for</param>
        /// <param name="path">TODO: REMOVE THIS | path to the folder where we should look</param>
        /// <returns>The requested patch texture data (when found) or a blank texture data (When not found)</returns>
        public static Stream locatePatchGraphic(string name, string path)
        {
            // load the file as a filestream
            FileStream patchStream;
            try
            {
                // IDEA: This could just iterate through a list of known filetypes and directories tbh
                // little bit of a mess here but we need to wildcard the name because of extensions
                DirectoryInfo dir = new DirectoryInfo(path);
// #if WINDOWS
//                 FileInfo[] fi = dir.GetFiles(@"*\\" + name + @"*");
// #else
                FileInfo[] fi = dir.GetFiles(name + @"*", SearchOption.AllDirectories);
// #endif
                if (fi.Length == 0)
                {
                    // just in case this turns out to be a wackass edgecase
                    patchStream = File.Open(path + "/Patches/" + name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                }
                else
                    patchStream = File.Open(fi[0].FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[CTexture] Patch loading failed! Does the patch exist?");
                Console.WriteLine(ex.Message);
                return null;
            }
            Console.WriteLine(name);
            // determine what type to return from mimetype on the file
            // kinda dangerous because if we hit a file that isn't a graphics lump but also not a standard picture
            // we are GOING TO CRASH due to my lack of any error handling in DPicture.cs right no
            try {
                if (FileTypeValidator.IsImage(patchStream))
                {
                    Stream imgstream = null;
                    patchStream.CopyTo(imgstream);
                    return imgstream;
                }
                else
                {
                    // not an image? ok let's assume it's a DPicture format
                    DPicture dPicture = new DPicture(name, patchStream);
                    patchStream.Close();
                    return dPicture.imgstream;
                }
            }
            catch(Exception ex) {
                // filetypevalidator broke so it's a doom picture
                DPicture dPicture = new DPicture(name, patchStream);
                patchStream.Close();
                return dPicture.imgstream;
            }
        }
    }
}

