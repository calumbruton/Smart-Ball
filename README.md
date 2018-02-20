# Smart-Ball

Smart Ball is a project in Unity that uses a neural network and a genetic algorithm to teach balls to safetly navigate a course. 

<a href="https://imgflip.com/gif/24u5nl"><img src="https://i.imgflip.com/24u5nl.gif" title="made at imgflip.com"/></a>
&nbsp;
## The Neural Network

The program starts by initializing x number of training balls. Each one of these training balls is assigned a random DNA sequence (an array of values) where each value is used as a weight for a connections or bias in its neural network (Its brain). The neural network takes 5 distances as inputs by using raycasts that are sent from the ball it is assigned to. The distance is the minimum of the closest object hit and a user specified distance. These distances come from the left, forward-left, forward, forward-right, and right (picture below). Every frame the neural network of each ball will make a decision on the ball rotation based on these 5 inputs. If the ball hits a wall, it will die.

<img width="250" alt="screen shot 2018-02-19 at 2 55 08 am" src="https://user-images.githubusercontent.com/12948431/36367561-3ae77b60-1521-11e8-81b9-09ff41ef7897.png">
&nbsp;

## Genetic Algorithm

The genetic algorithm used consisted of uniform DNA crossover, roulette parent selection, elitism and random mutation.

### Fitness

To score a ball (and its DNA's performance) a fitness score was used that increased every frame a ball was alive using its average distance from the walls around it.


### Parent Selection

At the end of each generation the fitness scores of each ball are used to allocate each a portion of a roulette wheel that ranges from 0 to 1. A random number is then chosen in this range and the ball that it lands on is chosen as a parent. The second parent is chosen randomly and this set of parents produces two children.

### DNA Crossover

The DNA of parents is combined into children using uniform crossover. For each index location in the DNA Array's one of the parents DNA is allocated to one child and the other is allocated to the other child based on random 50/50 chance. 

### Mutation

A mutation will occur on a random number of balls DNA based on a user defined percentage. The mutation simply takes a random index location and changes the value to a new float within the range -1 to 1. 

### Elitism

To avoid losing strong DNA sequences, elitism was introduced. This transfers the top y number of fitest balls DNA sequences to the next generation. When running the program I used 10% elitism, or 6 out of a population size of 60 balls. Mutation was not used on these balls.

