using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NeuralNetwork;
using System;

public class MasterController : MonoBehaviour
{
	public GameObject ball;
	public int pop_size; //pop size should be an even number
	public float time;
	public float generation_time; //seconds until a new generation is forced
	public Text gen_ui;

	//Genetic Variables
	private float[][] DNA;
	private int DNA_length = (NN.inpt_nds * NN.hdn_nds) + NN.hdn_nds + (NN.hdn_nds * NN.outpt_nds) + NN.outpt_nds;
	private int gen_num = 0;
	private int generations;
	private float mutation_prob = 5; // A 3 represents 30% i.e 3/10 chance
	public int elitism; //The number of most fit balls carried to next generation
	public bool increase_elites; //If true any balls that don't die in the generation time become elites

	//Lists
	public List<TrainingBallController> ball_pop = new List<TrainingBallController>();
	public List<TrainingBallController> ball_alive = new List<TrainingBallController>();

	//fitness scores is an array of each balls fitness score - roulette is a list used for parent selection
	public float[] fitness_scores;
	private List<float> roulette = new List<float>();


	// Use this for initialization
	void Start ()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		//Create the original DNA of the balls and spawn them with it
		createStartingDNA();
		spawnBalls();
	}



	// Update is called once per frame
	void Update ()
	{
		//update time
		time += Time.deltaTime;

		//Check if there are any active balls
		bool activeBalls = ball_alive.Count > 0;



		//If there are do this
		if (activeBalls) {

			//if the balls have been going around for more than the specified generation time
			if (time > generation_time) {

				//if increase elitism mode is active add all the balls that are alive's DNA to the elites
				if (increase_elites) {
					elitism = Math.Max(elitism, ball_alive.Count);
				}

				//Kill all active balls
				for (int i = ball_alive.Count-1; i >= 0; i--) {
					ball_alive[i].gameObject.SetActive(false);
					ball_alive.Remove(ball_alive[i]);
				}
			time = 0;
			}
		} 


		//If no more balls are alive then we need to increment the generation we are on perform 
		//selection, crossover and mutation, and then spawn a new generation of balls
		else {
			//reset the timer all balls have died
			time = 0;
			gen_num += 1;
			gen_ui.text = "Generation: " + gen_num.ToString ();


			//Get fitness scores
			fitness_scores = new float[pop_size];
			for (int i = 0; i < ball_pop.Count; i++) {
				fitness_scores [i] = ball_pop [i].fitness;
			}

			//Total the scores and allocate each ball their percent of total to the roulette wheel
			//This means a ball with 6 fitness out of a total generation of 10 will have a 6/10
			//chance of being selected as a parent
			float tot_gen_fitness = genFitness();

			roulette.Add (0);
			for (int i = 0; i < fitness_scores.Length-1; i++) {
				float alloc = fitness_scores [i] / tot_gen_fitness;
				roulette.Add (roulette [i] + alloc);
			}
			roulette.Add(1);


			//Use roulette selection to decide on parents -> parents are stored by their index
			System.Random rand = new System.Random();

			// need (population/2) parents because each pair of parents makes 2 children, we then subtract any elites
			int num_pairs_parents = (pop_size/2)-(elitism/2);

			int[][] parents = new int[num_pairs_parents][];

			// for each set of parents
			for(int i = 0; i < num_pairs_parents; i++){

				//roll the roulette wheel
				float roll = (float)(rand.NextDouble());

				//find the parent the roll is on and choose another random parents as its partner
				for (int j = 0; j < roulette.Count-1; j++){
					if ((roll >= roulette[j]) & (roll <= roulette[j+1])){
						parents[i] = new int[2];
						parents[i][0] = j;
						parents[i][1] = UnityEngine.Random.Range(0, pop_size);
						//print("roll is " + roll + " and j is " + j);
						break;
					}
				}
			}
			

			//Create a new DNA array to keep childrens DNA
			float [][] new_DNA = new float[pop_size][];

			//Use uniform crossover on parent and random other 
			for (int i = 0; i < parents.Length; i++) {
				//print ("parents " + i + " are " + parents [i][0] + " and the random " + parents[i][1]);

				float [] child1DNA = new float[DNA_length];
				float [] child2DNA = new float[DNA_length];

				//go through each location in the parents DNA and choose one gene to from each and 
				//randomly allocated it to one of the new children
				for (int loc = 0; loc < DNA_length; loc++){
					int r = UnityEngine.Random.Range(0,2);
					//arbitrary - if r is 1 then give child 1 parents 1's dna at that loc and child 2 the other parents
					if (r == 1){
						child1DNA[loc] = DNA[parents[i][0]][loc];
						child2DNA[loc] = DNA[parents[i][1]][loc];
					}
					//else do the other way around
					else {
						child1DNA[loc] = DNA[parents[i][1]][loc];
						child2DNA[loc] = DNA[parents[i][0]][loc];
					}
				}

				//add each childs DNA to our new generation DNA
				new_DNA[i*2] = child1DNA;
				new_DNA[(i*2)+1] = child2DNA;

			} // End of uniform crossover


			//bring elites DNA forward
			//Sort the fitness scores in a new array so index locations of original are intact
			float [] sorted_fitness = new float[fitness_scores.Length];
			Array.Copy (fitness_scores, sorted_fitness, fitness_scores.Length);
			Array.Sort (sorted_fitness);

			//recover the location of the elites
			int [] elite_locs = new int [elitism];
			for (int i = 0; i < elitism; i++) {

				//get the index of the elite from fitness scores, this is the same index as it's DNA
				int loc = Array.IndexOf(fitness_scores, sorted_fitness[(sorted_fitness.Length-1)-i]);

				//print(sorted_fitness[(sorted_fitness.Length-1)-i] + "in location " + loc);

				//Add the DNA of the elites to the new_DNA pool
				new_DNA[(pop_size-1)-i] = DNA[loc];
			}


			//Make the new_DNA the population DNA for the next generation
			DNA = new_DNA;

			//Mutate random offspring (no elites) based on the mutation probability identified in the class variable
			for (int child = 0; child < pop_size-elitism; child++) {
				int mut = UnityEngine.Random.Range (1, 10);
				if (mut < mutation_prob) {
					//Make a random location in the offspring's DNA a new random value
					int mut_loc = UnityEngine.Random.Range (0, DNA_length);
					System.Random random = new System.Random ();
					new_DNA[child][mut_loc] = (float)(random.NextDouble () * 2.0 - 1.0);
				}
			}

			//Clear generation of balls and spawn another generation of balls using new DNA
			ball_pop.Clear();
			spawnBalls();

		} //End of Else



	} //End of Update


	//Function that returns the generations total fitness
	public float genFitness(){
		float tot_gen_fit = 0;
		for (int i = 0; i < fitness_scores.Length; i++) {
			tot_gen_fit += fitness_scores[i];
		}
		return tot_gen_fit;
	}

	//This spawns the balls
	public void spawnBalls (){

		//Loop through the population size and instantiate balls and their brains
		for(int i=0; i<pop_size; i++){

			TrainingBallController b = Instantiate(ball).GetComponent<TrainingBallController>();

			//Attach a brain to the PlayerController
			b.brain = new NN(DNA[i]); 

			//Add the ball to the population and the alive balls
			ball_pop.Add(b); 
			ball_alive.Add(b); 

		}
	}



	//Create a DNA strand for each member of the population
	public void createStartingDNA (){

		//Need a DNA strand for each member of the population 
		DNA = new float[pop_size][];

		//initialize each array in the jagged array to be the length of the DNA
		for (int i = 0; i < pop_size; i++) {
			DNA[i] = new float[DNA_length];
		}
			
		//Fill all of the DNA with random float values
		for (int i = 0; i < pop_size; i++) {
			for (int j = 0; j < DNA_length; j++) {
			
				//initialize the weights and bias randomly in the range 0 to 1 in this constructor for the DNA
				System.Random random = new System.Random ();
				DNA [i][j] = (float)(random.NextDouble () * 2.0 - 1.0);
				print ("This"); print ("is here"); print ("so"); print ("balls initialize correctly");

			}
		}
	}
		
}
