// Pending review

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;
using System.Collections.Concurrent;
using MathNet.Numerics.Data.Text;
using MathNet.Numerics.LinearAlgebra;

namespace GraphLinearAlgebra
{
    public class IO
    {
        static string getString(string path) 
	    {
            StreamReader sr = new StreamReader(path);
            string str = sr.ReadToEnd();
            sr.Close();

		    return str;	
	    }
	
	    public static void setString(string path, string toWrite)
	    {
		    StreamWriter mmFile = new StreamWriter(path);
		    mmFile.WriteLine(toWrite);
		    mmFile.Close();	
	    }
	
	    static string getCsvString(string path)
	    {
            StreamReader scanner = new StreamReader(path);
            string str = scanner.ReadToEnd();
            string[] tokens = str.Split(',');
	    
	        string csv = null;

            for (int i = 0; i < tokens.Length; i++)
            {
                csv = csv + ',' + tokens[i];
            }
	     
	        scanner.Close();

		    return csv;		
	    }
    
        public static void createMatrixMarketFile(string pathReadMm, string pathWriteMm, Char delimiter, MatrixCoordinateMap replaceMap, bool isOneDimension)
        {
    	    string originalStr = IO.getString(pathReadMm);

    	    int countLine = 0;
    	    string originalLine;
    	    string updatedLine;
            string line;

            StringReader scanner = new StringReader(originalStr);
            StreamWriter outText = new StreamWriter(pathWriteMm, true);
    	    while ((line = scanner.ReadLine()) != null) {
                if (line.Length > 0) {
    		        countLine++;
                    originalLine = line;
    		
    		        if (countLine > 2) {
    			        updatedLine = parseAndReplaceStringByHashMap(originalLine, delimiter, replaceMap, isOneDimension);
    		        } else {
    			        updatedLine = originalLine;
    		        }
    		        outText.WriteLine(updatedLine);
                }
    	    }
    	
    	    scanner.Close();
    	    outText.Close();
        }
    
        public static void createMatrixMarketFile(string pathReadMm, string pathWriteMm, Char delimiter, MatrixCoordinateMap replaceMap1, MatrixCoordinateMap replaceMap2) 
        {
    	    string originalStr = IO.getString(pathReadMm);
    	    int countLine = 0;
    	    string originalLine;
    	    string updatedLine;
            string line;
    	
    	    StringReader scanner = new StringReader(originalStr);
    	    StreamWriter outText = new StreamWriter(pathWriteMm, true);
    	    while ((line = scanner.ReadLine()) != null) {
                if (line.Length > 0) {
    		        countLine++;
    		        originalLine = line;
    		
    		        if (countLine > 2) {
    			        updatedLine = parseAndReplaceStringByHashMap(originalLine, delimiter, replaceMap1, replaceMap2);
    		        } else {
    			        updatedLine = originalLine;
    		        }

                    outText.WriteLine(updatedLine);
                }
    	    }
    	
    	    scanner.Close();
            outText.Close();
        }
	
	    public static Matrix<double> createMatrixFromMatrixMarketFile(string path)
	    {
            Matrix<double> matrix = Matrix<double>.Build.SparseOfMatrix(MatrixMarketReader.ReadMatrix<double>(path));
            return matrix;
	    }
		
	    static string parseAndReplaceStringByHashMap (string originalStr, Char delimiter, MatrixCoordinateMap replaceMap, bool isFirstColumnOnly)
	    {
		    ConcurrentDictionary<string, string> hashMap = replaceMap.getHashMap();
		
		    StringBuilder buffer = new StringBuilder();

            char[] charSeparators = new char[] { delimiter };
            string[] tokens = originalStr.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
		    int length = tokens.Length;
		
		    string value1 = hashMap[tokens[0].Trim()];
		    if (value1 != null) {
			    tokens[0] = value1;
		    }
		
		    if (isFirstColumnOnly != true && length > 1) {
                string value2 = hashMap[tokens[1].Trim()];
			    if (value2 != null) {
				    tokens[1] = value2;
			    }
		    }
		
		    for (int i = 0; i < length; i++) {
			    if (i == 0) {
				    buffer.Append(tokens[i].Trim());
			    } else {
				    buffer.Append(delimiter);
                    buffer.Append(tokens[i].Trim());
			    }
		    }
		
		    string updatedStr = buffer.ToString();
		
		    return updatedStr;
	    }
	
	    static string parseAndReplaceStringByHashMap (string originalStr, Char delimiter, MatrixCoordinateMap replaceMap1, MatrixCoordinateMap replaceMap2) 
	    {
		    ConcurrentDictionary<string, string> hashMap1 = replaceMap1.getHashMap();
		    ConcurrentDictionary<string, string> hashMap2 = replaceMap2.getHashMap();
		
		    StringBuilder buffer = new StringBuilder();

            char[] charSeparators = new char[] { delimiter };
            string[] tokens = originalStr.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
		    int length = tokens.Length;

            string value1 = hashMap1[tokens[0].Trim()];
		    if (value1 != null) {
			    tokens[0] = value1;
		    }		
		
		    if (length > 1) {
                string value2 = hashMap2[tokens[1].Trim()];
			    if (value2 != null) {
				    tokens[1] = value2;
			    }
		    }
		
		    for (int i = 0; i < length; i++) {
			    if (i == 0) {
                    buffer.Append(tokens[i].Trim());
			    } else {
                    buffer.Append(delimiter);
                    buffer.Append(tokens[i].Trim());
			    }
		    }
		
		    string updatedStr = buffer.ToString();
		
		    return updatedStr;
	    }
    }
}
