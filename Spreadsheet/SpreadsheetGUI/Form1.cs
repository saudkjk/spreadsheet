using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadsheetUtilities;
using SS;

// implementation by Saoud Aldowaish

namespace SpreadsheetGUI
{
    public partial class Form1 : Form
    {

        private Spreadsheet sSheet;
        private string currentCell;
        private int currentCol;
        private int currentRow;
        private bool fileSaved = false;

        /// <summary>
        ///  Constructor for the spreadsheetGUI
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            sSheet = new Spreadsheet(s => Regex.IsMatch(s, @"^[a-zA-Z][1-9][0-9]?$"), s => s.ToUpper(), "ps6");
            AcceptButton = button1;
            spreadsheetPanel1.SetSelection(0, 0); // select the cell A1 when the form is created
            getNewCellData();
            spreadsheetPanel1.SelectionChanged += onSelectionChanged;
            ActiveControl = contentsBox;
        }
        /// <summary>
        /// Every time the selection changes, this method is called.
        /// </summary>
        /// <param name="ssp"></param>
        private void onSelectionChanged(SpreadsheetPanel ssp)
        {
            getNewCellData();
        }
        /// <summary>
        /// Every time button1 is clicked or ENTER is pressed, this method is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            changeCellContents();
        }
        /// <summary>
        /// gets the column and row from the current selection calculate the cell name then fills
        /// contentsBox with the cell contents, cellNameBox with the cell name and cellValueBox with the cellValue
        /// </summary>
        private void getNewCellData()
        {
            contentsBox.Focus();
            spreadsheetPanel1.GetSelection(out currentCol, out currentRow);
            currentCell = getCellName(currentCol, currentRow);

            if (sSheet.GetCellContents(currentCell) is Formula)
                contentsBox.Text = "=" + getCellContents(currentCell);
            else
                contentsBox.Text = getCellContents(currentCell);
            cellNameBox.Text = currentCell;
            cellValueBox.Text = getCellValue(currentCell);
        }
        /// <summary>
        /// gets the cells to recaculate by setting the selected cell to its contents then set the value
        /// in the spreadsheet panel and show the changes in the cellValueBox. If an invalid formula is used an error message shows up.
        /// </summary>
        private void changeCellContents()
        {
            try
            {
                IList<string> cellsToRecalculate = sSheet.SetContentsOfCell(currentCell, contentsBox.Text);
                recalculateCells(cellsToRecalculate);

                spreadsheetPanel1.SetValue(currentCol, currentRow, getCellValue(currentCell));
                cellValueBox.Text = getCellValue(currentCell);
            }
            catch (Exception)
            {
                MessageBox.Show("Please make sure your are using a valid formula", "Invaliud Formula",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// uses spreadsheet method GetCellValue to get the cell value. If it happens to be a FormualError it removes the SpreadsheetUtilities part of the type.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private string getCellValue(string cell)
        {
            string cellValue = sSheet.GetCellValue(cell).ToString();
            if (cellValue == "SpreadsheetUtilities.FormulaError")
                cellValue = "FormulaError";
            return cellValue;
        }
        /// <summary>
        /// uses spreadsheet method GetCellContentsto get the cell contents and change it to a string.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private string getCellContents(string cell)
        {
            return sSheet.GetCellContents(cell).ToString();
        }
        /// <summary>
        /// sets the new value of each cell in the spreadsheetPanel
        /// </summary>
        /// <param name="cellsToRecalculate"></param>
        private void recalculateCells(IEnumerable<string> cellsToRecalculate)
        {
            foreach (string cell in cellsToRecalculate)
            {
                getCellColAndRow(cell, out int col, out int row);
                spreadsheetPanel1.SetValue(col, row, getCellValue(cell));
            }
        }
        /// <summary>
        ///  clears the spreadsheetpanel
        /// </summary>
        /// <param name="oldSheetCells"></param>
        private void clearCells(IEnumerable<string> oldSheetCells)
        {
            foreach (string cell in oldSheetCells)
            {
                getCellColAndRow(cell, out int col, out int row);
                spreadsheetPanel1.SetValue(col, row, "");
            }
        }
        /// <summary>
        /// calculates the col and row from the cell name
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private void getCellColAndRow(string cell, out int col, out int row)
        {
            col = cell[0] - 65;
            row = int.Parse(cell.Substring(1)) - 1;
        }
        /// <summary>
        /// calculates the cell name from the column and row
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private string getCellName(int col, int row)
        {
            string cellCol = Char.ToString(Convert.ToChar(col + 65)).ToUpper();
            int cellRow = row + 1;
            return cellCol + cellRow;
        }
        /// <summary>
        /// uses spreadsheet save method
        /// </summary>
        private void save()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "sprd files (*.sprd)|*.sprd|All files (*.*)|*.*";
            save.FilterIndex = 1;
            save.RestoreDirectory = true;
            if (save.ShowDialog() == DialogResult.OK)
            {
                sSheet.Save(save.FileName);
                fileSaved = true;
            }
        }
        /// <summary>
        /// makes a new form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            DemoApplicationContext.getAppContext().RunForm(new Form1());
        }
        /// <summary>
        /// uses the save() method checks if the file was saved before if not it asks if the user still want to save or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileSaved == false)
            {
                DialogResult result = MessageBox.Show("The current file will be overwritten do you still want to save the changes you made on this spreadsheet?",
                   "Current file will be overwritten", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    save();
            }
            else
                save();
        }
        /// <summary>
        /// asks the user if they want to save the changes they mad. then opens a spreadsheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sSheet.Changed)
            {
                DialogResult result = MessageBox.Show("The current spreadsheet is not saved do you want to save it before opening a new spreadsheet?",
                   "spreadsheet is not saved", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    save();
            }

            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "sprd files (*.sprd)|*.sprd|All files (*.*)|*.*";

                if (open.ShowDialog() == DialogResult.OK)
                {
                    clearCells(sSheet.GetNamesOfAllNonemptyCells());
                    string version = sSheet.GetSavedVersion(open.FileName);
                    sSheet = new SS.Spreadsheet(open.FileName, s => Regex.IsMatch(s, @"^[a-zA-Z][1-9][0-9]?$"), input => input.ToUpper(), version);
                    recalculateCells(sSheet.GetNamesOfAllNonemptyCells());
                    getNewCellData();
                    fileSaved = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please make sure your are using a valid file", "Invaliud file",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        /// <summary>
        /// asks the user if they want to save before closing then it colses the currnet form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sSheet.Changed)
            {
                DialogResult result = MessageBox.Show("The current spreadsheet is not saved do you want to save it before closing?",
                   "spreadsheet is not saved", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    save();
            }
            Close();
        }
        /// <summary>
        /// detects when the user press one of the arrow keys and changes the cells depdending on what rrow key was pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                if (e.KeyCode == Keys.Up && currentRow != 0)
                    spreadsheetPanel1.SetSelection(currentCol, --currentRow);
                else if (e.KeyCode == Keys.Down && currentRow != 99)
                    spreadsheetPanel1.SetSelection(currentCol, ++currentRow);
                else if (e.KeyCode == Keys.Left && currentCol != 0)
                    spreadsheetPanel1.SetSelection(--currentCol, currentRow);
                else if (e.KeyCode == Keys.Right && currentCol != 26)
                    spreadsheetPanel1.SetSelection(++currentCol, currentRow);
                onSelectionChanged(spreadsheetPanel1);
            }
        }
        /// <summary>
        /// help the user know how to change selections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeSelectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("There are two ways to change selections. The first one is by left clicking the cell you want to modify with your mouse. " +
                 "The second one is by using the arrow keys to move to adjacent cells", "How to change selections?");
        }
        /// <summary>
        /// help the user know how to edit cells contents
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editCellContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Underneath the Change contents button there is a text box where you can enter new contents of a cell or change existing contents. " +
                "After typing or modifying contents left click the button or click the ENTER key to confirm the changes.", "How to edit cell contents?");
        }
        /// <summary>
        /// helps the user know how to save DOT files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void additionalFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can save the depenedents or the depndees for all the cells in the spreadsheet or a specific cell depenedents or depndees as a DOT file . " +
                           "To do that left click the Save DOT file menu and choose which option you want to save (if you want to save the depenedents or depndees of a" +
                           " specific cell make sure to select it before clicking the menu.", "How to save depenedents or depndees as a DOT file?");
        }      
        /// <summary>
        /// saves the depndees of all cells in the spreadsheet as a DOT file
        /// </summary>
        private void saveDependeesGraph()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "dot files (*.dot)|*.dot|All files (*.*)|*.*";
            save.FilterIndex = 1;
            save.RestoreDirectory = true;
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = null;
                StringBuilder graph = new StringBuilder();
                List<string> allCells = new List<string>(sSheet.GetNamesOfAllNonemptyCells());
                graph.Append("digraph " + "all dependees" + " {" + "\n");
                foreach (string cellName in allCells)
                {
   
                    HashSet<string> dependees = new HashSet<string>(sSheet.dGraph.GetDependees(cellName));
                    foreach (string cell in dependees)
                    {
                        graph.Append(cellName + "->" + cell + "\n");
                    }
                }

                graph.Append("}" + "\n");
                sw = new StreamWriter(save.FileName, true);
                sw.Write(graph.ToString());
                sw.Close();
            }
        }
        /// <summary>
        /// saves the depndees of all cells in the spreadsheet as a DOT file
        /// </summary>
        private void saveCellDependees()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "dot files (*.dot)|*.dot|All files (*.*)|*.*";
            save.FilterIndex = 1;
            save.RestoreDirectory = true;
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = null;
                StringBuilder graph = new StringBuilder();
                graph.Append("digraph " + currentCell + " dependees" + " {" + "\n");
                
                    HashSet<string> dependees = new HashSet<string>(sSheet.dGraph.GetDependees(currentCell));
                    foreach (string cell in dependees)
                    {
                        graph.Append(currentCell + "->" + cell + "\n");
                    }             

                graph.Append("}" + "\n");
                sw = new StreamWriter(save.FileName, true);
                sw.Write(graph.ToString());
                sw.Close();
            }
        }
        /// <summary>
        /// saves the depndents of the selected cell as a DOT file
        /// </summary>
        private void saveDependentsGraph()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "dot files (*.dot)|*.dot|All files (*.*)|*.*";
            save.FilterIndex = 1;
            save.RestoreDirectory = true;
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = null;
                StringBuilder graph = new StringBuilder();
                List<string> allCells = new List<string>(sSheet.GetNamesOfAllNonemptyCells());
                graph.Append("digraph " + "all dependents" + " {" + "\n");
                foreach (string cellName in allCells)
                {
                    HashSet<string> dependents = new HashSet<string>(sSheet.dGraph.GetDependents(cellName));
                    foreach (string cell in dependents)
                    {
                        graph.Append(cellName + "->" + cell + "\n");
                    }
                }

                graph.Append("}" + "\n");
                sw = new StreamWriter(save.FileName, true);
                sw.Write(graph.ToString());
                sw.Close();
            }
        }
        /// <summary>
        /// saves the depndents of the selected cell as a DOT file
        /// </summary>
        private void saveCellDependents()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "dot files (*.dot)|*.dot|All files (*.*)|*.*";
            save.FilterIndex = 1;
            save.RestoreDirectory = true;
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = null;
                StringBuilder graph = new StringBuilder();
                graph.Append("digraph " + currentCell + " dependents" + " {" + "\n");

                HashSet<string> dependents = new HashSet<string>(sSheet.dGraph.GetDependents(currentCell));
                foreach (string cell in dependents)
                {
                    graph.Append(currentCell + "->" + cell + "\n");
                }

                graph.Append("}" + "\n");
                sw = new StreamWriter(save.FileName, true);
                sw.Write(graph.ToString());
                sw.Close();
            }
        }
        /// <summary>
        /// calls saveDependeesGraph();
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dependeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveDependeesGraph();
        }
        /// <summary>
        /// calls  saveDependentsGraph();
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dependentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveDependentsGraph();
        }
        /// <summary>
        /// calls saveCellDependees();
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveSelectedCellDependeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveCellDependees();
        }
        /// <summary>
        /// calls  saveCellDependents();
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveSelectedCellDependentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveCellDependents();
        }
    }
}