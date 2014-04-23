using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Word;

namespace WordPDF_Converter
{
    public class ConverterPDF
    {
        private readonly Application _wordapp;
        public ConverterPDF()
        {
            _wordapp = new Application();
        }

        /// <summary>
        /// Writes to the word document, and saves it as a .pdf document
        /// </summary>
        /// <param name="calc">list of ITaskCost</param>
        /// <param name="templateLocation">Location of the template to use</param>
        /// <param name="pdfSaveLocation">location to save the .pdf document</param>
        public void WriteToTemplate(List<ITaskCost> calc, string templateLocation, string pdfSaveLocation)
        {
            try
            {
                int i = 1;
                Console.WriteLine("Processing ...");

                var doc = OpenWordDoc(templateLocation);

                //Activates the document so it can be used
                doc.Activate();

                //takes all the data in the ITaskCost list and inserts them into the template
                foreach (var taskCost in calc)
                {
                    doc.Bookmarks.get_Item("task" + i).Range.Text = taskCost.TaskName;
                    doc.Bookmarks.get_Item("Total" + i).Range.Text = taskCost.TotalHourCost.ToString();
                    i++;
                }

                SaveAsPDF(doc, pdfSaveLocation);
                Console.WriteLine("Done!");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Open a word document with a given name
        /// </summary>
        /// <param name="filename">name of the document to open</param>
        /// <returns>return a word document</returns>
        public Document OpenWordDoc(object filename)
        {
            object readOnly = false;
            object isVisible = false;

            _wordapp.Visible = false;

            object missing = System.Reflection.Missing.Value;

            //Opens the word document at the taget location
            var adoc = _wordapp.Documents.Open(ref filename, ref missing, ref readOnly, ref missing, ref missing,
                                                        ref missing, ref missing, ref missing, ref missing, ref missing,
                                                        ref missing, ref isVisible, ref missing, ref missing, ref missing,
                                                        ref missing);
            return adoc;
        }

        /// <summary>
        /// saves the given word document, as a .pdf document
        /// </summary>
        /// <param name="adoc">The document to save</param>
        /// <param name="filename">name of the .pdf document</param>
        public void SaveAsPDF(Document adoc, String filename)
        {
            object missing = System.Reflection.Missing.Value;

            object savePDFFormat = WdSaveFormat.wdFormatPDF;
            //Saves the file as .pdf
            adoc.SaveAs(filename, savePDFFormat, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing);

            adoc.Close(false, missing, missing);
            
        }
    }
}
