using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

// implementation by Saoud Aldowaish

namespace SS
{
    /// <summary>
    /// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string is a valid cell name if and only if:
    ///   (1) its first character is an underscore or a letter
    ///   (2) its remaining characters (if any) are underscores and/or letters and/or digits
    /// Note that this is the same as the definition of valid variable from the PS3 Formula class.
    /// 
    /// For example, "x", "_", "x2", "y_15", and "___" are all valid cell  names, but
    /// "25", "2x", and "&" are not.  Cell names are case sensitive, so "x" and "X" are
    /// different cell names.
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  (This
    /// means that a spreadsheet contains an infinite number of cells.)  In addition to 
    /// a name, each cell has a contents and a value.  The distinction is important.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In a new spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError,
    /// as reported by the Evaluate method of the Formula class.  The value of a Formula,
    /// of course, can depend on the values of variables.  The value of a variable is the 
    /// value of the spreadsheet cell it names (if that cell's value is a double) or 
    /// is undefined (otherwise).
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        public DependencyGraph dGraph;
        private Dictionary<String, Cell> cellsDic;
        /// <summary>
        /// keep track whether cells were changed in the spreadsheet
        /// </summary>
        public override bool Changed { get; protected set; }

        /// <summary>
        ///  Spreadsheet constructor which creates a Spreadsheet and gives it a DependencyGraph (dGraph) and Dictionary (cellsDic).
        /// </summary>
        public Spreadsheet() : base(s => true, s => s, "default")
        {
            dGraph = new DependencyGraph();
            cellsDic = new Dictionary<string, Cell>();
        }

        /// <summary>
        ///  Spreadsheet constructor which creates a Spreadsheet and gives it a DependencyGraph (dGraph) and Dictionary (cellsDic) and sets the validator, normalizer and version as given.
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
           : base(isValid, normalize, version)
        {
            dGraph = new DependencyGraph();
            cellsDic = new Dictionary<string, Cell>();
        }

        /// <summary>
        /// Spreadsheet constructor which creates a Spreadsheet and gives it a DependencyGraph (dGraph) and Dictionary (cellsDic) and 
        /// sets the validator, normalizer and version as given. And it sets the cells after reading the given xml file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(string file, Func<string, bool> isValid, Func<string, string> normalize, string version)
             : base(isValid, normalize, version)
        {
            dGraph = new DependencyGraph();
            cellsDic = new Dictionary<string, Cell>();
            string savedVersion = GetSavedVersion(file);
            if (version != savedVersion)
                throw new SpreadsheetReadWriteException("Version does not match");

            // note: I didn't have to use a try/catch block because I'm calling GetSavedVersion() before this step which catch the same exeptions because of the file (at least that what my testing suggested)
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            using (XmlReader reader = XmlReader.Create(file, settings))
                while (reader.Read())                                                    // reading until we find a cell
                    if (reader.Name == "cell")
                    {
                        reader.Read();
                        SetContentsOfCell(reader.ReadInnerXml(), reader.ReadInnerXml()); // we sets the cells we find
                    }
        }
        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override string GetSavedVersion(string filename)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    reader.ReadToFollowing("spreadsheet"); // read until we find the "spreadsheet"
                    reader.MoveToAttribute("version");     // get the version attribute
                    return reader.Value;
                }
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException(e.Message);
            }
        }
        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>cell name goes here</name>
        /// <contents>cell contents goes here</contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
            if (ReferenceEquals(filename, null) || filename.Equals(""))
                throw new SpreadsheetReadWriteException("file is eaither null or empty");

            Changed = false;
            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename))
                {
                    writer.WriteStartDocument(); // start the document
                    writer.WriteStartElement("spreadsheet"); // start <spreadsheet>
                    writer.WriteAttributeString("version", Version); // sets the version to the given version when creating the spreadsheet if not provided it would be "default"
                    foreach (string name in GetNamesOfAllNonemptyCells())
                    {
                        writer.WriteStartElement("cell"); // start <cell>
                        writer.WriteElementString("name", name);
                        if (cellsDic.TryGetValue(name, out Cell cell))
                        {
                            object content = cell.contents;
                            if (content is string || content is double) // if the cell is string or a double we add the string value of their contents
                                writer.WriteElementString("contents", content.ToString());
                            else
                                writer.WriteElementString("contents", "=" + content.ToString()); // if it is a formula we add = before adding the string value of their contents
                            writer.WriteEndElement(); // close <cell>
                        }
                    }
                    writer.WriteEndElement(); // close <spreadsheet>
                    writer.WriteEndDocument(); // end the document
                    writer.Dispose();
                }
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException(e.Message);
            }
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            CheckIsVaribale(name);
            string normalizedName = Normalize(name);
            if (cellsDic.TryGetValue(normalizedName, out Cell cell))
                return cell.contents;
            return "";
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            CheckIsVaribale(name);
            if (cellsDic.TryGetValue(Normalize(name), out Cell cell))
                return cell.value;
            return "";
        }
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown,
        ///       and no change is made to the spreadsheet.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a list consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            string normalizedName = Normalize(name);
            CheckIsNull(content);
            CheckIsVaribale(name);
            if (!IsValid(name))
                throw new InvalidNameException();

            if (Double.TryParse(content, out double d)) // if the content is double we use the double SetCellContents 
                SetCellContents(normalizedName, d);
            else if (content.StartsWith("=")) // if the content starts with an = it is a formula so we use the formula SetCellContents 
            {
                Formula f = new Formula(content.Substring(1), Normalize, IsValid);
                SetCellContents(normalizedName, f);
            }
            else
                SetCellContents(normalizedName, content); // else it is just a string so we use the string SetCellContents 

            foreach (string s in GetCellsToRecalculate(normalizedName)) // make sure to recalculate all cells that were affected by setting this cell contents
            {
                if (cellsDic.ContainsKey(Normalize(s)))
                    AddCell(Normalize(s), cellsDic[Normalize(s)].contents); // we add the cell again to recalculate it
            }
            Changed = true;
            return new List<string>(GetCellsToRecalculate(normalizedName));
        }
        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return cellsDic.Keys;
        }
        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            CheckIsVaribale(name);
            return dGraph.GetDependents(name);
        }
        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        public IEnumerable<string> GetDirectDependentes(string name)
        {
            CheckIsVaribale(name);
            return dGraph.GetDependents(name);
        }
        /// <summary>
        /// throws an InvalidNameException if the string is null or if it is not a varible else returns true
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool CheckIsVaribale(string s)
        {
            if (ReferenceEquals(s, null) || !Regex.IsMatch(s, @"^[a-zA-Z]+[\d]+$"))
                throw new InvalidNameException();
            return true;
        }
        /// <summary>
        /// returns an ArgumentNullException if the value is null
        /// </summary>
        /// <param name="value"></param>
        private void CheckIsNull(object value)
        {
            if (ReferenceEquals(value, null))
                throw new ArgumentNullException();
        }
        /// <summary>
        /// Creates a new cell then if it exists in the cellsDic (cells dictionary) we add its content if not we add the new cell to cellDic
        /// </summary>
        /// <param name="name"> the cell name </param>
        /// <param name="contents"> the cell content</param>
        private void AddCell(string name, object contents)
        {
            Cell cell;
            if (contents is Formula)
                cell = new Cell((Formula)contents, Lookup); // if the content is a formula we need a lookup for the varibles
            else
                cell = new Cell(contents);

            if (cellsDic.ContainsKey(name))
                cellsDic[name] = cell;
            else
                cellsDic.Add(name, cell);
        }
        /// <summary>
        /// checks if a cell exists in the dictionary if it does checks if it is a double returns it if it is or return an argument exception
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private double Lookup(string name)
        {
            if (cellsDic.TryGetValue(Normalize(name), out Cell cell))
            {
                if (cell.value is double)
                    return (double)cell.value;
            }
            throw new ArgumentException();
        }
        /// <summary>
        /// The contents of the named cell becomes number.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        protected override IList<string> SetCellContents(string name, double number)
        {
            AddCell(name, number);
            dGraph.ReplaceDependees(name, new HashSet<string>());
            return new List<string>(GetCellsToRecalculate(name));
        }
        /// <summary>
        /// The contents of the named cell becomes text.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        protected override IList<string> SetCellContents(string name, string text)
        {
            AddCell(name, text);
            if (cellsDic[name].contents.Equals("")) // if the cell value was an empty string then it means that the cell is unused.
                cellsDic.Remove(name); // so it gets removed
            dGraph.ReplaceDependees(name, new HashSet<string>());
            return new List<string>(GetCellsToRecalculate(name));
        }
        /// <summary>
        /// If changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException, and no change is made to the spreadsheet.
        /// 
        /// Otherwise, the contents of the named cell becomes formula. The method returns a
        /// list consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            IEnumerable<String> dependees = dGraph.GetDependees(name); // since we need to undo changes if we catched a circular dependency I'm saving the original depndees.
            dGraph.ReplaceDependees(name, formula.GetVariables());
            try
            {
                List<string> dependeesList = new List<string>(GetCellsToRecalculate(name));
                AddCell(name, formula);
                return dependeesList;
            }
            catch (CircularException)
            {
                dGraph.ReplaceDependees(name, dependees); // we catched a circular dependency so we return the original dependees
                throw new CircularException();
            }
        }
        /// <summary>
        /// an object that represent a cell for spreadsheet 
        /// </summary>
        private class Cell
        {
            public object contents { get; set; } // contents that can be a double, string or formula
            public object value { get; set; } // value which should become double after Evaluating
            /// <summary>
            /// Cell constructor where cell contents is given which can be a double, string or formula.
            /// </summary>
            public Cell(object content)
            {
                contents = content; // set the Cell contents.
                value = contents;   // set the Cell value.
            }
            public Cell(Formula formula, Func<string, double> lookup)
            {
                this.contents = formula; // set the Cell contents.
                value = formula.Evaluate(lookup);
            }
        }
    }
}
