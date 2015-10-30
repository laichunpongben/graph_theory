// Pending review

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.IO;

namespace GraphLinearAlgebra
{
    public class MatrixCoordinateMap
    {
        private ConcurrentDictionary<string, string> hashMap;
		
	    public MatrixCoordinateMap(string path)
        {
		    createBody(path, false);
	    }
	
	    public MatrixCoordinateMap(string path, bool isReverse)
        {
		    createBody(path, true);
	    }
	
	    private MatrixCoordinateMap()
        {
		    ;
	    }
	
	    private void setHashMap(string key, string value)
        {
		    hashMap.TryAdd(key, value);
	    }
	
	    public ConcurrentDictionary<string, string> getHashMap()
        {		
		    return hashMap;	
	    }
	
	    private void createBody(string path, bool isReverse)
        {
            hashMap = new ConcurrentDictionary<string, string>();

            StreamReader br = new StreamReader(path);
	    
		    string line = null;
		    while ((line = br.ReadLine()) != null) {
			
                if (line.Length >0 ) {
			        string[] str = line.Split(',');
			        string key = null;
			        string value = null;
			        if (isReverse != true) {
				        key = str[0].Trim();
                        value = str[1].Trim();
			        } else {
                        key = str[1].Trim();
                        value = str[0].Trim();
			        }
			        setHashMap(key, value);
			    }
		    }
		
		    br.Close();
	    }
	
	    public MatrixCoordinateMap copyAndReverseMap()
        {
		    MatrixCoordinateMap newMap = new MatrixCoordinateMap();
		    newMap.hashMap = new ConcurrentDictionary<string, string>();
		
		    ConcurrentDictionary<string, string> oldHashMap = this.getHashMap();
            foreach (var entry in oldHashMap) {
			    newMap.setHashMap(entry.Value, entry.Key);
		    }

		    return newMap;			
	    }
    }
}
