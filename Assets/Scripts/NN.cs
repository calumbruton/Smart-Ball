using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
	public class NN
	{
		//The number of nodes in each layer
		public static int inpt_nds = 5;
		public static int hdn_nds = 5;
		public static int outpt_nds = 1;

		//DNA is a float array with hidden layer weights, hidden layer bias, output layer weights, output layer bias
		public float[] DNA;

		//inputs array for 5 distances
		public float[] inputs;

		//Weights for each connection - hidden layer and output layer - 2d Arrays
		private float [,] hl1_weights;
		private float[,] ol_weights;

		//Hidden layer and output layer bias
		private float [] bias_hl;
		private float [] bias_ol;

		public NN (float[] DNA)
		{
			hl1_weights = new float[inpt_nds, hdn_nds];
			bias_hl = new float[hdn_nds];
			ol_weights = new float[hdn_nds, outpt_nds];
			bias_ol = new float[outpt_nds];


			//Initialize the weights and bias arrays
			InitWeights (DNA);

		} //End of constructor



		//Initialize the weights of the NN by reading the DNA
		private void InitWeights(float[] DNA){

			//DNA index location
			int loc = 0;

			//initialize hidden layer weights
			for (int i = 0; i < inpt_nds; i++) {
				for (int j = 0; j < hdn_nds; j++) {
					hl1_weights [i, j] = DNA[loc];
					loc++;
				}
			} 

			//initialize hidden layer bias
			for (int i = 0; i < hdn_nds; i++) {
				bias_hl [i] = DNA [loc];
				loc++;
			}


			//initialize output layer weights
			for (int i = 0; i < hdn_nds; i++) {
				for (int j = 0; j < outpt_nds; j++) {
					ol_weights [i, j] = DNA[loc];
					loc++;
				}
			} 

			//initialize output layer bias
			for (int i = 0; i < outpt_nds; i++) {
				bias_ol [i] = DNA [loc];
				loc++;
			}

		} //End of Init Weights



		//Softsign is an activation function that maps input in between -1 & 1
		float SoftSign(float inp)
		{
			return inp / (1 + Math.Abs(inp));
		}



		//Initialize the weights of the NN
		public float decision(float[] inputs){

			//z1 is the result of the matrix multiplication of inputs and weights of hidden layer + bias
			float[] z1 = MultiplyMatrix(inputs, hl1_weights);
				
			//a1 is the the result of using softsign on z1
			float[] a1 = new float [z1.Length];
			for (int i = 0; i < z1.Length; i++) {
				a1 [i] = SoftSign (z1 [i] + bias_hl [i]);
			}

			//z2 is the result of the matrix multiplication of a1 and weights of output layer + bias
			float[] z2 = MultiplyMatrix(a1, ol_weights);

			//o1 is the result of using softsign on z2 and is the final rotation value of the player
			float[] o1 = new float [z2.Length];
			for (int i = 0; i < z2.Length; i++) {
				o1 [i] = SoftSign (z2 [i]+ bias_ol [i]);
			}

			float output = o1[0];

			return output;

		}//End of Decision






		//function to matrix multiply a row matrix by a square matrix
		private float[] MultiplyMatrix(float[] a, float[,] b){

			//The matrix being returned is 1 x the number of columns
			float[] retMatrix = new float[b.GetLength (1)];

			//for each column of b
			for (int i = 0; i < b.GetLength (1); i++) {
				
				//multiply each row val by its corresponding a matrix val and sum the results
				float sum = 0;
				for (int j = 0; j < a.GetLength (0); j++) {
					sum += a [j] * b [j, i];
				}
				retMatrix [i] = SoftSign(sum);
			}

			return retMatrix;
		}//End of Multiply Matrix
	
	}
}

