// Pending review

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Data.Text;

namespace GraphLinearAlgebra
{
    public class AdjacencyMatrix
    {
        internal MatrixCoordinateMap classMap1;
        internal MatrixCoordinateMap classMap2;
        private Matrix<double> matrix;
	    private bool isOneDimension;
	
	    void setClassMap1(MatrixCoordinateMap map)
        {
		    classMap1 = map;
	    }
	
	    void setClassMap2(MatrixCoordinateMap map)
        {
		    classMap2 = map;
	    }
	
	    void setIsOneDimension(bool b)
        {
		    isOneDimension = b;
	    }
	
	    void setMatrix(Matrix<double> m)
        {
		    matrix = m;
	    }

        public MatrixCoordinateMap getClassMap1()
        {
		    return classMap1;
	    }

        public MatrixCoordinateMap getClassMap2()
        {
		    return classMap2;
	    }

        public Matrix<double> getMatrix()
        {
		    return matrix;
	    }
	
	    bool getIsOneDimension()
        {
		    return isOneDimension;
	    }
	
	    public AdjacencyMatrix(MatrixCoordinateMap classMap, string pathLibrary, string pathInput, bool isOneDimension)
        {
		    setClassMap1(classMap);
		    setIsOneDimension(isOneDimension);
		    IO.createMatrixMarketFile(pathLibrary, pathInput, ' ', classMap, isOneDimension);
		    setMatrix(IO.createMatrixFromMatrixMarketFile(pathInput));
	    }

        public AdjacencyMatrix(MatrixCoordinateMap classMap1, MatrixCoordinateMap classMap2, string pathLibrary, string pathInput)
        {
		    setClassMap1(classMap1);
		    setClassMap2(classMap2);
		    setIsOneDimension(false);
		    IO.createMatrixMarketFile(pathLibrary, pathInput, ' ', classMap1, classMap2);
		    setMatrix(IO.createMatrixFromMatrixMarketFile(pathInput));
	    }

        public AdjacencyMatrix(MatrixCoordinateMap classMap1, MatrixCoordinateMap classMap2, Matrix<double> matrix)
        {
		    setClassMap1(classMap1);
		    setClassMap2(classMap2);

            int columnCount = matrix.ColumnCount;
		    bool isOneDimension;
		    if (columnCount == 1) {
			    isOneDimension = true;
		    } else {
			    isOneDimension = false;
		    }
		
		    setIsOneDimension(isOneDimension);
		    setMatrix(matrix);
	    }
	
	    public AdjacencyMatrix computeAdjacencyMatrixProduct(AdjacencyMatrix aMatrix2)
        {		
		    Matrix<double> matrix1 = this.getMatrix();
            Matrix<double> matrix2 = aMatrix2.getMatrix();

            Matrix<double> matrix = matrix1.Multiply(matrix2);
		
		    MatrixCoordinateMap newClassMap1 = this.getClassMap1();
		    MatrixCoordinateMap newClassMap2 = aMatrix2.getClassMap2();

            return new AdjacencyMatrix(newClassMap1, newClassMap2, matrix);
	    }
	
	    public AdjacencyMatrix computeAdjacencyMatrixProduct(AdjacencyMatrix aMatrix2, AdjacencyMatrix aMatrix3)
        {
		    AdjacencyMatrix aMatrix23 = aMatrix2.computeAdjacencyMatrixProduct(aMatrix3);
            return this.computeAdjacencyMatrixProduct(aMatrix23);
	    }
	
	    public AdjacencyMatrix computeAdjacencyMatrixAddition(AdjacencyMatrix aMatrix2)
        {
            Matrix<double> matrix1 = this.getMatrix();
            Matrix<double> matrix2 = aMatrix2.getMatrix();
            Matrix<double> matrix = matrix1.Add(matrix2);
		
		    MatrixCoordinateMap newClassMap1 = this.getClassMap1();
		    MatrixCoordinateMap newClassMap2 = aMatrix2.getClassMap2();

            return new AdjacencyMatrix(newClassMap1, newClassMap2, matrix);
	    }
	
	    public AdjacencyMatrix getMatrixColumnVectorRatio()
        {
            Matrix<double> matrix = this.getMatrix();
		    MatrixCoordinateMap newClassMap1 = this.getClassMap1();
		    MatrixCoordinateMap newClassMap2 = this.getClassMap2();

            int size = matrix.ColumnCount;
            Matrix<double> matrixColumnVectorRatio = matrix.Clone();
    	
    	    for (int j = 0; j < size; j++) { //start at column 0

                Vector<double> vector = matrixColumnVectorRatio.Column(j);    	
    		    double vectorSum = vector.Sum();
    		
    			    if (vectorSum > 0) {
                        Vector<double> vectorRatio = vector.Multiply(1 / vectorSum);
    				    matrixColumnVectorRatio.SetColumn(j, vectorRatio);
    			    }
    	    }

            return new AdjacencyMatrix(newClassMap1, newClassMap2, matrixColumnVectorRatio);
	    }
	
	    public AdjacencyMatrix getMatrixColumnVectorSum()
        {
            Matrix<double> matrix = this.getMatrix();
		    MatrixCoordinateMap newClassMap1 = this.getClassMap1();
		    MatrixCoordinateMap newClassMap2 = this.getClassMap2();
		
    	    int size = matrix.ColumnCount;
            Matrix<double> matrixColumnVectorSum = Matrix<double>.Build.SparseIdentity(size);
    	
    	    for (int j = 0; j < size; j++) {
    		    Vector<double> vector = matrix.Column(j);    		
    		    double vectorSum = vector.Sum();
    		
    		    if (vectorSum > 0) {
    			    matrixColumnVectorSum.At(j, j, 1);	
    		    } else {
                    matrixColumnVectorSum.At(j, j, 0);
    		    }
    	    }

            return new AdjacencyMatrix(newClassMap1, newClassMap2, matrixColumnVectorSum);
	    }
	
	    public AdjacencyMatrix getMatrixIdentityComplement()
        {
            Matrix<double> matrix = this.getMatrix();
		    MatrixCoordinateMap newClassMap1 = this.getClassMap1();
		    MatrixCoordinateMap newClassMap2 = this.getClassMap2();
		
    	    int size = matrix.RowCount;
            int size2 = matrix.ColumnCount;
    	
    	    if (size != size2) {
    		    Console.WriteLine("row=" + size + ",col=" + size2);
    	    }

            Matrix<double> matrixIdentityComplement = matrix.Clone();
    	
    	    double valueOriginal = 0.0;
    	    double valueReplace = 0.0;
    	
    	    for (int i = 0; i < size; i++) {
    		    valueOriginal = matrixIdentityComplement.At(i, i);
    		    valueReplace = 1 - valueOriginal;
    		    matrixIdentityComplement.At(i, i, valueReplace);
    	    }

            return new AdjacencyMatrix(newClassMap1, newClassMap2, matrixIdentityComplement);
	    }
	
	    public AdjacencyMatrix getPseudoInverseDiagonal()
	    {
	
		    Matrix<double> matrix = this.getMatrix();
		    MatrixCoordinateMap newClassMap1 = this.getClassMap1();
		    MatrixCoordinateMap newClassMap2 = this.getClassMap2();

            int size = matrix.ColumnCount;
            int size2 = matrix.ColumnCount;
    	
    	    if (size != size2) {
    		    Console.WriteLine("row=" + size + ",col=" + size2);
    	    }

    	    Matrix<double> matrixPseudoInverseDiagonal = matrix.Clone();
    	
    	    for (int i = 0; i < size; i++) {
    		    double valueOriginal = matrixPseudoInverseDiagonal.At(i, i);
    		    double valueReplace = 0;
    		    if (valueOriginal != 0) {
    			    valueReplace = 1 / valueOriginal;
    		    }
    		
    		    matrixPseudoInverseDiagonal.At(i, i, valueReplace);
    		
    	    }

            return new AdjacencyMatrix(newClassMap1, newClassMap2, matrixPseudoInverseDiagonal);
	    }

        public AdjacencyMatrix transpose()
        {
            Matrix<double> matrix = this.getMatrix();
            MatrixCoordinateMap newClassMap1 = this.getClassMap1();
            MatrixCoordinateMap newClassMap2 = this.getClassMap2();

            Matrix<double> matrixTransposed = matrix.Transpose();

            return new AdjacencyMatrix(newClassMap2, newClassMap1, matrixTransposed);
        }

        public void exportMatrixMarketFile(string pathOutputCoordinate)
        {
            Matrix<double> matrix = this.getMatrix();
            MatrixMarketWriter.WriteMatrix(pathOutputCoordinate, matrix);
        }
	
	    public void copyCoordinateToLabelMatrixMarketFile(string pathOutputCoordinate, string pathOutputLabel) 
	    {
		    MatrixCoordinateMap classMap1 = this.getClassMap1();
		    MatrixCoordinateMap classMap2 = this.getClassMap2();
		    bool isOneDimension = this.getIsOneDimension();
		
		    if (isOneDimension) {
			    MatrixCoordinateMap classMapReverse1 = classMap1.copyAndReverseMap();
			    IO.createMatrixMarketFile(pathOutputCoordinate, pathOutputLabel, ' ', classMapReverse1, isOneDimension); 
		    } else {
			    MatrixCoordinateMap classMapReverse1 = classMap1.copyAndReverseMap();
			    MatrixCoordinateMap classMapReverse2 = classMap2.copyAndReverseMap();
			    IO.createMatrixMarketFile(pathOutputCoordinate, pathOutputLabel, ' ', classMapReverse1, classMapReverse2);
		    }
		
	    }

    }
}
