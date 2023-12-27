using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Xml;

// implementation by Saoud Aldowaish

namespace SS
{
    [TestClass]
    public class SpreadsheetTests
    {

        /// <summary>
        /// creating an empty spreadsheet
        ///</summary>
        [TestMethod()]
        public void Test1()
        {
            Spreadsheet s = new Spreadsheet();
        }

        /// <summary>
        /// creating a  spreadsheet with double as content
        ///</summary>
        [TestMethod()]
        public void Test2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("x1", "1.2");
            Assert.AreEqual(s.GetCellContents("x1"), 1.2);
        }

        /// <summary>
        /// creating a  spreadsheet with string as content
        ///</summary>
        [TestMethod()]
        public void Test3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("x1", "y");
            Assert.AreEqual(s.GetCellContents("x1"), "y");
        }

        /// <summary>
        /// creating a  spreadsheet with formula as content
        ///</summary>
        [TestMethod()]
        public void Test4()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("x1", "=1.2+y");
            Assert.AreEqual(s.GetCellContents("x1"), new Formula("1.2+y"));
        }

        /// <summary>
        /// getting contents from an empty cell
        ///</summary>
        [TestMethod()]
        public void Test5()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual(s.GetCellContents("x1"), "");
        }

        /// <summary>
        /// adding empty content (empty string "")
        ///</summary>
        [TestMethod()]
        public void Test6()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("x1", "");
            Assert.AreEqual(s.GetCellContents("x1"), "");
        }

        /// <summary>
        /// testing if SetCellContents retruns the correct dependees
        ///</summary>
        [TestMethod()]
        public void Test7()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("x2", "=x1 + x3");
            s.SetContentsOfCell("x4", "=x2 + x5");
            Assert.IsTrue(new HashSet<string>(s.SetContentsOfCell("x1", "=x10 + x11")).SetEquals(new HashSet<string>() { "x1", "x4", "x2" }));
        }

        /// <summary>
        /// adding contents to the same cell
        ///</summary>
        [TestMethod()]
        public void Test8()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("x1", "x2 + x3");
            s.SetContentsOfCell("x1", "x4 + x5");
            Assert.AreEqual(s.GetCellContents("x1"), "x4 + x5");
        }

        /// <summary>
        /// simple GetNamesOfAllNonemptyCells() test 
        ///</summary>
        [TestMethod()]
        public void Test9()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("x1", "1.2");
            s.SetContentsOfCell("x2", "2.3");
            s.SetContentsOfCell("x3", "3.4");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "x1", "x2", "x3" }));
        }

        /// <summary>
        /// adding null to a cell
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test10()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("x1", (string)null);
        }

        /// <summary>
        /// trying to add content to an invalid name
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test11()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("x", "1.2");
        }

        /// <summary>
        /// trying to add content to a null
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test12()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "1.2");
        }

        /// <summary>
        /// trying to cause a circular dependency
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void Test13()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("x1", "=x1 + x2");
        }

        /// <summary>
        /// Test isValid return false
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test14()
        {
            Spreadsheet s = new Spreadsheet(s => false, s => s.ToUpper(), "");
            s.SetContentsOfCell("x1", "1");
        }

        /// <summary>
        /// Test Lookup basic case
        /// </summary>
        [TestMethod]
        public void Test15()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "");
            s.SetContentsOfCell("x1", "1");
            s.SetContentsOfCell("x2", "1");
            s.SetContentsOfCell("x3", "=x1+x2");
            Assert.AreEqual(s.GetCellContents("x3"), new Formula("x1+x2"));
        }

        /// <summary>
        /// Test get cell value
        /// </summary>
        [TestMethod]
        public void Test16()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "");
            s.SetContentsOfCell("x1", "1");
            s.SetContentsOfCell("x2", "1");
            s.SetContentsOfCell("x3", "=x1+x2");
            Assert.AreEqual(s.GetCellValue("x3"), 2.0);
        }

        /// <summary>
        /// Test get cell value with a non initialized cell should return an empty string
        /// </summary>
        [TestMethod]
        public void Test17()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "");
            Assert.AreEqual(s.GetCellValue("x1"), "");
        }

        /// <summary>
        /// Test changed before and after changing
        /// </summary>
        [TestMethod]
        public void Test18()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "");
            Assert.IsFalse(s.Changed);
            s.SetContentsOfCell("x1", "1");
            Assert.IsTrue(s.Changed);
        }


        /// <summary>
        /// Test spreadsheet constructor with invalid file
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Test19()
        {
            Spreadsheet s = new Spreadsheet("DNE.txt", s => true, s => s, "default");
        }

        /// <summary>
        /// Test spreadsheet constructor correctly
        /// </summary>
        [TestMethod()]
        public void Test20()
        {
            Spreadsheet s = new Spreadsheet();
            s.Save("file.txt");
            s = new Spreadsheet("file.txt", s => true, s => s, "default");
        }

        /// <summary>
        /// Test version does not match
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Test21()
        {
            Spreadsheet s = new Spreadsheet(); // version should be default
            s.Save("file.txt");
            s = new Spreadsheet("file.txt", s => true, s => s, "NotDafault");
        }

        /// <summary>
        /// Test save an empty file
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Test22()
        {
            Spreadsheet s = new Spreadsheet();
            s.Save("");
        }

        /// <summary>
        /// Test save a null
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Test23()
        {
            Spreadsheet s = new Spreadsheet();
            s.Save(null);
        }

        /// <summary>
        /// Testing Save with different types of cells (double, string and formula)
        /// </summary>
        [TestMethod()]
        public void Test24()
        {
            Spreadsheet s = new Spreadsheet();
            s.Save("file.txt");
            s = new Spreadsheet("file.txt", s => true, s => s, "default");
            s.SetContentsOfCell("x1", "2.0");
            s.SetContentsOfCell("x2", "word");
            s.SetContentsOfCell("x3", "=1*2");
            s.Save("file2.text");
        }

        /// <summary>
        /// Testing creating a simple file and pasing it to the constructor
        /// </summary>
        [TestMethod()]
        public void Test25()
        {
            using (XmlWriter writer = XmlWriter.Create("file.txt"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "1.0");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "x1");
                writer.WriteElementString("contents", "100");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            Spreadsheet s = new Spreadsheet("file.txt", s => true, s => s, "1.0");
        }

        /// <summary>
        /// Testing save on an invalid path
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Test26()
        {
            Spreadsheet s = new Spreadsheet();
            s.Save("/very/bad/path.xml");
        }

        /// <summary>
        /// making a large file then calling the constructor
        /// </summary>
        [TestMethod()]
        public void StressTest1()
        {
            using (XmlWriter writer = XmlWriter.Create("file.txt"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "1.0");

                for (int i = 0; i < 1000; i++)
                {
                    writer.WriteStartElement("cell");
                    writer.WriteElementString("name", "x"+i);
                    writer.WriteElementString("contents", "100");
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            Spreadsheet s = new Spreadsheet("file.txt", s => true, s => s, "1.0");
        }

        /// <summary>
        /// setting many cells
        /// </summary>
        [TestMethod()]
        public void StressTest2()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "");

            for (int i = 0; i < 1000; i++)
                {
                s.SetContentsOfCell("x" + i, "2.0");
                s.SetContentsOfCell("y" + i, "word");
                s.SetContentsOfCell("z" + i, "=1+2");
            }

            Assert.AreEqual(s.GetCellValue("x1"), 2.0);
            Assert.AreEqual(s.GetCellValue("y1"), "word");
            Assert.AreEqual(s.GetCellValue("z1"), 3.0);
            Assert.AreEqual(s.GetCellValue("x999"), 2.0);
            Assert.AreEqual(s.GetCellValue("y999"), "word");
            Assert.AreEqual(s.GetCellValue("z999"), 3.0);
        }
    }
}
