using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Calculator.Tests
{
    [TestClass()]
    public class CalculatorTests
    {
        [TestMethod()]
        public void GetFileResultTest()
        {
            // Arrage
            Calculator calc = new Calculator();

            // Act
            calc.CalculateFile(Directory.GetCurrentDirectory() + "\\TestFile.txt", Directory.GetCurrentDirectory() + "\\TestFileResult.txt");

            string[] actual = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\TestFileResult.txt");
            string[] expected = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\RightFileResult.txt");

            //Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetExpressionResult()
        {
            // Arrage
            Calculator calc = new Calculator();

            string expected = "На ноль делить нельзя.";
            string actual = "";

            // Act
            try
            {
                actual = calc.GetExpressionResult("2*(-2*0,5/-10+30)*3+5/(-20+40-20)").ToString();
            }
            catch (DivideByZeroException e)
            {
                actual = e.Message;
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void GetExpressionResult2()
        {
            // Arrage
            Calculator calc = new Calculator();

            string expected = "Входная строка имела неверный формат.";
            string actual = "";

            // Act
            try
            {
                actual = calc.GetExpressionResult("(2-3*10)*2-x*(20+20/20)").ToString();
            }
            catch (Exception)
            {
                actual = "Входная строка имела неверный формат.";
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void GetExpressionResult3()
        {
            // Arrage
            Calculator calc = new Calculator();

            // Act
            double actual = calc.GetExpressionResult("(-20*-30+15/20)*(45+30/5)-(18+13-18)");
            double expected = 30625.25;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}