using System;

namespace AssemblyCSharp
{
	public class NN
	{
		//The number of nodes in each layer
		int inpt_nds = 5;
		int hdn_nds = 5;
		int outpt_nds = 2;

		//
		float[] inputs;

		//Weights for each connection - hidden layer and output layer - 2d Arrays
		float [,] hl1_weights = new float[inpt_nds, hdn_nds];
		float [,] ol_weights = new float[hdn_nds, outpt_nds];


		public NN ()
		{
		}


		//Initialize the weights of the NN
		private void InitWeights(){
			

		}


		//Initialize the weights of the NN
		private void decision(float[] inputs){
			float[,] z1;

			//z1 is the result of the matrix multiplication of inputs and weights of hidden layer + bias

			//a1 is the the result of using sigmoid on z1

			//z2 is the result of the matrix multiplication of a1 and weights of output layer + bias

			//o1 is the result of using sigmoid on z2 and is the final % value of choice of button

		}

	}
}

